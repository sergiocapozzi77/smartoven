using SmartOvenV2.Models;
using SmartOvenV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using SmartOvenV2.Managers;
using Syncfusion.SfChart.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartOvenV2.ViewModels
{
    internal class InfoViewModel : BaseViewModel
    {
        private readonly IAppStatusManager appStatusManager;
        private readonly IOtaService otaService;
        private double cpuTemperature;
        private string timerValue;
        private CancellationTokenSource cancellation;
        private int? firmware;
        private int latestFirmwareVersion;
        private bool newFirmwareAvailable;
        private string updateProgress;
        private bool isUpdating;
        private IProgressDialog progressDialog;
        private string newFirmwareAvailableInfo;

        public ObservableCollection<ChartDataPoint> TopTemperatures { get; set; }
        public ObservableCollection<ChartDataPoint> BottomTemperatures { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public string TimerValue
        {
            get => timerValue;
            set
            {
                this.timerValue = value;
                this.OnPropertyChanged();
            }
        }

        public int? Firmware
        {
            get => this.firmware;
            set => this.SetProperty(ref this.firmware, value);
        }

        public bool IsUpdating
        {
            get => this.isUpdating;
            set => this.SetProperty(ref this.isUpdating, value);
        }

        public bool NewFirmwareAvailable
        {
            get => this.newFirmwareAvailable;
            set => this.SetProperty(ref this.newFirmwareAvailable, value);
        }

        public string NewFirmwareAvailableInfo
        {
            get => this.newFirmwareAvailableInfo;
            set => this.SetProperty(ref this.newFirmwareAvailableInfo, value);
        }

        public InfoViewModel(IStatusPoller statusPoller, IBleConnector dataService, IAppStatusManager appStatusManager, IOtaService otaService) : base(statusPoller, dataService)
        {
            this.appStatusManager = appStatusManager;
            this.otaService = otaService;

            TopTemperatures = new ObservableCollection<ChartDataPoint>();
            BottomTemperatures = new ObservableCollection<ChartDataPoint>();

            TimerValue = TimeSpan.FromSeconds(0).ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);

            ResetCommand = new Command(ResetTimer);
            UpdateCommand = new Command(UpdateFirmware);

            this.otaService.OnProgressUpdate += OtaService_OnProgressUpdate;
            this.otaService.OnUpdateEnd += OtaService_OnUpdateEnd;
            progressDialog = UserDialogs.Instance.Progress(null, null, null, false);

            statusPoller.OnConnected += StatusPoller_OnConnected;
            dataService.OnFirmwareVersion += (sender, version) =>
            {
                Firmware = version.Firmware;
                CheckUpdate(this.Firmware, this.latestFirmwareVersion);
            };
        }

        private void OtaService_OnUpdateEnd(object sender, bool success)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                this.IsUpdating = false;
                progressDialog.Hide();
                UserDialogs.Instance.Toast(success ? "Update success" : "Update failed");
            });
        }

        private void OtaService_OnProgressUpdate(object sender, ProgressUpdate e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                progressDialog.Title = e.Info;
                progressDialog.PercentComplete = (int)e.Progress;
            });
        }

        private async void StatusPoller_OnConnected(object sender, EventArgs e)
        {
            dataService.ReadFirmwareVersion();

            latestFirmwareVersion = await otaService.GetLatestAvailableVersion();

            CheckUpdate(this.Firmware, this.latestFirmwareVersion);
        }

        private void CheckUpdate(int? deviceFirmware, int latestFirmware)
        {
            if (deviceFirmware.HasValue && deviceFirmware < latestFirmware)
            {
                NewFirmwareAvailable = true;
                NewFirmwareAvailableInfo = $"New firmware available (ver: {latestFirmware})";
            } else
            {
                NewFirmwareAvailable = false;
                NewFirmwareAvailableInfo = "";
            }
        }

        private void UpdateFirmware()
        {
            this.progressDialog.Show();
            this.IsUpdating = true;
            this.otaService.UpdateFirmware();
        }

        private void ResetTimer()
        {
            this.appStatusManager.ResetOvenTimer();
            TopTemperatures.Clear();
            BottomTemperatures.Clear();
        }

        void StartTimer()
        {
            IsStarted = true;
            this.cancellation = new CancellationTokenSource();

            this.appStatusManager.ResetOvenTimerIfNotStarted();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.appStatusManager.UpdateOvenTimer();
                var format = this.appStatusManager.AppStatus.OvenTimer.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                TimerValue = format;

                return !this.cancellation.IsCancellationRequested;
            });
        }

        public bool IsStarted { get; set; }

        void StopTimer()
        {
            this.cancellation.Cancel();
            IsStarted = false;
        }

        protected override void OnCpuStatusUpdated(CpuStatusInfo status)
        {
            this.CpuTemperature = status.TopCpuTemperature;
        }

        protected override void OnElementsStatusUpdated(ElementsStatusInfo status)
        {
            if (this.OvenStatus?.Status == 1)
            {
                TopTemperatures.Add(new ChartDataPoint(DateTime.Now, status.TopTemperature));
                BottomTemperatures.Add(new ChartDataPoint(DateTime.Now, status.BottomTemperature));
            }
        }

        protected override void OnOvenStatusUpdated(OvenStatus ovenStatus)
        {
            base.OnOvenStatusUpdated(ovenStatus);

            if (!IsStarted && ovenStatus.Status == 1)
            {
                StartTimer();
            }

            if (IsStarted && ovenStatus.Status == 0)
            {
                StopTimer();
            }

            if (ovenStatus.Status == 0)
            {
                this.appStatusManager.RemoveOvenTimer();
            }
        }

        public double CpuTemperature
        {
            get => this.cpuTemperature;
            set => this.SetProperty(ref this.cpuTemperature, value);
        }
    }
}