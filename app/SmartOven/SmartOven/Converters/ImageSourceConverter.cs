using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace SmartOvenV2.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return null;

                Client.Headers.Add("authorization", $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE2ODI3OTM2MzIsImR0YWJsZV91dWlkIjoiNmVmNTcwY2MtOGEzOC00NmYzLWE5ZGMtOWY4MzM5N2ZiZWFmIiwidXNlcm5hbWUiOiIiLCJwZXJtaXNzaW9uIjoicnciLCJhcHBfbmFtZSI6InNtYXJ0b3ZlbiJ9.nD3ir7ActdCB_oz7fXfY0FxgceCbs3lHfzeT2-W5aHM");
                var byteArray = Client.DownloadString(value.ToString());
                Console.WriteLine("Downloaded image " + byteArray);
                return null;
                //return ImageSource.FromStream(() => new MemoryStream(byteArray));
            } catch(Exception ex) {
                Console.WriteLine("Unable to download image: " + ex.Message, ex);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
