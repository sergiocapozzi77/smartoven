using SmartOvenV2.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartOvenV2.Services
{
    class FakeStatusPoller : IStatusPoller
    {
        public bool IsConnected { get; private set; }
        private readonly IBleDevice dataService;
        private Timer timer;

        public event EventHandler<ElementsStatusInfo> OnElementsStatusUpdate;
        public event EventHandler<CpuStatusInfo> OnCpuStatusUpdate;
        public event EventHandler<OvenStatus> OnOvenStatusUpdate;
        public event EventHandler<Exception> OnError;
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        public int IgnoreOvenStatusUpdates { get; set; }
        public int IgnoreElementsStatusUpdates { get; set; }
        Random rand = new Random();

        public FakeStatusPoller()
        {
            this.timer = new Timer(new TimerCallback(TimerTick));
            this.Resume();
        }

        void TimerTick(object state)
        {
            var s = new ElementsStatusInfo();

            s.BottomDesiredTemperature = 200;
            s.BottomTemperature += rand.NextDouble() * 100;
            s.TopDesiredTemperature = 300;
            s.TopTemperature += rand.NextDouble() * 100;
            s.BottomElementStatus = 1;
            s.TopElementStatus = 1;

            OnElementsStatusUpdate?.Invoke(this, s);
        }

        private void SendError(Exception exception)
        {
            this.IsConnected = false;
            this.OnError?.Invoke(this, exception); 
        }

        public void Pause()
        {
            this.timer.Change(Timeout.Infinite, 0);
        }

        public void Resume()
        {
            this.timer.Change(0, 1000);
        }

        public void Start()
        {
            
        }
    }
}
