<%@ Page Title="Poročilo KVP" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="FullKVPReport.aspx.cs" Inherits="KVP_Obrazci.KVPDocuments.FullKVPReport" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function clientFullKVPReportCallbackPanel_EndCallback(s, e) {
            clientLoadingPanel.Hide();

            if (gridFullKVPReport.GetVisibleRowsOnPage() > 0)
                clientBtnExportToExcel.SetEnabled(true);
        }

        function btnRefreshGrid_Click(s, e) {

            var dateFields = [clientDateEditDateFrom, clientDateEditDateTo];

            var process = InputFieldsValidation(null, null, dateFields, null, null, null);
            if (process) {
                clientLoadingPanel.Show();
                clientFullKVPReportCallbackPanel.PerformCallback('CreateReport');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="clientLoadingPanel" Modal="true" runat="server"></dx:ASPxLoadingPanel>
    <dx:ASPxCallbackPanel ID="FullKVPReportCallbackPanel" runat="server" Width="100%" ClientInstanceName="clientFullKVPReportCallbackPanel"
        OnCallback="FullKVPReportCallbackPanel_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="clientFullKVPReportCallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <h4 class="panel-title" style="display: inline-block;">Pregled KVP predlogov po obdobju</h4>
                                <a data-toggle="collapse" data-target="#collapseOne"></a>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row small-padding-bottom small-padding-top">
                                        <div class="col-md-3 hidden-sm"></div>
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
                                            <dx:ASPxGridViewExporter ID="FullKVPReportExporter" GridViewID="ASPxGridViewFullKVPReport" runat="server"></dx:ASPxGridViewExporter>
                                            <dx:ASPxGridView ID="ASPxGridViewFullKVPReport" runat="server" EnableCallbackCompression="true"
                                                ClientInstanceName="gridFullKVPReport" AutoGenerateColumns="true"
                                                Width="100%" KeyFieldName="idKVPFullReport" CssClass="gridview-no-header-padding" OnDataBinding="ASPxGridViewFullKVPReport_DataBinding">
                                                <Paddings Padding="5px" />
                                                <Settings ShowVerticalScrollBar="True" HorizontalScrollBarMode="Auto"
                                                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
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
                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPFullReport" Width="80px"
                                                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Leto" FieldName="Leto" Width="80px"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Mesec"
                                                        FieldName="Mesec" ShowInCustomizationForm="True"
                                                        Width="80px" >
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="TrenutniStatus"
                                                        FieldName="TrenutniStatus" ShowInCustomizationForm="True"
                                                        Width="150px" >
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px"
                                                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>


                                                    <dx:GridViewDataTextColumn Caption="KVP Skupina Koda"
                                                        FieldName="KVPSkupinaKoda" ShowInCustomizationForm="True"
                                                        Width="250px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP Skupina Naziv"
                                                        FieldName="KVPSkupinaNaziv" ShowInCustomizationForm="True"
                                                        Width="200px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataDateColumn Caption="Datum"
                                                        FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                                        Width="150px">
                                                        <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataDateColumn>


                                                    <dx:GridViewDataTextColumn Caption="Uporabnik"
                                                        FieldName="Uporabnik" ShowInCustomizationForm="True"
                                                        Width="0px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP Problem"
                                                        FieldName="KVPProblem" ShowInCustomizationForm="True"
                                                        Width="130px" Visible="false">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP Predlog Izboljšave"
                                                        FieldName="KVPPredlogIzbolsave" ShowInCustomizationForm="True"
                                                        Width="130px" Visible="false">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Vodja Za Odobritev"
                                                        FieldName="VodjaZaOdobritev" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Presoja"
                                                        FieldName="Presoja" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Realizator"
                                                        FieldName="Realizator" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Datum Zaključene Ideje"
                                                        FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>
                                                   

                                                    <dx:GridViewDataTextColumn Caption="Cas do zaklj. ideje"
                                                        FieldName="CasDoZakljuceneIdeje" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="TipIdeje"
                                                        FieldName="TipIdeje" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Točke Predlagatelj"
                                                        FieldName="TockePredlagatelj" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Točke Realizator"
                                                        FieldName="TockeRealizator" ShowInCustomizationForm="True"
                                                        Width="130px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="IDZaposlenega"
                                                        FieldName="ExternalIDaposlenega" ShowInCustomizationForm="True"
                                                        Width="130px">
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
                </div>
                <dx:XpoDataSource ID="XpoDSFullKVPReport" runat="server" ServerMode="True"
                    DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPFullReport">
                </dx:XpoDataSource>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
