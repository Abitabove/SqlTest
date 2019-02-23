using System.Collections.Generic;
using System.Text.RegularExpressions;
using SqlStatementParser.Objects;
using SqlStatementParser.Tokens;

namespace SqlStatementParser.BLL 
{
    public class SqlTokenDefinition 
    {
        private readonly TokenType _returnToken;
        private readonly int _priority;
        private Regex _regex;

        public SqlTokenDefinition(TokenType returnToken, int priority, string regex) 
        {
            _returnToken = returnToken;
            _priority = priority;
            _regex = new Regex (regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// Loop through Regex matches to get list of tokens
        /// </summary>
        /// <param name="input">String to search for matches</param>
        /// <returns>IEnumberable of TokenMatches</returns>
        public IEnumerable<SqlTokenMatch> GetMatches(string input) 
        {
            var matches = _regex.Matches(input);
            for (int i = 0; i < matches.Count; i++) 
            {
                yield return new SqlTokenMatch 
                {
                    StartIndex = matches[i].Index,
                    EndIndex = matches[i].Index + matches[i].Length,
                    TokenType = _returnToken,
                    Value = matches[i].Value,
                    Priority = _priority
                };
            }
        }
    }
}