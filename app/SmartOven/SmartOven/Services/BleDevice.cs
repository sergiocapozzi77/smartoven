using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.BLE.Abstractions.EventArgs;
using SmartOvenV2.Models;
using Xamarin.Forms;

namespace SmartOvenV2.Services
{
    class BleCommand
    {
        public BleCommand(string command, byte[] payload)
        {
            Command = command;
            Payload = payload;
        }

        public string Command { get; }
        public byte[] Payload { get;  }
    }

    class BleDevice : IBleDevice
    {
        private bool isOtaInProgress;
        private bool disconnected;

        private ICharacteristic commandsCharacteristic;
        private ICharacteristic otaCharacteristic;
        private ICharacteristic cpuTemperatureReadCharacteristic;
        private ICharacteristic ovenStatusReadCharacteristic;
        private ICharacteristic elementsStatusReadCharacteristic;

        public event EventHandler<double> BottomDesiredTemperatureChanged;
        public event EventHandler<double> TopDesiredTemperatureChanged;
        public event EventHandler<double[]> OnReadCpuTemperatures;
        public event EventHandler<OvenStatus> OnReadOvenStatus;
        public event EventHandler<FirmwareVersion> OnFirmwareVersion;
        public event EventHandler<ElementsStatusInfo> OnReadElementsStatusInfo;
        public event EventHandler<string> OnOtaResult;

        readonly BlockingCollection<BleCommand> blockingCollection = new BlockingCollection<BleCommand>();
        private Task commandThread;
        private ICharacteristic firmwareVersionReadCharacteristic;
        private ICharacteristic otaFinishCharacteristic;
        private IDevice bleDevice;

