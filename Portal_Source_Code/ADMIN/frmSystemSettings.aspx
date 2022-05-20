<%@ Page Title="IB ADMIN | System Parameters" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmSystemSettings.aspx.cs" Inherits="frmSystemSettings" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link rel="stylesheet" href="app_themes/Theme/jquery-ui.css" />
    <script type="text/javascript" src="js/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=btnUpdate.ClientID %>").button();
            $("#<%=btnSendtestmail.ClientID %>").button();

        });
    </script>
    <script type="text/javascript">
        window.history.forward(0);
    </script>
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Update System Parameters</legend>
        <table style="FONT-SIZE: 8pt; FONT-FAMILY: 'Century Gothic'" width="100%">
            <tr>
                <td align="right"></td>
                <td>SMTP CLIENT</td>
            </tr>
            <tr>
                <td align="right">Host:</td>
                <td>
                    <asp:TextBox ID="txthost" runat="server" Width="125px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">Port:</td>
                <td>
                    <asp:TextBox ID="txtport" runat="server" Width="125px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>SMTP USER INFO</td>
            </tr>
            <tr>
                <td align="right">Username:</td>
                <td>
                    <asp:TextBox ID="txtusername" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">Password:</td>
                <td>
                    <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" ></asp:TextBox>
                </td>
            </tr>
            <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="EnableSSL:"></asp:Label></td>
            <td>
                <asp:CheckBox ID="chkEnableSSL" runat="server"/>
            </td>
            </tr>
            <tr>
                <td align="right">&nbsp;</td>
                <td>TEST EMAIL USING THE ABOVE CREDENTIALS</td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label1" runat="server" Text="From"></asp:Label>
                    :</td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                    ;</td>
                <td>
                    <asp:TextBox ID="txtTo" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Text="Test Subject"></asp:Label>
                    :</td>
                <td>
                    <asp:TextBox ID="txtSubject" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label4" runat="server" Text="Test Message"></asp:Label>
                    ;</td>
                <td>
                    <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">&nbsp;</td>
                <td>
                    <asp:Button ID="btnSendtestmail" runat="server" Text="Send Test E-Mail" OnClientClick="this.disabled=true;this.value='Please wait ...';" UseSubmitBehavior="False" Font-Names="Century Gothic" Font-Size="8pt" OnClick="btnSendtestmail_Click" />
                </td>
            </tr>
            <tr>
                <td align="right">&nbsp;</td>
                <td>NEW CLIENT</td>
            </tr>
            <tr>
                <td align="right">PDF File Path:</td>
                <td>
                    <asp:TextBox ID="txtPWFilePath" runat="server" MaxLength="200" ToolTip="Path to keep login credentials for new client account."></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    &nbsp;</td>
                <td>
                    PASSWORD POLICY</td>
            </tr>
            <tr>
                <td align="right">
                    Login Attempts:</td>
                <td>
                    <asp:TextBox ID="txtAttempts" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Expire After:</td>
                <td>
                    <asp:TextBox ID="txtPWExpiry" runat="server"></asp:TextBox>
                    Days</td>
            </tr>
            <tr>
                <td align="right">
                    History:</td>
                <td>
                    <asp:TextBox ID="txtHistory" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update Settings" OnClientClick="return confirm(&quot;Are you sure you want to perform this task?&quot;);" Font-Names="Century Gothic" Font-Size="8pt" OnClick="btnUpdate_Click" />
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>

