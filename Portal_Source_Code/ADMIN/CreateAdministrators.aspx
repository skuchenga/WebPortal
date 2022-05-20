<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CreateAdministrators.aspx.cs" Inherits="CreateAdministrators" Title="Amana Capital ADMIN | Add New Administrator" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>--%>
    <table width="100%">
        <tr>
            <td align="right">
                <asp:Label ID="Label20" runat="server" Text="Admin Login ID:" Font-Bold="True" ForeColor="Blue"
                    CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtUserID" runat="server" Width="222px" TabIndex="2" MaxLength="10"
                    AutoPostBack="True" OnTextChanged="txtUserID_TextChanged">
                </telerik:RadTextBox>&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUserID"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label1" runat="server" Text="Full Name:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtFullname" runat="server" Width="300px" SkinID="4" TabIndex="4"
                    MaxLength="25">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFullname"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label6" runat="server" Text=" E-Mail:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtemail" runat="server" Width="227px" TabIndex="5" MaxLength="40">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtemail"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtemail"
                    Display="Dynamic" ErrorMessage="Invalid email address." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" CssClass="label" Text="Disable:"></asp:Label></td>
            <td align="left">
                <asp:CheckBox ID="CheckBox1" runat="server" /></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="Password:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtpassword" runat="server" Width="227px" MaxLength="15"
                    TextMode="Password" TabIndex="6">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpassword"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="Confirm Password:" CssClass="label"></asp:Label></td>
            <td align="left">
                <telerik:RadTextBox ID="txtconfipassword" runat="server" Width="227px" MaxLength="10"
                    TextMode="Password" TabIndex="7">
                </telerik:RadTextBox>
                &nbsp;&nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtpassword"
                    ControlToValidate="txtconfipassword" Display="Dynamic" ErrorMessage="The passwords do not match"></asp:CompareValidator>
                <asp:Button ID="btnResetpwd" runat="server" Font-Bold="True" Font-Size="8pt" Text="Reset Password"
                    Enabled="False" OnClick="Button1_Click" CssClass="button" /></td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td align="left">
                <asp:Button ID="btnadd" runat="server" Font-Bold="True" Font-Size="8pt" OnClick="btnadd_Click"
                    OnClientClick="javascript:return Confirm()" Text="Add" Width="93px" TabIndex="13"
                    CssClass="button" /><asp:Button ID="Btnsavechanges" runat="server" Font-Bold="True"
                        Font-Size="8pt" Text="Save" Width="83px" OnClick="Btnsavechanges_Click" OnClientClick="javascript:return Confirm()"
                        TabIndex="14" CssClass="button" /><asp:Button ID="btnclear" runat="server" Font-Bold="True"
                            Font-Size="8pt" OnClick="btnclear_Click" Text="Cancel" Width="74px" TabIndex="15"
                            CssClass="button" CausesValidation="False" /><asp:Button ID="btndelete" runat="server"
                                Font-Bold="True" Font-Size="8pt" Text="Delete" Width="85px" OnClick="btndelete_Click"
                                OnClientClick="javascript:return Confirm()" TabIndex="16" Enabled="False" CssClass="button" /></td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td align="left">
                <asp:Label ID="lblMessageLine" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="8pt" ForeColor="#8080FF"></asp:Label><asp:Label ID="lblmessageline2" runat="server"
                        Font-Bold="True" Font-Names="Arial" Font-Overline="False" Font-Size="8pt" ForeColor="#C00000"></asp:Label></td>
        </tr>
    </table>
    <div id="topBar" class="topBar" style="left: 3px">
        <div id="MainBody" style="left: 0px; width: 100%; top: 28px; height: 559px">
            &nbsp;<telerik:RadGrid ID="grdusers" runat="server" GridLines="None" Width="100%"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="5" OnItemCommand="grdusers_ItemCommand">
                <PagerStyle EnableSEOPaging="True"></PagerStyle>
                <MasterTableView DataKeyNames="UserID,FullName,Email,Disabled">
                    <Columns>
                        <telerik:GridButtonColumn CommandName="Select" Text="Select" UniqueName="column">
                            <HeaderStyle Width="110px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" ForeColor="Blue"></ItemStyle>
                        </telerik:GridButtonColumn>
                        <telerik:GridBoundColumn DataField="UserID" HeaderText="User ID" UniqueName="UserID"
                            DefaultInsertValue="">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FullName" HeaderText="Full Name" UniqueName="FullName"
                            DefaultInsertValue="">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Email" HeaderText="Email" UniqueName="Email"
                            DefaultInsertValue="">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Authorization Status" HeaderText="Authorization Status"
                            UniqueName="column1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="IsEnabled"
                            DefaultInsertValue="">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <Selecting AllowRowSelect="True"></Selecting>
                </ClientSettings>
            </telerik:RadGrid></div>
    </div>
</asp:Content>
