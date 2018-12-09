using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CameraGearViewer.Classes
{
    public class WebpageDeserializer
    {
        private static readonly Dictionary<string, Func<string, string>> regexDict = new Dictionary<string, Func<string, string>>() {
            {"([0-9]+(,|\\.))?[0-9]+(\\s|)€", str => str.Replace("€", "").Trim()},
            {"([0-9]+(,|\\.))?[0-9]+(\\s|)euro", str => str.Replace("euro", "").Trim()},
            {"([0-9]+(,|\\.))?[0-9]+(\\s|)eur", str => str.Replace("eur", "").Trim()},
            {"([0-9]+(,|\\.))?[0-9]+(\\s|)\\.-(\\s|)€", str => str.Replace(".-", "").Replace("€", "").Trim()},
            {"([0-9]+(,|\\.))?[0-9]+(\\s|),-(\\s|)€", str => str.Replace(",-", "").Replace("€", "").Trim()},
            {"([0-9]+)(,[0-9]+)?\\.-(\\s|)euro", str => str.Replace(".-", "").Replace("euro", "").Trim()},
            {"([0-9]+)(,[0-9]+)?,--(\\s|)eur", str => str.Replace(",--", "").Replace("eur", "").Trim()},
            {"eur(\\s|)([0-9]+)(,[0-9]+)?,-", str => str.Replace("eur", "").Replace(",-", "").Trim()},
            {"€(;)?(\\s|)([0-9])(\\.|)?([0-9]+)((,|\\.)[0-9]+)?(,-)?", str => str.Replace("€", "").Replace(";", "").Replace(",-", "").Trim()},
            {"€(\\s|)([0-9]+)(,[0-9]+)?", str => str.Replace("€", "").Trim()},
            {"([0-9]+(,|\\.))?[0-9]+(\\s|),-(\\s|)eur", str => str.Replace(",-", "").Replace("eur", "").Trim()},
            {"([0-9])(\\.|)?([0-9]+)((,|\\.)[0-9]+)?\\.-", str => str.Replace(".-", "").Replace(".", "").Trim()}
        };

        private WebpageDeserializer() { }

        private static string getLongestMatchingRegex(List<string> regexList, string input)
        {
            var maxLen = -1;
            var maxRegex = "";
            foreach(string rx in regexList)
            {
                var match = Regex.Match(input, rx).Value;
                if(match.Length > maxLen)
                {
                    maxLen = match.Length;
                    maxRegex = rx;
                }
            }

            return maxRegex;
        }

        public static GearComponent Deserialize(string address)
        {
            var web = new HtmlWeb();
            web.OverrideEncoding = CodePagesEncodingProvider.Instance.GetEncoding(1252);
            var htmlDoc = web.Load(address);
            try
            {

                var headerDiv = htmlDoc.DocumentNode.Descendants("div").Where(d => d.GetAttributeValue("class", "false").Equals("smallfont nomatch")).First();
                var strongHeader = headerDiv.Descendants("strong").First();
                var headerText = strongHeader.InnerText;

                var createdDate = htmlDoc.DocumentNode.Descendants("td").Where(td => td.GetAttributeValue("style", "false").Equals("font-weight:normal; border: 1px solid #D1D1E1; border-right: 0px")).First();

                var dateString = createdDate.InnerText.Trim().Replace("Heute", DateTime.Now.ToShortDateString()).Replace("Gestern", DateTime.Now.AddDays(-1).ToShortDateString());

                var postContent = htmlDoc.DocumentNode.Descendants("div").Where(d => Regex.IsMatch(d.GetAttributeValue("id", "false"), "post_message_[0-9]+")).First().InnerText;
                postContent = postContent.Replace("&#8364", "€").ToLower();

                var price = 0.0;

                var regexMatches = regexDict.Keys.Where(regex => Regex.IsMatch(postContent, regex)).ToList();
                if (regexMatches.Count > 0)
                {
                    var longestMatchingRegex = getLongestMatchingRegex(regexMatches, postContent);
                    regexDict.TryGetValue(longestMatchingRegex, out Func<string, string> strReplaceFunc);
                    try
                    {
                        price = Convert.ToDouble(strReplaceFunc.Invoke(Regex.Match(postContent, longestMatchingRegex).Value));
                    } catch(Exception e)
                    {
                        Console.WriteLine("Parsing price failed.");
                    }
                }
                else
                    Console.WriteLine("Couldn't fetch price.");

                return new GearComponent(headerText, price, address, DateTime.ParseExact(dateString, "dd.MM.yyyy, HH:mm", CultureInfo.GetCultureInfo("de-DE")));

            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
