<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="transactionsMenu.aspx.cs" Inherits="TransactionsMenu" Title="Amana Capital IB | My Account Activities" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%">
        <tr>
                <td>
                <table style="width: 100%; font-size: 8pt; font-family: Verdana;">
                    <tr>
                        <td width="15%" />
                        <td width="20%" />
                        <td width="20%" />
                        <td width="20%" />
                        <td width="25%" />

                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Label ID="Label1" runat="server" Text="From Date:"></asp:Label>

                        </td>
                        <td colspan="2" align="left" valign="bottom">
                            <telerik:RadDatePicker ID="RadDateInput1" runat="server" Skin="Web20">
                                <calendar runat="server" skin="Web20" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                    viewselectortext="x">
                                    </calendar>
                                <datepopupbutton hoverimageurl="" imageurl="" />
                                <dateinput runat="server" dateformat="dd/MMM/yyyy" displaydateformat="dd/MMM/yyyy">
                                    </dateinput>
                            </telerik:RadDatePicker>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="RadDateInput2"
                                ControlToValidate="RadDateInput1" ErrorMessage="The from date should be smaller than the to date date!"
                                Operator="LessThanEqual" Type="Date"></asp:CompareValidator></td>
                        <td align="right" valign="top" >
                                <asp:LinkButton ID="lnkBack" runat="server" Font-Underline="True" ForeColor="Blue"
                                OnClick="lnkBack_Click">Back To Main Menu</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" valign="bottom">
                            <asp:Label ID="Label2" runat="server" Text="To Date:"></asp:Label></td>
                        <td colspan="3" align="left" valign="bottom">
                            <telerik:RadDatePicker ID="RadDateInput2" runat="server" Skin="Web20">
                                <calendar runat="server" skin="Web20" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                    viewselectortext="x">
                                    </calendar>
                                <datepopupbutton hoverimageurl="" imageurl="" />
                                <dateinput runat="server" dateformat="dd/MMM/yyyy" displaydateformat="dd/MMM/yyyy">
                                    </dateinput>
                            </telerik:RadDatePicker></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" valign="bottom">
                        </td>
                        <td align="left" valign="bottom" colspan="4">
                            <asp:Button ID="btnProcess" runat="server" Width ="150px" OnClick="btnProcess_Click" Text="View Account Activities" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" valign="bottom">
                        </td>
                        <td align="left" valign="bottom" colspan ="4">
                            <asp:Button ID="btnGenerateReport" runat="server" Width ="150px" OnClick="btnGenerateReport_Click" Text="Generate Report" />
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td align="right" colspan="2" valign="top">
                            <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="8pt"
                                ForeColor="Black" Text="Export Features" Width="114px"></asp:Label></td>
                        <td align="left" valign="bottom" colspan="3">
                            <telerik:RadComboBox ID="cboExportOptions" runat="server" Skin="Office2007" >
                                <items>
                                        <telerik:RadComboBoxItem runat="server" Text="Excel" Value="Excel" />
                                        <telerik:RadComboBoxItem runat="server" Text="PDF" Value="PDF" />
                                        <telerik:RadComboBoxItem runat="server" Text="Word" Value="Word" />
                                        <telerik:RadComboBoxItem runat="server" Text="CSV" Value="CSV" />
                                    </items>
                            </telerik:RadComboBox>
                            <asp:Button ID="cmdExport" runat="server" Font-Bold="False" Font-Size="8pt" OnClick="cmdExport_Click"
                                Text="Export" Width="74px" /></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" valign="bottom">
                        </td>
                        <td align="left" valign="bottom" colspan="3">
                            <asp:Image ID="Image1" runat="server" Height="16px" ImageUrl="~/images/send_icon.gif"
                                Width="22px" />
                            <asp:LinkButton ID="LinkButton1" runat="server" Font-Underline="True" ForeColor="Blue"
                                OnClick="LinkButton1_Click">E-mail me this infomation</asp:LinkButton>
                        <asp:Label ID="lblMessageLine" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="8pt" ForeColor="#00C000"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" valign="bottom">
                        </td>
                        <td align="left" valign="bottom" colspan="3">
                        </td>
                    </tr>
                </table>
                <div id="divprint">
                    <div style="text-align: center">
                        <asp:Label ID="lblAccountTitle" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="9pt" ForeColor="Blue"></asp:Label><br />
                        <telerik:RadGrid ID="grdStatement" runat="server" AutoGenerateColumns="False" GridLines="None"
                            OnDataBound="grdStatement_DataBound" Skin="Office2007" OnItemCommand="GrdAccounts_OnItemCommand"
                            OnSelectedIndexChanged="GrdAccounts_SelectedIndexChanged" AllowSorting="True">
                            <mastertableview allowmulticolumnsorting="True" showfooter="True">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="SerialID" HeaderText="Ref-ID" UniqueName="Ref-ID"
                                            Visible="False" DefaultInsertValue="">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Date" HeaderText="Date" UniqueName="Date" DefaultInsertValue="">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="TrxType" HeaderText="TrxType" UniqueName="TrxType"
                                            Visible="True" DefaultInsertValue="">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Price" HeaderText="Price" UniqueName="Price" DefaultInsertValue="">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Particulars" HeaderText="Particulars" UniqueName="Particulars"
                                            EmptyDataText="--" DefaultInsertValue="">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Debits" HeaderText="Money Out" UniqueName="MoneyOut" DefaultInsertValue="">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Credits" HeaderText="Money In" UniqueName="MoneyIn" DefaultInsertValue="">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Units" HeaderText="Units" UniqueName="Units">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Value" HeaderText="Value Date" UniqueName="Value" DefaultInsertValue="" Display="false" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Debit" HeaderText="Debit" UniqueName="Debit" DefaultInsertValue="" Display="false" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataType="System.Double" HeaderText="Credit" UniqueName="Credit"
                                            DataField="Credit" DefaultInsertValue="" Display="false" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Closing" HeaderText="Balance" UniqueName="Closing" Display="false" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="NAV" HeaderText="NAV" UniqueName="NAV" Display="True" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </mastertableview>
                            <clientsettings allowcolumnsreorder="True" reordercolumnsonclient="True">
                                    <Selecting AllowRowSelect="True" />
                                </clientsettings>
                        </telerik:RadGrid>
                    <span style="color: #0000cc;" class="hovertable">*<asp:Image ID="Image2" runat="server"
                        ImageUrl="~/images/Note.gif" Width="44px" />:The figures above are only for the
                        duration selected. </span>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" rowspan="1">
            </td>
        </tr>
    </table>
    <asp:TextBox ID="TextBox1" runat="server" Height="5px" Visible="False"></asp:TextBox>
</asp:Content>
