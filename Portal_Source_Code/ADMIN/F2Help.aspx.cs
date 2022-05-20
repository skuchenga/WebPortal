using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HFCPortal.Common;

public partial class F2Help : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       

    }

    protected override void OnPreRender(EventArgs e)
    {
        //java-script
        string adminJs = CommonHelper.GetSystemLocation() + "Scripts/admin.js";
        Page.ClientScript.RegisterClientScriptInclude(adminJs, adminJs);

        base.OnPreRender(e);
    }



    
}