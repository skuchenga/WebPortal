using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Text;
using System.Data.SqlClient;
using HFCPortal;

public partial class _Default : System.Web.UI.Page
{
    string Password, strMsg = "", myDate;
    Boolean Disable, AllowMails, isAdmin, AllowTransfer, AllChqReq, AllowStopPay, Firstlogin;
    DateTime UpdateOn;

    Functions fn;
    DataAccess DataClass;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Form.Attributes.Add("autocomplete", "off");
        if (this.IsPostBack)
            return;

        txtLoginID.Focus();
        cmdContinue.Enabled = true;


    }
    protected void log()
    {
        fn = new Functions();
        DataClass = new DataAccess();
        DbDataReader rsUpdateLoggedIn = DataClass.GetDBResults(ref strMsg, "sp_updateLoggedIn", "@UserID", Session["UserID"].ToString());
        if (strMsg != "")
        {
            fn.logError(strMsg);
            lblMessageLine.Text = "Sorry an error occurred";
            return;
        }
        rsUpdateLoggedIn.Close();
        rsUpdateLoggedIn.Dispose();
        DataClass = null;
    }


    protected void cmdContinue_Click(object sender, EventArgs e)
    {
        lblMessageLine.Text = "";
        Page.Validate();
        if (!(Page.IsValid))
            return;
        fn = new Functions();

        if (fn.isSafe(txtLoginID.Text.Replace("'", "''")) == false)
        {
            fn.MessageLine(this.Page, "Unsafe Characters Identified In User ID Field.", System.Drawing.Color.Red, "lblMessageLine");
            return;
        }
        DataClass = new DataAccess();

            DbDataReader rows = DataClass.GetDBResults(ref strMsg, "sp_GetUserInfo",
             "@UserID", txtLoginID.Text.Replace("'", "''"));
        if (strMsg != "")
        {
            fn.logError(strMsg);
            lblMessageLine.Text = "Sorry an error occurred. Please contact the bank.";
            return;
        }
        if (rows.Read())
        {
            HttpContext.Current.Session["allowmails"] = rows["AllowMails"].ToString();
            HttpContext.Current.Session["Message"] = rows["ShortMessage"].ToString();
            HttpContext.Current.Session["username"] = txtLoginID.Text;
            bool disabled = System.Convert.ToBoolean(rows["disable"].ToString());
            HttpContext.Current.Session["Firstloggin"] = rows["firstloggin"].ToString();
            HttpContext.Current.Session["Email"] = rows["Email"].ToString();
            HttpContext.Current.Session["UpDatedOn"] = rows["UpDatedOn"].ToString();
            HttpContext.Current.Session["pwd"] = rows["Password"];//.ToString();
            HttpContext.Current.Session["FullName"] = rows["FullName"].ToString();
            bool IsSupervisionRequired = System.Convert.ToBoolean(rows["IsSupervisionRequired"].ToString());
            HttpContext.Current.Session["LastLoggin"] = rows["Lastlogin"].ToString();
            HttpContext.Current.Session["Allowpayroll"] = rows["Allowpayroll"].ToString();
            if (disabled == true)
            {
                lblMessageLine.Text = "Account disabled. Please Contact The Bank.";
                return;
            }
            if (IsSupervisionRequired == true)
            {
                lblMessageLine.Text = "Account is inactive. Please Contact The Bank.";
                return;
            }
            Response.Redirect("Level2Login.aspx");
        }
        else
        {
            lblMessageLine.ForeColor = System.Drawing.Color.Black;
            lblMessageLine.Text = "Incorrect user id. Cannot continue.";
        }
        rows.Close();



        DataClass = null;
        fn = null;
    }

}
