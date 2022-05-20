using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HFCPortal.Common;

public partial class Modules_BRIMSF2 : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            btnF2.OnClientClick = string.Format("javascript:OpenWindow('F2Help.aspx?sp={0}&sc={1}&cols={2}&txt={3}', 600, 800, true); return false;", this.spName, this.SelectColumn, this.cols(), txtValue.ClientID);
            txtValue.Attributes.Add("onkeyup", string.Format("javascript:OpenWindow('F2Help.aspx?sp={0}&sc={1}&cols={2}&txt={3}', 500, 300, true); return false;", this.spName, this.SelectColumn, this.cols(),txtValue.ClientID));
        }
    }


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

   public string spName
    {
        get
        {
            return hfSpName.Value;
        }
        set
        {
            hfSpName.Value = value;
        }
    }


    public string SelectColumn
    {
        get
        {
            return hfSelectCol.Value;
        }
        set
        {
            hfSelectCol.Value = value;
        }
    }

    //Param1
    public string Param1
    {
        get
        {
            return hfParam1.Value;
        }
        set
        {
            hfParam1.Value = value;
        }
    }

    
    //param2
    public string Param2
    {
        get
        {
            return hfParam2.Value;
        }
        set
        {
            hfParam2.Value = value;
        }
    }

  
    //Param 3
    public string Param3
    {
        get
        {
            return hfParam3.Value;
        }
        set
        {
            hfParam3.Value = value;
        }
    }

    
    //Param 4
    public string Param4
    {
        get
        {
            return hfParam4.Value;
        }
        set
        {
            hfParam4.Value = value;
        }
    }

  
    //Param5
    public string Param5
    {
        get
        {
            return HfParam5.Value;
        }
        set
        {
            HfParam5.Value = value;
        }
    }

   

    //Param 6
    public string Param6
    {
        get
        {
            return hfParam6.Value;
        }
        set
        {
            hfParam6.Value = value;
        }
    }

    protected string cols()
    {
        string strCols = string.Empty;

        foreach (Control  c in pnlData.Controls)
        {
            if (c is HiddenField)
            { 
                HiddenField hf = (HiddenField) c;

                if ((!hf.Equals(hfSpName)) && (!hf.Equals(hfSelectCol)))
                {
                    if (!string.IsNullOrEmpty(hf.Value))
                    {
                        if (!string.IsNullOrEmpty(strCols))
                        {
                            strCols = strCols + ";" + hf.Value;
                        }
                        else
                        {
                            strCols = hf.Value;
                        }
                    }
                }
            }
        }

        //if (strCols.EndsWith(";"))
        //{
        //    strCols.Remove((strCols.Length - 1),1);
        //    strCols.Substring(0, (strCols.Length - 1));
            
        //}

        return strCols;
    }
}