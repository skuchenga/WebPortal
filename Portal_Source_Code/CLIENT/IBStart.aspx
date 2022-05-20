<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IBStart.aspx.cs" Inherits="_Default"
    EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Amana Capital IB V.5.0 IB V.5.0</title>

    <script language="javascript" type="text/javascript">
        window.history.forward(0);


    </script>

    <link href="IBLogin.css" type="text/css" rel="Stylesheet" />
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
</head>
<body align="center" style="font-size: 8pt; font-family: Verdana">
    <form id="form1" runat="server" defaultbutton="cmdContinue">
        <div style="text-align: center" id="DIV1" align="center">
            <div id="loginFooter1" style="width: 36%; top: 294px; height: 38%; left: 13%;">
                <strong>User ID</strong>
                <asp:TextBox ID="txtLoginID" runat="server" Width="107px" BorderColor="#0000A0" Height="19px"
                    BorderStyle="Solid" MaxLength="15" Font-Names="Arial" Font-Size="9pt" ToolTip="User Login ID"></asp:TextBox>
                <asp:Button ID="cmdContinue" runat="server" OnClick="cmdContinue_Click" Text="Continue" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLoginID"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator><br />
                <br />
                <asp:Label ID="lblMessageLine" runat="server" Width="402px" Font-Bold="True" ForeColor="Crimson"
                    Font-Names="Verdana" Font-Size="0.8em"></asp:Label><br />
            </div>
        </div>
        <table style="width: 100%; font-size: 8pt; font-family: Verdana;" align="center">
            <tr>
                <td align="center" valign="middle" style="height: 608px">
                    <img usemap="#Map" id="IMG1" alt="image map" src="Images/Login_HFC.jpg" onclick="return IMG1_onclick()" />&nbsp;<br />
                </td>
            </tr>
        </table>
        &nbsp; &nbsp;&nbsp;
        <table width="135" border="0" cellpadding="2" cellspacing="0" title="Click to Verify - This site chose VeriSign SSL for secure e-commerce and confidential communications.">
            <tr>
                <td width="135" align="center" valign="top">

                    <script type="text/javascript" src="https://seal.verisign.com/getseal?host_name=e-bank.mebkenya.com&amp;size=M&amp;use_flash=YES&amp;use_transparent=YES&amp;lang=en"></script>

                    <br />
                    <a href="http://www.symantec.com/en/uk/ssl-certificates" target="_blank" style="color: #000000;
                        text-decoration: none; font: bold 7px verdana,sans-serif; letter-spacing: .5px;
                        text-align: center; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a></td>
            </tr>
        </table>
        &nbsp;
    </form>
</body>
</html>
