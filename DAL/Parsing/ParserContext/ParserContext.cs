using System;
using System.Collections.Generic;
using DAL.Models;
using DAL.Parsing.Parsers;

namespace DAL.Parsing.ParserContext
{
    internal sealed class ParserContext : IParserContext
    {
        public IParser Parser { get; private set; }

        public void SetParsingStrategy(IParser parser)
        {
            if (parser != null)
            {
                Parser = parser;
            }
            else
            {
                throw new ArgumentNullException(nameof(parser));
            }
            
        }
        public IEnumerable<Product> Parse(string keyword, int? limit, int? offset)
        {
            if (Parser != null)
            {
                return Parser.Parse(keyword, limit, offset);
            }
            else
            {
                throw new ArgumentNullException(nameof(Parser));
            }
            
        }
    }
}
