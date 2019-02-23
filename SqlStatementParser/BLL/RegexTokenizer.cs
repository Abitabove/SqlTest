using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SqlStatementParser.Exceptions;
using SqlStatementParser.Objects;
using SqlStatementParser.Tokens;

namespace SqlStatementParser.BLL 
{
    public class RegexTokenizer 
    {
        private readonly List<SqlTokenDefinition> _definitions;

        /// <summary>
        /// 
        /// </summary>
        public RegexTokenizer() 
        {
            _definitions = new List<SqlTokenDefinition> 
            {
                (new SqlTokenDefinition(TokenType.And, 1, "and")),
                (new SqlTokenDefinition(TokenType.AsciiStringLiteral, 1, "’([^’]*)’")),
                (new SqlTokenDefinition(TokenType.Asterisk, 1, "\\*")),
                (new SqlTokenDefinition(TokenType.Between, 1, "between")),
                (new SqlTokenDefinition(TokenType.CloseParenthesis, 1, "\\)")),
                (new SqlTokenDefinition(TokenType.Comma, 1, ",")),
                (new SqlTokenDefinition(TokenType.DateTimeValue, 1, @"(\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d)|(\d\d\d\d-\d\d-\d\d)")),
                (new SqlTokenDefinition(TokenType.Dot, 1, "\\.")),
                (new SqlTokenDefinition(TokenType.Equals, 1, "=")),
                (new SqlTokenDefinition(TokenType.From, 1, "From")),          
                (new SqlTokenDefinition(TokenType.Greater, 1, ">")),
                (new SqlTokenDefinition(TokenType.In, 1, "in")),
                (new SqlTokenDefinition(TokenType.Less, 1, "<")),
                (new SqlTokenDefinition(TokenType.Like, 1, "like")),                
                (new SqlTokenDefinition(TokenType.NotEquals, 1, "!=|<>")),
                (new SqlTokenDefinition(TokenType.NotIn, 1, "not in")),
                (new SqlTokenDefinition(TokenType.NotLike, 1, "not like")),
                (new SqlTokenDefinition(TokenType.Number, 2, "\\d+")),
                (new SqlTokenDefinition(TokenType.OpenParenthesis, 1, "\\(")),
                (new SqlTokenDefinition(TokenType.Or, 1, "or")),                
                (new SqlTokenDefinition(TokenType.Select, 1, "Select")),
                (new SqlTokenDefinition(TokenType.Semicolon, 1, ";")),
                (new SqlTokenDefinition(TokenType.StringValue, 2, "[^\\s]+")),
                (new SqlTokenDefinition(TokenType.Table, 1, @"`([^']*)`|\[([^\.]?[^[]*)\]")),
                (new SqlTokenDefinition(TokenType.Where, 1, "Where"))//,
                //(new SqlTokenDefinition(TokenType.WhiteSpace, 1, "(\\s*)")),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <returns></returns>
        public IEnumerable<SqlToken> Tokenize(string statementText)
        {
            var tokenMatches = GetTokenMatches(statementText);

            var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
                                             .OrderBy (x => x.Key)
                                             .ToList ();

            SqlTokenMatch lastMatch = null;

            for (int i = 0; i < groupedByIndex.Count; i++) 
            {
                var bestMatch = groupedByIndex[i].OrderBy(x => x.Priority).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                {
                    continue;
                }
                yield return new SqlToken(bestMatch.TokenType, bestMatch.Value);

                lastMatch = bestMatch;
            }

            yield return new SqlToken(TokenType.SequenceTerminator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statementText"></param>
        /// <returns></returns>
        private List<SqlTokenMatch> GetTokenMatches(string statementText) 
        {
            var tokenMatches = new List<SqlTokenMatch>();

            foreach (var definition in _definitions)
            {
                tokenMatches.AddRange(definition.GetMatches(statementText).ToList());
            }
            return tokenMatches;
        }
    }
}