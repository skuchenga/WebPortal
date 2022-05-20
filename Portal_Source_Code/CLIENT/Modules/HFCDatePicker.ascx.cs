using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Modules_HFCDatePicker : System.Web.UI.UserControl
{
    #region Overrides

    protected override void OnLoad(EventArgs e)
    {
        this.Format = "dd-MMM-yyyy";
        base.OnLoad(e);
    }

    #endregion

    #region Properties
    public bool ShowTime
    {
        get
        {
            return Convert.ToBoolean(ViewState["ShowTime"]);
        }
        set
        {
            ViewState["ShowTime"] = value;
        }
    }

    public DateTime? SelectedDate
    {
        get
        {
            DateTime inputDate;
            if (!DateTime.TryParse(txtDateTime.Text, out inputDate))
            {
                return null;
            }
            return inputDate;
        }
        set
        {
            ajaxCalendar.SelectedDate = value;
        }
    }

    public string Format
    {
        get
        {
            return ajaxCalendar.Format;
        }
        set
        {
            ajaxCalendar.Format = value;
        }
    }
    #endregion


}

    
