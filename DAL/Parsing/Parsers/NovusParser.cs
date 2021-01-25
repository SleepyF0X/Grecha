using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DAL.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DAL.Parsing.Parsers
{
    internal sealed class NovusParser : IParser
    {
        private const string ShopUrl = "https://novus.zakaz.ua/";

        public IEnumerable<Product> Parse(string keyword, int? limit = int.MaxValue, int? offset = 0)
        {
            var htmlWeb = new HtmlWeb();
            var html = htmlWeb.Load("https://novus.zakaz.ua/ru/search/?q=" + keyword);
            var document = html.DocumentNode;
            var lis = document.QuerySelectorAll("div.products-box__list-item");

            var list = new List<Product>();
            foreach (var htmlNode in lis)
            {
                Product product = new Product();
                product.Shop = "novus";
                var link = htmlNode.QuerySelector("div.products-box__list-item > a")
                    .GetAttributeValue("href", string.Empty);
                product.Link = link;
                product.Name = htmlNode.QuerySelector("div.products-box__list-item .product_tile_title").InnerText;
                var price = htmlNode.QuerySelector(".Price__value_caption").InnerText;
                product.Price = double.Parse(price, NumberStyles.AllowDecimalPoint);
                product.Img = htmlNode.QuerySelector(".product-tile__image-i").GetAttributeValue("src", string.Empty);
                product.TradeMark = GetTrademark(ShopUrl + link);
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
                    "body > div.ReactModalPortal > div > div > div.jsx-385435685.general-modal__body-wrapper > div > div:nth-child(6) > div > div.jsx-3554221871.big-product-card__general-info.big-product-card__section > ul > li:nth-child(1) > div.jsx-3554221871.big-product-card__entry-value > a")
                .InnerText;
        }
    }
}