using System;
using System.Data;
using System.Configuration;
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
using Telerik.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Web.UI.Adapters;
using System.Net.Mail;
using System.Web.Mail;
using System.Reflection;
using System.Net;
using HFCPortal;
using BRIMSPortalWebService;

public partial class mainmenu : System.Web.UI.Page
{

    String strMsg = "";
    protected System.Web.UI.HtmlControls.HtmlInputFile File1;
    Functions fn;
    public char NoSpace = (char)160;
    DataAccess DataClass;
    BRInvestmentPortalService hfcs;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Keys.Count == 0 || Session["UserID"] == null || Session["UserID"].ToString() == "")
        {
            Response.Redirect("IBStart.aspx");
        }

        if (this.IsPostBack)
            return;

        //lblnorights.Visible = false;
        Response.Expires = -1;
        Response.Buffer = true;
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        strMsg = "";
        SaveActivity SaveUserActivity = new SaveActivity();


        SaveUserActivity.Activity = "Viewed Balances for all accounts";

        if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
        {
            lblMessage.Text = strMsg;
            return;
        }
        SaveUserActivity = null;


        RadGrid1.MasterTableView.DataKeyNames = new string[] { "OurBranchID", "AccountID", "CurrencyName", "AccountName" };
        //UpdateBalance();
        BindData();
        //BindDate();

    }

    protected void BindData()
    {
        lblMessageLine.Text = "";
        Functions fn = new Functions();
        DataAccess DataClass = new DataAccess();
        DataTable dtMainMenu = null;
        string strAccountIDS = string.Empty;


        try
        {

            DataTable dtResult = DataClass.GetDataTable(ref strMsg, "sp_GetLinkedAccounts",
                "@UserID", Session["UserID"].ToString().Replace("'", "''"));



            if (strMsg != "")
            {
                lblMessageLine.Text = strMsg;
                return;
            }

            if (dtResult.Rows.Count < 1)
            {
                lblMessageLine.Text = "Sorry, no accounts have been setup for your portal account.";
                return;
            }

            //Pass the AccountIDS to base

            if (hfcs == null)
            {
                hfcs = new BRInvestmentPortalService();
            }

            int rowNo = dtResult.Rows.Count - 1;

            foreach (DataRow dr in dtResult.Rows)
            {

                if (rowNo != 0)
                {
                    strAccountIDS = strAccountIDS + dr["AccountID"].ToString() + ",";
                }
                else
                {
                    strAccountIDS = strAccountIDS + dr["AccountID"].ToString();
                }

                rowNo -= 1;
            }

            if (strAccountIDS.EndsWith(","))
            {
                //Remove the separator
                strAccountIDS = strAccountIDS.Substring(0, strAccountIDS.Length - 1);
            }

            if (!string.IsNullOrEmpty(strAccountIDS))
            {


                string[] spParams = { "@OurBranchID", dtResult.Rows[0]["OurBranchID"].ToString(), "@AccountIDS", strAccountIDS };
                try
                {

                    dtMainMenu = hfcs.GetMainMenu(ref strMsg, spParams);
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    lblMessageLine.Text = "Please ensure that the web service is running.";
                }

            }
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLowerInvariant() == ("Unable to connect to the remote server").ToLowerInvariant())
            {
                fn.logError(ex.Message);
                lblMessageLine.ForeColor = System.Drawing.Color.Red;
                lblMessageLine.Text = "Please ensure that the web service is running.";
                return;
            }
            else
            {
                throw ex;
            }

        }

        RadGrid1.DataSource = dtMainMenu;
        RadGrid1.DataBind();

        DataClass = null;

        fn = null;
    }

   
    protected void GrdAccounts1_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "Select" && e.Item is GridDataItem)
        {
            e.Item.Selected = true;
            Session["AccountID"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AccountID"].ToString();
            Session["Acc_OurBranchID"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OurBranchID"].ToString();
            Session["Acc_Currency"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CurrencyName"].ToString();
            Session["AccountTitle"] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AccountName"].ToString();
            detailWindow();
        }

    }
    protected void detailWindow()
    {
        Response.Redirect("transactionsMenu.aspx");

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        string EmailBody;
        string Subject;
        string strAccountIDS = string.Empty;
        DataTable dtMainMenu = null;
        DataAccess DataClass = new DataAccess();
        DateTime Date = DateTime.Now;

        EmailBody = "<HTML>";
        EmailBody = EmailBody + "<HEAD>";
        EmailBody = EmailBody + "<TITLE>My Account</TITLE>";
        EmailBody = EmailBody + "</HEAD>";
        EmailBody = EmailBody + "<BODY>";
        EmailBody = EmailBody + "<TABLE Border = 1 BGCOLOR=#FFE0C0>";
        EmailBody = EmailBody + "<tr>";
        EmailBody = EmailBody + "<th>A/C Number </th>";
        EmailBody = EmailBody + "<th>Branch</th>";
        EmailBody = EmailBody + "<th>Acnt Type </th>";
        EmailBody = EmailBody + "<th>Balance</th>";
        EmailBody = EmailBody + "<th>Currency</th>";
        EmailBody = EmailBody + "</tr>";

        DataTable dtResult = DataClass.GetDataTable(ref strMsg, "sp_getAccountsRealBalance",
               "@UserID", Session["UserID"].ToString().Replace("'", "''"));



        if (strMsg != "")
        {
            lblMessageLine.Text = strMsg;
            return;
        }

        if (dtResult.Rows.Count < 1)
        {
            lblMessageLine.Text = "Sorry, no accounts have been setup for your portal account.";
            return;
        }

        //Pass the AccountIDS to base

        if (hfcs == null)
        {
            hfcs = new BRInvestmentPortalService();
        }

        int rowNo = dtResult.Rows.Count - 1;

        foreach (DataRow dr in dtResult.Rows)
        {

            if (rowNo != 0)
            {
                strAccountIDS = strAccountIDS + dr["AccountID"].ToString() + ",";
            }
            else
            {
                strAccountIDS = strAccountIDS + dr["AccountID"].ToString();
            }

            rowNo -= 1;
        }

        if (strAccountIDS.EndsWith(","))
        {
            //Remove the separator

        }

        if (!string.IsNullOrEmpty(strAccountIDS))
        {


            string[] spParams = { "@OurBranchID", dtResult.Rows[0]["OurBranchID"].ToString(), "@AccountIDS", strAccountIDS };
            try
            {

                dtMainMenu = hfcs.GetMainMenu(ref strMsg, spParams);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                lblMessageLine.Text = "Please ensure that the web service is running.";
            }

        }

        foreach (DataRow dr in dtMainMenu.Rows)
        {
            EmailBody = EmailBody + "<tr>";
            EmailBody = EmailBody + "<TD>" + dr["AccountID"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + dr["OurBranchID"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + dr["AccountName"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + dr["AccountBalance"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + dr["CurrencyName"].ToString() + "</TD>";
            EmailBody = EmailBody + "</tr>";
        }
        EmailBody = EmailBody + "</TABLE>";
        EmailBody = EmailBody + "</BODY>";
        EmailBody = EmailBody + "</HTML>";


        Subject = "My Accounts";

        Functions fn = new Functions();
        strMsg = "";
        string Error = fn.SendE_mail(Session["Email"].ToString(), Subject, EmailBody);
        if (Error != "")
        {
            fn.logError(strMsg);
            lblMessageLine.Text = "Error sending e-mail";
        }

        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Account(s) summary information has been sent to your email address.";
        }
        fn = null;
        DataClass = null;


    }
    private void ShowPdf(string strS)
    {
        strS = "MyAccount.pdf";
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + strS);
        Response.TransmitFile(strS);
        Response.End();
        //Response.WriteFile(strS);
        Response.Flush();
        Response.Clear();
    }

    protected void GrdAccounts_DataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    //{
    //    double Number;
    //    string Data;

    //    foreach (GridDataItem item in RadGrid1.Items)
    //    {

    //        Number = 0;
    //        Data = item["Tot"].Text;

    //        if (double.TryParse(Data, out Number) == true)
    //        {
    //            Number = System.Convert.ToDouble(Data);


    //            if (double.TryParse(Data, out Number) == true)
    //            {
    //                Number = System.Convert.ToDouble(Data);

    //                if (Number < 0)
    //                {
    //                    item["Tot"].ForeColor = System.Drawing.Color.Red;
    //                    item["Tot"].Text = "(" + String.Format("{0:N}", Number).Substring(1, String.Format("{0:N}", Number).Length - 1) + ")";
    //                }
    //                else
    //                {

    //                    item["Tot"].Text = String.Format("{0:N}", Number);
    //                }
    //                item["Tot"].HorizontalAlign = HorizontalAlign.Right;
    //            }
    //            else
    //                item["Tot"].Text = NoSpace.ToString();




    //        }
    //    }
    //}



}


