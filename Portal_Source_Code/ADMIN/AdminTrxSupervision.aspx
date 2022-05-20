<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AdminTrxSupervision.aspx.cs" Inherits="AdminTrxSupervision" Title="Amana Capital ADMIN | Supervise New Client" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%--    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>--%>
     
        <table style="width: 99%" align="center">
            <tr>
                <td style="height: 21px;" align="right" colspan="2">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                        Text="Password:" Width="56px" CssClass="label"></asp:Label>&nbsp;
                </td>
                <td style="height: 21px">
                    <asp:TextBox ID="txtPassword" runat="server" OnTextChanged="TextBox1_TextChanged"
                        TextMode="Password" MaxLength="15" Width="153px"></asp:TextBox>
                    <asp:Button ID="btnApp" runat="server" OnClick="btnApp_Click" Text="+" /></td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="height: 21px">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="8pt" Text="Module Name:"
                        Font-Names="Arial" CssClass="label"></asp:Label>&nbsp;</td>
                <td style="height: 21px">
                    <asp:DropDownList ID="cmbModules" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="cmbModules_SelectedIndexChanged" Width="252px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="height: 21px" align="right" colspan="2">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                        Text="Comment:" Width="56px" CssClass="label"></asp:Label>&nbsp;</td>
                <td colspan="4" style="height: 21px">
                    <asp:TextBox ID="txtcomment" runat="server" BackColor="#FFFFC0" MaxLength="50" Width="425px"
                        OnTextChanged="txtcomment_TextChanged"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtcomment"
                        Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="height: 21px">
                </td>
                <td colspan="4" style="height: 21px">
                    <asp:Button ID="btnPass" runat="server" Text="Pass" Width="80px" OnClick="btnPass_Click"
                        Font-Bold="True" Font-Size="8pt" CssClass="button" />
                    <asp:Button ID="btnReject" runat="server" Text="Reject" Width="74px" Font-Bold="True"
                        Font-Size="8pt" OnClick="btnReject_Click" CssClass="button" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click"
                        Font-Bold="True" Font-Size="8pt" Width="75px" CssClass="button" /></td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="height: 21px">
                </td>
                <td colspan="4" style="height: 21px">
                    <asp:Label ID="lblMessageLine" runat="server" Font-Bold="True" Font-Size="8pt" ForeColor="Blue"
                        Font-Names="Arial"></asp:Label></td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="height: 21px">
                </td>
                <td colspan="4" style="height: 21px">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                        ForeColor="Red" Width="505px"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td colspan="6" style="font-size: 8pt; font-family: Arial">
                    &nbsp;<telerik:RadGrid ID="RadGrid1" runat="server" GridLines="None" Skin="Web20"
                        Height="443px">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="chkAccount1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAccount1" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                        </ClientSettings>
                    </telerik:RadGrid></td>
            </tr>
        </table>
        
        <asp:SqlDataSource ID="dsmodule" runat="server" OnSelecting="dsmodule_Selecting"></asp:SqlDataSource>
    
</asp:Content> 