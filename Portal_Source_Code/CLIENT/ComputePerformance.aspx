<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ComputePerformance.aspx.cs" Inherits="ComputePerformance" %>

<%@ Register TagPrefix="hfc" TagName="ComputePerformance" Src="~/Modules/ComputerPerformance.ascx" %>

<asp:Content ID="content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <hfc:ComputePerformance ID="ctrlComputePerformance" runat="server" />
</asp:Content>
