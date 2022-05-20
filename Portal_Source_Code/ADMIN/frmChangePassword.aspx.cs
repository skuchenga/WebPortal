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

public partial class frmChangePassword : System.Web.UI.Page
{
    string strMsg = "";
    DataAccess DataClass;
    Functions fn;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
    {
        lblErrorMsg.Text = "";
        Page.Validate();
        if (!(this.IsValid))
            return;
        fn = new Functions();
        DateTime Ctrldate = DateTime.Now;

        double pwd = fn.EncryptUserPassword(CurrentPassword.Text, DateTime.Parse(Session["Updatedon"].ToString()));
        if (pwd != double.Parse(Session["pwd"].ToString()))
        {
            lblErrorMsg.Text = "Incorrect old password";
            return;

        }
        else
        {

            try
            {
                DataClass = new DataAccess();
                DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_ResetAdminPassword",
                    "@UserID", Session["LoginID"].ToString(),
                    "@Password", fn.EncryptUserPassword(NewPassword.Text, Ctrldate),
                    "@Updatedon", Ctrldate,
                    "@OperatorID", Session["LoginID"].ToString());
                if (strMsg != "")
                {
                    fn.logError(strMsg);
                    lblErrorMsg.Text = "sorry an error occurred";
                    return;
                }


            }
            catch (System.Exception ex)
            {
                fn.logError(ex.Message);
                lblErrorMsg.Text = "sorry an error occurred";
            }

        }
        //Response.Redirect("default.aspx");
    }
    protected void ChangePasswordPushButton_Click(object sender, EventArgs e)
    {
        lblErrorMsg.Text = "";
    }
}
