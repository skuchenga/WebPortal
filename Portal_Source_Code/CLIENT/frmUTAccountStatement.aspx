<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPage.master" CodeFile="frmUTAccountStatement.aspx.cs" Inherits="frmUTAccountStatement" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%">
        <tr>
            <td width="50%">
            </td>
            <td width="50%">
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="left">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <CR:CrystalReportViewer ID="crUTAccountStatement" runat="server" AutoDataBind="False" />
            </td>
        </tr>
    </table>

</asp:Content>