using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;
using BrDataEncryption;

namespace BRInvestmentPortal.AppCode
{
    public class Connector
    {
            DataTable rs = null;
            DataTable ResultSet = null;

            private static BrWebDataEcryption EncDepc = new BrWebDataEcryption();
            private SqlConnection conn = null;

            public Connector()
            {
               // HttpContext.Current.Session["DBType"] = "MSSQL";
            }



            public DataTable GetDBResults(ref String errMsg, string StoredProcedure, params object[] ProcedureParameters)
            {

                int i = 0;
                //string ParameterName = "";
                string SQLStatement = "";
               // string DataProviderName = "System.Data.SqlClient";

                SqlConnection BR_cnConnection = null;
                
                try
                {
                    BR_cnConnection = OpenDBConnection();
                }
                catch (System.InvalidOperationException ex)
                {
                    errMsg = ex.Message.ToString();
                    return rs;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    errMsg = ex.Message.ToString();
                    return rs;
                }
                catch (System.Exception ex)
                {
                    errMsg = ex.Message.ToString();
                    return rs;
                }

                SqlCommand Command = new SqlCommand();
                Command.Connection = OpenDBConnection();

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
                Command.CommandTimeout = 36000000;
                Command.CommandType = CommandType.Text;
                Command.CommandText = StoredProcedure;
                try
                {
                    var adapter = new SqlDataAdapter(Command);
                    ResultSet = new DataTable();
                    adapter.Fill(ResultSet);
                }
                catch (SqlException ex)
                {
                    errMsg = ex.Message.ToString();
                    return rs;
                }


                return ResultSet;
                //}

            }

            public string strConnection()
            { 
                string strDBServer = "";
                string strDBName = "";
                string strDBUserID = "";
                string strDBUserPWD = "";
                string strDataItem = "0" ;//WebConfigurationManager.AppSettings["DataItem"];

                strDBServer = WebConfigurationManager.AppSettings["DBServer"];
                strDBName = WebConfigurationManager.AppSettings["DBName"];
                strDBUserID = WebConfigurationManager.AppSettings["DBUserID"];
                strDBUserPWD = WebConfigurationManager.AppSettings["DBUserPWD"];


                if (strDataItem.ToString() == "1")
                {
                    strDBUserPWD = EncDepc.DecyptKey(strDBUserPWD).ToString();
                }
            //return  "user id=" + strDBUserID + ";password=" + DecryptItem(strDBUserPWD) + ";server=" + strDBServer + ";Trusted_Connection=no;database=" + strDBName + ";connection timeout=300";
            return ";server=" + strDBServer + ";Trusted_Connection=SSPI;database=" + strDBName + ";connection timeout=300";

        }

            public string EnConString()
            {
                string strDBServer = "";
                string strDBName = "";
                string strDBUserID = "";
                string strDBUserPWD = "";
                string strDataItem = "0";//WebConfigurationManager.AppSettings["DataItem"];

                strDBServer = EncDepc.EncryptString(WebConfigurationManager.AppSettings["DBServer"]).ToString();
                strDBName = EncDepc.EncryptString(WebConfigurationManager.AppSettings["DBName"]).ToString();
                strDBUserID = EncDepc.EncryptString(WebConfigurationManager.AppSettings["DBUserID"]).ToString();
                strDBUserPWD = EncDepc.EncryptString(DecryptItem(WebConfigurationManager.AppSettings["DBUserPWD"])).ToString();


                if (strDataItem.ToString() == "1")
                {
                    strDBUserPWD = EncDepc.DecyptKey(strDBUserPWD).ToString();
                }
            //return "user id=" + strDBUserID + ";password=" + strDBUserPWD + ";server=" + strDBServer + ";Trusted_Connection=no;database=" + strDBName + ";connection timeout=300";
            return "user id=sa;password=@groundzer036;server=(local);Trusted_Connection=no;database=Amana_Capital;connection timeout=300";
        }

