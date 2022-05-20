<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilesUpload.ascx.cs" Inherits="Modules_FilesUpload" %>

<%@ Register TagPrefix="BRIMS" TagName="ToolTipLabel" Src="~/Modules/ToolTipLabelControl.ascx" %>

<script type="text/javascript">
    function showUploadPanels() {
        if ($('#upl20').is(':hidden')) {
            $('#upl20').show();
            $('#upl21').show();
        }
        else if ($('#upl30').is(':hidden')) {
            $('#upl20').show();
            $('#upl21').show();
            $('#upl30').show();
        }
        else if ($('#upl31').is(':hidden')) {
            $('#upl31').show();
        }
        else if ($('#upl32').is(':hidden')) {
            $('#upl32').show();
        }
        else if ($('#upl33').is(':hidden')) {
            $('#upl33').show();
        }
        else {

            $('#<%=btnMoreUploads.ClientID %>').attr("disabled", "disabled");
        }
}
</script>

<asp:Panel runat="server" ID="pnlData">

    <p>
        <strong>
            Attach Files
        </strong>
    </p>
    <table class="adminContent">
        <tr>
            <td class="adminTitle">
                <BRIMS:ToolTipLabel runat="server" ID="lblFile" Text="Select File: "
                    ToolTip="Select File" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <asp:FileUpload ID="fuFile1" CssClass="adminInput" runat="server"
                    ToolTip="Choose a file to upload" />
            </td>
        </tr>
        <tr id="upl20" style="display: none;">
            <td colspan="2">
                <br />
            </td>
        </tr>
        <tr id="upl21" style="display: none;">
            <td class="adminTitle">
                <BRIMS:ToolTipLabel runat="server" ID="lblSelectFile2" Text="Select File: "
                    ToolTip="Select a file to upload" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <asp:FileUpload ID="fuFile2" CssClass="adminInput" runat="server"
                    ToolTip="Select a file to upload" />
            </td>
        </tr>
        <tr id="upl30" style="display: none;">
            <td colspan="2">
                <br />
            </td>
        </tr>
        <tr id="upl31" style="display: none;">
            <td class="adminTitle">
                <BRIMS:ToolTipLabel runat="server" ID="lblSelectFile3" Text="Select File: "
                    ToolTip="Select a file to upload" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <asp:FileUpload ID="fuFile3" CssClass="adminInput" runat="server"
                    ToolTip="Select a file to upload" />
            </td>
        </tr>
        <tr id="upl32" style="display: none;">
            <td class="adminTitle">
                <BRIMS:ToolTipLabel runat="server" ID="lblSelectFile4" Text="Select File: "
                    ToolTip="Select a file to upload" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <asp:FileUpload ID="fuFile4" CssClass="adminInput" runat="server"
                    ToolTip="Select a file to upload" />
            </td>
        </tr>
        <tr id="upl33" style="display: none;">
            <td class="adminTitle">
                <BRIMS:ToolTipLabel runat="server" ID="lblSelectFile5" Text="Select File: "
                    ToolTip="Select a file to upload" ToolTipImage="~/Common/ico-help.gif" />
            </td>
            <td class="adminData">
                <asp:FileUpload ID="fuFile5" CssClass="adminInput" runat="server"
                    ToolTip="Select a file to upload" />
            </td>
        </tr>
        <tr>
        <td class="adminTitle">
            <asp:Button runat="server" ID="btnMoreUploads" CssClass="f2Button" Text="+" />
        </td>
            <td class="adminData">
                <asp:Button runat="server" ID="btnUploadFile" CssClass="f2Button" Text="Upload"
                    ValidationGroup="UploadNewFile" OnClick="btnUploadFile_Click"
                    ToolTip="Upload the file" />
            </td>
        </tr>
    </table>

</asp:Panel>