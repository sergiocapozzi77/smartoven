using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SmartOvenV2.Models;

namespace SmartOvenV2.Services
{
    class OtaService : IOtaService
    {
        private readonly IBleConnector bleConnector;
        public event EventHandler<ProgressUpdate> OnProgressUpdate;
        public event EventHandler<bool> OnUpdateEnd;
        const string BaseUrl = "https://raw.githubusercontent.com/sergiocapozzi77/smartoven/main/";

        public OtaService(IBleConnector bleConnector)
        {
            this.bleConnector = bleConnector;
            bleConnector.OnOtaResult += BleConnectorOnOtaResult;
        }

        private void BleConnectorOnOtaResult(object sender, string e)
        {
            bool success = false;
            switch (e)
            {
                case "SU":
                    {
                        this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Update success, oven is restarting"));
                        success = true;
                        break;
                    }
                case "ERE":
                    {
                        this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Update failed: code 1"));
                        break;
                    }
                case "ERB":
                    {
                        this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Update failed: code 2"));
                        break;
                    }
                case "ERW":
                    {
                        this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Update failed: code 3"));
                        break;
                    }
                default:
                    {
                        this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Update failed: code 9"));
                        break;
                    }
            }

            bleConnector.StopOta();
            OnUpdateEnd?.Invoke(this, success);
        }

        public async Task<int> GetLatestAvailableVersion()
        {
            var stream = await DownloadFileAsync(BaseUrl + "version.txt");

            if (stream != null)
            {
                stream.Position = 0;
                var version = Encoding.UTF8.GetString(stream.ToArray());
                return int.Parse(version);
            }

            return 0;
        }


        public async Task<MemoryStream> GetFirmware()
        {
            var stream = await DownloadFileAsync(BaseUrl + "firmware.bin");

            if (stream != null)
            {
                stream.Position = 0;
                return stream;
            }

            return null;
        }

        public async Task UpdateFirmware()
        {
            // await bleConnector.SendOtaData(Encoding.ASCII.GetBytes("@START"));
            bleConnector.StartOta();
            this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Starting...", 0));

            try
            {
                this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Downloading the new firmware", 0));
                var firmware = await this.GetFirmware();
                if (firmware != null)
                {

                    var packetSize = 512;
                    var packetNumber = firmware.Length / packetSize;
                    var lastPacketSize = firmware.Length % packetSize;
                    var buffer = new byte[packetSize];
                    for (var i = 0; i < packetNumber; i++)
                    {
                        var progress = ((i + 1) / (double)packetNumber) * 100.0;
                        this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Updating", progress));
                        var read = await firmware.ReadAsync(buffer, 0, packetSize);
                        if (read != packetSize)
                        {
                            throw new Exception("OTA Failed");
                        }
                        var result = await bleConnector.SendOtaData(buffer);
                        if (!result)
                        {
                            throw new Exception("OTA Failed");
                        }
                    }

                    this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Updating", 100));
                    if (lastPacketSize > 0)
                    {
                        buffer = new byte[lastPacketSize];
                        var read = await firmware.ReadAsync(buffer, 0, (int)lastPacketSize);
                        if (read != lastPacketSize)
                        {
                            throw new Exception("OTA Failed");
                        }
                        var result = await bleConnector.SendOtaData(buffer);
                        if (!result)
                        {
                            throw new Exception("OTA Failed");
                        }
                    }

                    this.OnProgressUpdate?.Invoke(this, new ProgressUpdate("Finalizing", 100));
                    await bleConnector.SendOtaData(Encoding.ASCII.GetBytes("@END"));
                    OnUpdateEnd?.Invoke(this, true);
                }
                else
                {
                    OnUpdateEnd?.Invoke(this, false);
                }
            }
            catch
            {
                OnUpdateEnd?.Invoke(this, false);
            }
            finally
            {
                bleConnector.StopOta();
            }
        }

        private async Task<MemoryStream> DownloadFileAsync(string fileUrl)
        {
#if !DEBUG
            try
            {
                using (var client = new HttpClient())
                {
                    var downloadStream = await client.GetStreamAsync(fileUrl);

                    var memStream = new MemoryStream();
                    await downloadStream.CopyToAsync(memStream);
                    return memStream;
                }
            }
            catch (Exception ex)
            {
                //TODO handle exception
                return null;
            }
#else
            return null;
#endif
        }
    }
}