            public SqlConnection OpenDBConnection()
            {
                string strDBServer = "";
                string strDBName = "";
                string strDBUserID = "";
                string strDBUserPWD = "";
                string strDataItem = "0" ;//WebConfigurationManager.AppSettings["DataItem"];

                strDBServer = WebConfigurationManager.AppSettings["DBServer"];
                strDBName = WebConfigurationManager.AppSettings["DBName"];
                strDBUserID = WebConfigurationManager.AppSettings["DBUserID"];
            string strstring = DecryptItem(WebConfigurationManager.AppSettings["DBUserPWD"]);
                strDBUserPWD = EncDepc.DecyptKey( WebConfigurationManager.AppSettings["DBUserPWD"]).ToString();
            //strDBUserPWD = strDBUserPWD.Substring(0, strDBUserPWD.Length - 1);
                if (strDataItem.ToString() == "1")
                {
                    strDBUserPWD = EncDepc.DecyptKey(strDBUserPWD).ToString();
                }
            //conn = new SqlConnection("user id=" + strDBUserID + ";password=" + DecryptItem(strDBUserPWD) + ";server=" + strDBServer + ";Trusted_Connection=no;database=" + strDBName + ";connection timeout=3000");
            //string strCon = "user id=" + strDBUserID + ";password=" + strDBUserPWD + ";server=" + strDBServer + ";Trusted_Connection=no;database=" + strDBName + ";connection timeout=3000";
            string strCon = "server=" + strDBServer + ";Trusted_Connection=SSPI;database=" + strDBName + ";connection timeout=3000";

            conn = new SqlConnection(strCon);
            conn.Open();
                return conn;
            }

            public void CloseDBConnection()
            {
                conn.Close();
                conn.Dispose();
            }


            public string DecryptItem(string StringToDecrypt)
            {
                string SeedValue = "0123456789ABCDEF";
                String Temp = "";

                int i, dblCountLength;
                int intLengthChar, intCountChar, intRandomSeed, intBeforeMulti, intAfterMulti, intSubNinetyNine, intInverseAsc;
                double dblCurrentChar;
                String strCurrentChar, Decrypt = "";

                if (SeedValue != "0123456789ABCDEF")
                    return "";

                try
                {
                    for (dblCountLength = 0; dblCountLength < StringToDecrypt.Length; dblCountLength++)
                    {
                        intLengthChar = System.Convert.ToInt16(StringToDecrypt.Substring(dblCountLength, 1));
                        strCurrentChar = StringToDecrypt.Substring(dblCountLength + 1, intLengthChar);
                        dblCurrentChar = 0;

                        for (intCountChar = 0; intCountChar < strCurrentChar.Length; intCountChar++)
                        {
                            double x = 93, y = System.Convert.ToDouble(strCurrentChar.Length - intCountChar - 1);
                            dblCurrentChar = dblCurrentChar + (strCurrentChar[intCountChar] - 33) * (Math.Pow(x, y));
                        }
                        intRandomSeed = System.Convert.ToInt16(dblCurrentChar.ToString().Substring(2, 2));

                        intBeforeMulti = System.Convert.ToInt32(dblCurrentChar.ToString().Substring(0, 2) + dblCurrentChar.ToString().Substring(4, 2));
                        intAfterMulti = intBeforeMulti / intRandomSeed;
                        intSubNinetyNine = intAfterMulti - 99;
                        intInverseAsc = 256 - intSubNinetyNine;
                        Decrypt = Decrypt + System.Convert.ToChar(intInverseAsc).ToString();
                        dblCountLength = dblCountLength + intLengthChar;
                    }

                    for (i = 0; i < Decrypt.Length; i++)
                    {
                        char c = (char)((int)(Decrypt[i] - (Decrypt.Length + i + 1)));
                        Temp = Temp + c.ToString();
                    }
                }
                catch (Exception se)
                {
                    ;
                }
                Decrypt = Temp;
                return Decrypt;
            }
    
    }
}