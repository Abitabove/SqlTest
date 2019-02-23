using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SqlStatementParser.Exceptions;
using SqlStatementParser.Objects.QueryObjects;
using SqlStatementParser.Tokens;

namespace SqlStatementParser.BLL
{
    public class SqlParser
    {

        #region ForgetThis
        private Stack<SqlToken> _tokenStack;
        private SqlToken _lookaheadFirst;
        private SqlToken _lookaheadSecond;

        private SqlQueryModel _queryModel;
        private QueryMatchCondition _currentMatchCondition;

        private const string ExpectedObjectErrorText = "Expected =, !=, LIKE, NOT LIKE, IN or NOT IN but found: ";

        public SqlQueryModel Parse(List<SqlToken> sqlTokens)
        {
            LoadSequenceStack(sqlTokens);
            PrepareLookaheads();
            _queryModel = new SqlQueryModel();

            MatchCondition();

            DiscardToken(TokenType.SequenceTerminator);

            return _queryModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTokens"></param>
        private void LoadSequenceStack(List<SqlToken> sqlTokens)
        {
            _tokenStack = new Stack<SqlToken>();
            int count = sqlTokens.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                _tokenStack.Push(sqlTokens[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PrepareLookaheads()
        {
            _lookaheadFirst = _tokenStack.Pop();
            _lookaheadSecond = _tokenStack.Pop();
        }

        private SqlToken ReadToken(TokenType tokenType)
        {
            if (_lookaheadFirst.TokenType != tokenType)
                throw new SqlParserException(string.Format("Expected {0} but found: {1}", tokenType.ToString().ToUpper(), _lookaheadFirst.Value));

            return _lookaheadFirst;
        }

        private void DiscardToken()
        {
            _lookaheadFirst = _lookaheadSecond.Clone();

            if (_tokenStack.Any())
                _lookaheadSecond = _tokenStack.Pop();
            else
                _lookaheadSecond = new SqlToken(TokenType.SequenceTerminator, string.Empty);
        }

        private void DiscardToken(TokenType tokenType)
        {
            if (_lookaheadFirst.TokenType != tokenType)
                throw new SqlParserException(string.Format("Expected {0} but found: {1}", tokenType.ToString().ToUpper(), _lookaheadFirst.Value));

            DiscardToken();
        }

        private void MatchCondition()
        {
            CreateNewMatchCondition();

            if (IsObject(_lookaheadFirst))
            {
                if (IsEqualityOperator(_lookaheadSecond))
                {
                    EqualityMatchCondition();
                }
                else if (_lookaheadSecond.TokenType == TokenType.In)
                {
                    InCondition();
                }
                else if (_lookaheadSecond.TokenType == TokenType.NotIn)
                {
                    NotInCondition();
                }
                else
                {
                    throw new SqlParserException(ExpectedObjectErrorText + " " + _lookaheadSecond.Value);
                }

                
            }
            else
            {
                throw new SqlParserException(ExpectedObjectErrorText + _lookaheadFirst.Value);
            }
        }

        private void EqualityMatchCondition()
        {
            _currentMatchCondition.Object = GetObject(_lookaheadFirst);
            DiscardToken();
            _currentMatchCondition.Operator = GetOperator(_lookaheadFirst);
            DiscardToken();
            _currentMatchCondition.Value = _lookaheadFirst.Value;
            DiscardToken();
        }

        private SqlObject GetObject(SqlToken sqlToken)
        {
            switch (sqlToken.TokenType)
            {
                case TokenType.Select:
                    return SqlObject.Select;
                case TokenType.From:
                    return SqlObject.From;
                default:
                    throw new SqlParserException(ExpectedObjectErrorText + sqlToken.Value);
            }
        }

        private QueryOperator GetOperator(SqlToken sqlToken)
        {
            switch (sqlToken.TokenType)
            {
                case TokenType.Equals:
                    return QueryOperator.Equals;
                case TokenType.NotEquals:
                    return QueryOperator.NotEquals;
                case TokenType.Like:
                    return QueryOperator.Like;
                case TokenType.NotLike:
                    return QueryOperator.NotLike;
                case TokenType.In:
                    return QueryOperator.In;
                case TokenType.NotIn:
                    return QueryOperator.NotIn;
                default:
                    throw new SqlParserException("Expected =, !=, <>, LIKE, NOT LIKE, IN, NOT IN but found: " + sqlToken.Value);
            }
        }

        private void NotInCondition()
        {
            ParseInCondition(QueryOperator.NotIn);
        }

        private void InCondition()
        {
            ParseInCondition(QueryOperator.In);
        }

        private void ParseInCondition(QueryOperator inOperator)
        {
            _currentMatchCondition.Operator = inOperator;
            _currentMatchCondition.Values = new List<string>();
            _currentMatchCondition.Object = GetObject(_lookaheadFirst);
            DiscardToken();

            if (inOperator == QueryOperator.In)
                DiscardToken(TokenType.In);
            else if (inOperator == QueryOperator.NotIn)
                DiscardToken(TokenType.NotIn);

            DiscardToken(TokenType.OpenParenthesis);
            StringLiteralList();
            DiscardToken(TokenType.CloseParenthesis);
        }

        private void StringLiteralList()
        {
            _currentMatchCondition.Values.Add(ReadToken(TokenType.StringValue).Value);
            DiscardToken(TokenType.StringValue);
            StringLiteralListNext();
        }

        private void StringLiteralListNext()
        {
            if (_lookaheadFirst.TokenType == TokenType.Comma)
            {
                DiscardToken(TokenType.Comma);
                _currentMatchCondition.Values.Add(ReadToken(TokenType.StringValue).Value);
                DiscardToken(TokenType.StringValue);
                StringLiteralListNext();
            }
            else
            {
                // nothing
            }
        }

       private bool IsSelect(SqlToken sqlToken)
        {
            return sqlToken.TokenType == TokenType.Select;
        }

        private bool IsObject(SqlToken sqlToken)
        {
            return sqlToken.TokenType == TokenType.Select
                   || sqlToken.TokenType == TokenType.From;
        }

        private bool IsEqualityOperator(SqlToken token)
        {
            return token.TokenType == TokenType.Equals
                   || token.TokenType == TokenType.NotEquals
                   || token.TokenType == TokenType.Like
                   || token.TokenType == TokenType.NotLike;
        }

        private void CreateNewMatchCondition()
        {
            _currentMatchCondition = new QueryMatchCondition();
            _queryModel.MatchConditions.Add(_currentMatchCondition);
        }

        #endregion
    }
}