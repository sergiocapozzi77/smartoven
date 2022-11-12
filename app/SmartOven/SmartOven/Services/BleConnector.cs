using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using SmartOvenV2.Models;

namespace SmartOvenV2.Services
{
    class BleConnector : IBleConnector
    {
        public bool IsConnected { get; private set; }
        private IBluetoothLE ble;
        private IAdapter adapter;
        private CancellationTokenSource _cancellationTokenSource;
        private IBleDevice bleDevice;

        public event EventHandler OnDisconnected;
        public event EventHandler<IDevice> OnConnected;

        public event EventHandler<double> BottomDesiredTemperatureChanged;
        public event EventHandler<double> TopDesiredTemperatureChanged;
        public event EventHandler<double[]> OnReadCpuTemperatures;
        public event EventHandler<OvenStatus> OnReadOvenStatus;
        public event EventHandler<ElementsStatusInfo> OnReadElementsStatusInfo;
        public event EventHandler<FirmwareVersion> OnFirmwareVersion;
        public event EventHandler<string> OnOtaResult;

        public void StartOta()
        {
            this.bleDevice?.StartOta();
        }

        public Task<int> SetMtu(int mtu)
        {
            return this.bleDevice?.SetMtu(mtu);
        }

        public Task<bool> SendOtaData(byte[] data)
        {
            return this.bleDevice?.SendOtaData(data);
        }

        public void StopOta()
        {
            this.bleDevice?.StopOta();
        }

        public void ReadCpuTemperatures()
        {
            this.bleDevice?.ReadCpuTemperatures();
        }

        public void ReadOvenStatus()
        {
            this.bleDevice?.ReadOvenStatus();
        }

        public void ReadElementsStatus()
        {
            this.bleDevice?.ReadElementsStatus();
        }

        public void ReadFirmwareVersion()
        {
            this.bleDevice?.ReadFirmwareVersion();
        }

        public void SwitchOn()
        {
            this.bleDevice?.SwitchOn();
        }

        public void SwitchOff()
        {
            this.bleDevice?.SwitchOff();
        }

        public void SetBottomTemperature(double temperature)
        {
            this.bleDevice?.SetBottomTemperature(temperature);
        }

        public void SetTopTemperature(double temperature)
        {
            this.bleDevice?.SetTopTemperature(temperature);
        }

        public void Boost()
        {
            this.bleDevice?.Boost();
        }

        public void SetTopPower(double power)
        {
            this.bleDevice?.SetTopPower(power);
        }

        public void SetBottomPower(double power)
        {
            this.bleDevice?.SetBottomPower(power);
        }

        public void StopBoost(double top, double bottom)
        {
            this.bleDevice?.StopBoost(top, bottom);
        }

        public void SetBottomMaxPower(double bottomMaxPower)
        {
            this.bleDevice?.SetBottomMaxPower(bottomMaxPower);
        }

        public void SetTopMaxPower(double topMaxPower)
        {
            this.bleDevice?.SetTopMaxPower(topMaxPower);
        }

        public void LightOff()
        {
            this.bleDevice?.LightOff();
        }

        public void LightOn()
        {
            this.bleDevice?.LightOn();
        }

        public BleConnector()
        {

            this.ble = CrossBluetoothLE.Current;
            this.adapter = CrossBluetoothLE.Current.Adapter;

            this.adapter.ScanTimeout = int.MaxValue;
            this.adapter.ScanMode = ScanMode.LowLatency;

            this.adapter.ScanTimeoutElapsed += this.Adapter_ScanTimeoutElapsed;
            this.adapter.DeviceDiscovered += this.Adapter_DeviceDiscovered;
            this.adapter.DeviceConnected += this.Adapter_DeviceConnected;
            this.adapter.DeviceDisconnected += this.Adapter_DeviceDisconnected;
            this.adapter.DeviceConnectionLost += this.Adapter_DeviceConnectionLost;


            if (this.ble.State == BluetoothState.Off)
            {
                UserDialogs.Instance.Alert("Bluetooh", "You must activate the bluetooth!!", "Ok");
            }

            this.ble.StateChanged += (s, e) =>
            {
                this.ToggleDiscovery(e.NewState);
            };

            this.ToggleDiscovery(this.ble.State);
        }

