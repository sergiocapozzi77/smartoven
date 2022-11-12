using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SmartOvenV2.Managers;
using SmartOvenV2.Models;
using SmartOvenV2.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartOvenV2.ViewModels
{
    class OvenViewModel : BaseViewModel
    {

        private double topMaxPower;
        private double bottomMaxPower;
        public ICommand TopMinusPowerCommand { get; }
        public ICommand TopPlusPowerCommand { get; }

        public ICommand BottomMinusPowerCommand { get; }
        public ICommand BottomPlusPowerCommand { get; }

        private bool isOvenOn;

        public OvenViewModel(IBleConnector dataService, IStatusPoller statusPoller) : base(statusPoller, dataService)
        {
            DeviceDisplay.KeepScreenOn = true;
            TopMinusPowerCommand = new Command(() => {
                TopMaxPower -= 25;
                if (TopMaxPower < 0)
                {
                    TopMaxPower = 0;
                }

                this.statusPoller.IgnoreElementsStatusUpdates = 2;


                this.dataService.SetTopMaxPower(TopMaxPower);
            });

            TopPlusPowerCommand = new Command(() => {
                TopMaxPower += 25;
                if (TopMaxPower > 100)
                {
                    TopMaxPower = 100;
                }
                this.statusPoller.IgnoreElementsStatusUpdates = 2;


                this.dataService.SetTopMaxPower(TopMaxPower);
            });

            BottomMinusPowerCommand = new Command(() => {
                BottomMaxPower -= 25;
                if (BottomMaxPower < 0)
                {
                    BottomMaxPower = 0;
                }
                this.statusPoller.IgnoreElementsStatusUpdates = 2;


                this.dataService.SetBottomMaxPower(BottomMaxPower);
            });

            BottomPlusPowerCommand = new Command(() => {
                BottomMaxPower += 25;
                if (BottomMaxPower > 100)
                {
                    BottomMaxPower = 100;
                }
                this.statusPoller.IgnoreElementsStatusUpdates = 2;


                this.dataService.SetBottomMaxPower(BottomMaxPower);
            });

            statusPoller.Start();
        }

        internal void TopDesiredTemperatureChanging(double temperature)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.SetTopElementInfo(temperature);
            });
        }

        internal void BottomDesiredTemperatureChanging(double temperature)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.SetBottomElementInfo(temperature);
            });
        }

        internal void SetTopDesiredTemperature(double temperature)
        {
            try
            {
                /*  Device.BeginInvokeOnMainThread(() =>
                  {
                      this.SetTopElementInfo(temperature);
                  });*/
                this.statusPoller.IgnoreElementsStatusUpdates = 2;


                this.dataService.SetTopTemperature(temperature);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to send command " + ex.Message);
            }
        }

        protected override void OnElementsStatusUpdated(ElementsStatusInfo status)
        {
            base.OnElementsStatusUpdated(status);


            if (this.statusPoller.IgnoreElementsStatusUpdates > 0)
            {
                return;
            }

            this.topMaxPower = status.TopMaxPower;
            this.bottomMaxPower = status.BottomMaxPower;

            this.OnPropertyChanged(nameof(TopMaxPower));
            this.OnPropertyChanged(nameof(BottomMaxPower));
        }

        internal void SetBottomDesiredTemperature(double temperature)
        {
            try
            {
                /*  Device.BeginInvokeOnMainThread(() =>
                  {
                      this.SetBottomElementInfo(temperature);
                  });*/
                this.statusPoller.IgnoreElementsStatusUpdates = 2;


                this.dataService.SetBottomTemperature(temperature);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to send command " + ex.Message);
            }
        }

        public double TopMaxPower
        {
            get => this.topMaxPower;
            set
            {
                this.SetProperty(ref this.topMaxPower, value);
            }
        }

        public double BottomMaxPower
        {
            get => this.bottomMaxPower;
            set
            {
                this.SetProperty(ref this.bottomMaxPower, value);
            }
        }

        public bool IsOvenOn
        {
            get => this.isOvenOn;
            set
            {
                if (this.SetProperty(ref this.isOvenOn, value))
                {
                    ToggleOven();
                }
            }
        }


        private bool isLightOn;

        public bool IsLightOn
        {
            get => this.isLightOn;
            set
            {
                if (this.SetProperty(ref this.isLightOn, value))
                {
                    ToggleLight();
                }
            }
        }

        private void ToggleLight()
        {
            if (this.IsLightOn)
            {
                this.dataService.LightOn();
            }
            else
            {
                this.dataService.LightOff();
            }
        }

        protected override void OnOvenStatusUpdated(OvenStatus ovenStatus)
        {
            base.OnOvenStatusUpdated(ovenStatus);

            if (this.statusPoller.IgnoreOvenStatusUpdates > 0)
            {
                return;
            }

            this.isOvenOn = ovenStatus.Status == 1;
            OnPropertyChanged(nameof(this.IsOvenOn));
        }

        void ToggleOven()
        {
            try
            {
                this.statusPoller.IgnoreOvenStatusUpdates = 2;
                this.statusPoller.IgnoreElementsStatusUpdates = 2;
                if (this.IsOvenOn)
                {
                    this.dataService.SwitchOn();
                    this.dataService.SetTopMaxPower(100);
                    this.dataService.SetBottomMaxPower(100);
                    this.dataService.SetTopTemperature(this.TopDesiredTemperature);
                    this.dataService.SetBottomTemperature(this.BottomDesiredTemperature);
                }
                else
                {
                    this.dataService.SwitchOff();
                }

                this.OvenStatus.Status = this.IsOvenOn ? 1 : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to send command " + ex.Message);
            }
        }
    }
}
