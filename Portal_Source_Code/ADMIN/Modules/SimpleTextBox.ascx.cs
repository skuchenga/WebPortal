using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Modules_SimpleTextBox : System.Web.UI.UserControl
{
    public string Text
    {
        get
        {
            return txtValue.Text;
        }
        set
        {
            txtValue.Text = value;
        }
    }

    public string ErrorMessage
    {
        get
        {
            return rfvValue.ErrorMessage;
        }
        set
        {
            rfvValue.ErrorMessage = value;
        }
    }

    public Unit Width
    {
        get
        {
            return txtValue.Width;
        }
        set
        {
            txtValue.Width = value;
        }
    }

    public string CssClass
    {
        get
        {
            return txtValue.CssClass;
        }
        set
        {
            txtValue.CssClass = value;
        }
    }

    public string ValidationGroup
    {
        get
        {
            return rfvValue.ValidationGroup;
        }
        set
        {
            txtValue.ValidationGroup = value;
            rfvValue.ValidationGroup = value;
        }
    }
}