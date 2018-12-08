using CameraGearViewer.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CameraGearViewer.BackgroundTask
{
    public class WebCrawlerService : BackgroundService
    {

        private readonly HttpClient client = new HttpClient();

        private readonly string requestUrl = "https://localhost:44377/api/controller/";

        public WebCrawlerService() { }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(new GearComponent("Name", "Description", "08.12.2018")), Encoding.UTF8, "application/json");
                var result = client.PostAsync(requestUrl, httpContent);
                await Task.Delay(2000);
            }
        }
    }
}
