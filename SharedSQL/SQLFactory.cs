using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Data.Common;
using System.Globalization;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace SharedSQL
{
    public static class SQLFactory
    {


        #region Variable Declaration
        static public string _ConnString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        static public string ConnectionName = "ConnString";
        //static private int _count;
        //static private string SQLSetStringPrefix =
        //                                        @"SET Dateformat dmy;
        //                                        SET FMTONLY OFF; 
        //                                        SET NO_BROWSETABLE ON;
        //                                        ";
        //static private string SQLSetStringSufix =
        //                                        @"
        //                                        SET NO_BROWSETABLE OFF;
        //                                        ";
        #endregion

        #region Methods

        #region ReadData

        static public void SetConn(string _Conn)
        {
            _ConnString = _Conn;
        }
        static public void SetConnName(string _Conn)
        {
            ConnectionName = _Conn;
        }

        public static string GetIdentityColumnByTableName(string TableName)
        {
            string SQL = @"select COLUMN_NAME " ;
            SQL += " from INFORMATION_SCHEMA.COLUMNS ";
            SQL += " where TABLE_SCHEMA = 'dbo'" ;
            SQL += " and COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 ";
            SQL += " and TABLE_NAME = '" + TableName + "'";

            Database db;
            string sqlCommand;
            db = DatabaseFactory.CreateDatabase(ConnectionName);
            sqlCommand = SQL;
            IDataReader dr = db.ExecuteReader(CommandType.Text, SQL);
            //DataSet ds = db.ExecuteDataSet(sqlCommand);
            while (dr.Read())
            {
                return dr.GetValue(0).ToString();
            }
            return "";
        }

        public static object ExecuteScaler(string SQL)
        {

            Database db;
            string sqlCommand;
            DataTable dt = new DataTable();

            //db.ConnectionString = _ConnString;
            db = DatabaseFactory.CreateDatabase(ConnectionName);
            sqlCommand = SQL;
            IDataReader dr = db.ExecuteReader(CommandType.Text, SQL);
            //DataSet ds = db.ExecuteDataSet(sqlCommand);
            while (dr.Read())
            {
                return dr.GetValue(0).ToString();
            }
            return "";
        }

        public static void ExecuteNonQuery(string SQL)
        {

            Database db;
            string sqlCommand;
            DataTable dt = new DataTable();

            //db.ConnectionString = _ConnString;
            db = DatabaseFactory.CreateDatabase(ConnectionName);
            sqlCommand = SQL;
            db.ExecuteNonQuery(CommandType.Text, SQL);
            
            
        }

        public static String connectionString = _ConnString;//ConfigurationManager.ConnectionStrings["ConnString"].ToString();
        public static SqlConnection connection;

        public static void OpenConnection()
        {
            connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed)
                connection.Open();

        }

        public static void CloseConnection()
        {
            connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Open)
                connection.Close();

        }

        public static List<T> ReadDataFromTable<T>(string _TableName, string _WhereCondition,string OrderBy)
        {
            try
            {
                if (_TableName == "")
                    return null;
                Database db;
                string sqlCommand;
                DataTable dt = new DataTable();
                //OpenConnection();
                db = new SqlDatabase(_ConnString);
                db = DatabaseFactory.CreateDatabase(ConnectionName);
                sqlCommand = @"SELECT * FROM " + _TableName;
                if (_WhereCondition != "")
                    sqlCommand += @" WHERE " + _WhereCondition;
                if (OrderBy != "")
                    sqlCommand += @" ORDER BY " + OrderBy;
                using (IDataReader objReader = db.ExecuteReader(CommandType.Text, sqlCommand))
                {
                    List<T> list = new List<T>();
                    list = DataReaderMapToList<T>(objReader);
                    //CloseConnection();
                    return list;
                }

                



            }
            catch (Exception)
            {
                CloseConnection();

                throw; 
                

            }
            finally
            {

            }
        }

        public static List<T> ReadDataFromTable<T>(string _TableName, string _WhereCondition)
        {
            try
            {
                if (_TableName == "")
                    return null;
                Database db;
                string sqlCommand;
                DataTable dt = new DataTable();
                //OpenConnection();
                db = new SqlDatabase(_ConnString);
                db = DatabaseFactory.CreateDatabase(ConnectionName);
                sqlCommand = @"SELECT * FROM " + _TableName;
                if (_WhereCondition != "")
                    sqlCommand += @" WHERE " + _WhereCondition;
                
                using (IDataReader objReader = db.ExecuteReader(CommandType.Text, sqlCommand))
                {
                    List<T> list = new List<T>();
                    list = DataReaderMapToList<T>(objReader);
                    //CloseConnection();
                    return list;
                }





            }
            catch (Exception)
            {
                CloseConnection();

                throw;


            }
            finally
            {

            }
        }



        public static List<T> ReadDataFromStoredProc<T>(string _storedProc, List<Parameters> objParameters)
        {

            try
            {
                Database db;
                string sqlCommand;
                DbCommand dbCommand;
                DataTable dt = new DataTable();

                db = DatabaseFactory.CreateDatabase("ConnString");
                sqlCommand = _storedProc;
                dbCommand = db.GetStoredProcCommand(sqlCommand);
                if (objParameters.Count > 0)
                {
                    foreach (Parameters objPar in objParameters)
                    {
                        db.AddInParameter(dbCommand, objPar.PName, objPar.PType, objPar.PValue);
                    }


                }
                //db.AddInParameter(dbCommand, "@DepID ", DbType.Int64, 0);
                //db.AddInParameter(dbCommand, "@CenDepID ", DbType.Int64, 0);
                //db.AddInParameter(dbCommand, "@SecID ", DbType.Int64, 0);
                //db.AddInParameter(dbCommand, "@StatType ", DbType.Int32, 1);
                //dt = db.ExecuteDataSet(dbCommand).Tables[0];

                using (IDataReader objReader = db.ExecuteReader(dbCommand))
                {
                    //objPrdList = ReadReaders(objReader);
                    List<T> list = new List<T>();
                    list = DataReaderMapToList<T>(objReader);
                    return list;
                }



            }
            catch (Exception)
            {

                throw;

            }
            finally
            {

            }

        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                
                int FieldCount = dr.FieldCount;
                for (int i = 0; i < FieldCount; i++)
                {
                    string ColumnName = dr.GetName(i);
                    string MemberName = ColumnName;

                    
                    PropertyInfo prop = obj.GetType().GetProperty(MemberName);
                    Type PrpType;
                    if (prop != null)
                    {
                        if (prop.Name == "TableName")
                            continue;
                        PrpType = prop.PropertyType;
                        object Val = null;
                        try
                        {
                            if (dr.GetValue(i).GetType() != typeof(System.DBNull))
                            {
                                PrpType = ResolveType(PrpType.ToString());
                                Val = Convert.ChangeType(dr.GetValue(i), PrpType);
                                prop.SetValue(obj, Val, null);
                            }

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }


        
        public static object InsertIntoTable(object objNew)
        {
            string SQL = GetInsertStatement(objNew);

            Database db;
            string sqlCommand;
            DataTable dt = new DataTable();

            db = DatabaseFactory.CreateDatabase("ConnString");
            SQL = SQL + Environment.NewLine +" SELECT @@IDENTITY";
            sqlCommand = SQL;
            object dr = db.ExecuteScalar(CommandType.Text, SQL);

            return dr;

        }

        public static object UpdateCurrentObject(object objNew,string WhereCond)
        {
            string SQL = GetUpdateStatment(objNew);

            Database db;
            string sqlCommand;
            DataTable dt = new DataTable();

            db = DatabaseFactory.CreateDatabase("ConnString");
            SQL = SQL + Environment.NewLine + "WHERE " + WhereCond;
            sqlCommand = SQL;
            object dr = db.ExecuteScalar(CommandType.Text, SQL);

            return dr;

        }

        public static string GetUpdateStatment(object objNew)
        {
            string TableName = objNew.GetType().Name;
            //TableName = TableName.Remove(TableName.IndexOf("OutPuts"));
            string IdentityCol = GetIdentityColumnByTableName(TableName);
            PropertyInfo[] objprops = objNew.GetType().GetProperties();
            string SQL = "Update " + TableName + " SET ";


            for (int i = 0; i < objprops.Length; i++)
            {
                PropertyInfo prop = objprops[i];
                if (prop.Name == IdentityCol)
                    continue;
                Type PrpType;
                if (prop != null)
                {
                    PrpType = prop.PropertyType;
                    object Val = null;

                    Val = GetPropertyValue(objNew, prop.Name);
                    if (Val == null)
                        continue;
                    else if (Val.ToString().Contains("1/1/0001") || Val.ToString().Contains("01/01/0001"))
                        continue;
                    else if (Val.ToString() == "-1")
                        continue;


                    SQL += objprops[i].Name + " = " + Formatting(Val.ToString()) + " ,";


                }

            }

            SQL = SQL.Substring(0, SQL.Length - 1);

            return SQL;
        }

        public static string GetInsertStatement(object objNew)
        {
            string TableName = objNew.GetType().Name;
            //TableName = TableName.Remove(TableName.IndexOf("OutPuts"));
            string IdentityCol = GetIdentityColumnByTableName(TableName);
            PropertyInfo[] objprops = objNew.GetType().GetProperties();
            string SQL = "INSERT INTO " + TableName + "({0})";
            string ColumnsNames = string.Empty;
            string ColumnsValues = string.Empty;
            string SQLValues = " VALUES({0})";
            for (int i = 0; i < objprops.Length; i++)
            {
                PropertyInfo prop = objprops[i];
                if (prop.Name == IdentityCol)
                    continue;
                    Type PrpType;
                    if (prop != null)
                    {
                        PrpType = prop.PropertyType;
                        object Val = null;
                        
                        Val = GetPropertyValue(objNew,prop.Name);
                        if (Val == null)
                            continue;
                        else if (Val.ToString().Contains("1/1/0001") || Val.ToString().Contains("01/01/0001"))
                            continue;
                        else if (Val.ToString() == "-1")
                            continue;
                        if (ColumnsNames == string.Empty)
                        {
                            ColumnsNames = objprops[i].Name;
                            ColumnsValues = Formatting(Val.ToString());
                        }
                        else
                        {
                            ColumnsNames += "," + objprops[i].Name;
                            ColumnsValues += "," + Formatting(Val.ToString());
                        }
                        
                    }
                
            }
            SQL = string.Format(SQL, ColumnsNames);
            SQLValues = string.Format(SQLValues, ColumnsValues);
            SQL = SQL + SQLValues;
            return SQL;
        }
        #endregion
        private static string Formatting(string item)
        {
            bool bool1 = false;
            DateTime date1 = DateTime.Now;
            Int32 int1 = 0;
            if (Int32.TryParse(item, out int1))
                return item;
            else if (bool.TryParse(item, out bool1))
                return BooleanToInteger(item);
            else if (DateTime.TryParse(item, out date1))
                return "N'" + DateTime.Parse(item).ToString("MM/dd/yyyy HH:mm:ss") + "'";
            else
                return "N'" + item.Replace("'", "''") + "'";
        }
        #region DataTable
        //public static DataTable GetDataTable_FromStoredProcedure(string _storedName)
        //{
        //    DataTable _DT;
        //    SqlCommand _command;
        //    try
        //    {
        //        Database db;
        //        string sqlCommand;
        //        DbCommand dbCommand;
        //        DataTable dt = new DataTable();

        //        db = DatabaseFactory.CreateDatabase("ConnString");
        //        sqlCommand = "dbo." + _storedName ;
        //        dbCommand = db.GetStoredProcCommand(sqlCommand);


        //        return _DT;
        //    }
        //    catch (Exception)
        //    {

        //        throw;

        //    }
        //    finally
        //    {

        //        //if (ConnString.State != ConnectionState.Closed)
        //        //    ConnString.Close();  
        //    }

        //}

        #endregion

        private static Type ResolveType(String typeName)
        {
            Type t = Type.GetType(typeName);
            if (t == null)
                return null;

            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
            {
                t = u;
            }
            return t;
        }

        #endregion

        static private string BooleanToInteger(string item)
        {
            if (item.ToLower() == "false")
                return "0";
            else if (item.ToLower() == "true")
                return "1";
            else
                return "0";
        }

        static public object GetPropertyValue(object src, string PropertyName)
        {
            PropertyInfo PropInfo = null;
            try
            {
                PropInfo = src.GetType().GetProperty(PropertyName);
                return PropInfo.GetValue(src, null);

            }
            catch
            {
                return null;
            }
        }
        static public void SetPropertyValue(object src, string PropertyName, object value)
        {
            PropertyInfo PropInfo = null;
            try
            {
                PropInfo = src.GetType().GetProperty(PropertyName);
                if (PropInfo == null)
                    return;

                value = ChangeType(value, PropInfo.PropertyType);
                PropInfo.SetValue(src, value, null);

            }
            catch
            {
                throw;
            }
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t, CultureInfo.CurrentUICulture);
        }
    }

}
