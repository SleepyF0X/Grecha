using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using DAL.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DAL.Parsing.Parsers
{
    public sealed class NovusParser : IParser
    {
        private const string ShopUrl = "https://novus.zakaz.ua/";

        public IEnumerable<Product> Parse(string keyword, int? limit = int.MaxValue, int? offset = 0)
        {
            var httpWebRequest =
                (HttpWebRequest) WebRequest.Create("https://stores-api.zakaz.ua/stores/48201070/products/search/?q=" +
                                                   keyword);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("accept-language", "uk");

            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            dynamic config;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                config = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
            }

            var list = new List<Product>();
            foreach (var elem in config.results)
            {
                var product = new Product();
                product.Name = elem.title;
                product.Link = elem.web_url;
                product.Img = elem.img.s350x350;
                product.Price = ((double) elem.price) / 100;
                product.TradeMark = elem.producer.trademark;
                product.Shop = "novus";
                
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