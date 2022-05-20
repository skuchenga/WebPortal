using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Common;
using Telerik.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Net;
using System.Net.Mail;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using HFCPortal;
using HFCPortal.Common;
using BRIMSPortalWebService;
using BrDataEncryption;
using System.Drawing;

public partial class Default2 : System.Web.UI.Page
{
    String strMsg = "";
    Functions fn;
    ReportDocument rpt;
    BRInvestmentPortalService hfc = null;
    private static BrWebDataEcryption EncDepc = new BrWebDataEcryption();

    protected void Page_Load(object sender, EventArgs e)
    {

        this.Form.Attributes.Add("autocomplete", "off");
        if (Session.Keys.Count == 0 || Session["LoginID"] == null || Session["LoginID"].ToString() == "")
        {
            HttpContext.Current.Session.Clear();
            Server.Transfer("Login.aspx");

        }
        if (this.IsPostBack)
            return;
        //LoadAccounts();
        //LoadFromAccounts();
        //LoadToAccount();
        F2Config();

    }

    private void F2Config()
    { 
        //FROM AccountID
        txtFromAccountID.spName = "help_GetUTAccountID";
        txtFromAccountID.SelectColumn = "AccountID";
        txtFromAccountID.Param1 = "AccountID";
        txtFromAccountID.Param2 = "ClientID";
        txtFromAccountID.Param3 = "FundID";

        txtToAccountID.spName = "help_GetUTAccountID";
        txtToAccountID.SelectColumn = "AccountID";
        txtToAccountID.Param1 = "AccountID";
        txtToAccountID.Param2 = "ClientID";
        txtToAccountID.Param3 = "FundID";


    }

    protected override void OnPreRender(EventArgs e)
    {
        //java-script
        string adminJs = CommonHelper.GetSystemLocation() + "Scripts/admin.js";
        Page.ClientScript.RegisterClientScriptInclude(adminJs, adminJs);

        base.OnPreRender(e);
    }

    protected void LoadAccounts()
    {
        lblMsg.Text = "";
        fn = new Functions();
        if (hfc == null)
        {
            hfc = new BRInvestmentPortalService();
        }

        DataTable dt = null;

        try
        {
            dt = hfc.GetUTAccountsWeb(ref strMsg);
        }

        catch (System.Net.Sockets.SocketException ex)
        {
            lblMsg.Text = "Please ensure that the web service is running.";
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLowerInvariant() == ("Unable to connect to the remote server").ToLowerInvariant())
            {
                fn.logError(ex.Message);
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please ensure that the web service is running.";
                return;
            }
            else
            {
                throw ex;
            }

        }
        if (strMsg != "")
        {
            fn.logError(strMsg);
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "sorry an error occurred";
            return;
        }

