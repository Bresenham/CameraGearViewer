using CameraGearViewer.Classes;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
            var web = new HtmlWeb();
            web.AutoDetectEncoding = false;
            web.OverrideEncoding = Encoding.GetEncoding("UTF-8");
            var htmlDoc = web.Load("https://www.dslr-forum.de/forumdisplay.php?f=109");
            htmlDoc.DocumentNode.Descendants("a").Where(a => Regex.IsMatch(a.GetAttributeValue("id", "false"), "thread_title_[0-9]+")).ToList().ForEach(async link =>
            {
                var isValidOffering = link.ParentNode.Descendants("font").First().Descendants("strong").First().InnerText.Equals("[Biete]");
                if (isValidOffering)
                {
                    var gearComponent = WebpageDeserializer.Deserialize("https://www.dslr-forum.de/" + link.GetAttributeValue("href", "false").Replace("&amp;", "&"));
                    var httpContent = new StringContent(JsonConvert.SerializeObject(gearComponent), Encoding.UTF8, "application/json");
                    var result = client.PostAsync(requestUrl, httpContent);
                    await Task.Delay(2000);
                }
            });
        }
    }
}
