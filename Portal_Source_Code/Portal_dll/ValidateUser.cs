using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;


/// <summary>
/// Summary description for ValidateUser
/// </summary>
/// 
namespace HFCPortal
{

    public class ValidateUser
    {
        public ValidateUser()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    public class SystemUser
    {
        private string strFullName;
        private string strBranch = string.Empty;
        private double dlPassword;
        private Boolean BolEnabled;
        private string strEmail;
        private string strExtension=string.Empty;
        private string strPosition=string.Empty;
        private Boolean BolFirstloggin;
        private Boolean BolIsSupervised;
        private DateTime DtUpdatedon;
        private string strUserID;
        private Boolean isAdmin;
        private string strMsg = string.Empty;

        public SystemUser()
        {
            this.strMsg = "";
        }
        public string FullName
        {
            get { return strFullName; }
        }
        public string Branch
        {
            get { return strBranch; }
        }
        public double Password
        {
            get { return dlPassword; }
        }
        public Boolean Enabled
        {
            get { return BolEnabled; }
        }

        public Boolean IsAdmin
        {
            get { return isAdmin; }
        }
        public string Email
        {
            get { return strEmail; }
        }
        public string Extension
        {
            get { return strExtension; }
        }
        public string Position
        {
            get { return strPosition; }
        }
        public Boolean Firstloggin
        {
            get { return BolFirstloggin; }
        }

        public DateTime Updatedon
        {
            get { return DtUpdatedon; }
        }

        public Boolean IsSupervised
        {
            get { return BolIsSupervised; }
        }
        public string UserID
        {
            get { return strUserID; }
            set { strUserID = value; }
        }
        public string getSystemUser(ref string strMsg)
        {

            DataAccess DataClass = new DataAccess();
            DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_getAdminInfo", "@UserID", strUserID);
            if (strMsg != "")
            {
                return strMsg;
            }
            if (rs.Read())
            {
                strEmail = rs["Email"].ToString();
                strFullName = rs["FullName"].ToString();
                BolFirstloggin = Boolean.Parse(rs["FirstLogin"].ToString());
                DtUpdatedon = DateTime.Parse(rs["UpdatedOn"].ToString());
                BolEnabled = Boolean.Parse(rs["Disabled"].ToString());
                BolIsSupervised = Boolean.Parse(rs["IsSupervisionRequired"].ToString());
                dlPassword = double.Parse(rs["Password"].ToString());
                isAdmin = Boolean.Parse(rs["IsAdmin"].ToString());
                strMsg = "";
            }
            else
            {

                strMsg = "Invalid user name.";
                return strMsg;
            }
            rs.Close();
            rs.Dispose();
            DataClass = null;
            return strMsg;
        }

    }
    public class SaveActivity
    {
        private string strMsg;
        private string strActivity;

        public SaveActivity()
        {
            this.strMsg = "";
        }
        public string Activity
        {
            set { strActivity = value; }
        }
        public string SaveUserActivity(ref string strStatusMessage)
        {
            strMsg = "";
            DataAccess DataClass = new DataAccess();
            HttpContext.Current.Session["LoginID"] = "Reggie";
            DbDataReader rs = DataClass.GetDBResults(ref strMsg, "sp_AdminTrack",
                "@UserID", HttpContext.Current.Session["LoginID"].ToString(),
                "@DateAndTimeIN", DateTime.Now,
                "@Activity", strActivity,
                "@Clientip", HttpContext.Current.Session["ClientIP"].ToString());

            if (strMsg != "")
            {
                strStatusMessage = strMsg;
                return strMsg;
            }
            rs.Close();
            rs.Dispose();
            DataClass = null;
            return strMsg;
        }

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



}
