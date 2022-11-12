using SmartOvenV2.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace SmartOvenV2.Services
{
    class StatusPoller : IStatusPoller
    {
        public bool IsConnected { get; private set; }
        private readonly IBleConnector bleConnector;
        private readonly Timer cpuTemperatureTimer;

        public event EventHandler<ElementsStatusInfo> OnElementsStatusUpdate;
        public event EventHandler<CpuStatusInfo> OnCpuStatusUpdate;
        public event EventHandler<OvenStatus> OnOvenStatusUpdate;
        public event EventHandler<Exception> OnError;
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        public int IgnoreOvenStatusUpdates { get; set; }
        public int IgnoreElementsStatusUpdates { get; set; }
        readonly object lockTimerCpu = new object();
        readonly object lockTimerStatus = new object();
        readonly object lockTimerElements = new object();

        private readonly Timer ovenStatusTimer;
        private Timer elementsStatusTimer;

        public StatusPoller(IBleConnector bleConnector)
        {
            this.bleConnector = bleConnector;

            bleConnector.OnDisconnected += this.BleConnector_OnDisconnected;
            bleConnector.OnConnected += this.BleConnector_OnConnected;
            
            this.bleConnector.OnReadCpuTemperatures += BleDeviceOnOnReadCpuTemperatures;
            this.bleConnector.OnReadElementsStatusInfo += BleDeviceOnOnReadElementsStatusInfo;
            this.bleConnector.OnReadOvenStatus += BleDeviceOnOnReadOvenStatus;

            this.cpuTemperatureTimer = new Timer(CpuTemperaturesTimer);
            this.ovenStatusTimer = new Timer(OvenStatusTimer);
            this.elementsStatusTimer = new Timer(ElementsStatusTimer);

            this.Resume();
        }

        private void BleDeviceOnOnReadOvenStatus(object sender, OvenStatus e)
        {
            if (IgnoreOvenStatusUpdates > 0)
            {
                IgnoreOvenStatusUpdates--;
                return;
            }

            this.OnOvenStatusUpdate?.Invoke(this, e);
        }

        private void BleDeviceOnOnReadElementsStatusInfo(object sender, ElementsStatusInfo e)
        {
            if (IgnoreElementsStatusUpdates > 0)
            {
                IgnoreElementsStatusUpdates--;
                return;
            }

            OnElementsStatusUpdate?.Invoke(this, e);
        }

        private void BleDeviceOnOnReadCpuTemperatures(object sender, double[] e)
        {
            this.OnCpuStatusUpdate?.Invoke(this, CpuStatusInfo.FromDouble(e));
        }

        async Task<bool> CheckPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        UserDialogs.Instance.Alert("Dai all'applicazione i permessi per accedere alla locazione", "Need location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        private void BleConnector_OnDisconnected(object sender, EventArgs e)
        {
            this.IsConnected = false;
            this.OnDisconnected?.Invoke(this, EventArgs.Empty);
        }

        private void BleConnector_OnConnected(object sender, IDevice e)
        {
            this.IsConnected = true;
            this.OnConnected?.Invoke(this, EventArgs.Empty);
        }

        void ElementsStatusTimer(object state)
        {
            if (!this.IsConnected)
            {
                return;
            }

            var lockTaken = false;
            try
            {
                Monitor.TryEnter(lockTimerElements, 3000, ref lockTaken);
                if (lockTaken)
                {
                    try
                    {
                        this.bleConnector.ReadElementsStatus();
                    }
                    catch (Exception ex)
                    {
                        this.SendError(ex.GetBaseException());
                    }
                }
                else
                {
                    //this.bleDevice.Close(); //?
                }
            }
            finally
            {
                // Ensure that the lock is released.
                if (lockTaken)
                {
                    Monitor.Exit(lockTimerElements);
                }
            }
        }


        void CpuTemperaturesTimer(object state)
        {
            if (!this.IsConnected)
            {
                return;
            }

            var lockTaken = false;
            try
            {
                Monitor.TryEnter(lockTimerCpu, 3000, ref lockTaken);
                if (lockTaken)
                {
                    try
                    {
                        this.bleConnector.ReadCpuTemperatures();
                    }
                    catch (Exception ex)
                    {
                        this.SendError(ex.GetBaseException());
                    }
                }
                else
                {
                    //this.bleDevice.Close(); //?
                }
            }
            finally
            {
                // Ensure that the lock is released.
                if (lockTaken)
                {
                    Monitor.Exit(lockTimerCpu);
                }
            }
        }

        void OvenStatusTimer(object state)
        {
            if (!this.IsConnected)
            {
                return;
            }

            var lockTaken = false;
            try
            {
                Monitor.TryEnter(lockTimerStatus, 3000, ref lockTaken);
                if (lockTaken)
                {
                    try
                    {
                        this.bleConnector.ReadOvenStatus();
                    }
                    catch (Exception ex)
                    {
                        this.SendError(ex.GetBaseException());
                    }
                }
                else
                {
                    //this.bleDevice.Close(); //?
                }
            }
            finally
            {
                // Ensure that the lock is released.
                if (lockTaken)
                {
                    Monitor.Exit(lockTimerStatus);
                }
            }
        }

        private void SendError(Exception exception)
        {
            this.OnError?.Invoke(this, exception);
        }

        public void Pause()
        {
            this.cpuTemperatureTimer.Change(Timeout.Infinite, 0);
            this.ovenStatusTimer.Change(Timeout.Infinite, 0);
            this.elementsStatusTimer.Change(Timeout.Infinite, 0);
        }

        public void Resume()
        {
            this.cpuTemperatureTimer.Change(0, 1000);
            this.ovenStatusTimer.Change(0, 1000);
            this.elementsStatusTimer.Change(0, 1000);
            bleConnector.Resume();
        }

        public void Start()
        {
            CheckPermission().ContinueWith((t) =>
            {
                if (t.Result)
                {
                    bleConnector.Resume();
                }
                else
                {
                    UserDialogs.Instance.Alert("Per funzionare la app ha bisogno dell'accesso alla locazione", "Need location", "OK");
                }
            });
        }
    }
}
