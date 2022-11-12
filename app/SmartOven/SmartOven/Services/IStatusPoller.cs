using SmartOvenV2.Models;
using System;
using System.Threading.Tasks;

namespace SmartOvenV2.Services
{
    interface IStatusPoller
    {
        event EventHandler<Exception> OnError;
        event EventHandler<ElementsStatusInfo> OnElementsStatusUpdate;
        event EventHandler<CpuStatusInfo> OnCpuStatusUpdate;
        event EventHandler<OvenStatus> OnOvenStatusUpdate;
        event EventHandler OnConnected;
        event EventHandler OnDisconnected;
        int IgnoreOvenStatusUpdates { get; set; }
        int IgnoreElementsStatusUpdates { get; set; }

        void Pause();
        void Resume();
        void Start();
    }
}