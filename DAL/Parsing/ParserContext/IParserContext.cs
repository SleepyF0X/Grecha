using DAL.Parsing.Parsers;

namespace DAL.Parsing.ParserContext
{
    internal interface IParserContext : IParser
    {
        void SetParsingStrategy(IParser parser);
    }
}
