<%@ Control Language="C#" AutoEventWireup="true" CodeFile="F2Help.ascx.cs" Inherits="Modules_F2Help" %>



<asp:Panel runat="server" ID="pnlData">
    <div id="divContainer">
        <asp:PlaceHolder ID="phControls" runat="server"></asp:PlaceHolder>
        <br />
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="adminButton" />
        <br />
        <asp:Panel runat="server" ID="pnlError" EnableViewState="false" Visible="false" class="messageBox messageBoxError">
            <asp:Literal runat="server" ID="lErrorTitle" EnableViewState="false" />
        </asp:Panel>
        <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="gvRecords_PageIndexChanging" AllowPaging="true" PageSize="15" OnRowDataBound="gvRecords_RowDataBound" OnRowCommand="gvRecords_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="col0">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnSelect" runat="server" CommandName="cmdSelect" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="col1" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCol1" runat="server" CssClass="adminData"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="col2" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCol2" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="col3" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCol3" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="col4" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCol4" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="col5" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCol5" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="col6" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCol6" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>
