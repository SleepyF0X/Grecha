using System.Collections.Generic;
using DAL.Models;

namespace Grecha.Parsing.Parsers
{
    public interface IParser
    {
        public IEnumerable<Product> Parse(string keyword, int? limit, int? offset);
    }
}