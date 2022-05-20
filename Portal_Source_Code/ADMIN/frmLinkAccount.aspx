<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmLinkAccount.aspx.cs" Inherits="frmLinkAccount" Title="Amana Capital Admin | Link Client Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
    <%--<telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>--%>
    <table style="font-size: 8pt; font-family: Verdana" width="100%">
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Client User ID:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtuserID" runat="server" Width="150px" AutoPostBack="True"
                    OnTextChanged="txtuserID_TextChanged1">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtuserID"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="Full Name:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtUsername" runat="server" Width="300px" Enabled="False">
                </telerik:RadTextBox>&nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="Branch:" CssClass="label"></asp:Label></td>
            <td align="left">
                <asp:DropDownList ID="cmbBranch" runat="server" AppendDataBoundItems="True"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cmbBranch"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" valign="middle">
                <asp:Label ID="Label5" runat="server" Text="Account No.:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtAccountID" runat="server" MaxLength="15" AutoPostBack="True"
                    OnTextChanged="txtAccountID_TextChanged" Width="150px">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAccountID"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" valign="middle">
                <asp:Label ID="Label6" runat="server" Text="Account Title:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtName" runat="server" ReadOnly="True" Width="300px">
                </telerik:RadTextBox>
                <telerik:RadTextBox ID="txtCurrencyName" runat="server" Enabled="False">
                </telerik:RadTextBox>
                <telerik:RadTextBox ID="txtCurrencyID" runat="server" Enabled="False" Visible ="false" >
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td align="right" style="height: 19px">
            </td>
            <td align="left" style="height: 19px">
                <asp:Button ID="btnAdd" runat="server" Height="19px" OnClick="btnAdd_Click" Text="Link Account"
                    Width="123px" Font-Bold="True" Font-Size="8pt" Enabled="False" CssClass="button" />&nbsp;
                <asp:Button ID="btnDelete" runat="server" Height="19px" OnClientClick="javascript:return check()"
                    OnClick="btnDelete_Click" Style="z-index: 105; left: 743px; top: 135px" Text="De-Link Account"
                    Width="123px" Font-Bold="True" Font-Size="8pt" Enabled="False" CssClass="button" CausesValidation="False" /></td>
        </tr>
        <tr>
            <td align="right" style="height: 19px">
            </td>
            <td align="left" style="height: 19px">
                <asp:Label ID="lblMessageLine" runat="server" Font-Bold="True" CssClass="label"></asp:Label></td>
        </tr>
    </table>
    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" GridLines="None" OnItemCommand="Accounts_ItemCommand">
        <mastertableview datakeynames="OurBranchID,AccountID,NickName,FullName,Currency">
                <Columns>
                    <telerik:GridCheckBoxColumn UniqueName="chkLinked"></telerik:GridCheckBoxColumn>
                    <telerik:GridButtonColumn CommandName="Select" Text="Select" UniqueName="column">
                    </telerik:GridButtonColumn>
                    <telerik:GridBoundColumn DataField="OurBranchID" HeaderText="Branch Code"
                        UniqueName="column3" DefaultInsertValue="">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AccountID" HeaderText="Account Number"
                        UniqueName="column1" DefaultInsertValue="">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NickName" HeaderText="Customer Name"
                        UniqueName="column2" DefaultInsertValue="">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Currency" HeaderText="Currency"
                        UniqueName="column4" DefaultInsertValue="" Visible="false">
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="CurrencyName" HeaderText="Currency Name"
                        UniqueName="CurrencyName" DefaultInsertValue="">
                    </telerik:GridBoundColumn>
                </Columns>
            </mastertableview>
    </telerik:RadGrid>
    <asp:Label ID="Label1" runat="server" Text="."></asp:Label>
    <asp:Label ID="InjectScriptLabel" runat="server" Text="."></asp:Label>&nbsp;
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT [WorkFlowID], [WorkFlowName] FROM [t_Rolesworkflow]">
    </asp:SqlDataSource>
</asp:Content> 