using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CameraGearViewer.Classes
{
    public class WebpageDeserializer
    {
        public WebpageDeserializer() {}

        public static GearComponent Deserialize(string address)
        {
            var web = new HtmlWeb();
            web.AutoDetectEncoding = false;
            web.OverrideEncoding = Encoding.GetEncoding("UTF-8");
            var htmlDoc = web.Load(address);
            try
            {
                var headerDiv = htmlDoc.DocumentNode.Descendants("div").Where(d => d.GetAttributeValue("class", "false").Equals("smallfont nomatch")).First();
                var strongHeader = headerDiv.Descendants("strong").First();
                var headerText = strongHeader.InnerText;

                var createdDate = htmlDoc.DocumentNode.Descendants("td").Where(td => td.GetAttributeValue("style", "false").Equals("font-weight:normal; border: 1px solid #D1D1E1; border-right: 0px")).First();

                var dateString = createdDate.InnerText.Trim().Replace("Heute", DateTime.Now.ToShortDateString());

                var postContent = htmlDoc.DocumentNode.Descendants("div").Where(d => Regex.IsMatch(d.GetAttributeValue("id", "false"), "post_message_[0-9]+")).First();

                var price = Convert.ToDouble(Regex.Match(postContent.InnerText, "([0-9]+(,|\\.))?[0-9]+�").Value.Replace("�", "").Replace(",", "."));

                return new GearComponent(headerText, price, address, dateString);

            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
