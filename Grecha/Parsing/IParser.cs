using System.Collections.Generic;
using DAL.Models;

namespace Grecha.Parsing
{
    public interface IParser
    {
        public List<Product> Parse(string keyword, int? limit, int? offset);
    }
}