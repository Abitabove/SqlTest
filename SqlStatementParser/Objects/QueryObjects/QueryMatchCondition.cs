using System.Collections.Generic;

namespace SqlStatementParser.Objects.QueryObjects
{
    public class QueryMatchCondition
    {
        public SqlObject Object { get; set; }
        public QueryOperator Operator { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }        
        public QueryLogicalOperator LogOpToNextCondition { get; set; }
    }
}