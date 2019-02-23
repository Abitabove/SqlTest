using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SqlStatementParser.BLL;
using SqlStatementParser.Objects;
using SqlStatementParser.Tokens;

namespace SqlStatementParser 
{
    class Program 
    {
        static void Main(string[] args) 
        {
            new Program().Start();
        }

        public void Start() 
        {
            string path = "SqlStatements.json";
            var jsonSerializer = new JsonFileSerializer();

            while (true) 
            {
                Console.WriteLine($"Press 1 : Tokenize list of sql statements{Environment.NewLine}");

                var selection = Console.ReadKey();

                switch (selection.KeyChar.ToString()) 
                {
                    case "1":
                        foreach(var statement in jsonSerializer.ParseSqlStrings(path))
                        {
                            var tonkenizer = new RegexTokenizer();

                            Console.WriteLine($"Outputing token list:{Environment.NewLine}");
                            OutputTokenList(tonkenizer, statement);
                        }
                        break;
                    default:
                        Console.WriteLine("Enter a valid selection.");
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <param name="sqlStatements"></param>
        public void OutputTokenList(RegexTokenizer tokenizer, SqlStatement sqlStatement) 
        {
            sqlStatement.tokenList.AddRange(tokenizer.Tokenize(sqlStatement.StatementValue));
          
            sqlStatement.tokenList.ForEach(x => Console.WriteLine($"Token Type: {x.TokenType}, Token Value: {x.Value}"));

            //var temp = parser.Parse(sqlStatement.tokenList);
            OutputParseTree(sqlStatement);

            Console.WriteLine($"End of statement.{Environment.NewLine}");
        }

        public void OutputParseTree(SqlStatement sqlStatement)
        {
            Console.WriteLine($"Parse tree: {Environment.NewLine}");
            Console.WriteLine("Query");

            int numberToSkip = 0;
            int nestedCounter = 0;
            string tabs = string.Empty;
            for(int i = 0; i < sqlStatement.tokenList.Count; i++)
            {
                if(nestedCounter > 0)
                {
                    tabs = "\t";
                }

                if(sqlStatement.tokenList[i].TokenType == TokenType.Select)
                {
                    Console.WriteLine($"{tabs}SELECT");
                    Console.WriteLine($"{tabs}\tSelectList");
                    nestedCounter++;
                }
                else if(sqlStatement.tokenList[i].TokenType == TokenType.From)
                {
                    Console.WriteLine($"{tabs}FROM");
                    Console.WriteLine($"{tabs}\tFromList");
                    nestedCounter++;
                }
                else if(sqlStatement.tokenList[i].TokenType == TokenType.Where)
                {
                    Console.WriteLine($"{tabs}WHERE");
                    Console.WriteLine($"{tabs}\tCondition");
                    nestedCounter++;
                }
            }
        }
    }
}