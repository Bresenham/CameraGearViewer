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

        private static readonly List<string> priceRegexMatchers = new[]
        {
            "([0-9]+(,|\\.))?[0-9]+(\\s|)€",
            "([0-9]+(,|\\.))?[0-9]+(\\s|)euro",
            "([0-9]+(,|\\.))?[0-9]+(\\s|)eur",
            "([0-9]+(,|\\.))?[0-9]+(\\s|).-(\\s|)€",
            "([0-9]+(,|\\.))?[0-9]+(\\s|),-(\\s|)€",
            "eur(\\s|)([0-9]+)(,[0-9]+)?,-",
            "([0-9]+(,|\\.))?[0-9]+(\\s|),-(\\s|)eur"
        }.ToList();

        public WebpageDeserializer() {}

        public static GearComponent Deserialize(string address)
        {
            var web = new HtmlWeb();
            web.OverrideEncoding = Encoding.Unicode;
            var htmlDoc = web.Load(address);
            try
            {

                var headerDiv = htmlDoc.DocumentNode.Descendants("div").Where(d => d.GetAttributeValue("class", "false").Equals("smallfont nomatch")).First();
                var strongHeader = headerDiv.Descendants("strong").First();
                var headerText = strongHeader.InnerText;

                var createdDate = htmlDoc.DocumentNode.Descendants("td").Where(td => td.GetAttributeValue("style", "false").Equals("font-weight:normal; border: 1px solid #D1D1E1; border-right: 0px")).First();

                var dateString = createdDate.InnerText.Trim().Replace("Heute", DateTime.Now.ToShortDateString());

                var postContent = htmlDoc.DocumentNode.Descendants("div").Where(d => Regex.IsMatch(d.GetAttributeValue("id", "false"), "post_message_[0-9]+")).First().InnerText;
                postContent = postContent.Replace("&#8364;", "€").Replace("�", "€");

                var price = 0.0;

                var regexMatches = priceRegexMatchers.Where(regex => Regex.Match(postContent.ToLower(), regex).Success).ToList();
                if (regexMatches.Count > 0)
                    price = Convert.ToDouble(Regex.Match(postContent, regexMatches.First()).Value.Replace("€", "").Replace("-", "").Replace("Euro", "").Replace("euro", "").Replace("eur", ""));
                else
                    Console.WriteLine("Couldn't fetch price.");

                return new GearComponent(headerText, price, address, dateString);

            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
