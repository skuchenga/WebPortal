<%@ Page Language="C#" AutoEventWireup="true" CodeFile="F2Help.aspx.cs" MasterPageFile="~/popup.master" Inherits="F2Help" %>

<%@ Register TagPrefix="BRIMS" TagName="F2Search" Src="~/Modules/F2Help.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <BRIMS:F2Search ID="BRIMSF2" runat="server" />
</asp:Content>
