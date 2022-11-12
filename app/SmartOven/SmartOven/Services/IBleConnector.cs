using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Threading.Tasks;

namespace SmartOvenV2.Services
{
    public interface IBleConnector: IBleCommands
    {

        event EventHandler OnDisconnected;
        event EventHandler<IDevice> OnConnected;

        Task Pause();
        Task Resume();
    }
}