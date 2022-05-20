<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="sendstatement.aspx.cs" Inherits="Default2" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="BRIMS" TagName="F2Help" Src="~/Modules/BRIMSF2.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table style="width: 80%; padding-top: 10px; font-family: Verdana; font-size: 8pt;">
        <tr>
            <td align="right">
                From Account:
            </td>
            <td>
                <BRIMS:F2Help ID="txtFromAccountID" runat="server" />
            </td>
            <td align="right">
                From Date:
            </td>
            <td>
                <telerik:RadDatePicker ID="dtFromDate" runat="server">
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td align="right">
                To Account:
            </td>
            <td>
                <BRIMS:F2Help ID="txtToAccountID" runat="server" />
            </td>
            <td align="right">
                To Date:
            </td>
            <td>
                <telerik:RadDatePicker ID="dtToDate" runat="server">
                </telerik:RadDatePicker>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" align="left">
                
                The message below will be placed in the subject of the mail
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" align="left" >
                <telerik:RadTextBox ID="txtEmailSubject" Width="100%" runat="server">
                </telerik:RadTextBox>    
            </td>
            
        </tr>
        <tr>
            <td></td>
            <td colspan="2" align="left">
                The message below will be placed in the body of the mail
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <telerik:RadTextBox ID="txtEmailBody"  Width="100%" runat="server" Height="150px" TextMode ="MultiLine" >
                </telerik:RadTextBox>    
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td align="right">                
                <asp:Button ID="cmdSend" runat="server" Text="Send" Width="93px" OnClick="CmdSend_Click"
                    Font-Bold="True" Font-Size="8pt" TabIndex="11" Font-Names="Arial"
                    Height="21px" CssClass="button" />        
            </td>
        </tr>
         <tr>
         <td></td>
            <td colspan="3">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
