using System.Collections.Generic;
using SqlStatementParser.Tokens;

namespace SqlStatementParser.Objects {
    public class SqlStatement {
        public SqlStatement (string statementName, string statementValue) {
            this.StatementName = statementName;
            this.StatementValue = statementValue;
            this.tokenList = new List<SqlToken>();
        }
        public string StatementName { get; set; }
        public string StatementValue { get; set; }
        public List<SqlToken> tokenList { get; set; }
    }
}