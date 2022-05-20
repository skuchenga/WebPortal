<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmChangePassword.aspx.cs" Inherits="frmChangePassword" Title="Amana Capital | Change password" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>--%>
    <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse"
        width="100%">
        <tr>
            <td>
                <table border="0" cellpadding="0" style="width: 100%">
                    <tr>
                        <td align="right">
                            <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword"
                                Font-Size="1em" CssClass="label">Password:</asp:Label></td>
                        <td>
                            &nbsp;<telerik:RadTextBox ID="CurrentPassword" runat="server" MaxLength="16" TabIndex="1"
                                TextMode="Password" Width="200px">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword"
                                Font-Size="1em" CssClass="label">New Password:</asp:Label></td>
                        <td>
                            &nbsp;<telerik:RadTextBox ID="NewPassword" runat="server" MaxLength="16" TabIndex="1"
                                TextMode="Password" Width="200px">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                ErrorMessage="New Password is required." ToolTip="New Password is required."
                                ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="NewPassword"
                                ErrorMessage="Must have at least 1 number, 1 special character, and more than 6 characters."
                                ValidationExpression="(?=^.{6,}$)(?=.*\d)(?=.*\W+)(?![.\n]).*$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword"
                                Font-Size="1em" CssClass="label">Confirm New Password:</asp:Label></td>
                        <td>
                            &nbsp;<telerik:RadTextBox ID="ConfirmNewPassword" runat="server" MaxLength="16" TabIndex="1"
                                TextMode="Password" Width="200px">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                            <asp:Button ID="ChangePasswordPushButton" runat="server" Text="Change Password" ValidationGroup="ChangePassword1"
                                OnClick="ChangePassword1_ChangedPassword" CssClass="button" />
                            <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel" CssClass="button" /></td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                            <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                ValidationGroup="ChangePassword1"></asp:CompareValidator></td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    &nbsp;<asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
</asp:Content>
