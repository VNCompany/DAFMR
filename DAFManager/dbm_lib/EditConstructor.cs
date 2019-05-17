using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbm_lib.components;

namespace dbm_lib
{
    public class DBConstructor
    {
        public string TableName { get; set; }
        public string Wheres { get; set; }
        public List<DBKV> Values = new List<DBKV>();

        public DBConstructor() { }
        public DBConstructor(IEnumerable<DBKV> vals)
        {
            Values = vals.ToList();
        }

        public DBConstructor(IEnumerable<DBKV> vals, string wheres)
        {
            Values = vals.ToList();
            Wheres = wheres;
        }

        public DBConstructor(IEnumerable<DBKV> vals, string wheres, string tableName)
        {
            Values = vals.ToList();
            Wheres = wheres;
            TableName = tableName;
        }

        public bool isWheres
        {
            get { return Wheres != null && !string.IsNullOrWhiteSpace(Wheres); }
        }

        public bool isValues
        {
            get { return Values.Count > 0; }
        }

        public virtual string GetValuesString()
        {
            return Values.Select(t => $"`{t.Key}`='{t.Value}'").JoinStr(", ");
        }

        public bool isKeyExists(object key)
        {
            string k = key.ToString();
            return Values.Where(t => t.Key == k).Count() > 0;
        }
    }

    public struct DBKV
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public DBKV(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public class UpdateConstructor : DBConstructor
    {
        public override string ToString()
        {
            StringBuilder cmd_build = new StringBuilder();
            cmd_build.Append("UPDATE `" + TableName + "` SET " + GetValuesString());
            if (isWheres)
                cmd_build.Append(" WHERE " + Wheres);
            return cmd_build.ToString();
        }
    }

    public class InsertConstructor : DBConstructor
    {
        public override string GetValuesString()
        {
            return Values.Select(t => "'" + t.Value + "'").JoinStr(",");
        }

        public override string ToString()
        {
            StringBuilder cmd_build = new StringBuilder();
            cmd_build.Append("INSERT INTO `" + TableName + "`");

            cmd_build.Append("(" + Values.Select(t => "`" + t.Key + "`").JoinStr(","));

            cmd_build.Append(") VALUES(" + GetValuesString() + ")");

            return cmd_build.ToString();
        }
    }

}
