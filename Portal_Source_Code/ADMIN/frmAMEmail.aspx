<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="frmAMEmail.aspx.cs" Inherits="frmAMEmail" Title="Amana Capital ADMIN | Asset Management Email" %>

<%@ Register TagPrefix="BRIMS" TagName="EmailTextBox" Src="~/Modules/EmailTextBox.ascx" %>
<%@ Register TagPrefix="BRIMS" TagName="FilesUpload" Src="~/Modules/FilesUpload.ascx" %>
<%@ Register TagPrefix="BRIMS" TagName="F2Help" Src="~/Modules/BRIMSF2.ascx" %>
<%@ Register TagPrefix="BRIMS" TagName="ClientTypes" Src="~/Modules/BRIMSClientTypes.ascx" %>
<%@ Register TagPrefix="BRIMS" TagName="EmailControl" Src="~/Modules/BRIMSEmailControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <table class="adminContent">
        <tr>
            <td class="adminTitle-right">
                <asp:Label runat="server" ID="lblEmailID" Text="E-Mail Login Name: " ToolTip="E-Mail Login Name" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <BRIMS:EmailTextBox ID="txtEmailID" runat="server" />
            </td>
            <td class="adminTitle-right">
                <asp:Label runat="server" ID="lblPassword" Text="Email Password: " ToolTip="Password" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <asp:TextBox ID="txtEmailPassword" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="adminTitle-right">
                <asp:Label runat="server" ID="Label1" Text="From Client ID: " ToolTip="From Client ID" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <BRIMS:F2Help ID="txtFromClientID" runat="server" spName="spName" SelectColumn="ClientID" Param1="ClientID" Param2="ClientType" Param3="ClientName" />
            </td>
            <td class="adminTitle-right">
                <asp:Label runat="server" ID="Label2" Text="To Client ID: " ToolTip="From Client ID" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <BRIMS:F2Help ID="txtToClientID" runat="server" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td class="adminData" colspan="3">
                <BRIMS:ClientTypes ID="ClientTypes" runat="server" />
            </td>
        </tr>
        <tr>
        <td></td>
        <td colspan="3">
            <BRIMS:EmailControl ID="emAMEmail" runat="server" />
        </td>
        </tr>
        <tr>
            <td>
                
            </td>
        </tr>
    </table>
</asp:Content>
