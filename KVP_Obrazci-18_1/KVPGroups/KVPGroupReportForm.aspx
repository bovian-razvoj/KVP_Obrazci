<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="KVPGroupReportForm.aspx.cs" Inherits="KVP_Obrazci.KVPGroups.KVPGroupReportForm" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function clientKVPGroupReportCallbackPanel_EndCallback(s, e) {
            clientLoadingPanel.Hide();

            if (gridKVPGroupReport.GetVisibleRowsOnPage() > 0)
                clientBtnExportToExcel.SetEnabled(true);
        }

        function btnRefreshGrid_Click(s, e) {

            var dateFields = [clientDateEditDateFrom, clientDateEditDateTo];

            var process = InputFieldsValidation(null, null, dateFields, null, null, null);
            if (process) {
                clientLoadingPanel.Show();
                clientKVPGroupReportCallbackPanel.PerformCallback('CreateReport');
            }
        }

        function clientSelectionRadioButton_SelectedIndexChanged(s, e)
        {
            clientKVPGroupReportCallbackPanel.PerformCallback('HideColumns');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="clientLoadingPanel" runat="server"></dx:ASPxLoadingPanel>
    <dx:ASPxCallbackPanel ID="KVPGroupReportCallbackPanel" runat="server" Width="100%" ClientInstanceName="clientKVPGroupReportCallbackPanel"
        OnCallback="KVPGroupReportCallbackPanel_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="clientKVPGroupReportCallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <h4 class="panel-title" style="display: inline-block;">Pregled KVP skupin po obdobju</h4>
                                <a data-toggle="collapse" data-target="#collapseOne"></a>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row small-padding-bottom small-padding-top">
                                        <div class="col-md-3">
                                            <dx:ASPxRadioButtonList ID="SelectionRadioButton" ClientInstanceName="clientSelectionRadioButton" runat="server" RepeatColumns="2"
                                                Caption="Filtiraj po" CaptionSettings-HorizontalAlign="Center" CaptionSettings-ShowColon="true">
                                                <Items>
                                                    <dx:ListEditItem Text="Datum vnosa" Value="StartKVP" />
                                                    <dx:ListEditItem Text="Datum zaključene ideje" Value="CompletedKVP" />
                                                </Items>
                                                <ClientSideEvents SelectedIndexChanged="clientSelectionRadioButton_SelectedIndexChanged" />
                                            </dx:ASPxRadioButtonList>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row2 align-item-centerV-endH">
                                                <div class="col-sm-0 big-margin-r">
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="DATUM OD : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-7 no-padding-left">
                                                    <dx:ASPxDateEdit ID="DateEditDateFrom" runat="server" EditFormat="Date" Width="100%"
                                                        CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="clientDateEditDateFrom">
                                                        <FocusedStyle CssClass="focus-text-box-input" />
                                                        <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                        <DropDownButton Visible="true"></DropDownButton>
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row2 align-item-centerV-startH">
                                                <div class="col-sm-0 big-margin-r">
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DATUM DO : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-7 no-padding-left">
                                                    <dx:ASPxDateEdit ID="DateEditDateTo" runat="server" EditFormat="Date" Width="100%"
                                                        CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="clientDateEditDateTo">
                                                        <FocusedStyle CssClass="focus-text-box-input" />
                                                        <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                        <DropDownButton Visible="true"></DropDownButton>
                                                    </dx:ASPxDateEdit>
                                                </div>
                                                <div class="col-xs-2">

                                                    <dx:ASPxButton ID="btnRefreshGrid" runat="server" UseSubmitBehavior="false" AutoPostBack="false"
                                                        ClientInstanceName="clientBtnRefreshGrid" Text="Osveži" Width="100px">
                                                        <Paddings PaddingLeft="25px" PaddingRight="25px" PaddingTop="0px" PaddingBottom="0px" />
                                                        <ClientSideEvents Click="btnRefreshGrid_Click" />
                                                    </dx:ASPxButton>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 hidden-sm"></div>
                                    </div>

                                    <div class="row small-padding-bottom">
                                        <div class="col-xs-12">
                                            <dx:ASPxButton ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click" RenderMode="Link" ClientEnabled="false"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcel" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <Paddings PaddingLeft="10px" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" />
                                            </dx:ASPxButton>
                                            <dx:ASPxGridViewExporter ID="KVPGroupReportxporter" GridViewID="ASPxGridViewKVPGroupReport1" runat="server"></dx:ASPxGridViewExporter>
                                            <dx:ASPxGridView ID="ASPxGridViewKVPGroupReport1" runat="server" EnableCallbackCompression="true"
                                                ClientInstanceName="gridKVPGroupReport" AutoGenerateColumns="false"
                                                Width="100%" KeyFieldName="idKVPGroupReport" CssClass="gridview-no-header-padding" OnDataBinding="ASPxGridViewKVPGroupReport1_DataBinding">
                                                <Paddings Padding="5px" />
                                                <Settings ShowVerticalScrollBar="True" HorizontalScrollBarMode="Auto"
                                                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="500"
                                                    ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                    <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                    </PageSizeItemSettings>
                                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                </SettingsPager>
                                                <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
                                                <Styles Header-Wrap="True">
                                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                </Styles>
                                                <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPGroupReport" Width="80px"
                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Koda" FieldName="SkupinaKoda" Width="180px"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains"  />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Naziv"
                                                        FieldName="SkupinaNaziv" ShowInCustomizationForm="True"
                                                        Width="180px" >
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Podani KVP-ji"
                                                        FieldName="Podani" ShowInCustomizationForm="True"
                                                        Width="150px" >
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                     <dx:GridViewDataTextColumn Caption="Odprti KVP-ji"
                                                        FieldName="Odprti" ShowInCustomizationForm="True"
                                                        Width="200px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Zaključeni KVP-ji" FieldName="Realizirani" Width="140px" ShowInCustomizationForm="True"
                                                        Visible="false">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Zavrnjeni KVP-ji"
                                                        FieldName="Zavrnjeni" ShowInCustomizationForm="True"
                                                        Width="250px" Visible="false">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                   
                                                </Columns>
                                            </dx:ASPxGridView>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
