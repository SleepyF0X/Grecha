using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using DAL.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Grecha.Parsing
{
    public class AtbParser : IParser
    {
        public List<Product> Parse(string keyword, int? limit = Int32.MaxValue, int? offset = 0)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load("https://zakaz.atbmarket.com/search/786?text=" + keyword);
            var document = html.DocumentNode;
            var lis = document.QuerySelectorAll("div.product-wrap");

            List<Product> list = new List<Product>();
            foreach (var htmlNode in lis)
            {
                Product product = new Product();
                product.Shop = "atb";
                var link = htmlNode.QuerySelector("div.product-detail > a").GetAttributeValue("href", string.Empty);
                var res = link.Split("/");
                product.IdFromShop = Int32.Parse(res.Last());
                product.Name = htmlNode.QuerySelector("div.product-detail > a > div").InnerText;
                var price = htmlNode.QuerySelector(".price").InnerText;
                price = price.Insert(price.Length - 2, ",");
                product.Price = Double.Parse(price, NumberStyles.AllowDecimalPoint);
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