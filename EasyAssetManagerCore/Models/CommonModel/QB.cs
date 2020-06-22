using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyAssetManagerCore.Model.CommonModel
{
    public class QB<TSource> where TSource : class
    {
        public static string Select(Expression<Func<TSource, bool>> whereExpression = null)
        {
            var excludes = new List<string>();
            var set = string.Empty;
            var where = string.Empty;

            Type type = typeof(TSource);
            var columns = GetTableColumns();

            if (whereExpression != null)
            {
                var expression = TrimedExpression(whereExpression.Body.ToString());
                where = GetWhereCondition(expression);
            }

            set = string.Join(",", columns);
            where = where.TrimEnd(',');


            where = string.IsNullOrWhiteSpace(where) ? string.Empty : string.Format(" WHERE {0} ", where);

            return "SELECT " + set + " FROM " + GetTableName() + where;


        }

        public static string SelectById(int id)
        {
            var excludes = new List<string>();
            var set = string.Empty;
            var where = string.Empty;
            var primaryKeyColumn = GetPrimaryKeyColumns().FirstOrDefault();
            var tableName = GetTableName();

            Type type = typeof(TSource);
            var columns = GetTableColumns();



            set = string.Join(",", columns);
            where = tableName + "." + primaryKeyColumn + "=" + id;


            where = string.IsNullOrWhiteSpace(where) ? string.Empty : string.Format(" WHERE {0} ", where);

            return "SELECT " + set + " FROM " + tableName + where;


        }

        public static string Insert()
        {
            var query = string.Empty;
            List<string> allColumns = GetColumns();
            string tableName = GetTableName();
            string primaryKey = GetPrimaryKeyColumns().FirstOrDefault();

            var columns = string.Join(",", allColumns);
            var values = ":" + string.Join(",:", allColumns);
            query = " INSERT INTO " + tableName;
            if (string.IsNullOrWhiteSpace(columns) || string.IsNullOrWhiteSpace(values))
            {
                throw new Exception("No column is found in the model.");
            }

            // columns = "( " + primaryKey + "," + columns + " )";
            columns = " ( " + columns + " )";
            values = " VALUES (" + values.TrimStart(',') + " )";
            return query + columns + values;
        }

        public static string InsertWithPrimaryKey()
        {
            var query = string.Empty;
            List<string> allColumns = GetTableColumns();
            string tableName = GetTableName();
            string primaryKey = GetPrimaryKeyColumns().FirstOrDefault();
            var columns = string.Join(",", allColumns);
            var values = "@" + string.Join(",@", allColumns);
            query = " INSERT INTO " + tableName;
            if (string.IsNullOrWhiteSpace(columns) || string.IsNullOrWhiteSpace(values))
            {
                throw new Exception("No column is found in the model.");
            }
            columns = " ( " + columns + " )";
            values = " VALUES (" + values.TrimStart(',') + " )";
            return query + columns + values + "; SELECT COALESCE(MAX("+ primaryKey + "),0) PrimarykeyValue FROM "+tableName+";";
        }

        public static string Update(Expression<Func<TSource, bool>> whereExpression = null, Expression<Func<TSource, bool>> Excludes = null)
        {
            var whereExcludes = new List<string>();
            var updateExcludes = new List<string>();
            var primaryKeyColumns = new List<string>();
            var set = string.Empty;
            var where = string.Empty;

            Type type = typeof(TSource);
            var columns = GetColumns();

            if (Excludes != null)
            {
                var expression = TrimedExpression(Excludes.Body.ToString());
                updateExcludes = GetExcludes(expression);
                columns = columns.Except(updateExcludes).ToList();
            }

            if (whereExpression != null)
            {
                var expression = TrimedExpression(whereExpression.Body.ToString());
                whereExcludes = GetExcludes(expression);
                where = GetWhereCondition(expression);
                columns = columns.Except(whereExcludes).ToList();
            }
            else
            {
                primaryKeyColumns = GetPrimaryKeyColumns();
                foreach (var v in primaryKeyColumns)
                {
                    where = string.Format("{0} {1}=@{1} AND", where, v);
                }

                where = where.TrimEnd('D').TrimEnd('N').TrimEnd('A');
            }



            foreach (var v in columns)
            {
                set = string.Format("{0} {1}=@{1},", set, v);
            }


            where = where.TrimEnd(',');
            set = set.TrimEnd(',');

            return "Update " + GetTableName() + " SET " + set + " WHERE " + where + ";  select cast(@@ROWCOUNT as int)";


        }

        public static string UpdateOnly(Expression<Func<TSource, bool>> Includes, Expression<Func<TSource, bool>> whereExpression = null)
        {
            var whereExcludes = new List<string>();
            var updateIncludes = new List<string>();
            var primaryKeyColumns = new List<string>();
            var set = string.Empty;
            var where = string.Empty;

            Type type = typeof(TSource);



            var expression = TrimedExpression(Includes.Body.ToString());
            updateIncludes = GetExcludes(expression);
            var columns = updateIncludes.ToList();


            if (whereExpression != null)
            {
                expression = TrimedExpression(whereExpression.Body.ToString());
                whereExcludes = GetExcludes(expression);
                where = GetWhereCondition(expression);
                columns = columns.Except(whereExcludes).ToList();
            }
            else
            {
                primaryKeyColumns = GetPrimaryKeyColumns();
                foreach (var v in primaryKeyColumns)
                {
                    where = string.Format("{0} {1}=@{1} AND", where, v);
                }

                where = where.TrimEnd('D').TrimEnd('N').TrimEnd('A');
            }



            foreach (var v in columns)
            {
                set = string.Format("{0} {1}=@{1},", set, v);
            }


            where = where.TrimEnd(',');
            set = set.TrimEnd(',');

            return "Update " + GetTableName() + " SET " + set + " WHERE " + where + ";  select cast(@@ROWCOUNT as int);";


        }

        public static string Delete(Expression<Func<TSource, bool>> whereExpression = null)
        {
            var excludes = new List<string>();
            var set = string.Empty;
            var where = string.Empty;

            Type type = typeof(TSource);
            var columns = GetColumns();

            if (whereExpression != null)
            {
                var expression = TrimedExpression(whereExpression.Body.ToString());
                excludes = GetExcludes(expression);
                where = GetWhereCondition(expression);
                columns = columns.Except(excludes).ToList();
            }
            else
            {
                excludes = GetPrimaryKeyColumns();
                foreach (var v in excludes)
                {
                    where = string.Format("{0} {1}=@{1} AND", where, v);
                }

                where = where.TrimEnd('D').TrimEnd('N').TrimEnd('A');
            }



            foreach (var v in columns)
            {
                set = string.Format("{0} {1}=@{1},", set, v);
            }


            where = where.TrimEnd(',');
            set = set.TrimEnd(',');

            return "DELETE FROM " + GetTableName() + " WHERE " + where + "; select cast(@@ROWCOUNT as int)";


        }

        private static List<string> GetExcludes(string expression)
        {
            var excludes = new List<string>();
            expression = TrimedExpression(expression);
            var words = expression.Split(' ');
            var reg = new Regex(@"^(([a-z0-9]+)\.[(a-z0-9)]+)$", RegexOptions.IgnoreCase);
            for (int i = 0; i < words.Count(); i++)
            {
                words[i] = words[i].Replace("{", "")
                                    .Replace("(", "")
                                    .Replace(")", "")
                                    .Replace("}", "");

                var match = reg.Match(words[i]);
                if (match.Success)
                {
                    excludes.Add(match.Value.Split('.')[1]);
                }
            }
            excludes = excludes.Distinct().ToList();
            return excludes;
        }
        private static string GetWhereCondition(string text)
        {
            var pattern = @"(\= [a-z]+)(\.)";

            text = Regex.Replace(text, pattern, m => "= @" + m.Groups[3].Value);
            pattern = @"(\<> [a-z]+)(\.)";
            text = Regex.Replace(text, pattern, m => "<> @" + m.Groups[3].Value);
            pattern = @"([a-z]+)(\.)";
            text = Regex.Replace(text, pattern, m => m.Groups[3].Value);

            return text;
        }
        private static bool IsValidPropertyType(Type type)
        {

            if (type == typeof(Nullable<Int32>) || type == typeof(Int32))
                return true;

            if (type == typeof(Nullable<bool>) || type == typeof(bool))
                return true;

            if (type == typeof(Nullable<DateTime>) || type == typeof(DateTime))
                return true;
            if (type == typeof(Nullable<decimal>) || type == typeof(decimal))
                return true;
            if (type == typeof(Nullable<double>) || type == typeof(double))
                return true;

            if (type == typeof(string))
                return true;

            if (type.IsGenericParameter)
                return false;

            if (type.IsArray)
                return false;

            return false;
        }

        private static List<string> GetTableColumns()
        {
            var columns = new List<string>();


            var properties = typeof(TSource).GetProperties();
            foreach (var v in properties)
            {

                if (IsValidPropertyType(v.PropertyType))
                {
                    var attribute = v.GetCustomAttributes(false);
                    var hasAttrubute = attribute.FirstOrDefault(a => a.GetType() == typeof(NotMapedAttribute));

                    if (hasAttrubute == null)
                    {
                        columns.Add(v.Name);
                    }
                }
            }

            return columns;
        }

        private static List<string> GetColumns(bool includeAll = false)
        {
            var columns = new List<string>();


            var properties = typeof(TSource).GetProperties();
            foreach (var v in properties)
            {

                if (IsValidPropertyType(v.PropertyType))
                {
                    if (!includeAll)
                    {
                        var attribute = v.GetCustomAttributes(false);
                        var hasAttrubute = attribute.FirstOrDefault(a => a.GetType() == typeof(PKeyAttribute) || a.GetType() == typeof(NotMapedAttribute));

                        if (hasAttrubute == null)
                        {
                            columns.Add(v.Name);
                        }
                    }
                    else
                    {
                        columns.Add(v.Name);
                    }


                }
            }

            return columns;
        }
        public static List<string> GetPrimaryKeyColumns()
        {
            var columns = new List<string>();
            var properties = typeof(TSource).GetProperties();
            foreach (var v in properties)
            {
                if (IsValidPropertyType(v.PropertyType))
                {
                    var attribute = v.GetCustomAttributes(false);

                    var hasAttrubute = attribute.FirstOrDefault(a => a.GetType() == typeof(PKeyAttribute));

                    if (hasAttrubute != null)
                    {
                        columns.Add(v.Name);
                    }
                }
            }

            return columns;
        }
        private static string GetTableName()
        {
            var modelAttribute = ((TableAttribute[])typeof(TSource).GetTypeInfo().GetCustomAttributes(typeof(TableAttribute), false)).FirstOrDefault();
            if (modelAttribute == null)
            {
                Type type = typeof(TSource);
                return string.Format("dbo.{0}", type.Name);
            }
            else
            {
                return string.Format("{0}.{1}", modelAttribute.Schema, modelAttribute.Name);

            }
        }
        private static string TrimedExpression(string expression)
        {
            return expression.Replace("AndAlso", "AND").Replace("OrElse", "OR").Replace("Convert", "").Replace("==", "=").Replace("!=","<>");
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Optional Table attribute.
        /// </summary>
        /// <param name="tableName"></param>
        public TableAttribute(string tableName, string schemaName = "dbo")
        {
            Name = tableName;
            Schema = schemaName;
        }
        /// <summary>
        /// Name of the table
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Name of the schema
        /// </summary>
        public string Schema { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PKeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotMapedAttribute : Attribute
    {
    }
}
