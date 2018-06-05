using System.Text.RegularExpressions;

namespace WebScrapper.Data
{
    internal class ScrapeCriteriaPart
    {
        public string Regex { get; set; }
        public RegexOptions RegexOption { get; set; }
    }
}