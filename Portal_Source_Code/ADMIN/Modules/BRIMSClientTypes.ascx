<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BRIMSClientTypes.ascx.cs" Inherits="Modules_BRIMSClientTypes" %>

<script type="text/javascript">

    $(window).bind('load', function () {
        var cbHeader = $(".cbHeader input");
        var cbRowItem = $(".cbRowItem input");
        cbHeader.bind("click", function () {
            cbRowItem.each(function () { this.checked = cbHeader[0].checked; })
        });
        cbRowItem.bind("click", function () { if ($(this).checked == false) cbHeader[0].checked = false; });
    });
</script>
<asp:Panel runat="server" ID="pnlError" EnableViewState="false" Visible="false" class="messageBox messageBoxError">
    <asp:Literal runat="server" ID="lErrorTitle" EnableViewState="false" />
</asp:Panel>
<asp:Panel ID="pnlAMClient" runat="server" GroupingText="Select Client Type:" CssClass="panel-data">
    <asp:GridView ID="gvAMClientTypes" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="true" PageSize="15" OnRowDataBound="gvAMClientTypes_RowDataBound"
       >
        <Columns>
            <asp:TemplateField ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                <HeaderTemplate>
                    <asp:CheckBox ID="cbSelectAll" runat="server" CssClass="cbHeader" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbClientType" runat="server" CssClass="cbRowItem" />
                    <asp:HiddenField ID="hfClientTypeId" runat="server" Value='<%# Eval("Code") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Client Type" ItemStyle-Width="35%">
                <ItemTemplate>
                    <asp:Label ID="lblClientType" runat="server" Text='<%# Eval("ClientType") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>
