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
using System.Net.Mail;
using System.Web.Mail;
using System.Data.Common;
//using Telerik.WebControls;
using Telerik.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Web.UI.Adapters;
using HFCPortal;
using BRIMSPortalWebService;


public partial class TransactionsMenu : System.Web.UI.Page
{
    public char NoSpace = (char)160;
    String strMsg = "";
    int SumCredit = 0, SumDebit = 0;
    BRInvestmentPortalService hfcs;

    string IBmessage = "", CURRENCY = "";
    Functions fn;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session.Keys.Count == 0 || Session["UserID"] == null || Session["UserID"].ToString() == "")
        {
            Response.Redirect("IBStart.aspx");
        }


        if (this.IsPostBack)
            return;

        //if (Session["Acc_Currency"].ToString() == "KES")
        //{
        //    CURRENCY = "L";
        //}
        //else
        //{
        //    CURRENCY = "F";
        //}


        if (hfcs == null)
        {
            hfcs = new BRInvestmentPortalService();
        }

        string strCurrencyName = hfcs.GetCurrencyName(Session["Acc_Currency"].ToString());

        lblAccountTitle.Text = "Branch ID :: " + Session["Acc_OurBranchID"].ToString() + "  ,  " + "Account ID : " + Session["AccountID"].ToString() + "  ,  " + "CURRENCY :  " + strCurrencyName;// + " :: " +  Session["AccountTitle"].ToString();
        //btnGenerateReport.OnClientClick = string.Format("javascript:OpenWindow('frmUTAccountStatement.aspx', 800, 600, true); return false;");
        Session["Account_Details"] = lblAccountTitle.Text;
        RadDateInput1.SelectedDate = DateTime.Now;
        RadDateInput2.SelectedDate = DateTime.Now;
        Session["AccFromDate"] = RadDateInput1.SelectedDate;
        Session["AccTodate"] = RadDateInput2.SelectedDate;
        try
        {
            LOADTRANSACTIONS();
        }
        catch (Exception ex)
        {
            lblMessageLine.Text = ex.Message;
        }


        strMsg = "";
        SaveActivity SaveUserActivity = new SaveActivity();
        SaveUserActivity.Activity = "Viewed Statement For Accounts" + Session["Acc_OurBranchID"].ToString() + " - " + Session["AccountID"].ToString();
        if (SaveUserActivity.SaveUserActivity(ref strMsg) != "")
        {
            Session["ErrorLogMessage"] = strMsg;
        }
        SaveUserActivity = null;

    }
    public bool IsNumeric(string input)
    {
        bool result = true;
        for (int i = 0; i < input.Length; i++)
        {
            if (!Char.IsNumber(input[i]))
            {
                result = false;
                break;
            }
        }
        return result;
    }
    protected void grdStatement_DataBound(object sender, EventArgs e)
    {
        double Number;
        string Data;
        DateTime Date;
        Double minBalance = 0;
        Double maxBalance = 0;
        try
        {

            foreach (GridDataItem item in grdStatement.Items)
            {

                Data = item["Date"].Text;
                if (DateTime.TryParse(Data, out Date) == true)
                {
                    Date = System.Convert.ToDateTime(item["Date"].Text);
                    item["Date"].Text = String.Format("{0:dd-MMM-yyyy}", Date);
                }
                else
                    item["Date"].Text = NoSpace.ToString();

                Data = item["Value"].Text;
                if (DateTime.TryParse(Data, out Date) == true)
                {
                    Date = System.Convert.ToDateTime(item["Value"].Text);
                    item["Value"].Text = String.Format("{0:dd-MMM-yyyy}", Date);
                }
                else
                    item["Value"].Text = NoSpace.ToString();

                Number = 0;
                Data = item["Debit"].Text;

                if (double.TryParse(Data, out Number) == true)
                {
                    Number = System.Convert.ToDouble(Data);
                    if (Number < 0)
                        item["Debit"].ForeColor = System.Drawing.Color.Red;

                    item["Debit"].Text = String.Format("{0:N}", Number);
                    item["Debit"].HorizontalAlign = HorizontalAlign.Right;
                }
                else
                    item["Debit"].Text = NoSpace.ToString();

                Number = 0;
                Data = item["Credit"].Text;

                if (double.TryParse(Data, out Number) == true)
                {
                    Number = System.Convert.ToDouble(Data);
                    if (Number < 0)
                        item["Credit"].ForeColor = System.Drawing.Color.Red;

                    item["Credit"].Text = String.Format("{0:N}", Number);
                    item["Credit"].HorizontalAlign = HorizontalAlign.Right;
                }
                else
                    item["Credit"].Text = NoSpace.ToString();

                Number = 0;
                Data = item["Closing"].Text;

                if (double.TryParse(Data, out Number) == true)
                {
                    Number = System.Convert.ToDouble(Data);
                    if (minBalance > Number)
                    {
                        minBalance = Number;
                    }
                    if (maxBalance < Number)
                    {
                        maxBalance = Number;
                    }
                    if (Number < 0)
                        item["Closing"].ForeColor = System.Drawing.Color.Red;

                    item["Closing"].Text = String.Format("{0:N}", Number);
                    item["Closing"].HorizontalAlign = HorizontalAlign.Right;
                }
                else
                    item["Closing"].Text = NoSpace.ToString();


            }
        }
        catch (Exception ex)
        {
            lblMessageLine.Text = ex.ToString();
        }

    }
    protected void LOADTRANSACTIONS()
    {
        fn = new Functions();
        lblMessageLine.Text = "";
        try
        {
            if (hfcs == null)
            {
                hfcs = new BRInvestmentPortalService();
            }

            string[] spParams = {"@OurBranchID", Session["Acc_OurBranchID"].ToString(),
                "@FromAccountID", Session["AccountID"].ToString(),
                "@ToAccountID", Session["AccountID"].ToString(),
                "@FromDate", fn.getCustomDate(DateTime.Parse(RadDateInput1.SelectedDate.ToString())),
                "@ToDate", fn.getCustomDate(DateTime.Parse(RadDateInput2.SelectedDate.ToString()))};

            DataTable dt = null;
            try
            {
                dt = hfcs.GetUTAccountStatement(ref strMsg, spParams);
            }

            catch (System.Net.Sockets.SocketException ex)
            {
                lblMessageLine.Text = "Please ensure that the web service is running.";
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



            Session["TransFromDate"] = fn.getCustomDate(DateTime.Parse(RadDateInput1.SelectedDate.ToString()));
            Session["TransToDate"] = fn.getCustomDate(DateTime.Parse(RadDateInput2.SelectedDate.ToString()));

            if (strMsg != "")
            {
                lblMessageLine.ForeColor = System.Drawing.Color.Red;
                lblMessageLine.Text = "Error occurred.";
                fn.logError(strMsg);
                return;
            }

            grdStatement.DataSource = dt;
            grdStatement.DataBind();
        }
        catch (Exception ex)
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Red;
            lblMessageLine.Text = "Error occurred.";
            fn.logError(ex.Message);
        }
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {

        LOADTRANSACTIONS();

    }
    protected void GrdAccounts_OnItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.CommandName == "btnDetail")
        {

            Response.Redirect(string.Format("TransactionDetail.aspx?RowID=" + e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RowID"].ToString() + ""));
        }
    }
    protected void a(object sender, EventArgs e)
    {
        Response.Redirect("mainmenu.aspx");
    }
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("mainmenu.aspx");

    }
    protected void Lkbprint_Click(object sender, EventArgs e)
    {
        DataAccess DataClass = new DataAccess();
        DbDataReader rsTrx = DataClass.GetDBResults(ref strMsg, "sp_AccountStatementViewIBanking", "@OurBranchID", Session["Acc_OurBranchID"].ToString(), "@AccountID", Session["AccountID"].ToString(), "@FromDate", RadDateInput1.SelectedDate.Value.Day.ToString() + "/" + RadDateInput1.SelectedDate.Value.Month.ToString() + "/" + RadDateInput1.SelectedDate.Value.Year.ToString(), "@ToDate", RadDateInput2.SelectedDate.Value.Day.ToString() + "/" + RadDateInput2.SelectedDate.Value.Month.ToString() + "/" + RadDateInput2.SelectedDate.Value.Year.ToString(), "@Currency", Session["Acc_Currency"].ToString());
        if (strMsg != "")
        {
            Session["ErrorLogMessage"] = strMsg;
        }
        grdStatement.DataSource = rsTrx;
        grdStatement.DataBind();

        rsTrx.Close();
        rsTrx.Dispose();
        lblMessageLine.ForeColor = System.Drawing.Color.Blue;
    }
    protected void ExportStatementFromBR()
    {
        grdStatement.ExportSettings.ExportOnlyData = true;
        grdStatement.ExportSettings.IgnorePaging = true;
        grdStatement.ExportSettings.OpenInNewWindow = true;
        grdStatement.ExportSettings.HideStructureColumns = false;
        grdStatement.ExportSettings.FileName = "Account_Transactions";
        switch (cboExportOptions.SelectedValue)
        {
            case "Excel":
                {
                    grdStatement.MasterTableView.ExportToExcel();
                    break;
                }
            case "PDF":
                {
                    grdStatement.MasterTableView.ExportToPdf();
                    break;
                }

            case "CSV":
                {
                    grdStatement.MasterTableView.ExportToCSV();
                    break;
                }

            case "Word":
                {
                    grdStatement.MasterTableView.ExportToWord();
                    break;
                }
            case "Detailed":
                {
                    Response.Redirect("PrintTransactions.apsx");
                    break;
                }

        }
    }
    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        Session["TransFromDate"] = RadDateInput1.SelectedDate;
        Session["TransToDate"] = RadDateInput2.SelectedDate;
        Response.Redirect("frmUTAccountStatement.aspx");
    }

    protected void cmdExport_Click(object sender, EventArgs e)
    {
        //ExportStatementFromBR();
        //return;
        Session["AccFromDate"] = RadDateInput1.SelectedDate;
        Session["AccTodate"] = RadDateInput2.SelectedDate;
        grdStatement.ExportSettings.ExportOnlyData = true;
        grdStatement.ExportSettings.IgnorePaging = true;
        grdStatement.ExportSettings.OpenInNewWindow = true;
        grdStatement.ExportSettings.HideStructureColumns = false;
        grdStatement.ExportSettings.FileName = "Account_Transactions";
        switch (cboExportOptions.SelectedValue)
        {
            case "Excel":
                {
                    grdStatement.MasterTableView.ExportToExcel();
                    break;
                }
            case "PDF":
                {
                    //GrdAccounts.ExportSettings.Pdf.PageTitle = "Account Statement [";//+ Request.QueryString["OurBranchID"].ToString() + " :: " + Request.QueryString["AccountID"].ToString() + " :: From Date - " + dtFromDate.SelectedDate.ToString() + " :: To Date - " + dtToDate.SelectedDate.ToString() + "]";
                    grdStatement.MasterTableView.ExportToPdf();

                    break;
                }
            case "CSV":
                {
                    grdStatement.MasterTableView.ExportToCSV();
                    //grdStatement.MasterTableView();
                    break;
                }
            case "Word":
                {
                    grdStatement.MasterTableView.ExportToWord();
                    break;
                }
            case "Detailed":
                {

                    Response.Redirect("frmReportViewer.aspx?OurBranchID=" +
                        HttpContext.Current.Session["Acc_OurBranchID"].ToString().ToUpper() + "&AccountID=" +
                        HttpContext.Current.Session["AccountID"].ToString() + "&FromDate=" +
                         RadDateInput1.SelectedDate + "&Todate=" +
                        RadDateInput2.SelectedDate + "&Currency=" +
                        HttpContext.Current.Session["Acc_Currency"].ToString() + "&UserID=" +
                        HttpContext.Current.Session["UserID"].ToString() + "&AccountDetails=" +
                        HttpContext.Current.Session["Account_Details"].ToString() + "&GLOrAccount=0 &WorkingDate=" +
                        HttpContext.Current.Session["WorkingDate"].ToString() + "&EndOfDayDone=1&DetailOrSummary=D&Email=False");
                    break;
                }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        fn = new Functions();
        string EmailBody;
        string Subject;
        DateTime Date = DateTime.Now;


        EmailBody = "<HTML>";
        EmailBody = EmailBody + "<HEAD>";
        EmailBody = EmailBody + "<TITLE>Transaction</TITLE>";
        EmailBody = EmailBody + "</HEAD>";
        EmailBody = EmailBody + "<BODY>";
        EmailBody = EmailBody + "<TABLE Border = 1 BGCOLOR=White style=\"font-size: 8pt; font-family: verdana;width=560px;\">";
        EmailBody = EmailBody + "<tr>";
        EmailBody = EmailBody + "<th>Date </th>";
        EmailBody = EmailBody + "<th>TrxType</th>";
        EmailBody = EmailBody + "<th>Price</th>";
        EmailBody = EmailBody + "<th>Particulars</th>";
        EmailBody = EmailBody + "<th>Money Out</th>";
        EmailBody = EmailBody + "<th>Money In</th>";
        EmailBody = EmailBody + "<th>Units</th>";
        EmailBody = EmailBody + "</tr>";

        if (hfcs == null)
        {
            hfcs = new BRInvestmentPortalService();
        }

        string[] spParams = {"@OurBranchID", Session["Acc_OurBranchID"].ToString(),
                "@FromAccountID", Session["AccountID"].ToString(),
                "@ToAccountID", Session["AccountID"].ToString(),
                "@FromDate", fn.getCustomDate(DateTime.Parse(RadDateInput1.SelectedDate.ToString())),
                "@ToDate", fn.getCustomDate(DateTime.Parse(RadDateInput2.SelectedDate.ToString()))};

        DataTable dt = null;
        try
        {
            dt = hfcs.GetUTAccountStatement(ref strMsg, spParams);
        }

        catch (System.Net.Sockets.SocketException ex)
        {
            lblMessageLine.Text = "Please ensure that the web service is running.";
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


        if (strMsg != "")
        {
            fn = new Functions();
            fn.logError(strMsg);
            lblMessageLine.Text = "Sorry an error occurred.";
            return;
        }

        foreach (DataRow rsTrxx in dt.Rows)
        {
            EmailBody = EmailBody + "<tr>";
            EmailBody = EmailBody + "<TD>" + rsTrxx["Date"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + rsTrxx["TrxType"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + rsTrxx["Price"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + rsTrxx["Particulars"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + rsTrxx["Debits"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + rsTrxx["Credits"].ToString() + "</TD>";
            EmailBody = EmailBody + "<TD>" + rsTrxx["Units"].ToString() + "</TD>";
            EmailBody = EmailBody + "</tr>";
        }
        EmailBody = EmailBody + "</TABLE>";
        EmailBody = EmailBody + "</BODY>";
        EmailBody = EmailBody + "</HTML>";


        Subject = "Account Activities. Account No:" + Session["AccountID"].ToString();

        fn = new Functions();
        strMsg = "";
        string Error = fn.SendE_mail(Session["Email"].ToString(), Subject, EmailBody);
        if (Error != "")
        {
            fn.logError(strMsg);
            lblMessageLine.Text = "Error sending request";
        }
        else
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Blue;
            lblMessageLine.Text = "E-Mail sent successfully.";
        }
        fn = null;


    }

    protected void AutoMail_Click(object sender, EventArgs e)
    {
        Response.Redirect("Autostatement.aspx");

    }
    protected void GrdAccounts_SelectedIndexChanged(object source, EventArgs e)
    {
        Functions fn = new Functions();
        //String wewe = e.Item.Cells[4].Text;
        int rowindex = 0;
        double Debit = 0.0, Credit = 0.0;
        Session["TransactionDetails"] = "";
        foreach (GridDataItem row in grdStatement.Items)
        {
            rowindex += 1;
            Control control = row.FindControl("chkAccount");
            CheckBox checkBox = control as CheckBox;
            Type type = control.GetType();
            string ns = type.Namespace;
            if (checkBox != null && checkBox.Checked)
            {
                if (fn.isDate(row.Cells[4].Text.ToString()) == true)
                {
                    TableCellCollection cellCollection = row.Cells;
                    Session["TransactionDetails"] += DateTime.Parse(row.Cells[4].Text.ToString()) + " " + row.Cells[6].Text;
                    if (row.Cells[7].Text != "&nbsp;")
                    {
                        Debit = Math.Round(double.Parse(row.Cells[7].Text.ToString()));
                        Session["TransactionDetails"] += " " + row.Cells[7].Text;
                    }
                    if (row.Cells[8].Text != "&nbsp;")
                    {
                        Credit = Math.Round(double.Parse(row.Cells[8].Text.ToString()));
                        Session["TransactionDetails"] += " " + row.Cells[8].Text;
                    }
                    if (IsNumeric(Credit.ToString()) == true)
                    {
                        SumCredit += int.Parse(Credit.ToString());
                    }
                    if (IsNumeric(Debit.ToString()) == true)
                    {
                        SumDebit += int.Parse(Debit.ToString());
                    }
                    Session["TransactionDetails"] = Session["TransactionDetails"].ToString() + "<br />";
                }
            }

        }

        Session["value"] = SumCredit + " " + SumDebit;
    }
    protected void btnSubmitTrx_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session["EmailFile"] = "<table border=\"1\" class=\"hovertable\" width=\"100%\"><tr><td colspan=\"6\" class=\"DataTableTitle\"></tr>";
        HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<tr><td>Date</td><td>Value</td><td>Description</td><td>Debit</td><td>Credit</td><td>Closing</td></tr>";

        int rowindex = 0;
        foreach (GridDataItem row in grdStatement.Items)
        {
            rowindex += 1;
            Control control = row.FindControl("chkAccount");
            CheckBox checkBox = control as CheckBox;
            Type type = control.GetType();
            string ns = type.Namespace;
            if (checkBox != null && checkBox.Checked)
            {
                TableCellCollection cellCollection = row.Cells;
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<tr>";
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<td>" + row.Cells[3].Text + "</td>";
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<td>" + row.Cells[4].Text + "</td>";
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<td>" + row.Cells[5].Text + "</td>";
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<td>" + row.Cells[6].Text + "</td>";
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<td>" + row.Cells[7].Text + "</td>";
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "<td>" + row.Cells[8].Text + "</td>";
                HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "</tr>";
            }

        }
        HttpContext.Current.Session["EmailFile"] = HttpContext.Current.Session["EmailFile"].ToString() + "</table>";

        detailWindow();
    }
    protected void detailWindow()
    {
        Response.Redirect("QueryTransactions.aspx");

    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("transactionsMenu.aspx");
    }
}