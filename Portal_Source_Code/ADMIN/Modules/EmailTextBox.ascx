<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmailTextBox.ascx.cs" Inherits="Modules_EmailTextBox" %>

<asp:TextBox ID="txtValue" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue"
    ErrorMessage="Email required" Display="None" />
<asp:RegularExpressionValidator ID="revValue" runat="server" ControlToValidate="txtValue"
    ValidationExpression=".+@.+\..+" ErrorMessage="Invalid email format"
    Display="None" />
<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="rfvValueE" TargetControlID="rfvValue"
    HighlightCssClass="validatorCalloutHighlight" />
<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="revValueE" TargetControlID="revValue"
    HighlightCssClass="validatorCalloutHighlight" />