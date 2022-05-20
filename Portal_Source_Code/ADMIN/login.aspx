<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #div1
        {
	        float: center;
	        padding: 250px 0px 0px 0px; 
	         
	        
	        
        }
        
    </style>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body id="body">
    <form id="form1" runat="server">
        <div id="div1" align="center" style="font-family: Verdana; font-size: 0.8em; background-color: white;">
            <table cellpadding="4" cellspacing="0" style="border-collapse: collapse; width: 50%;
                border-right: gray thin solid; border-top: gray thin solid; border-left: gray thin solid;
                border-bottom: gray thin solid;">
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 200px; border-right: gray thin solid;">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/hfc_logo.jpg" />
                                    &nbsp;<br />
                                </td>
                                <td style="width: 59px">
                                    <table width="100%">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="label">User Name:</asp:Label></td>
                                            <td align="left">
                                                <asp:TextBox ID="UserName" runat="server" Font-Size="1em" Width="200px" Font-Names="Verdana"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="label">Password:</asp:Label></td>
                                            <td align="left">
                                                <asp:TextBox ID="Password" runat="server" Font-Size="1em" TextMode="Password" Width="200px"
                                                    Font-Names="Verdana"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="cmdLogin" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1"
                                                    OnClick="cmdLogin_Click" CssClass="button" /></td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="lblMsg" runat="server" EnableViewState="False"></asp:Literal></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblCopyright" runat="server"></asp:Label> </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
