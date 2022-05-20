<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="mainmenu.aspx.cs" Inherits="mainmenu" Title="Amana Capital IB | My Accounts" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     
            <table style="width: 100%;">
                <tr>
                    <td align="center" style="height: 19px" valign="middle" colspan="3">
                        <telerik:radgrid id="RadGrid1" runat="server" autogeneratecolumns="False" gridlines="None"
                            skin="Office2007" onitemdatabound="GrdAccounts_DataBound" onitemcommand="GrdAccounts1_ItemCommand">
                            <mastertableview datakeynames="OurBranchID,AccountID,CurrencyName,AccountName">
                                    <Columns>
                                        <telerik:GridButtonColumn CommandName="Select" DataTextField="AccountID" HeaderText="A/c Number"
                                            UniqueName="column">
                                            <ItemStyle HorizontalAlign="Left" ForeColor="Blue" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="AccountName" HeaderText="Account Name" UniqueName="AccountName">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="FundType" HeaderText="Fund Type" UniqueName="FundType">
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="AccountBalance" HeaderText="Account Balance" UniqueName="AccountBalance">
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CurrencyName" HeaderText="Currency Name"
                                            UniqueName="CurrencyName" DefaultInsertValue="">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </mastertableview>
                        </telerik:radgrid>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height: 52px" valign="top" colspan="3">
                        <asp:Image ID="Image1" runat="server" Height="16px" ImageUrl="~/images/send_icon.gif"
                            Width="22px" />
                        <asp:Button ID="Button1" runat="server" Text="[E-Mail me This Information]" BackColor="#D4E3E5"
                            BorderStyle="None" Font-Bold="True" Font-Size="8pt" ForeColor="Blue" OnClick="btnGo_Click"
                            Width="200px" Font-Underline="True" />
                        &nbsp;<span style="font-size: 10pt; color: Navy; font-family: Arial; font-weight: bold;">&nbsp;</span>&nbsp;</td>
                </tr>
                <tr>
                    <td align="left" colspan="3" valign="top">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                            ForeColor="Black"></asp:Label><asp:Label ID="lblMessageLine" runat="server" Font-Bold="True"
                                Font-Names="Arial" Font-Size="8pt" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
            </table>
             
        
</asp:Content>
