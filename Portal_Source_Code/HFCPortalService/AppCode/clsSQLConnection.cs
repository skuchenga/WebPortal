using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.Configuration;
using BrDataEncryption;

namespace HFCPortalService.AppCode
{
    public class clsSQLConnection
    {
        private static BrWebDataEcryption EncDepc = new BrWebDataEcryption();
        private SqlConnection conn = null;

        public SqlConnection OpenDBConnection()
        {
            string strDBServer = "";
            string strDBName = "";
            string strDBUserID = "";
            string strDBUserPWD = "";
            string strDataItem = WebConfigurationManager.AppSettings["DataItem"];

            strDBServer = WebConfigurationManager.AppSettings["DBServer"];
            strDBName = WebConfigurationManager.AppSettings["DBName"];
            strDBUserID = WebConfigurationManager.AppSettings["DBUserID"];
            strDBUserPWD = WebConfigurationManager.AppSettings["DBUserPWD"];


            if (strDataItem.ToString() == "1")
            {
                strDBUserPWD = EncDepc.DecyptKey(strDBUserPWD).ToString();
            }
            conn = new SqlConnection("user id=" + strDBUserID + ";password=" + strDBUserPWD + ";server=" + strDBServer + ";Trusted_Connection=no;database=" + strDBName + ";connection timeout=300");
            conn.Open();
            return conn;
        }
        public void CloseDBConnection()
        {
            conn.Close();
        }


    }
}
