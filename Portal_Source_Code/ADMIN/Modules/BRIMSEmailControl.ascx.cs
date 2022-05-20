using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Modules_BRIMSEmailControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Properties
    public string EmailSubject
    {
        get
        {
            return txtSubject.Text;
        }
        set
        {
            txtSubject.Text = value;
        }
    }


    public string EmailBody
    {
        get
        {
            return txtEmailBody.Value;
        }
        set
        {
            txtEmailBody.Value = value;
        }
    }

    public List<FileUpload> UploadedFiles
    {
        get
        {
            return fuFileUploads.Attachments();
        }
    }

    #endregion
}