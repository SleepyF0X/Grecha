using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DAL.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DAL.Parsing.Parsers
{
    internal sealed class AtbParser : IParser
    {
        public IEnumerable<Product> Parse(string keyword, int? limit = int.MaxValue, int? offset = 0)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load("https://zakaz.atbmarket.com/search/786?text=" + keyword);
            var document = html.DocumentNode;
            var lis = document.QuerySelectorAll("div.product-wrap");

            var list = new List<Product>();
            foreach (var htmlNode in lis)
            {
                var product = new Product
                {
                    Shop = "ATB"
                };
                var link = htmlNode.QuerySelector("div.product-detail > a").GetAttributeValue("href", string.Empty);
                product.Link = "https://zakaz.atbmarket.com"+link;
                var res = link.Split("/");
                product.IdFromStore = int.Parse(res.Last());
                product.Name = htmlNode.QuerySelector("div.product-detail > a > div").InnerText;
                var price = htmlNode.QuerySelector(".price").InnerText;
                price = price.Insert(price.Length - 2, ",");
                product.Price = double.Parse(price, NumberStyles.AllowDecimalPoint);
                product.TradeMark = GetTrademark("https://zakaz.atbmarket.com" + link);
                list.Add(product);
            }

            return list;
        }

        private string GetTrademark(string url)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load(url);
            return html.DocumentNode
                .QuerySelector(
                    "body > section > div > div > div > div.col-sm-3.collection-filter > div > div.collection-collapse-block.border-0.open > div > div > ul > li > a")
                .InnerText;
        }
    }
}