using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HFCPortal.Common;

public partial class Modules_FilesUpload : System.Web.UI.UserControl
{
    static bool _showUploadButton;

    protected void btnUploadFile_Click(object sender, EventArgs e)
    {

    }

    protected override void OnPreRender(EventArgs e)
    {
        CommonHelper.BindJQuery(this.Page);
        btnUploadFile.Visible = showUploadButton;
        this.btnMoreUploads.Attributes["onclick"] = "showUploadPanels(); return false;";
    }
    
    public List<FileUpload> Attachments()
    {
        try
        {
            List<FileUpload> fileUploads = new List<FileUpload>();
            fileUploads.Add(fuFile1);
            fileUploads.Add(fuFile2);
            fileUploads.Add(fuFile3);
            fileUploads.Add(fuFile4);
            fileUploads.Add(fuFile5);

            return fileUploads;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    #region "Properties"
    
    public bool showUploadButton
    {
        get
        {
            return _showUploadButton;
        }
        set
        {
            _showUploadButton = value;
        }
    }

    #endregion
}