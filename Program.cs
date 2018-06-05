using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using WebScrapper.Builders;
using WebScrapper.Data;
using WebScrapper.Workers;
using static System.Console;

namespace WebScrapper
{
    class Program
    {
        private const string Method = "search";
        static void Main(string[] args)
        {
            try
            {
                Write("Please enter the city you like to be scraped:\t");
                var craiglistCity = ReadLine() ?? string.Empty;

                Write("Please enter the CraigList catagory:\t");
                var craiglistCatagory = ReadLine() ?? string.Empty;

                using (WebClient client = new WebClient())
                {
                    string content = client.DownloadString($"http://{craiglistCity.Replace(" ", string.Empty)}.craigslist.org/{Method}/{craiglistCatagory}");

                    ScrapeCriteria scrapeCriteria = new ScrapeCriteriaBuilder()
                        .WithData(content)
                        .WithRegex(@"<a href=\""(.*?)\"" data-id=\""(.*?)\"" class=\""result-title-hdrlnk\"">(.*?)</a>")
                        .WithPart(new ScrapeCriteriaPartBuilder()
                            .WithRegex(@">(.*?)</a>")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Build())
                        .WithPart(new ScrapeCriteriaPartBuilder()
                            .WithRegex(@">(.*?)</a>")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Build())
                        .Build();

                    Scraper scraper = new Scraper();
                    List<string> scrapedItems = scraper.Scrape(scrapeCriteria);

                    if (scrapedItems.Any())
                    {
                        foreach (var item in scrapedItems) WriteLine();
                    }
                    else
                    {
                        WriteLine("There are no matches found .....");
                    }
                }

            }
            catch(Exception ex)
            {
                WriteLine(ex);
            }
        }
    }
}
