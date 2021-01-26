using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using DAL.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Parsing.Parsers
{
    public sealed class NovusParser : IParser
    {
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
                var product = new Product
                {
                    Name = elem.title,
                    Link = elem.web_url,
                    Img = elem.img.s350x350,
                    Price = (double) elem.price / 100,
                    TradeMark = elem.producer.trademark,
                    Shop = "Novus"
                };

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