        fn.FillCombo(dt, "cmbFromAccount", this.Page);
        fn.FillCombo(dt, "cmbToAccount", this.Page);
        fn = null;
        dt.Dispose();
    }

   

    protected void CmdSend_Click(object sender, EventArgs e)
    {
        //Get the accounts
        lblMsg.Text = "";
        fn = new Functions();
        if (hfc == null)
        {
            hfc = new BRInvestmentPortalService();
        }
        

        if (dtFromDate.SelectedDate == null)
        {
            lblMsg.Text = "Please select a valid Date From.";
            return;
        }

        if (dtToDate.SelectedDate == null)
        {
            lblMsg.Text = "Please select a valid Date To";
            return;
        }

        DateTime? DateFrom = dtFromDate.SelectedDate;
        DateTime? DateTo = dtToDate.SelectedDate;

        //DateTime fromDate = 
        //string FromAccountID = cmbFromAccount.SelectedItem.Text;
        //string ToAccountID = cmbToAccount.SelectedItem.Text;
        
        //FromAccountID =    FromAccountID.Substring(FromAccountID.LastIndexOf("-") + 1, (FromAccountID.Length -1) - FromAccountID.LastIndexOf("-"));
        //ToAccountID = ToAccountID.Substring(ToAccountID.LastIndexOf("-") + 1, (ToAccountID.Length -1) - ToAccountID.LastIndexOf("-"));

        string[] spParams = {"@OurBranchID", "001",
               "@FromAccountID", txtFromAccountID.Text,
               "@ToAccountID", txtToAccountID.Text,
               "@FromDate", fn.getCustomDate(DateTime.Parse(DateFrom.ToString())),
               "@ToDate", fn.getCustomDate(DateTime.Parse(DateFrom.ToString())) }; 

        DataTable dt = null;
        try
        {
            dt = hfc.GetUTAccountStatement(ref strMsg, spParams);
        }

        catch (System.Net.Sockets.SocketException ex)
        {
            lblMsg.Text = "Please ensure that the web service is running.";
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLowerInvariant() == ("Unable to connect to the remote server").ToLowerInvariant())
            {
                fn.logError(ex.Message);
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please ensure that the web service is running.";
                return;
            }
            else
            {
                throw ex;
            }

        }

        //core.SetConnectionProperties();

        if (strMsg != "")
        {
            fn.logError(strMsg);
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "sorry an error occurred";
            return;
        }

       List<string> lstAccounts = new List<string>();
        SortedDictionary<string, string> accountEmails = new SortedDictionary<string,string>();

        foreach (DataRow dr in dt.Rows)
        {

            if (!lstAccounts.Contains(dr["AccountID"].ToString())) 
            {
                lstAccounts.Add(dr["AccountID"].ToString());
                accountEmails.Add(dr["AccountID"].ToString(), dr["utccontactemail"].ToString());
            }
        }

        //string OurBranchID = "001";
        //string Currency = "18";
        string OperatorID = Session["LoginID"].ToString();

        
        string reportPath = Server.MapPath("rptUTAccountStatement.rpt");


        CommonHelper commonHelper = new CommonHelper();
        CrystalReportsHelper crHelper = new CrystalReportsHelper();
        PDFHelper pdfHelper = new PDFHelper();

        try { 
        CommonHelper.SetConnectionProperties();


        foreach (string accountID in lstAccounts)
        {
            string fileName = string.Format("{0}-Transactions_{1}.pdf", accountID, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            string filePath2 = string.Format("{0}ExportImport\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);
            string filePath = string.Format("{0}ExportImport\\{1}", HttpContext.Current.Server.MapPath("~"), fileName);

            rpt = new ReportDocument();
            rpt.Load(reportPath);

            //ConfigureCrystalReports();

            //Generate Crystal report
            string success = GenerateAccountStatements(rpt, "001", accountID, accountID, DateFrom.ToString(), DateTo.ToString(), null, OperatorID);
                       

            //Generate pdf 
            if (rpt != null)
            {
                string strPdfExportMessage = crHelper.ExportReportToPDF(rpt, filePath);
                if (!string.IsNullOrEmpty(strPdfExportMessage)) 
                {
                    throw new ArgumentException(strPdfExportMessage);    
                }
            }


            //send email
            string accountEmailID = string.Empty;
            string emailResponse = string.Empty;

            accountEmails.TryGetValue(accountID, out accountEmailID);
            lblMsg.ForeColor = Color.Blue;
            if (!string.IsNullOrEmpty(accountEmailID))
            {
                emailResponse = pdfHelper.sendPDFEmail(filePath, accountEmailID, txtEmailSubject.Text, txtEmailBody.Text);
            }
            else 
            {
                lblMsg.Text = lblMsg.Text + "<BR/>" + accountID + " : Email address not found";
            }

            if (string.IsNullOrEmpty(emailResponse))
            {

                if (string.IsNullOrEmpty(lblMsg.Text))
                {
                    lblMsg.Text = "Statements Successfully sent.";
                }   
            }
            else
            {
                lblMsg.ForeColor = Color.Red;
                if (!string.IsNullOrEmpty(accountEmailID))
                {
                    lblMsg.Text = lblMsg.Text + "<BR/>" + accountID + " : " + accountEmailID + " : " +  emailResponse;
                }
            }

        }
            

        }
        catch (System.Exception ex)
        {
            lblMsg.ForeColor = Color.Red;
            lblMsg.Text = lblMsg.Text + "<BR/>" + ex.Message;

            throw ex;
        }   
    }

    string strMessage = string.Empty;

    protected string GenerateAccountStatements(ReportDocument rpt, string OurBranchID, string FromAccountID, string ToAccountID, string FromDate, string ToDate, string Currency, string OperatorID)
    {
        try
        {


            //string reportPath = Server.MapPath("rptUTAccountStatement.rpt");


            CommonHelper.SetConnectionProperties();



            rpt.SetParameterValue("@OurBranchID", OurBranchID);
            rpt.SetParameterValue("@FromAccountID", FromAccountID);
            rpt.SetParameterValue("@ToAccountID", ToAccountID);
            rpt.SetParameterValue("@FromDate", FromDate);
            rpt.SetParameterValue("@ToDate", ToDate);
            rpt.SetParameterValue("@Currency", Currency);
            rpt.SetParameterValue("@DateOrValue", null);
            rpt.SetParameterValue("@ReturnFields", null);
            rpt.SetParameterValue("@ReturnFilters", null);
            rpt.SetParameterValue("Map", "Bankers Realm  1.045p");
            rpt.SetParameterValue("OperatorID", OperatorID);

            //rpt.SetDatabaseLogon(coreDB.dbUser, coreDB.dbPWD, coreDB.dbServer, coreDB.dbName);

            if (hfc == null)
            {
                hfc = new BRInvestmentPortalService();
            }


            ConnectionInfo connectionInfo = new ConnectionInfo();

            string conString = hfc.GetMyString();
            string[] conProperties;

            if (!string.IsNullOrEmpty(conString))
            {
                conProperties = conString.Split(';');
                foreach (string conProp in conProperties)
                {
                    if (conProp.Contains("server="))
                    {
                        connectionInfo.ServerName = EncDepc.DecyptKey(conProp.Replace("server=", "")).ToString();
                    }
                    else if (conProp.Contains("database="))
                    {
                        connectionInfo.DatabaseName = EncDepc.DecyptKey(conProp.Replace("database=", "")).ToString();
                    }
                    else if (conProp.Contains("user id="))
                    {
                        connectionInfo.UserID = EncDepc.DecyptKey(conProp.Replace("user id=", "")).ToString();
                    }
                    else if (conProp.Contains("password="))
                    {
                        connectionInfo.Password = EncDepc.DecyptKey(conProp.Replace("password=", "")).ToString();
                    }

                }

            }

            connectionInfo.IntegratedSecurity = false;




            SetDBLogonForReport(connectionInfo, rpt);


            return string.Empty;
            /*CrystalReportViewer1.EnableDatabaseLogonPrompt = false;

            CrystalReportViewer1.LogOnInfo;*/

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void SetDBLogonForReport(ConnectionInfo myConnectionInfo, ReportDocument myReportDocument)
    {
        Tables myTables = myReportDocument.Database.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table myTable in myTables)
        {
            TableLogOnInfo myTableLogonInfo = myTable.LogOnInfo;
            myTableLogonInfo.ConnectionInfo = myConnectionInfo;
            myTable.ApplyLogOnInfo(myTableLogonInfo);
        }
    }

    protected void SetDBLogonForSubreports(ConnectionInfo myConnectionInfo, ReportDocument myReportDocument)
    {
        Sections mySections = myReportDocument.ReportDefinition.Sections;
        foreach (Section mySection in mySections)
        {
            ReportObjects myReportObjects = mySection.ReportObjects;
            foreach (ReportObject myReportObject in myReportObjects)
            {
                if (myReportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject mySubreportObject = (SubreportObject)myReportObject;
                    ReportDocument subReportDocument = mySubreportObject.OpenSubreport(mySubreportObject.SubreportName);
                    SetDBLogonForReport(myConnectionInfo, subReportDocument);
                }
            }
        }
    } 

}