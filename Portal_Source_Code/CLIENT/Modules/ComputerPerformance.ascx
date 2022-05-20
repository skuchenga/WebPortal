<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ComputerPerformance.ascx.cs"
    Inherits="ComputerPerformance" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Panel ID="pnlPerformance" runat="server">
    <asp:UpdatePanel ID="upPerformanceFunds" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        REPORT FROM
                    </td>
                    <td>
                        REPORT TO
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadDatePicker ID="dtReportFrom" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="dtReportTo" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:Button ID="btnGenerateReport" runat="server" Text="Calculate Performance" OnClick="btnGenerateReport_Click" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdateProgress ID="up1" runat="server" AssociatedUpdatePanelID="upPerformanceFunds">
        <ProgressTemplate>
            <div class="progress">
                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/images/UpdateProgress.gif"
                    AlternateText="update" />
                <span style="color: Red">GENERATING EXCEL REPORT.Please wait...... </span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Panel>
