using System.Collections.Generic;

namespace SqlStatementParser.Objects
{
    public class Condition
    {
        public List<Attribute> attributes {get;set;}
        public List<Query> queries {get;set;}
        public List<Pattern> patterns {get;set;}
    }
}