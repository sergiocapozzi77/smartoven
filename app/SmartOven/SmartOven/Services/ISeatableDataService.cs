using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace SmartOvenV2.Services
{
    internal interface ISeatableDataService
    {
        Task<JArray> GetData(string table);
        Task<string> GetImageLink(string image);

        Task<JArray> GetDataSql(string table, string[] ids);
    }
}