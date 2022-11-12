using Plugin.BLE.Abstractions.Contracts;
using System.Threading.Tasks;

namespace SmartOvenV2.Services
{
    public interface IBleDevice : IBleCommands
    {
        Task Initialise(IDevice device);
        void Close();
    }
}