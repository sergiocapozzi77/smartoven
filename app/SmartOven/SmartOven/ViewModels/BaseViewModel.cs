using SmartOvenV2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using SmartOvenV2.Models;
using Xamarin.Forms;

namespace SmartOvenV2.ViewModels
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        private bool isConnected;
        private bool topElementOn;
        private bool bottomElementOn;
        private double topTemperature;
        private double topDesiredTemperature;

        private double bottomTemperature;
        private double bottomDesiredTemperature;
        private string bottomElementInfo;
        private string topElementInfo;
        protected readonly IStatusPoller statusPoller;
        protected IBleConnector dataService;
        protected bool neverInitialised = true;
        public string TopElementInfo
        {
            get => this.topElementInfo;
            set => this.SetProperty(ref this.topElementInfo, value);
        }

        public string BottomElementInfo
        {
            get => this.bottomElementInfo;
            set => this.SetProperty(ref this.bottomElementInfo, value);
        }

        public double TopTemperature
        {
            get => this.topTemperature;
            set => this.SetProperty(ref this.topTemperature, value);
        }

        public bool TopElementOn
        {
            get => this.topElementOn;
            set => this.SetProperty(ref this.topElementOn, value);
        }

        public bool BottomElementOn
        {
            get => this.bottomElementOn;
            set => this.SetProperty(ref this.bottomElementOn, value);
        }

        public double TopDesiredTemperature
        {
            get => this.topDesiredTemperature;
            set => this.SetProperty(ref this.topDesiredTemperature, value);
        }

        public double TopPower
        {
            get => this.topPower;
            set => this.SetProperty(ref this.topPower, value);
        }

        public double BottomPower
        {
            get => this.bottomPower;
            set => this.SetProperty(ref this.bottomPower, value);
        }

        public double BottomDesiredTemperature
        {
            get => this.bottomDesiredTemperature;
            set => this.SetProperty(ref this.bottomDesiredTemperature, value);
        }

        public double BottomTemperature
        {
            get => this.bottomTemperature;
            set => this.SetProperty(ref this.bottomTemperature, value);
        }

        public bool IsConnected
        {
            get => this.isConnected;
            set => this.SetProperty(ref this.isConnected, value);
        }

        public OvenStatus OvenStatus
        {
            get => this.ovenStatus;
            set => this.SetProperty(ref this.ovenStatus, value);
        }

        string title = string.Empty;
        private OvenStatus ovenStatus;
        private double topPower;
        private double bottomPower;

        internal BaseViewModel(IStatusPoller statusPoller, IBleConnector dataService)
        {
            this.statusPoller = statusPoller;
            this.dataService = dataService;

            this.ovenStatus = new OvenStatus();
            
            statusPoller.OnElementsStatusUpdate += this.StatusPollerOnStatusUpdate;
            statusPoller.OnCpuStatusUpdate += this.StatusPollerOnCpuStatusUpdate;
            statusPoller.OnOvenStatusUpdate += this.StatusPollerOnOvenStatusUpdate;
            statusPoller.OnError += this.StatusPollerOnError;
            statusPoller.OnConnected += this.StatusPollerOnConnected;
            statusPoller.OnDisconnected += this.StatusPollerOnDisconnected;

            dataService.TopDesiredTemperatureChanged += (sender, temperature) =>
            {
                this.TopDesiredTemperature = temperature;
                this.SetTopElementInfo(this.TopDesiredTemperature);
            };
            dataService.BottomDesiredTemperatureChanged += (sender, temperature) =>
            {
                this.BottomDesiredTemperature = temperature;
                this.SetBottomElementInfo(this.BottomDesiredTemperature);
            };

            SetTopElementInfo(0);
            SetBottomElementInfo(0);
//#if DEBUG
//            this.IsConnected = true;
//#endif
        }

        private void StatusPollerOnOvenStatusUpdate(object sender, OvenStatus status)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.OnOvenStatusUpdated(status);
            });
        }

        protected virtual void OnOvenStatusUpdated(OvenStatus status)
        {
            this.OvenStatus = status;
        }

        private void StatusPollerOnDisconnected(object sender, EventArgs e)
        {
            this.IsConnected = false;
        }

        private void StatusPollerOnConnected(object sender, EventArgs e)
        {
            this.IsConnected = true;
            neverInitialised = true;
        }

        private void StatusPollerOnError(object sender, Exception e)
        {
#if !DEBUG
            this.IsConnected = false;
#endif

            Device.BeginInvokeOnMainThread(() =>
            {
                UserDialogs.Instance.Alert("Error", e.Message, "OK");
            });
        }

        private void StatusPollerOnStatusUpdate(object sender, Models.ElementsStatusInfo status)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.OnElementsStatusUpdated(status);
            });
        }

        private void StatusPollerOnCpuStatusUpdate(object sender, Models.CpuStatusInfo status)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.OnCpuStatusUpdated(status);
            });
        }

        protected virtual void OnCpuStatusUpdated(Models.CpuStatusInfo status)
        {
            
        }


        protected virtual void OnElementsStatusUpdated(Models.ElementsStatusInfo status)
        {
            if (neverInitialised)
            {
                this.TopDesiredTemperature = status.TopDesiredTemperature;
                this.BottomDesiredTemperature = status.BottomDesiredTemperature;
                neverInitialised = false;
            }

            this.TopTemperature = status.TopTemperature;
            this.BottomTemperature = status.BottomTemperature;
            this.TopPower = status.TopPower;
            this.BottomPower = status.BottomPower;
            this.TopElementOn = status.TopElementStatus == 1;
            this.BottomElementOn = status.BottomElementStatus == 1;
            this.SetTopElementInfo(this.TopDesiredTemperature);
            this.SetBottomElementInfo(this.BottomDesiredTemperature);
        }

        protected void SetTopElementInfo(double desiredTemp)
        {
            TopElementInfo = $"{(int)TopTemperature}° / {(int)desiredTemp}°";
        }

        protected void SetBottomElementInfo(double desiredTemp)
        {
            BottomElementInfo = $"{(int)BottomTemperature}° / {(int)desiredTemp}°";
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

