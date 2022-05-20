<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BRIMSF2.ascx.cs" Inherits="Modules_BRIMSF2" %>

<asp:Panel runat="server" ID="pnlData">
      <asp:TextBox ID="txtValue" runat="server" MaxLength="20" CssClass="adminData" ToolTip="Press F2 Key to search" ></asp:TextBox>
      <asp:Button ID="btnF2" runat="server" Text ="F2" CssClass="f2Button" ToolTip="Click button to search if F2 key does not work."  />
    
    <asp:HiddenField ID="hfSpName" runat="server" />
    <asp:HiddenField ID="hfSelectCol" runat="server" />
    <asp:HiddenField ID="hfParam1" runat="server" />
    <asp:HiddenField ID="hfParam2" runat="server" />
    <asp:HiddenField ID="hfParam3" runat="server" />
    <asp:HiddenField ID="hfParam4" runat="server" />
    <asp:HiddenField ID="HfParam5" runat="server" />
    <asp:HiddenField ID="hfParam6" runat="server" />
</asp:Panel>
