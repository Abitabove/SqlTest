namespace SqlStatementParser.Tokens 
{
    public class SqlToken 
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenType"></param>
        public SqlToken(TokenType tokenType) 
        {
            TokenType = tokenType;
            Value = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="value"></param>
        public SqlToken(TokenType tokenType, string value) 
        {
            TokenType = tokenType;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SqlToken Clone() 
        {
            return new SqlToken(TokenType, Value);
        }
    }
}