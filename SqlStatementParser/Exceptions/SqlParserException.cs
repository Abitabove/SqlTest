using System;

namespace SqlStatementParser.Exceptions 
{

    [Serializable]
    public class SqlParserException : Exception 
    {
        public SqlParserException(string message) : base (message) { }
    }
}