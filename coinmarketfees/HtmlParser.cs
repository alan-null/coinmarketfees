using coinmarketfees.Model;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace coinmarketfees
{
    public class HtmlParser
    {
        public Exchange GetExchange(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.SelectNodes("//input").Where(e => e.GetAttributeValue("name", string.Empty) == "exchange").FirstOrDefault();
            var selector = doc.DocumentNode.SelectNodes("//select").Where(e => e.HasClass("comparison_select")).FirstOrDefault()?
                .SelectNodes(".//option").Where(a => a.Attributes["selected"] != null).FirstOrDefault();
            if (node != null && selector != null)
            {
                return new Exchange
                {
                    Id = int.Parse(node.GetAttributeValue("value", "0")),
                    Symbol = selector.Attributes["value"].Value,
                    Name = selector.InnerText
                };
            }
            return null;
        }

        public List<Exchange> GetExchanges(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodes = doc.DocumentNode.SelectNodes("//div").Where(e => e.HasClass("info_text")).ToList();
            var exchanges = new List<Exchange>();
            foreach (var item in nodes)
            {
                var linkNode = item.SelectSingleNode(".//a");
                string url = linkNode.Attributes["href"].Value;
                var ex = new Exchange
                {
                    Name = linkNode.InnerText,
                    Symbol = url.Substring(url.LastIndexOf('/') + 1)
                };
                exchanges.Add(ex);
            }
            return exchanges;
        }

        public List<FeeInfo> GetFeeInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//div").Where(e => e.HasClass("item_cSearch") && e.HasClass("item_coin_network")).ToList();
            List<FeeInfo> feeInfos = new List<FeeInfo>();
            foreach (var item in nodes)
            {
                var symbol = item.Attributes["name"].Value;
                var name = item.SelectSingleNode(".//h1[contains(@class, 'ctitle')]").InnerText;
                var fee = item.SelectSingleNode(".//div[contains(@class, 'ttop network-fee')]").InnerText.TrimStart('$');

                var fi = new FeeInfo
                {
                    CoinSymbol = symbol,
                    CoinName = name,
                    Price = fee == "N/A" ? 0 : decimal.Parse(fee)
                };
                feeInfos.Add(fi);
            }
            return feeInfos;
        }
    }
}
