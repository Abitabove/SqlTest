namespace SqlStatementParser.Objects {
    public class Query {
        public Query(SelectList selectList, FromList fromList, Condition condition) {
            this.selectList = selectList;
            this.fromList = fromList;
            this.condition = condition;

        }
        public SelectList selectList { get; set; }
        public FromList fromList { get; set; }
        public Condition condition { get; set; }
    }
}