        private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            Disconnect();
        }

        private void Adapter_DeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            Disconnect();
        }

        void Disconnect()
        {
            if (bleDevice != null)
            {
                this.bleDevice.OnReadCpuTemperatures -= BleDeviceOnReadCpuTemperatures;
                this.bleDevice.OnReadElementsStatusInfo -= BleDeviceOnReadElementsStatusInfo;
                this.bleDevice.OnFirmwareVersion -= BleDeviceOnOnFirmwareVersion;
                this.bleDevice.OnOtaResult -= OtaResult;
                this.bleDevice.OnReadOvenStatus -= BleDeviceOnReadOvenStatus;
                this.bleDevice.BottomDesiredTemperatureChanged -= BleDeviceBottomDesiredTemperatureChanged;
                this.bleDevice.TopDesiredTemperatureChanged -= BleDeviceTopDesiredTemperatureChanged;
                this.bleDevice?.Close();
                this.bleDevice = null;
            }

            this.IsConnected = false;
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }

        private void BleDeviceOnOnFirmwareVersion(object sender, FirmwareVersion e)
        {
            this.OnFirmwareVersion?.Invoke(sender, e);
        }

        private void OtaResult(object sender, string e)
        {
            this.OnOtaResult?.Invoke(sender, e);
        }

        private async void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
        {
            this.bleDevice = new BleDevice();
            await this.bleDevice.Initialise(e.Device);

            this.bleDevice.OnReadCpuTemperatures += BleDeviceOnReadCpuTemperatures;
            this.bleDevice.OnReadElementsStatusInfo += BleDeviceOnReadElementsStatusInfo;
            this.bleDevice.OnReadOvenStatus += BleDeviceOnReadOvenStatus;
            this.bleDevice.OnFirmwareVersion += BleDeviceOnOnFirmwareVersion;
            this.bleDevice.OnOtaResult += OtaResult;
            this.bleDevice.BottomDesiredTemperatureChanged += BleDeviceBottomDesiredTemperatureChanged;
            this.bleDevice.TopDesiredTemperatureChanged += BleDeviceTopDesiredTemperatureChanged;

            this.IsConnected = true;
            this.OnConnected?.Invoke(this, e.Device);
        }

        private void BleDeviceTopDesiredTemperatureChanged(object sender, double e)
        {
            this.TopDesiredTemperatureChanged?.Invoke(sender, e);
        }

        private void BleDeviceBottomDesiredTemperatureChanged(object sender, double e)
        {
            this.BottomDesiredTemperatureChanged?.Invoke(sender, e);
        }

        private void BleDeviceOnReadOvenStatus(object sender, OvenStatus e)
        {
            this.OnReadOvenStatus?.Invoke(sender, e);
        }

        private void BleDeviceOnReadElementsStatusInfo(object sender, ElementsStatusInfo e)
        {
            this.OnReadElementsStatusInfo?.Invoke(sender, e);
        }

        private void BleDeviceOnReadCpuTemperatures(object sender, double[] e)
        {
            this.OnReadCpuTemperatures?.Invoke(sender, e);
        }

        private void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if(this.IsConnected)
            {
                return;
            }

            this.adapter.ConnectToDeviceAsync(e.Device, new ConnectParameters(autoConnect: true, forceBleTransport: true));
        }

        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            this.ToggleDiscovery(this.ble.State);
        }

        private Task ToggleDiscovery(BluetoothState state)
        {
            if (state == BluetoothState.On && !IsConnected)
            {
                return this.Resume();
            }
            else
            {
                return this.Pause();
            }
        }
        
        public async Task Pause()
        {
            if (adapter.IsScanning)
            { 
                await adapter.StopScanningForDevicesAsync();
            }
        }

        public async Task Resume()
        {
            if (ble.State == BluetoothState.On)
            {
                if (!adapter.IsScanning)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    await adapter.StartScanningForDevicesAsync(new[]
                    {
                        Guid.Parse("a18ba38e-c3d7-4df8-a6c2-906dfd6a605b"), 
                        Guid.Parse("ef19c991-0aaa-40c0-82cd-3602722a9cdb")
                    }, _cancellationTokenSource.Token);
                }
            }            
        }
    }
}
