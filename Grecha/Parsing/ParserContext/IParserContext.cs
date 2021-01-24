using Grecha.Parsing.Parsers;

namespace Grecha.Parsing.ParserContext
{
    public interface IParserContext : IParser
    {
        void SetParsingStrategy(IParser parser);
    }
}