        public async Task Initialise(IDevice device)
        {
            this.bleDevice = device;
            var service = await device.GetServiceAsync(Guid.Parse("a18ba38e-c3d7-4df8-a6c2-906dfd6a605b"));
            commandsCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("fec39133-ef77-4bd0-bc1d-d7e5c1290bf8"));
            cpuTemperatureReadCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("c0fd6f7c-1b05-11ed-861d-0242ac120002"));
            ovenStatusReadCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("5f1a489b-7fb6-49ac-b9a2-5807befe2c3a"));
            elementsStatusReadCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("a3068be8-1b05-11ed-861d-0242ac120002"));

            var otaService = await device.GetServiceAsync(Guid.Parse("ef19c991-0aaa-40c0-82cd-3602722a9cdb"));
            otaCharacteristic = await otaService.GetCharacteristicAsync(Guid.Parse("1bfab929-ec5c-4d21-8f06-494105887f68"));
            firmwareVersionReadCharacteristic = await otaService.GetCharacteristicAsync(Guid.Parse("790dbac1-ee08-4dbf-8b4a-6914ed3fe6bf"));
            otaFinishCharacteristic = await otaService.GetCharacteristicAsync(Guid.Parse("e4caefcc-fd51-42e8-8960-41fa20bb10a4"));

            otaFinishCharacteristic.ValueUpdated += OtaCharacteristicOnValueUpdated;
            await otaFinishCharacteristic.StartUpdatesAsync();

            await this.bleDevice.RequestMtuAsync(512);

            commandThread = Task.Factory.StartNew(() =>
            {
                //Loop will continue as long as IsCompleted returns false
                while (!blockingCollection.IsCompleted)
                {
                    try
                    {
                        BleCommand command;
                        if (blockingCollection.TryTake(out command))
                        {
                            ExceuteCommand(command);
                        }
                    }
                    catch
                    {
                    }
                }
            });

        }

        private void ExceuteCommand(BleCommand cmd)
        {
            if (isOtaInProgress || disconnected)
            {
                return;
            }

            try
            {
                switch (cmd.Command)
                {
                    case "ReadCpuTemperatures":
                        {
                            var binaryTemperatures = this.cpuTemperatureReadCharacteristic.ReadAsync().Result;
                            var temperatures = Encoding.UTF8.GetString(binaryTemperatures);
                            this.OnReadCpuTemperatures?.Invoke(this, JsonConvert.DeserializeObject<double[]>(temperatures));
                            break;
                        }
                    case "ReadFirmwareVersion":
                    {
                        var value = this.firmwareVersionReadCharacteristic.ReadAsync().Result;
                        var version = Encoding.UTF8.GetString(value);
                        this.OnFirmwareVersion?.Invoke(this, JsonConvert.DeserializeObject<FirmwareVersion>(version));
                        break;
                    }
                    case "ReadOvenStatus":
                        {
                            var value = this.ovenStatusReadCharacteristic.ReadAsync().Result;
                            var status = Encoding.UTF8.GetString(value);
                            this.OnReadOvenStatus?.Invoke(this, JsonConvert.DeserializeObject<OvenStatus>(status));
                            break;
                        }
                    case "ReadElementsStatus":
                        {
                            var value = this.elementsStatusReadCharacteristic.ReadAsync().Result;
                            var status = Encoding.UTF8.GetString(value);
                            if(status.Contains("nan"))
                            {
                                return;
                            }

                            var statusCmd =
                                ElementsStatusInfo.FromCommand(
                                    JsonConvert.DeserializeObject<ElementsStatusInfoCommand>(status));
                            this.OnReadElementsStatusInfo?.Invoke(this, statusCmd);
                            break;
                        }
                    case "SwitchOn":
                    case "SwitchOff":
                    case "SetBottomTemperature":
                    case "SetTopTemperature":
                    case "SetTopPower":
                    case "SetBottomPower":
                    case "LightOn":
                    case "LightOff":
                    case "SetBottomMaxPower":
                    case "SetTopMaxPower":
                        {
                            this.SendCommandRetry(cmd.Payload);
                            break;
                        }
                }

            }
            catch (Exception)
            {
            }
        }

        private void OtaCharacteristicOnValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            var otaResult = e.Characteristic.Value;
            var result = Encoding.UTF8.GetString(otaResult);

            StopOta();

            this.OnOtaResult?.Invoke(this, result);
        }

        public void StartOta()
        {
            isOtaInProgress = true;
        }

        public Task<int> SetMtu(int mtu)
        {
            return bleDevice.RequestMtuAsync(mtu);
        }

        public Task<bool> SendOtaData(byte[] data)
        {
            return otaCharacteristic.WriteAsync(data);
        }

        public void StopOta()
        {
            isOtaInProgress = false;
        }

        public void ReadFirmwareVersion()
        {
            this.blockingCollection.Add(new BleCommand("ReadFirmwareVersion", null));
        }

        public void ReadCpuTemperatures()
        {
            this.blockingCollection.Add(new BleCommand("ReadCpuTemperatures", null));
        }

        public void ReadOvenStatus()
        {
            this.blockingCollection.Add(new BleCommand("ReadOvenStatus", null));
        }

        public void ReadElementsStatus()
        {
            this.blockingCollection.Add(new BleCommand("ReadElementsStatus", null));
        }

        public Task<ElementsStatusInfo> GetInfoAsync()
        {
            throw new NotImplementedException();
        }

        public void SwitchOn()
        {
            this.blockingCollection.Add(new BleCommand("SwitchOn", Encoding.UTF8.GetBytes("ON")));
        }

        public void SwitchOff()
        {
            this.blockingCollection.Add(new BleCommand("SwitchOff", Encoding.UTF8.GetBytes("OFF")));
        }

        public void SetBottomTemperature(double temperature)
        {
            this.BottomDesiredTemperatureChanged?.Invoke(this, temperature);
            this.blockingCollection.Add(new BleCommand("SetBottomTemperature", Encoding.UTF8.GetBytes($"TBO:{temperature:F}")));
        }

        public void SetTopTemperature(double temperature)
        {
            this.TopDesiredTemperatureChanged?.Invoke(this, temperature);
            this.blockingCollection.Add(new BleCommand("SetTopTemperature", Encoding.UTF8.GetBytes($"TTO:{temperature:F}")));
        }

        void SendCommandRetry(byte[] cmd)
        {
            var r = commandsCharacteristic.WriteAsync(cmd).Result;
        }

        public void Close()
        {
            disconnected = true;
            this.blockingCollection.CompleteAdding();
        }

        public void Boost()
        {
            this.SetTopTemperature(500);
            this.SetBottomTemperature(500);
        }

        public void SetTopPower(double power)
        {
            this.blockingCollection.Add(new BleCommand("SetTopPower", Encoding.UTF8.GetBytes($"PWT:{power:F}")));
        }

        public void SetBottomPower(double power)
        {
            this.blockingCollection.Add(new BleCommand("SetBottomPower", Encoding.UTF8.GetBytes($"PWB:{power:F}")));
        }

        public void StopBoost(double top, double bottom)
        {
            this.SetTopTemperature(top);
            this.SetBottomTemperature(bottom);
        }

        public void SetBottomMaxPower(double bottomMaxPower)
        {
            this.blockingCollection.Add(new BleCommand("SetBottomMaxPower", Encoding.UTF8.GetBytes($"BMX:{bottomMaxPower:F}")));
        }

        public void SetTopMaxPower(double topMaxPower)
        {
            this.blockingCollection.Add(new BleCommand("SetTopMaxPower", Encoding.UTF8.GetBytes($"TMX:{topMaxPower:F}")));
        }

        public void LightOff()
        {
            this.blockingCollection.Add(new BleCommand("LightOff", Encoding.UTF8.GetBytes($"REL2OFF")));
        }

        public void LightOn()
        {
            this.blockingCollection.Add(new BleCommand("LightOn", Encoding.UTF8.GetBytes($"REL2ON")));
        }
    }
}
