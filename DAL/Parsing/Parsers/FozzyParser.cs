using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DAL.Parsing.Parsers
{
    public class FozzyParser : IParser
    {
        public IEnumerable<Product> Parse(string keyword, int? limit = int.MaxValue, int? offset = 0)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load("https://fozzyshop.ua/ru/search?controller=search&s=" + keyword);
            var document = html.DocumentNode;
            var allPages = document.QuerySelectorAll(".page-list > li");
            var numberOfPages = 1;


            var numbers = (from page in allPages where page.QuerySelector("a > i") == null && page.QuerySelector("span") == null select Convert.ToInt32(page.QuerySelector("a").InnerText)).ToList();

            if (numbers.Count != 0)
                numberOfPages = numbers.Max();

            var products = new List<Product>();
            var isAble = true;

            for (var i = 1; i <= numberOfPages; i++)
            {
                html = htmlWeb.Load("https://fozzyshop.ua/ru/search?controller=search&page=" + i + "&s=" + keyword);
                document = html.DocumentNode;
                var list = document.QuerySelectorAll(".js-product-miniature-wrapper");


                foreach (var elem in list)
                {
                    if (!elem.HasClass("product_grey"))
                    {
                        var product = new Product
                        {
                            Shop = "Fozzy",
                            Name = GetProductName(elem.QuerySelector(".product-title > a")
                                .GetAttributeValue("href", string.Empty)),
                            TradeMark = elem.QuerySelector(".product-brand > a").InnerText
                        };
                        var htmlPrice = elem.QuerySelector(".product-price").Attributes["content"].Value;
                        product.Price = Convert.ToDouble(htmlPrice);
                        product.Img = elem.QuerySelector(".thumbnail-container > a > img").Attributes["src"].Value;
                        product.Link = elem.QuerySelector(".product-title > a")
                            .GetAttributeValue("href", string.Empty);
                        products.Add(product);
                    }
                    else
                    {
                        isAble = false;
                        break;
                    }
                }

                if (!isAble)
                    break;
            }

            return products;
        }

        private string GetProductName(string url)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load(url);
            var document = html.DocumentNode;
            var name = document.QuerySelector(".page-title > span").InnerText.Replace("&quot;", "'");
            return name;
        }
    }
}