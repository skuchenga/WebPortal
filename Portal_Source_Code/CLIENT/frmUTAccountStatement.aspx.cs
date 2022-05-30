using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using HFCPortal;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using HFCPortal.Common;
using BRIMSPortalWebService;
using BrDataEncryption;

public partial class frmUTAccountStatement : System.Web.UI.Page
{
    string str;
    Functions fn;
    ReportDocument rpt;
    BRInvestmentPortalService hfcs;
    private static BrWebDataEcryption EncDepc = new BrWebDataEcryption();

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session.Keys.Count == 0 || Session["UserID"] == null || Session["UserID"].ToString() == "")
        {
            Response.Redirect("IBStart.aspx");
        }

        ConfigureCrystalReports();
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.Keys.Count == 0 || Session["UserID"] == null || Session["UserID"].ToString() == "")
        {
            Response.Redirect("IBStart.aspx");
        }

        fn = new Functions();

        try
        {

            ////Load the report
            //if (!Page.IsPostBack)
            //{
            //    ConfigureCrystalReports();
            //}
            // ConfigureCrystalReports();
        }
        catch (System.Exception ex)
        {

            fn.logError(ex.Message);
        }

    }

    private void ConfigureCrystalReports()
    {
        try
        {

            rpt = new ReportDocument();

            string reportPath = Server.MapPath("rptUTAccountStatement.rpt");


            CommonHelper.SetConnectionProperties();

            rpt.Load(reportPath);

            rpt.Refresh();
            rpt.SetParameterValue("@OurBranchID", Session["Acc_OurBranchID"].ToString());
            rpt.SetParameterValue("@FromAccountID", Session["AccountID"].ToString());
            rpt.SetParameterValue("@ToAccountID", Session["AccountID"].ToString());
            rpt.SetParameterValue("@FromDate", Session["TransFromDate"].ToString());
            rpt.SetParameterValue("@ToDate", Session["TransToDate"].ToString());
            rpt.SetParameterValue("@Currency", Session["Acc_Currency"]);
            rpt.SetParameterValue("@DateOrValue", null);
            rpt.SetParameterValue("@ReturnFields", null);
            rpt.SetParameterValue("@ReturnFilters", null);
            rpt.SetParameterValue("Map", "Bankers Realm  1.045p");
            rpt.SetParameterValue("OperatorID", Session["UserID"].ToString());

            

            if (hfcs == null)
            {
                hfcs = new BRInvestmentPortalService();
            }


            ConnectionInfo connectionInfo = new ConnectionInfo();

            string conString = hfcs.GetMyString();
            string[] conProperties;

            if (!string.IsNullOrEmpty(conString))
            {
                conProperties = conString.Split(';');
                foreach (string conprop in conProperties)
                {
                    if (conprop.Contains("server="))
                    {
                        connectionInfo.ServerName = EncDepc.DecyptKey(conprop.Replace("server=", "")).ToString();
                    }
                    else if (conprop.Contains("database="))
                    {
                        connectionInfo.DatabaseName = EncDepc.DecyptKey(conprop.Replace("database=", "")).ToString();
                    }
                    else if (conprop.Contains("user id="))
                    {
                        connectionInfo.UserID = EncDepc.DecyptKey(conprop.Replace("user id=", "")).ToString();
                    }
                    else if (conprop.Contains("password="))
                    {
                        connectionInfo.Password = EncDepc.DecyptKey(conprop.Replace("password=", "")).ToString();
                    }

                }

            }
         

            crUTAccountStatement.ReportSource = rpt;
            SetDBLogonForReport(connectionInfo, rpt);
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SetDBLogonForReport(ConnectionInfo myConnectionInfo, ReportDocument myReportDocument)
    {
        Tables myTables = myReportDocument.Database.Tables;
        foreach (CrystalDecisions.CrystalReports.Engine.Table myTable in myTables)
        {
            TableLogOnInfo myTableLogonInfo = myTable.LogOnInfo;
            myTableLogonInfo.ConnectionInfo = myConnectionInfo;
            myTable.ApplyLogOnInfo(myTableLogonInfo);
        }
    }

    private void SetDBLogonForSubreports(ConnectionInfo myConnectionInfo, ReportDocument myReportDocument)
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

    protected void btnBack_Click(object sender, EventArgs e)
    {

        try
        {
            Response.Redirect("transactionsMenu.aspx");
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }

    //  private void ExportTo

}