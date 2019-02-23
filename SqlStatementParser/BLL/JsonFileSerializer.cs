using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SqlStatementParser.Objects;

namespace SqlStatementParser.BLL 
{
    public class JsonFileSerializer 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<SqlStatement> ParseSqlStrings(string filePath) 
        {
            var items = new List<SqlStatement>();

            using (StreamReader r = new StreamReader(filePath)) 
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<SqlStatement>>(json);
            }
            return items;
        }
    }
}