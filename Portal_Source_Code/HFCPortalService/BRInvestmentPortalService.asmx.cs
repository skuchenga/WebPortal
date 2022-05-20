using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.Services;
using BRInvestmentPortal.AppCode;

namespace BRInvestmentPortal
{
    /// <summary>
    /// Summary description for HFCBaseService
    /// </summary>
    [WebService(Namespace = "http://localhost/HFCPortalService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BRInvestmentPortalService : System.Web.Services.WebService
    {

        SortedDictionary<string,string> currencyLookup =null;
        private Connector conn;

        [WebMethod]
        public DataTable QueryDefault(ref String errMsg, string StoredProcedure, params object[] ProcedureParameters)
        {
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, StoredProcedure, ProcedureParameters);
            conn.CloseDBConnection(); 
            return dr;
        }

        [WebMethod]
        public DataTable BRIMSData(ref String errMsg, string StoredProcedure, params object[] ProcedureParameters)
        {
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, StoredProcedure, ProcedureParameters);
            dr.TableName = "BRIMS datatable";
            conn.CloseDBConnection();
            return dr;
        }


        //[WebMethod]
        //public DataTable GetUTAccountsWeb(ref String errMsg)
        //{

        //    string sp = "sp_GetUTAccounts_web";
        //    if (conn == null)
        //    {
        //        conn = new Connector();
        //    }
        //    DataTable dr = conn.GetDBResults(ref errMsg, sp);
        //    dr.TableName = "ClientAccount";
        //    conn.CloseDBConnection();
        //    return dr;
        //}

        [WebMethod]
        public DataTable GetUTAccountsWeb(ref string errMsg)
        {
            
            string sp = "sp_GetUTAccounts_web";
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, sp);
            dr.TableName = "ClientAccount";
            conn.CloseDBConnection();
            return dr;
        }

        [WebMethod]
        public String  GetCurrencyName(string CurrencyID)
        {
            string currencyName = string.Empty;
            String errMsg = string.Empty;

            if (currencyLookup == null)
            {
                FillCurrencyDictionary();
            }

            currencyLookup.TryGetValue(CurrencyID, out currencyName);

            if (string.IsNullOrEmpty(currencyName))
            {
                FillCurrencyDictionary();
                currencyLookup.TryGetValue(CurrencyID, out currencyName);
            }

            return currencyName;
        }


        [WebMethod]
        public DataTable GetClientAccount(ref String errMsg, params object[] ProcedureParameters)
        {
            //string errMsg = "";
            String sp = "sp_getClientAccount";
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, sp, ProcedureParameters);
            dr.TableName = "ClientAccount";
            conn.CloseDBConnection();
            return dr;
        }

        [WebMethod]
        public DataTable GetClientsAccount()
        {
            String errMsg = string.Empty;
            object[] ProcedureParameters = new object[] { "@OurBranchID","001", "@AccountID","0000000087" } ;
            //string errMsg = "";
            String sp = "sp_getClientAccount";
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, sp, ProcedureParameters);
            dr.TableName = "ClientAccount";
            conn.CloseDBConnection();
            return dr;
        }

        [WebMethod]
        public DataTable GetAccountNAV(ref String errMsg, params object[] ProcedureParameters)
        {
            String sp = "sp_GetAccountsNAV";
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, sp, ProcedureParameters);
            dr.TableName = "AccountNAV";
            return dr;
        }
        
        [WebMethod]
        public DataTable GetUTAccountStatement(ref String errMsg, params object[] ProcedureParameters)
        {

            String sp = "rpt_UTAccountStatement";
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, sp,ProcedureParameters);
            dr.TableName = "AccountStatement";
            conn.CloseDBConnection();
            return dr;
        }

        [WebMethod]
        public DataTable GetFundType(ref String errMsg, params object[] ProcedureParameters)
        {

            String sp = "portal_GetFundType";
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, sp, ProcedureParameters);
            dr.TableName = "FundTypes";
            conn.CloseDBConnection();
            return dr;
        }

        [WebMethod]
        public DataTable GetMainMenu(ref String errMsg, params object[] ProcedureParameters)
        {

            String sp = "Portal_MainMenu";
            if (conn == null)
            {
                conn = new Connector();
            }
            DataTable dr = conn.GetDBResults(ref errMsg, sp, ProcedureParameters);
            dr.TableName = "MainMenu";
            conn.CloseDBConnection();
            return dr;
        }


        [WebMethod]
        public string  GetMyString()
        {

            if (conn == null)
            {
                conn = new Connector();
            }

            string myString = conn.EnConString();
            return myString; 
            
        }


        protected void FillCurrencyDictionary()
        {
           
            String sp = "sp_GetCurrencyDictionary";
            String errMsg = string.Empty;

                currencyLookup = new SortedDictionary<string, string>();
                Connector conn = new Connector();
                DataTable dt = conn.GetDBResults(ref errMsg, sp);

                foreach (DataRow dr in dt.Rows)
                {
                    currencyLookup.Add(dr["CurrencyID"].ToString(), dr["CurrencyName"].ToString());
                }

            }

    }
}
