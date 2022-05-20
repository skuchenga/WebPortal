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
using System.Data.Common;
using HFCPortal;

public partial class Default2 : System.Web.UI.Page
{
    string strMsg = "";
    DataAccess DataClass;
    Functions fn;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Form.Attributes.Add("autocomplete", "off");
        if (Session["UserID"] == null || Session["UserID"].ToString() == "" || Session.Keys.Count == 0)
        {
            Response.Redirect("Ibstart.aspx");
        }
        if (this.IsPostBack)
            return;

    }
    protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
    {
        Page.Validate();
        if (!(Page.IsValid))
            return;
        fn = new Functions();
        DateTime Ctrldate = DateTime.Now;

        double pwd = fn.EncryptUserPassword(CurrentPassword.Text, DateTime.Parse(Session["UpDatedOn"].ToString()));
        double oldPassword = double.Parse(Session["pwd"].ToString());//fn.EncryptUserPassword(Session["pwd"].ToString(), DateTime.Parse(Session["UpDatedOn"].ToString()));
        
        if (pwd != oldPassword)
            {
            lblErrorMsg.Text = "Invalid Old Password";
            return;

        }
        else
        {

            try
            {
                DataClass = new DataAccess();
                DbDataReader rows = DataClass.GetDBResults(ref strMsg, "sp_ChangeClientPassword",
                    "@UserID", Session["UserID"].ToString(),
                    "@Password", fn.EncryptUserPassword(NewPassword.Text, Ctrldate),
                    "@UpdatedOn", Ctrldate.ToString("yyyy/mm/dd hh:mm:ss") );
                if (strMsg != "")
                {
                    fn.logError(strMsg);
                    lblErrorMsg.Text = "sorry an error occurred";
                    return;
                }
                Button2.Visible = true;
                lblErrorMsg.Text = "Password changed successfully.";




            }
            catch (Exception ex)
            {
                fn.logError(ex.Message);
                lblErrorMsg.Text = "sorry an error occurred";
            }

        }
        //Response.Redirect("default.aspx");
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("mainmenu.aspx");
    }
}
