//using SmartOvenV2.Models;
//using System;
//using System.Threading.Tasks;
//using Xamarin.Forms;

//namespace SmartOvenV2.Services
//{
//    class BluetoothDataService : IDataService
//    {
//        private const string DeviceName = "SmartOvenV2";
//        private IBluetoothHelper bluetoothHelper;

//        public BluetoothDataService()
//        {
//            bluetoothHelper = DependencyService.Get<IBluetoothHelper>();
//        }

//        public event EventHandler<double> BottomDesiredTemperatureChanged;

//        public event EventHandler<double> TopDesiredTemperatureChanged;

//        public Task<StatusInfo> GetInfoAsync()
//        {
//            return GenericCommand(() => this.bluetoothHelper.GetInfo());
//        }

//        public Task<StatusInfo> SwitchOn()
//        {
//            return GenericCommand(() => this.bluetoothHelper.SetOvenStatus(1));
//        }

//        public Task<StatusInfo> SetTopTemperature(double temperature)
//        {
//            this.TopDesiredTemperatureChanged?.Invoke(this, temperature);

//            return GenericCommand(() => this.bluetoothHelper.SetTopDesiredTemperature(temperature));
//        }

//        public Task<StatusInfo> SetBottomTemperature(double temperature)
//        {
//            this.BottomDesiredTemperatureChanged?.Invoke(this, temperature);

//            return GenericCommand(() => this.bluetoothHelper.SetBottomDesiredTemperature(temperature));
//        }

//        public Task<StatusInfo> SwitchOff()
//        {
//            return GenericCommand(() => this.bluetoothHelper.SetOvenStatus(0));
//        }

//        public Task<StatusInfo> GenericCommand(Func<StatusInfo> action)
//        {
//            return Task.Factory.StartNew(() =>
//            {
//                if (!bluetoothHelper.IsConnected)
//                {
//                    if (!bluetoothHelper.Connect(DeviceName))
//                    {
//                        return null;
//                    }
//                }

//                return action();
//            });
//        }

//        public Task<StatusInfo> GenericCommand(Func<double, StatusInfo> action, double temperature)
//        {
//            return Task.Factory.StartNew(() =>
//            {
//                if (!bluetoothHelper.IsConnected)
//                {
//                    if (!bluetoothHelper.Connect(DeviceName))
//                    {
//                        return null;
//                    }
//                }

//                return action(temperature);
//            });
//        }

//        public void Close()
//        {
//            this.bluetoothHelper.Close();
//        }

//        public void Boost()
//        {
//            SetTopTemperature(500.0);
//            SetBottomTemperature(500.0);
//        }

//        public void StopBoost(double top, double bottom)
//        {
//            SetTopTemperature(top);
//            SetBottomTemperature(bottom);
//        }
//    }

//}

