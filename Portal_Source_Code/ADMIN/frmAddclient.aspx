<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frmAddclient.aspx.cs" Inherits="frmAddclient" Title="Amana Capital ADMIN | Add New Client" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <%-- <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>--%>
    <table align="left" style="width: 98%; font-size: 8pt; font-family: Verdana;">
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Client User ID:" CssClass="label"></asp:Label>
            </td>
            <td>
                <telerik:RadTextBox ID="txtUserID" runat="server" Width="200px" MaxLength="40" AutoPostBack="True"
                    OnTextChanged="txtUserID_TextChanged" TabIndex="1">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                    ControlToValidate="txtUserID" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="+" /></td>
            <td align="right" colspan="2">
                &nbsp;</td>
            <td align="left" colspan="1" style="text-align: left;">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="text-align: right;" align="right">
                <asp:Label ID="Label14" runat="server" Text="Branch:" CssClass="label"></asp:Label></td>
            <td>
                <asp:DropDownList ID="cboBranch" runat="server" Width="200px" AppendDataBoundItems="True"
                    TabIndex="2">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                    ControlToValidate="cboBranch" Display="Dynamic"></asp:RequiredFieldValidator></td>
            <td>
                <strong>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;</strong>
            </td>
            <td>
            </td>
        </tr>
        <tr>
          <td align="right">
                <asp:Label ID="lblClientType" CssClass="label" runat="server" Text="Client/User Type:"></asp:Label>
          </td>
          <td>
                <asp:DropDownList ID="ddlClientType" runat="server">
                    <asp:ListItem Value="UT">Unit Trust</asp:ListItem>
                </asp:DropDownList>
          </td>
          <td>
          
          </td>
          <td>
          
          </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label1" runat="server" CssClass="label">Full Name:</asp:Label>
            </td>
            <td>
                <telerik:RadTextBox ID="txtFullName" runat="server" Width="275px" MaxLength="35"
                    TabIndex="3">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                    ControlToValidate="txtFullName" Display="Dynamic"></asp:RequiredFieldValidator></td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label6" runat="server" CssClass="label">E-Mail:</asp:Label>
            </td>
            <td style="height: 23px; text-align: left">
                <telerik:RadTextBox ID="txteMail" runat="server" Width="275px" MaxLength="40" TabIndex="4">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                    ControlToValidate="txteMail" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid email address."
                    ControlToValidate="txteMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    Display="Dynamic"></asp:RegularExpressionValidator></td>
            <td align="right" colspan="2">
                &nbsp;
            </td>
            <td align="left" colspan="1" style="height: 23px">
            </td>
        </tr>
        <tr>
            <td style="height: 29px; text-align: right" align="right">
                <asp:Label ID="Label7" runat="server" Text="PostalAddress:" CssClass="label"></asp:Label>
            </td>
            <td style="height: 29px; text-align: left">
                <telerik:RadTextBox ID="txtPostalAddress" runat="server" Width="275px" TabIndex="5">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                    ControlToValidate="txtPostalAddress" Display="Dynamic"></asp:RequiredFieldValidator></td>
            <td align="center" colspan="3" style="height: 29px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label11" runat="server" Text="Mobile:" CssClass="label"></asp:Label>
            </td>
            <td style="height: 23px; text-align: left">
                <telerik:RadTextBox ID="txtMobile" runat="server" MaxLength="10" Width="200px" TabIndex="6">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                    ControlToValidate="txtMobile" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td align="right" colspan="2">
            </td>
            <td align="left" colspan="1" style="height: 23px">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="height: 20px">
                <asp:Label ID="Label3" runat="server" Text="Disable:" CssClass="label"></asp:Label></td>
            <td style="height: 20px">
                <asp:CheckBox ID="CheckBox1" runat="server" TabIndex="7" /></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="PDF Password:" CssClass="label"></asp:Label><strong></strong></td>
            <td>
                <telerik:RadTextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px"
                    MaxLength="10" TabIndex="8">
                </telerik:RadTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="height: 29px" align="right">
                <asp:Label ID="Label5" runat="server" Text="Confirm PDF  Password:" CssClass="label"></asp:Label><strong></strong></td>
            <td style="height: 29px">
                <telerik:RadTextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Width="200px"
                    MaxLength="10" TabIndex="9">
                </telerik:RadTextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword"
                    ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="Pdf Password must match confirm pdf password"></asp:CompareValidator>
                <asp:Button ID="changePWD" runat="server" Text="Reset User Password" OnClick="changePWD_Click"
                    Font-Bold="True" Font-Size="8pt" Width="150px" TabIndex="10" Enabled="False"
                    Height="21px" OnClientClick="check()" CssClass="button" CausesValidation="False" />
            </td>
        </tr>
        <tr>
            <td style="height: 25px; text-align: right" align="right">
            </td>
            <td align="left" colspan="4">
                <asp:Button ID="cmdAddRecord" runat="server" Text="Add" Width="93px" OnClick="cmdAddRecord_Click1"
                    Enabled="False" Font-Bold="True" Font-Size="8pt" TabIndex="11" Font-Names="Arial"
                    Height="21px" CssClass="button" />
                <asp:Button ID="BtnSaveChanges" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="8pt" Height="21px" Text="Save" Width="92px" CausesValidation="False"
                    CssClass="button" TabIndex="12" OnClick="BtnSaveChanges_Click" />
                <asp:Button ID="BtnClear" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                    Height="21px" OnClick="BtnClear_Click" Text="Cancel" Width="89px" CausesValidation="False"
                    CssClass="button" TabIndex="13" />&nbsp;
                <asp:Button ID="Button1" runat="server" Enabled="False" Font-Bold="True" Font-Size="8pt"
                    Height="21px" OnClick="Button1_Click1" Text="Unlock User" CausesValidation="False"
                    CssClass="button" TabIndex="14" />
                <asp:Button ID="BTNDELETE" runat="server" Font-Bold="True" Font-Size="8pt" ForeColor="Red"
                    Height="21px" OnClick="BTNDELETE_Click" Text="Delete" Width="110px" CausesValidation="False"
                    CssClass="button" TabIndex="15" />
                    </td>
        </tr>
        <tr>
            <td style="height: 25px; text-align: right" align="right">
            </td>
            <td align="center" colspan="4" style="height: 25px; text-align: left">
                <asp:Label ID="lblMessageLine" runat="server" Width="539px" ForeColor="Blue" Font-Bold="True"
                    Font-Names="Arial" Font-Size="8pt"></asp:Label></td>
        </tr>
        <tr>
            <td align="right" colspan="5" style="height: 25px; text-align: right">
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    GridLines="None" PageSize="5" OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand">
                    <MasterTableView DataKeyNames="OurBranchID,UserID,FullName,Disable,PostalAddress,Email,Mobile">
                        <Columns>
                            <telerik:GridButtonColumn CommandName="Select" Text="Select" UniqueName="column">
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="UserID" HeaderText="Client User ID" UniqueName="column8" DefaultInsertValue="">
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FullName" HeaderText="Full Name" UniqueName="column2" DefaultInsertValue="">
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Authorization Status" HeaderText="Authorization Status"
                                UniqueName="column3" DefaultInsertValue="">
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="column5" DefaultInsertValue="">
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid></td>
        </tr>
    </table>
</asp:Content>
