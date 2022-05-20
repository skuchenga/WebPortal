<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ToolTipLabelControl.ascx.cs" Inherits="Modules_ToolTipLabelControl" %>
<span class="brims-tooltip">
    <asp:Image runat="server" ID="imgToolTip" AlternateText="?" />
    <asp:Label runat="server" ID="lblValue" />
</span>