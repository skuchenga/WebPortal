<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BRIMSEmailControl.ascx.cs" Inherits="Modules_BRIMSEmailControl" %>
<%@ Register TagPrefix="BRIMS" TagName="FilesUpload" Src="~/Modules/FilesUpload.ascx" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>


<table class="adminContent">
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td class="adminData" colspan="2">
            <asp:Label ID="lblTitle" Text="The message below will be placed in the Subject of the mail" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="adminTitle">
            <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="adminData">
            <asp:Label ID="lblFileUpload" runat="server" Text="File Attachments" />
        </td>
    </tr>
    <tr>
        <td colspan="2" class="adminData">
            <BRIMS:FilesUpload ID="fuFileUploads" runat="server" showUploadButton="false" />
        </td>
    </tr>
    <tr>
        <td colspan="2" class="adminData">
            <asp:Label ID="Label1" runat="server" Text="The message below will be placed in the Body of the mail" />
        </td>
    </tr>
    <tr>
        <td colspan="2" class="adminData">
            <FCKeditorV2:FCKeditor ID="txtEmailBody" runat="server" AutoDetectLanguage="false" Height="350" Width="800px" />
        </td>
    </tr>
</table>
