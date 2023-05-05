using Acr.UserDialogs.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartOvenV2.Models;
using Syncfusion.SfRangeSlider.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SmartOvenV2.Services
{
    internal class SeatableDataService : ISeatableDataService
    {
        private HttpClient client;
        public string access_token;
        string base_uuid;

        public SeatableDataService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
//            client.DefaultRequestHeaders.Add("content-type", "application/json");
            client.DefaultRequestHeaders.Add("authorization", "Bearer ea6fa8dd468f479db3e2528d55423b9d27c51622");
        }

        async Task GetBaseToken()
        {
            if(!string.IsNullOrEmpty(this.access_token) && !string.IsNullOrEmpty(this.base_uuid))
            {
                return;
            }

            var response = await client.GetAsync("https://cloud.seatable.io/api/v2.1/dtable/app-access-token/");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(content);

                Console.WriteLine("GetBaseToken: " + content.ToString());
                this.access_token = data.Value<string>("access_token");
                this.base_uuid = data.Value<string>("dtable_uuid");
                client.DefaultRequestHeaders.Remove("authorization");
                client.DefaultRequestHeaders.Add("authorization", $"Bearer {this.access_token}");
            }
        }

        public async Task<JArray> GetData(string table)
        {
            await GetBaseToken();

            var response = await client.GetAsync($"https://cloud.seatable.io/dtable-server/api/v1/dtables/{this.base_uuid}/rows/?table_name={table}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(content);
                Console.WriteLine("GetData: " + obj.ToString());
                return obj.Value<JArray>("rows");
            }
            else
            {
                return new JArray();
            }
        }

        public async Task<JArray> GetDataSql(string table, string[] ids)
        {
            if(ids == null || ids.Length == 0)
            {
                return new JArray();
            }

            await GetBaseToken();

            
            var sql = new JObject();
            sql.Add("sql", $"SELECT * FROM {table} WHERE _id in ({String.Join(",", ids.Select(x => $"\"{x}\""))})");
            sql.Add("convert_keys", true);

            var requestContent = new StringContent(sql.ToString(), Encoding.UTF8, "application/json");

            Console.WriteLine("GetDataSql request: " + sql.ToString());
            var response = await client.PostAsync($"https://cloud.seatable.io/dtable-db/api/v1/query/{this.base_uuid}", requestContent);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(content);
                Console.WriteLine("GetDataSql: " + obj.ToString());
                if (obj.Value<bool>("success"))
                {
                    return obj.Value<JArray>("results");
                } else
                {
                    Console.WriteLine("GetDataSql error: " + obj.Value<string>("error_message"));
                    return new JArray();
                }
            }
            else
            {
                return new JArray();
            }
        }

        public async Task<string> GetImageLink(string image)
        {
            await GetBaseToken();
            
            var imagePath = "/images" + image.Split(new string[] { "/images" }, StringSplitOptions.RemoveEmptyEntries)[1];
            var url = $"https://cloud.seatable.io/api/v2.1/dtable/app-download-link/?path={imagePath}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(content);
                Console.WriteLine("Download link " + url);
                return obj.Value<string>("download_link");
            }
            else
            {
                Console.WriteLine("Failed to retrieve image link from " + url);
                return "";
            }
        }
    }
}
