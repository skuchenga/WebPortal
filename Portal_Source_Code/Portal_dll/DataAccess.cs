using System;
using System.Data;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;
using HFCPortal.Common;



/// <summary>
/// Summary description for DataAccess
/// </summary>

namespace HFCPortal
{

    //+++++++++++++++++++++++ end of switf connection+++++++++++++++++++
    public class DataAccess
    {
        private SqlConnection conn = null;

        public DataAccess()
        {
            
            try
            {
                HttpContext.Current.Session["DBType"] = "MSSQL";
            }
            catch
            {

            }
            
        }
        public DbDataReader GetDBResults(ref String errMsg, string StoredProcedure, params object[] ProcedureParameters)
        {

            int i = 0;
            //string ParameterName = "";
            string SQLStatement = "";
            string DataProviderName = "System.Data.SqlClient";
            Functions fn = new Functions();
            DbProviderFactory dpf = DbProviderFactories.GetFactory(DataProviderName);
            DbConnection BR_cnConnection = dpf.CreateConnection();

            //BR_cnConnection.ConnectionString = CommonHelper.GetConnectionString();

            BR_cnConnection.ConnectionString = "Data Source=. ;Initial Catalog = BR_Portal ;Persist Security Info=True;User ID=sa;Password=@groundzer036 ;connection timeout=300";

            string c = BR_cnConnection.ConnectionString.ToString();

            try
            {
                BR_cnConnection.Open();
            }
            catch (System.InvalidOperationException ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            DbCommand Command = dpf.CreateCommand();
            Command.Connection = BR_cnConnection;

            foreach (object o in ProcedureParameters)
            {
                if (i % 2 == 0)
                    SQLStatement = SQLStatement + (string)o + "=";
                else
                {
                    if (o.GetType() == typeof(string))
                        SQLStatement = SQLStatement + " '" + (string)o + "',";
                    else if (o.GetType() == typeof(DateTime))
                        SQLStatement = SQLStatement + " '" + o.ToString() + "',";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Double))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Decimal))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Boolean))
                    {
                        if ((bool)o == true)
                            SQLStatement = SQLStatement + " 1" + ",";
                        else
                            SQLStatement = SQLStatement + " 0" + ",";
                    }
                    else
                        SQLStatement = SQLStatement + " '" + (string)o + "',";

                }
                i = i + 1;
            }
            if (SQLStatement.Length > 1)
                SQLStatement = SQLStatement.Substring(0, SQLStatement.Length - 1);

            StoredProcedure = "EXEC " + StoredProcedure + " " + SQLStatement;

            Command.CommandType = CommandType.Text;
            Command.CommandText = StoredProcedure;
            DbDataReader ResultSet = null;
            try
            {
                ResultSet = Command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                errMsg = ex.Message.ToString();
                DbDataReader rs = null;
                return rs;
            }
            return ResultSet;
            //}

        }

        public DataTable GetDataTable(ref String errMsg, string StoredProcedure, params object[] ProcedureParameters)
        {

            int i = 0;
            //string ParameterName = "";
            string SQLStatement = "";
            //Functions fn = new Functions();
            
            

            SqlConnection conn = null;

            try
            {
                conn = OpenDBConnection();
            }
            catch (System.InvalidOperationException ex)
            {
                errMsg = ex.Message.ToString();
                DataTable dt = null;
                return dt;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                errMsg = ex.Message.ToString();
                DataTable dt = null;
                return dt;
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message.ToString();
                DataTable dt = null;
                return dt;
            }
            SqlCommand Command = new SqlCommand();
            Command.Connection = conn;

            foreach (object o in ProcedureParameters)
            {
                if (i % 2 == 0)
                    SQLStatement = SQLStatement + (string)o + "=";
                else
                {
                    if (o.GetType() == typeof(string))
                        SQLStatement = SQLStatement + " '" + (string)o + "',";
                    else if (o.GetType() == typeof(DateTime))
                        SQLStatement = SQLStatement + " '" + o.ToString() + "',";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Double))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Decimal))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Boolean))
                    {
                        if ((bool)o == true)
                            SQLStatement = SQLStatement + " 1" + ",";
                        else
                            SQLStatement = SQLStatement + " 0" + ",";
                    }
                    else
                        SQLStatement = SQLStatement + " '" + (string)o + "',";

                }
                i = i + 1;
            }
            if (SQLStatement.Length > 1)
                SQLStatement = SQLStatement.Substring(0, SQLStatement.Length - 1);

            StoredProcedure = "EXEC " + StoredProcedure + " " + SQLStatement;

            Command.CommandType = CommandType.Text;
            Command.CommandText = StoredProcedure;
            DataTable ResultSet = null;
            try
            {
                var adapter = new SqlDataAdapter(Command);
                ResultSet = new DataTable();
                adapter.Fill(ResultSet);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                errMsg = ex.Message.ToString();
                DataTable dt = null;
                return dt;
            }
            return ResultSet;

        }

        public SqlConnection OpenDBConnection()
        {
            conn = new SqlConnection(CommonHelper.GetConnectionString());
            conn.Open();
            return conn;
        }
        public void CloseDBConnection()
        {
            conn.Close();
        }

        public DataSet GetDBResultsDS(string StoredProcedure, params object[] ProcedureParameters)
        {

            int i = 0;
            DataSet ds = new DataSet();
            string SQLStatement = "";
            string DataProviderName = "System.Data.SqlClient";
            DbProviderFactory dpf = DbProviderFactories.GetFactory(DataProviderName);
            SqlConnection sql_Connection = dpf.CreateConnection() as SqlConnection;
            sql_Connection.ConnectionString = CommonHelper.GetConnectionString();
            sql_Connection.Open();
            SqlCommand Command = sql_Connection.CreateCommand();

            //if (HttpContext.Current.Session["DBType"].ToString() == "MSSQL")
            //{
            foreach (object o in ProcedureParameters)
            {
                if (i % 2 == 0)
                    SQLStatement = SQLStatement + (string)o + "=";
                else
                {
                    if (o.GetType() == typeof(string))
                        SQLStatement = SQLStatement + " '" + (string)o + "',";
                    else if (o.GetType() == typeof(DateTime))
                        SQLStatement = SQLStatement + " '" + o.ToString() + "',";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Double))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Int32))
                        SQLStatement = SQLStatement + o.ToString() + ",";
                    else if (o.GetType() == typeof(Boolean))
                    {
                        if ((bool)o == true)
                            SQLStatement = SQLStatement + " 1" + ",";
                        else
                            SQLStatement = SQLStatement + " 0" + ",";
                    }
                    else
                        SQLStatement = SQLStatement + " '" + (string)o + "',";

                }
                i = i + 1;
            }
            if (SQLStatement.Length > 1)
                SQLStatement = SQLStatement.Substring(0, SQLStatement.Length - 1);

            StoredProcedure = "EXEC " + StoredProcedure + " " + SQLStatement;
            Command.CommandTimeout = 36000;
            Command.CommandType = CommandType.Text;
            Command.CommandText = StoredProcedure;


            SqlDataAdapter adapter = new SqlDataAdapter(Command);
            adapter.Fill(ds);

            return ds;
            //}

        }

    }
}
