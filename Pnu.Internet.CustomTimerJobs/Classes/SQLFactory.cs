using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs
{
    public static class SQLFactory
    {
        public static string GetConnectionString(string ConnectionName)
        {
            string connection = "";
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList list = web.Lists.TryGetList("ConnectionStrings");

                        SPQuery query = new SPQuery();
                        query.Query = string.Concat(
                                         $@"<Where>
                                      <Eq>
                                         <FieldRef Name='Title' />
                                         <Value Type='Text'>{ConnectionName}</Value>
                                      </Eq>
                                   </Where>");

                        SPListItemCollection coll = list.GetItems(query);
                        if (coll != null && coll.Count > 0)
                            connection = coll[0]["ConnectionString"].ToString();
                    }
                }
            });

            return connection;
        }


        static public string _ConnString = "";
        static public string ConnectionName = "";
        static public void SetConn(string _Conn)
        {
            _ConnString = _Conn;
            ConnectionName = _Conn;
            connectionString = _Conn;
        }




        

        public static object ExecuteScaler(string SQL)
        {

            Microsoft.Practices.EnterpriseLibrary.Data.Database db;
            string sqlCommand;
            DataTable dt = new DataTable();

            //db.ConnectionString = _ConnString;
            db = DatabaseFactory.CreateDatabase(_ConnString);
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

            Microsoft.Practices.EnterpriseLibrary.Data.Database db;
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

        public static List<T> ReadDataFromTable<T>(string _TableName, string _WhereCondition, string OrderBy = "", string TopSelect = "")
        {

            try
            {
                if (_TableName == "")
                    return null;

                string sqlCommand;
                DataTable dt = new DataTable();

                OpenConnection();

                sqlCommand = $@"SELECT  {TopSelect} * FROM " + _TableName;
                if (_WhereCondition != "")
                    sqlCommand += @" WHERE " + _WhereCondition;
                if (OrderBy != "")
                    sqlCommand += @" ORDER BY " + OrderBy;
                SqlCommand cmd = new SqlCommand(sqlCommand, connection);

                using (IDataReader objReader = cmd.ExecuteReader())
                {
                    List<T> list = new List<T>();
                    list = DataReaderMapToList<T>(objReader);
                    CloseConnection();
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
                Microsoft.Practices.EnterpriseLibrary.Data.Database db;
                string sqlCommand;
                DbCommand dbCommand;
                DataTable dt = new DataTable();

                db = DatabaseFactory.CreateDatabase(_ConnString);
                sqlCommand = _storedProc;
                dbCommand = db.GetStoredProcCommand(sqlCommand);
                if (objParameters.Count > 0)
                {
                    foreach (Parameters objPar in objParameters)
                    {
                        db.AddInParameter(dbCommand, objPar.PName, objPar.PType, objPar.PValue);
                    }


                }

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
            object ID;
            string SQL = GetInsertStatement(objNew);

            SqlConnection conn = new SqlConnection(ConnectionName);
            conn.Open();
            SQL = SQL + Environment.NewLine + " SELECT @@IDENTITY";
            SqlCommand cmd = new SqlCommand(SQL, conn);

            ID = cmd.ExecuteScalar();

            //Microsoft.Practices.EnterpriseLibrary.Data.Database db;
            //string sqlCommand;
            //DataTable dt = new DataTable();

            //db = DatabaseFactory.CreateDatabase(_ConnString);

            //sqlCommand = SQL;
            //object dr = db.ExecuteScalar(CommandType.Text, SQL);
            conn.Close();
            return ID;

        }

        public static void InsertIntoTable<T>(List<T> objNew, bool DeleteAllData)
        {
            SqlConnection conn = new SqlConnection(ConnectionName);
            conn.Open();
            string SQL = "";
            string TableName = "";
            foreach (object obj in objNew)
            {
                if (TableName == "")
                    TableName = obj.GetType().Name;
                SQL += GetInsertStatement(obj);
                SQL = SQL + Environment.NewLine;
            }
            if (DeleteAllData)
            {
                string DelSQL = $@"DELETE FROM [dbo].[{TableName}]
                        DBCC CHECKIDENT ('[{TableName}]', RESEED, 0);";

                SqlCommand cmdDel = new SqlCommand(DelSQL, conn);
                cmdDel.ExecuteScalar();

            }
            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.ExecuteScalar();
            conn.Close();


        }

        public static object UpdateCurrentObject(object objNew, string WhereCond)
        {
            string SQL = GetUpdateStatment(objNew);

            OpenConnection();
            string sqlCommand;
            DataTable dt = new DataTable();


            SQL = SQL + Environment.NewLine + "WHERE " + WhereCond;
            sqlCommand = SQL;
            SqlCommand cmd = new SqlCommand(SQL, connection);
            object dr = cmd.ExecuteScalar();
            CloseConnection();
            connection.Dispose();
            return dr;

        }


        public static object Update(string SQL)
        {
            OpenConnection();
            string sqlCommand;
            DataTable dt = new DataTable();
            sqlCommand = SQL;
            SqlCommand cmd = new SqlCommand(SQL, connection);
            object dr = cmd.ExecuteScalar();
            CloseConnection();
            connection.Dispose();
            return dr;

        }

        public static string GetUpdateStatment(object objNew)
        {
            string TableName = objNew.GetType().Name;
            //TableName = TableName.Remove(TableName.IndexOf("OutPuts"));
            string IdentityCol = "ID";//GetIdentityColumnByTableName(TableName);
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
                    //else if (Val.ToString().Contains("1/1/0001") || Val.ToString().Contains("01/01/0001"))
                    //    continue;
                    //else if (Val.ToString() == "-1")
                    //    continue;


                    SQL += objprops[i].Name + " = " + Formatting(Val.ToString()) + " ,";


                }

            }

            SQL = SQL.Substring(0, SQL.Length - 1);

            return SQL;
        }


        public static string GetUpdateStatmentForObject(object objNew)
        {
            string TableName = objNew.GetType().Name;
            //TableName = TableName.Remove(TableName.IndexOf("OutPuts"));
            string IdentityCol = "ID";//GetIdentityColumnByTableName(TableName);
            PropertyInfo[] objprops = objNew.GetType().GetProperties();
            string SQL = "Update " + TableName + " SET ";
            string WHERECondition = "";

            for (int i = 0; i < objprops.Length; i++)
            {
                PropertyInfo prop = objprops[i];
                if (prop.Name == IdentityCol)
                {
                    object Val = null;

                    Val = GetPropertyValue(objNew, prop.Name);
                    WHERECondition = $" WHERE ID={Formatting(Val.ToString())}";
                    continue;
                }
                Type PrpType;
                if (prop != null)
                {
                    PrpType = prop.PropertyType;
                    object Val = null;

                    Val = GetPropertyValue(objNew, prop.Name);
                    if (Val == null)
                        continue;
                    //else if (Val.ToString().Contains("1/1/0001") || Val.ToString().Contains("01/01/0001"))
                    //    continue;
                    //else if (Val.ToString() == "-1")
                    //    continue;


                    SQL += objprops[i].Name + " = " + Formatting(Val.ToString()) + " ,";


                }

            }

            SQL = SQL.Substring(0, SQL.Length - 1);

            SQL = SQL + WHERECondition;
            return SQL;
        }

        public static string GetInsertStatement(object objNew)
        {
            string TableName = objNew.GetType().Name;
            //TableName = TableName.Remove(TableName.IndexOf("OutPuts"));
            string IdentityCol = "ID";//GetIdentityColumnByTableName(TableName);
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

                    Val = GetPropertyValue(objNew, prop.Name);
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
        private static string Formatting(string item)
        {
            bool bool1 = false;
            DateTime date1 = DateTime.Now;
            Int32 int1 = 0;
            if (Int32.TryParse(item, out int1))
                return item;
            else if (bool.TryParse(item, out bool1))
                return BooleanToInteger(item);
            //else if (DateTime.TryParse(item, out date1))
            //    return "N'" + DateTime.Parse(item).ToString("MM/dd/yyyy HH:mm:ss") + "'";
            else
                return "N'" + item.Replace("'", "''") + "'";
        }


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

    public class Parameters
    {
        string pName;
        object pValue;
        DbType pType;

        #region Properites

        public DbType PType
        {
            get { return pType; }
            set { pType = value; }
        }

        public object PValue
        {
            get { return pValue; }
            set { pValue = value; }
        }

        public string PName
        {
            get { return pName; }
            set { pName = value; }
        }
        #endregion

        public Parameters(string _pName, object _pValue, DbType _pType)
        {
            PName = _pName;
            PValue = _pValue;
            PType = _pType;
        }

        public Parameters()
        {

        }


    }

    public class PrimaryKeys
    {
        public string COLUMN_NAME { get; set; }

        public string TABLE_NAME { get; set; }
    }

}
