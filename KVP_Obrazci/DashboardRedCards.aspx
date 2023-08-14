<%@ Page Title="Nadzorna plošča" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DashboardRedCards.aspx.cs" Inherits="KVP_Obrazci.DashboardRedCards" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var activeTabName = "";
        var isUserTpmAdmin = '<%= KVP_Obrazci.Helpers.PrincipalHelper.IsUserTpmAdmin() %>';
        $(document).ready(function () {
            var submitKVPSuccess = GetUrlQueryStrings()['successMessage'];
            activeTabName = GetUrlQueryStrings()['activeTab'];
            CurrentStRK = GetUrlQueryStrings()['stRK'];

            if (submitKVPSuccess) {
                $("#successModal").modal("show");

                // set RK number from qerry string
                CurrentStRK = CurrentStRK.replace(/%20/g, " ");

                $("#stRK").append(CurrentStRK); 
                 //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.stRK;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);


                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.successMessage;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }

            if (activeTabName != "") {
                $('.nav-tabs a[href="' + activeTabName + '"]').tab('show');
                var params = QueryStringsToObject();
                delete params.activeTab;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }

            if (isUserTpmAdmin == 'True') {
                $("#dashboard").removeClass("col-sm-9");
                $("#dashboard").addClass("col-sm-12");
                $("#openProfile").removeClass("hidden");
                $("#idRedCard").empty();
                $("#idRedCard").append("Vsi RK-ji");
                $('#profileCol').animate({
                    opacity: 0,
                    width: "toggle"
                }, 10);
            }

            $("#closeProfile").on("click", function () {

                $('#profileCol').animate({
                    opacity: 0,
                    width: "toggle"
                }, 350);

                $("#dashboard").animate({
                    width: "100%"
                }, 350, function () {
                    $("#dashboard").removeClass("col-sm-9");
                    $("#dashboard").addClass("col-sm-12");
                    $("#openProfile").removeClass("hidden");
                });
            });

            $("#openProfile").on("click", function () {
                $('#profileCol').animate({
                    opacity: 1,
                    width: "toggle"
                }, 350);

                $("#dashboard").animate({
                    width: "75%"
                }, 350, function () {
                    $("#dashboard").removeClass("col-sm-9");
                    $("#dashboard").addClass("col-sm-12");
                    $("#openProfile").addClass("hidden");
                });
            });

        });


        //All red card or my red cards
        function RowDoubleClickRedCard(s, e) {
            activeTabName = "#RedCards";
            gridKVPDocumentRedCard.GetRowValues(gridKVPDocumentRedCard.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesRedCard);
        }
        function OnGetRowValuesRedCard(value) {
            gridKVPDocumentRedCard.PerformCallback('DblClickRedCard;' + value + ";" + activeTabName);
        }


        //Red cards to confirm
        function RowDoubleClickRCsToConfirm(s, e) {
            activeTabName = "#RCsToConfirm";
            gridRCsToConfirm.GetRowValues(gridRCsToConfirm.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesRCsToConfirm);
        }
        function OnGetRowValuesRCsToConfirm(value) {
            gridKVPDocumentRedCard.PerformCallback('DblClickRCsToConfirm;' + value + ";" + activeTabName);
        }

        //Red cards that needs to be realized
        function RowDoubleClickRCsToRealize(s, e) {
            activeTabName = "#RKRealizator";
            gridRKRealizator.GetRowValues(gridRKRealizator.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesRCsToRealize);
        }
        function OnGetRowValuesRCsToRealize(value) {
            gridKVPDocumentRedCard.PerformCallback('DblClickRCsToRealize;' + value + ";" + activeTabName);
        }


        //Red cards that are already realized
        function RowDoubleClickRealizedRCs(s, e) {
            activeTabName = "#RealizedRC";
            gridRealizedRCs.GetRowValues(gridRealizedRCs.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesRealizedRCs);
        }
        function OnGetRowValuesRealizedRCs(value) {
            gridKVPDocumentRedCard.PerformCallback('DblClickRealizedRCs;' + value + ";" + activeTabName);
        }

        $(document).on('show.bs.tab', '.nav-tabs a', function (e) {
            activeTabName = e.target.hash;
        });


        function gridRCsToConfirm_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelRCsToConfirm);
        }

        function gridKVPDocumentRedCard_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelKVPDocumentRedCard);
        }

        function gridRKRealizator_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelRKRealizator);
        }

        function gridRealizedRCs_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelRealizedRC);
        }
        function SetEnableExportBtn(sender, button) {
            if (sender.GetVisibleRowsOnPage() > 0)
                button.SetEnabled(true);
        }

        function gridKVPDocumentAllRedCard_Init(s, e)
        {
            SetEnableExportBtn(s, clientBtnExportToExcelAllRedCards);
        }

        //Red cards that are already realized
        function RowDoubleClickAllRedCard(s, e) {
            activeTabName = "#AllRC";
            gridAllRedCards.GetRowValues(gridAllRedCards.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesAllRCs);
        }
        function OnGetRowValuesAllRCs(value) {
            gridKVPDocumentRedCard.PerformCallback('DblClickAllRCs;' + value + ";" + activeTabName);
        }

        function gridKVPDocumentRedCard_EndCallback(s, e) {
             if (s.cpPrintID != "" && s.cpPrintID != undefined)
            {
                window.open(s.cpPrintID, '_blank');
                delete (s.cpPrintID);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h3 style="display: inline-block;">Rdeči kartoni</h3>
            <!--<span style="margin-left: 15px;">Moji predlogi</span>-->
            <span id="openProfile" class="hidden"><i class="fa fa-gear" style="font-size: 25px; color: #777"></i></span>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-9" id="dashboard">
            <ul class="nav nav-tabs" runat="server" id="navTabs">
                <li id="redCards" class="active"><a data-toggle="tab" href="#RedCards"><span id="RedCardsBadge" runat="server" class="badge">0</span> <span id="idRedCard">Moji RK-ji</span></a></li>
                <li id="rcToConfirm"><a data-toggle="tab" href="#RCsToConfirm"><span id="RCsToConfirmBadge" runat="server" class="badge">0</span> RK-ji za potrditev</a></li>
                <li id="rkRealizator"><a data-toggle="tab" href="#RKRealizator"><span id="RKRealizatorBadge" runat="server" class="badge">0</span> RK-ji kjer sem realizator</a></li>
                <li id="rcRealized" style="background-color: #FF9A33"><a data-toggle="tab" href="#RealizedRC"><span id="RealizedRCBadge" runat="server" class="badge">0</span> RK-ji Realizirani</a></li>
                <li id="AllRC"><a data-toggle="tab" href="#AllRedCards"><span id="AllRCBadge" runat="server" class="badge">0</span> Vsi RK-ji</a></li>
            </ul>
            <div class="tab-content">
                <div id="RedCards" class="tab-pane fade in active">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled oddanih RK-ji</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelKVPDocumentRedCard" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelKVPDocumentRedCard_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelKVPDocumentRedCard" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#collapseRedCards"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="collapseRedCards" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewKVPDocumentRedCardExporter" GridViewID="ASPxGridViewKVPDocumentRedCard" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewKVPDocumentRedCard" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridKVPDocumentRedCard" SettingsBehavior-AutoFilterRowInputDelay="3000"
                                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    DataSourceID="XpoDataSourceKVPDOcumentRedCard" OnCustomCallback="ASPxGridViewKVPDocumentRedCard_CustomCallback"
                                    OnDataBound="ASPxGridViewKVPDocumentRedCard_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared"
                                    OnHtmlRowPrepared="ASPxGridViewKVPDocument_HtmlRowPrepared" OnCustomButtonCallback="ASPxGridViewKVPDocumentRedCard_CustomButtonCallback">
                                    <ClientSideEvents RowDblClick="RowDoubleClickRedCard" Init="gridKVPDocumentRedCard_Init" EndCallback="gridKVPDocumentRedCard_EndCallback" />
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="450" 
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsPopup>
                                        <HeaderFilter Height="450px" />
                                    </SettingsPopup>
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="false" AutoFilterRowInputDelay="8000"  />

                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>
                                        <dx:GridViewCommandColumn ButtonRenderMode="Image" Width="100px" Caption="Dokument" Visible="false">
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="Print">
                                                    <Image ToolTip="Natisni" SpriteProperties-CssClass="print-btn" />
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:GridViewCommandColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka RK" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis napake"
                                            FieldName="OpisNapakeRK" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowEllipsisInText="true" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Name="Priloga" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Varnost"
                                            FieldName="VarnostRK" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="id Tip popravila" FieldName="idTipRdeciKarton.idTipRdeciKarton" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tip popravila" FieldName="idTipRdeciKarton.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Aktivnost"
                                            FieldName="AktivnostRK" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowEllipsisInText="true" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewBandColumn Caption="Predlagatelj">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewBandColumn Caption="Realizator">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Realizator.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Realizator.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewDataDateColumn Caption="Termin za izvedbo"
                                            FieldName="RokOdziva" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. aktivnosti"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Prenos KVP"
                                            FieldName="PrenosRKizKVP" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>


                                         <dx:GridViewBandColumn Caption="RK vnesel">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="RCsToConfirm" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled oddanih rdečih kartonov</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelRCsToConfirm" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelRCsToConfirm_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelRCsToConfirm" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#rcsToConfirm"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="rcsToConfirm" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewRCsToConfirmExporter" GridViewID="ASPxGridViewRCsToConfirm" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewRCsToConfirm" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridRCsToConfirm" SettingsBehavior-AutoFilterRowInputDelay="3000"
                                    DataSourceID="XpoDSRCsToConfirm" Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    OnCustomCallback="ASPxGridViewRCsToConfirm_CustomCallback"
                                    OnDataBound="ASPxGridViewRCsToConfirm_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared"
                                    OnHtmlRowPrepared="ASPxGridViewKVPDocument_HtmlRowPrepared">
                                    <ClientSideEvents RowDblClick="RowDoubleClickRCsToConfirm" Init="gridRCsToConfirm_Init" />
                                    <Paddings Padding="0" />
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="false" AutoFilterRowInputDelay="8000"  />
                                    <Settings ShowVerticalScrollBar="True" GridLines ="Both"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="450" 
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsPopup>
                                        <HeaderFilter Height="450px" />
                                    </SettingsPopup>                                    
                                    
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka RK1" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis napake"
                                            FieldName="OpisNapakeRK" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="200">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowEllipsisInText="true"  />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Name="Priloga" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Varnost"
                                            FieldName="VarnostRK" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="id Tip popravila" FieldName="idTipRdeciKarton.idTipRdeciKarton" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tip popravila" FieldName="idTipRdeciKarton.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Aktivnost"
                                            FieldName="AktivnostRK" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewBandColumn Caption="Predlagatelj">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewBandColumn Caption="Realizator">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Realizator.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Realizator.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewDataDateColumn Caption="Termin za izvedbo"
                                            FieldName="RokOdziva" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. aktivnosti"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Prenos KVP"
                                            FieldName="PrenosRKizKVP" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                         <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>


                                         <dx:GridViewBandColumn Caption="RK vnesel">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="RKRealizator" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled rdečih kartonov kjer sem realizator</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelRKRealizator" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelRKRealizator_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelRKRealizator" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#RKrealizator"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="RKrealizator" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewRKRealizatorExporter" GridViewID="ASPxGridViewRKRealizator" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewRKRealizator" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridRKRealizator" SettingsBehavior-AutoFilterRowInputDelay="3000" DataSourceID="XpoDSRCsRealizator"
                                    KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared"
                                    OnHtmlRowPrepared="ASPxGridViewKVPDocument_HtmlRowPrepared"
                                    OnDataBound="ASPxGridViewRKRealizator_DataBound">
                                    <Paddings Padding="0" />
                                    <ClientSideEvents Init="gridRKRealizator_Init" RowDblClick="RowDoubleClickRCsToRealize" />
                                    <Settings ShowVerticalScrollBar="True"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="450" 
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsPopup>
                                        <HeaderFilter Height="450px" />
                                    </SettingsPopup>
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000"  />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka RK" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis napake"
                                            FieldName="OpisNapakeRK" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Name="Priloga" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Varnost"
                                            FieldName="VarnostRK" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="id Tip popravila" FieldName="idTipRdeciKarton.idTipRdeciKarton" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tip popravila" FieldName="idTipRdeciKarton.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Aktivnost"
                                            FieldName="AktivnostRK" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewBandColumn Caption="Predlagatelj">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewBandColumn Caption="Realizator">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Realizator.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Realizator.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewDataDateColumn Caption="Termin za izvedbo"
                                            FieldName="RokOdziva" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. aktivnosti"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Prenos KVP"
                                            FieldName="PrenosRKizKVP" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>


                                         <dx:GridViewBandColumn Caption="RK vnesel">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="RealizedRC" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled realiziranih rdečih kartonov</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelRealizedRC" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelRealizedRC_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelRealizedRC" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#RealizedRCCollapse"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="RealizedRCCollapse" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewRealizedRCExporter" GridViewID="ASPxGridViewRealizedRC" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewRealizedRC" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridRealizedRCs" SettingsBehavior-AutoFilterRowInputDelay="3000"
                                    Width="100%" KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    DataSourceID="XPODSRealizedRCs" OnDataBound="ASPxGridViewRealizedRC_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared"
                                    OnHtmlRowPrepared="ASPxGridViewKVPDocument_HtmlRowPrepared">
                                    <ClientSideEvents RowDblClick="RowDoubleClickRealizedRCs" Init="gridRealizedRCs_Init" />
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="450" 
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsPopup>
                                        <HeaderFilter Height="450px" />
                                    </SettingsPopup>
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka RK" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis napake"
                                            FieldName="OpisNapakeRK" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Name="Priloga" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Varnost"
                                            FieldName="VarnostRK" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="id Tip popravila" FieldName="idTipRdeciKarton.idTipRdeciKarton" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tip popravila" FieldName="idTipRdeciKarton.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Aktivnost"
                                            FieldName="AktivnostRK" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewBandColumn Caption="Predlagatelj">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewBandColumn Caption="Realizator">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Realizator.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Realizator.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewDataDateColumn Caption="Termin za izvedbo"
                                            FieldName="RokOdziva" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. aktivnosti"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Prenos KVP"
                                            FieldName="PrenosRKizKVP" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>


                                         <dx:GridViewBandColumn Caption="RK vnesel">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="AllRedCards" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled oddanih RK-ji</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelAllRedCards" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelAllRedCards_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelAllRedCards" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#collapseAllRedCards"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="collapseAllRedCards" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewExporterAllRedCards" GridViewID="ASPxGridViewAllRedCards" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewAllRedCards" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridAllRedCards" SettingsBehavior-AutoFilterRowInputDelay="3000"
                                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    DataSourceID="XpoDSAllRC" OnCustomCallback="ASPxGridViewAllRedCards_CustomCallback"
                                    OnDataBound="ASPxGridViewAllRedCards_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared"
                                    OnHtmlRowPrepared="ASPxGridViewKVPDocument_HtmlRowPrepared">
                                    <ClientSideEvents RowDblClick="RowDoubleClickAllRedCard" Init="gridKVPDocumentAllRedCard_Init" />
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="450" 
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="70,90,110" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsPopup>
                                        <HeaderFilter Height="450px" />
                                    </SettingsPopup>
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka RK" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis napake"
                                            FieldName="OpisNapakeRK" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Name="Priloga" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Varnost"
                                            FieldName="VarnostRK" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="id Tip popravila" FieldName="idTipRdeciKarton.idTipRdeciKarton" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tip popravila" FieldName="idTipRdeciKarton.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Aktivnost"
                                            FieldName="AktivnostRK" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewBandColumn Caption="Predlagatelj">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Predlagatelj.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewBandColumn Caption="Realizator">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Realizator.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Realizator.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewDataDateColumn Caption="Termin za izvedbo"
                                            FieldName="RokOdziva" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. aktivnosti"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="Prenos KVP"
                                            FieldName="PrenosRKizKVP" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                         <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>


                                         <dx:GridViewBandColumn Caption="RK vnesel">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" ExportWidth="90">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="RKVnesel.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" ExportWidth="120">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-3" id="profileCol">
            <div class="panel-group">
                <div class="panel panel-default">
                    <div class="panel-body" style="padding: 0;">
                        <div class="panel panel-default" style="background-color: #00A7D0; padding-bottom: 30px;">
                            <div class="panel-body" style="color: #fff;">

                                <span id="closeProfile">
                                    <i class="fa fa-close" style="font-size: 18px; color: gray;"></i>
                                </span>

                                <h4>
                                    <dx:ASPxLabel ID="lblUsersName" runat="server" ForeColor="White" Font-Size="26px" />
                                </h4>
                                <p>
                                    <dx:ASPxLabel ID="lblDepartmentParagraph" runat="server" ForeColor="White" Font-Size="15px" />
                                </p>
                            </div>
                        </div>
                        <div class="panel panel-default" style="margin-top: 0; padding-top: 30px;">
                            <div class="panel-body">
                                <div class="row" style="margin: 0 auto; text-align: center;">
                                    <div class="col-xs-6" style="border-right: 1px solid #e1e1e1;">
                                        <div style="padding-bottom: 7px;"><span runat="server" id="RCNumber" class="badge">X</span></div>
                                        <div>Vseh RKjev</div>
                                    </div>
                                    <div class="col-xs-6">
                                        <div style="padding-bottom: 7px;"><span runat="server" id="RCCompleted" class="badge">X</span></div>
                                        <div>Zaključenih RKjev</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="img-profile-wrap">
                    <img src="Images/defaultPerson.png" width="100" alt="defaultPic" />
                </div>
                <div class="panel panel-default" style="margin-top: 15px;">
                    <div class="panel-heading">
                        <h4 class="panel-title" style="display: inline-block;">Moji podatki</h4>
                        <a data-toggle="collapse" data-target="#MyInfo"></a>
                    </div>
                    <div id="MyInfo" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <div class="row2 small-padding-bottom">
                                <div class="col-xs-4">Oddelek: </div>
                                <div class="col-xa-8">
                                    <dx:ASPxLabel ID="lblDepartment" runat="server" ForeColor="Black" Font-Bold="true" />
                                </div>
                            </div>
                            <div class="row2 small-padding-bottom">
                                <div class="col-xs-4">Vodja oddelka: </div>
                                <div class="col-xa-8">
                                    <dx:ASPxLabel ID="lblSupervisor" runat="server" ForeColor="Black" Font-Bold="true" />
                                </div>
                            </div>
                            <div class="row2 small-padding-bottom">
                                <div class="col-xs-4">Champion: </div>
                                <div class="col-xa-8">
                                    <dx:ASPxLabel ID="lblChampion" runat="server" ForeColor="Black" Font-Bold="true" />
                                </div>
                            </div>
                            <div class="row2 small-padding-bottom">
                                <div class="col-xs-4">KVP skupina: </div>
                                <div class="col-xa-8">
                                    <dx:ASPxLabel ID="lblKvpGroup" runat="server" ForeColor="Black" Font-Bold="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <dx:ASPxButton ID="btnAddNewKvp" runat="server" Text="Nov KVP" RenderMode="Button" ImagePosition="Top"
                    AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnAddNewKvp_Click" Width="180px">
                    <Image Url="Images/add.png" UrlHottracked="Images/addHover.png" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnAddNewredCard" runat="server" Text="Nov Rdeči karton" RenderMode="Button" ImagePosition="Top"
                    OnClick="btnAddNewredCard_Click" AutoPostBack="false" UseSubmitBehavior="false" Width="180px">
                    <Image Url="Images/redcard.png" UrlHottracked="Images/redcardHoover.png" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div id="successModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: #47c9a2; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="fa fa-check-square-o" style="font-size: 60px; color: white"></i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Odlično!</h3>
                    <p>Vaš rdeči karton je bil uspešno posredovan TPM administratorju!</p>
                    <p>Za hitrejšo odpravo napake napišite št. RK na rdeč kartonček in tega namestite na mesto napake.</p>
                    <p>Št. rdečega kartona:</p><b style="font-size:25px" id="stRK"></b>
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
            </div>

        </div>
    </div>

    <dx:XpoDataSource ID="XpoDataSourceKVPDOcumentRedCard" runat="server" ServerMode="True"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is not null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSRCsToConfirm" runat="server"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is not null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSRCsRealizator" runat="server"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is not null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XPODSRealizedRCs" runat="server" ServerMode="True"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is not null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSAllRC" runat="server" ServerMode="True"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is not null">
    </dx:XpoDataSource>

</asp:Content>