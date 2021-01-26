using System.Collections.Generic;
using DAL.Models;

namespace DAL.Parsing.Parsers
{
    internal interface IParser
    {
        IEnumerable<Product> Parse(string keyword, int? limit, int? offset);
    }
}