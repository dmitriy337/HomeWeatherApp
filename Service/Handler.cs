using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;

namespace HomeWeatherApp.Service
{
    class Handler
    {
        public delegate void OnNewRequest(Dictionary<string, string> valuePairs);
        public OnNewRequest onNewRequest;

        public delegate void OnException(Exception exception);
        public OnException onException;

        private WebClient client;

        public Handler() { this.client = new WebClient(); }

        public void StartPooling(int Timeout, string url) 
        {
            Task.Run(() => { RecieveAsync(Timeout: Timeout, url:url); });
        }

        private async void RecieveAsync(int Timeout, string url)
        {
            while (true)
            {
                try
                {
                    //Example of responce {"success":true,"temp":22.3,"humidity":67.1}
                    string result = client.DownloadString(url);
                    JsonDocument doc = JsonDocument.Parse(result);

                    string Success = doc.RootElement.GetProperty("success").ToString();

                    if (bool.Parse(Success)!=true){
                        this.onException?.Invoke(new Exception(message:"Success is not true!"));
                        System.Threading.Thread.Sleep(Timeout * 1000);
                        continue;}

                    string Temp = doc.RootElement.GetProperty("temp").ToString();
                    string Humidity = doc.RootElement.GetProperty("humidity").ToString();

                    this.onNewRequest?.Invoke(
                            new Dictionary<string, string>{
                                 { "success", Success },
                                 { "temp", Temp },
                                 {"humidity", Humidity }});
                }
                catch (Exception e)
                {
                    this.onException?.Invoke(e);
                }
                System.Threading.Thread.Sleep(Timeout*1000);
            }
        }


    }
}
