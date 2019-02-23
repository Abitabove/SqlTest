using System.Collections.Generic;

namespace SqlStatementParser.Objects.QueryObjects
{
    public class SqlQueryModel
    {
        public SqlQueryModel()
        {
            MatchConditions = new List<QueryMatchCondition>();
        }

        public DateRange DateRange { get; set; }
        public int? Limit { get; set; }
        public IList<QueryMatchCondition> MatchConditions { get; set; }
    }
}