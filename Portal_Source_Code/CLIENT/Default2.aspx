<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" Title="Change Password | Patapawa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse"
                width="100%">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" style="width: 100%">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword" Font-Size="1em" CssClass="label">Password:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="CurrentPassword" runat="server" Font-Size="1em" TextMode="Password" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword" Font-Size="1em" CssClass="label">New Password:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="NewPassword" runat="server" Font-Size="1em" TextMode="Password" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                        ErrorMessage="New Password is required." ToolTip="New Password is required."
                                        ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator 
                                         ID="RegularExpressionValidator1" 
                                         runat="server" 
                                         ControlToValidate="NewPassword"
                                         ErrorMessage="Must have at least 1 number, 1 special character, and more than 6 characters." 
                                         ValidationExpression="(?=^.{6,}$)(?=.*\d)(?=.*\W+)(?![.\n]).*$"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword" Font-Size="1em" CssClass="label">Confirm New Password:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="ConfirmNewPassword" runat="server" Font-Size="1em" TextMode="Password" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                        ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                        ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                        ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                        ValidationGroup="ChangePassword1"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" CssClass="button" OnClick="ChangePassword1_ChangedPassword"
                                        Text="Change Password" /></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;<asp:Label ID="lblErrorMsg" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
    &nbsp;
</asp:Content>

