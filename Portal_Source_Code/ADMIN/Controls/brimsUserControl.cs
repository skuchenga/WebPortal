using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HFCPortal.Common;
using AjaxControlToolkit;


namespace BRIMS.Web
{
    /// <summary>
    /// Summary description for brimsUserControl
    /// </summary>
    public partial class brimsUserControl : UserControl
    {
        protected virtual void BindJQuery()
        {
            CommonHelper.BindJQuery(this.Page);
        }

        protected virtual void BindJQueryIdTabs()
        {
            string jqueryTabs = CommonHelper.GetSystemLocation() + "Scripts/jquery.idTabs.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jqueryTabs, jqueryTabs);
        }

        protected void SelectTab(TabContainer tabContainer, string tabId)
        {
            if (tabContainer == null)
                throw new ArgumentNullException("tabContainer");

            if (!String.IsNullOrEmpty(tabId))
            {
                AjaxControlToolkit.TabPanel tab = tabContainer.FindControl(tabId) as AjaxControlToolkit.TabPanel;
                if (tab != null)
                {
                    tabContainer.ActiveTab = tab;
                }
            }
        }

        protected string GetActiveTabId(TabContainer tabContainer)
        {
            if (tabContainer == null)
                throw new ArgumentNullException("tabContainer");

            if (tabContainer.ActiveTab != null)
                return tabContainer.ActiveTab.ID;

            return string.Empty;
        }
    }
}