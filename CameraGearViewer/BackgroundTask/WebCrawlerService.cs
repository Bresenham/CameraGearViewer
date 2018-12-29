using CameraGearViewer.Classes;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private readonly string dslrSonyForum = "https://www.dslr-forum.de/forumdisplay.php?f=109&page=";

        public WebCrawlerService() { }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var web = new HtmlWeb
            {
                OverrideEncoding = CodePagesEncodingProvider.Instance.GetEncoding(1252)
            };
            while (true)
            {
                for (var i = 1; i <= 5; i++)
                {
                    var htmlDoc = web.Load(dslrSonyForum + i.ToString());
                    var nextLinks = htmlDoc.DocumentNode.Descendants("a").Where(a => Regex.IsMatch(a.GetAttributeValue("id", "false"), "thread_title_[0-9]+")).ToList();
                    nextLinks.ForEach(async link =>
                    {
                        var isValidOffering = link.ParentNode.Descendants("font").First().Descendants("strong").First().InnerText.Equals("[Biete]");
                        var currentLink = "https://www.dslr-forum.de/" + link.GetAttributeValue("href", "false").Replace("&amp;", "&");
                        if (isValidOffering)
                        {
                            var gearComponent = WebpageDeserializer.Deserialize(currentLink);
                            var httpContent = new StringContent(JsonConvert.SerializeObject(gearComponent), Encoding.UTF8, "application/json");
                            var result = client.PostAsync(requestUrl, httpContent);
                            while (!result.IsCompleted) ;
                            if (result.Result.StatusCode == HttpStatusCode.BadRequest)
                                Console.WriteLine("Something went wrong.");
                            await Task.Delay(2 * 1000);
                        }
                        else
                        {
                        /* TODO: Remove offers which became invalid from database */
                            Console.WriteLine("Didn't accept.");
                        }
                    });
                }

                await Task.Delay(60 * 1000);
            }
        }
    }
}
