using SqlStatementParser.Tokens;

namespace SqlStatementParser.BLL 
{
    public class SqlTokenMatch
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Priority { get; set; }
    }
}