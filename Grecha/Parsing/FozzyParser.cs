using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Grecha.Parsing
{
    public class FozzyParser : IParser
    {
        public List<Product> Parse(string keyword, int? limit = Int32.MaxValue, int? offset = 0)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load("https://fozzyshop.ua/ru/search?controller=search&s=" + keyword);
            var document = html.DocumentNode;
            var allPages = document.QuerySelectorAll(".page-list > li");
            List<int> numbers = new List<int>();
            int numberOfPages;

            
            //foreach (var page in allPages)
            //{
            //    if (!page.QuerySelector("a").InnerText.Equals(""))
            //    {
            //        numbers.Add(Convert.ToInt32(page.QuerySelector("a").InnerText.Replace(" ", "")));
            //    }
            //}
            //numberOfPages = numbers.Max();

            List<Product> products = new List<Product>();
            for (int i = 1; i <= 4; i++) //
            {
                html = htmlWeb.Load("https://fozzyshop.ua/ru/search?controller=search&page=" + i + "&s=" + keyword);
                document = html.DocumentNode;
                var list = document.QuerySelectorAll(".js-product-miniature-wrapper");

                
                foreach (var elem in list)
                {
                    if (!elem.HasClass("product_grey"))  
                    {
                        Product product = new Product();
                        product.Shop = "Fozzy";
                        product.Name = GetProductName(elem.QuerySelector(".product-title > a")
                            .GetAttributeValue("href", string.Empty));
                        product.TradeMark = elem.QuerySelector(".product-brand > a").InnerText;
                        var htmlPrice = elem.QuerySelector(".product-price").Attributes["content"].Value.Replace(".",",");
                        product.Price = Convert.ToDouble(htmlPrice);

                        products.Add(product);
                    }
                }
            }

            return products;
        }

        private string GetProductName(string url)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load(url);
            var document = html.DocumentNode;
            var name = document.QuerySelector(".page-title > span").InnerText.Replace("&quot;","'");
            return name;
        }
    }
}
