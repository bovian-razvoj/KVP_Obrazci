<%@ Page Title="Nadzorna plošča" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="KVP_Obrazci.Dashboard" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var activeTabName = "";
        var isUserChampion = '<%= KVP_Obrazci.Helpers.PrincipalHelper.IsUserChampion() %>';
        $(document).ready(function () {
            var submitKVPSuccess = GetUrlQueryStrings()['successMessage'];
            var trasnferToRedCard = GetUrlQueryStrings()['<%= KVP_Obrazci.Common.Enums.QueryStringName.transferToRedCard.ToString() %>'];
            activeTabName = GetUrlQueryStrings()['activeTab'];

            if (submitKVPSuccess) {
                $("#successModal").modal("show");

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

            if (trasnferToRedCard) {
                $(".modal-body p").empty();
                $(".modal-body p").append("KVP predlog je bil uspešno prenešen v rdeči karton");
                $("#successModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.transferToRedCard;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }

            if (isUserChampion == 'True') {
                $("#dashboard").removeClass("col-sm-9");
                $("#dashboard").addClass("col-sm-12");
                $("#openProfile").removeClass("hidden");

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

        function RowDoubleClick(s, e) {
            activeTabName = "#MyKVPs";
            gridKVPDocument.GetRowValues(gridKVPDocument.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValues);
        }
        function OnGetRowValues(value) {
            gridKVPDocument.PerformCallback('DblClick;' + value + ";" + activeTabName);
        }



        function RowDoubleClickKVPsToConfirm(s, e) {
            activeTabName = "#KVPsToConfirm";
            gridKVPsToConfirm.GetRowValues(gridKVPsToConfirm.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesKVPsToConfirm);
        }
        function RowDoubleClickKVPsToCheck(s, e) {
            activeTabName = "#KVPsToCheck";
            gridKVPsToCheck.GetRowValues(gridKVPsToCheck.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesKVPsToCheck);
        }
        function OnGetRowValuesKVPsToConfirm(value) {
            gridKVPDocument.PerformCallback('DblClickKVPsToConfirm;' + value + ";" + activeTabName);
        }
        function OnGetRowValuesKVPsToCheck(value) {
            gridKVPDocument.PerformCallback('DblClickKVPsToCheck;' + value + ";" + activeTabName);
        }

        function RowDoubleClickAuditorKVPs(s, e) {
            activeTabName = "#KVPAuditor";
            gridAuditorKVPs.GetRowValues(gridAuditorKVPs.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesAuditorKVPs);
        }
        function OnGetRowValuesAuditorKVPs(value) {
            gridKVPDocument.PerformCallback('DblClickAuditorsKVPs;' + value + ";" + activeTabName);
        }

        function RowDoubleClickKVPsToRealize(s, e) {
            activeTabName = "#KVPRealizator";
            gridKVPDocumentToRealize.GetRowValues(gridKVPDocumentToRealize.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesKVPsToRealize);
        }
        function OnGetRowValuesKVPsToRealize(value) {
            gridKVPDocument.PerformCallback('DblClickKVPsToRealize;' + value + ";" + activeTabName);
        }

        function RowDoubleClickRealizedKVPs(s, e) {
            activeTabName = "#RealizedKVP";
            gridRealizedKVPs.GetRowValues(gridRealizedKVPs.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesRealizedKVPs);
        }
        function OnGetRowValuesRealizedKVPs(value) {
            gridKVPDocument.PerformCallback('DblClickRealizedKVPs;' + value + ";" + activeTabName);
        }

        function RowDoubleClickChampionAllKVPs(s, e) {
            activeTabName = "#ChampionAllKVP";
            gridChampionAllKVPs.GetRowValues(gridChampionAllKVPs.GetFocusedRowIndex(), 'idKVPDocument', OnGetRowValuesChampionAllKVPs);
        }
        function OnGetRowValuesChampionAllKVPs(value) {
            gridKVPDocument.PerformCallback('DblClickChampionAllKVPs;' + value + ";" + activeTabName);
        }

        function gridKVPDocumentRealize_SelectionChanged(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnRealizeSelected.SetEnabled(true);
            else
                clientBtnRealizeSelected.SetEnabled(false);
        }

        //function gridKVPsToConfirm_SelectionChanged(s, e) {
        //    alert(s);
        //}


        $(document).on('show.bs.tab', '.nav-tabs a', function (e) {
            activeTabName = e.target.hash;
        });

        function gridKVPDocument_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelMyKVPDocuments);
        }

        function gridKVPDocumentToRealize_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelKVPToRealize);
        }

        function gridKVPsToConfirm_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelKVPsToConfirm);
        }

        function gridKVPsToCheck_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelKVPsToCheck);
        }



        function gridRKRealizator_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelRKRealizator);
        }

        function gridAuditorKVPs_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelAuditorKVPs);
        }

        function gridRealizedKVPs_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelRealizedKVP);
        }

        function gridAllKVPs_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelAllKVP);
        }

        function SetEnableExportBtn(sender, button) {
            if (sender.GetVisibleRowsOnPage() > 0)
                button.SetEnabled(true);
        }

        function MyKVPDocumentGrid_EndCallback(s, e) {
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
            <h3 style="display: inline-block;">KVP Sistem</h3>
            <span style="margin-left: 15px;">Moji predlogi</span>
            <span id="openProfile" class="hidden"><i class="fa fa-gear" style="font-size: 25px; color: #777"></i></span>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-9" id="dashboard">
            <ul class="nav nav-tabs" runat="server" id="navTabs">
                <li id="myKVPs" class="active"><a data-toggle="tab" href="#MyKVPs"><span id="MyKVPsBadge" runat="server" class="badge">0</span> Moji KVP-ji</a></li>
                <li id="kvpRealizator"><a data-toggle="tab" href="#KVPRealizator"><span id="KVPRealizatorBadge" runat="server" class="badge">0</span> KVP-ji kjer sem realizator</a></li>
                <li id="kvpToConfirm"><a data-toggle="tab" href="#KVPsToConfirm"><span id="KVPsToConfirmBadge" runat="server" class="badge">0</span> KVP-ji za potrditev</a></li>
                <li id="kvpToCheck"><a data-toggle="tab" href="#KVPsToCheck"><span id="KVPsToCheckBadge" runat="server" class="badge">0</span> KVP-ji za preverjanje</a></li>
                <li id="kvpAuditor"><a data-toggle="tab" href="#KVPAuditor"><span id="KVPAuditorBadge" runat="server" class="badge">0</span> KVP-ji kjer sem presojevalec</a></li>
                <li id="kvpRealized" style="background-color: #FF9A33"><a data-toggle="tab" href="#RealizedKVP"><span id="RealizedKVPBadge" runat="server" class="badge">0</span> Realizirani KVP-ji</a></li>
                <li id="championAllKVPs" style="background-color: #00CED1"><a data-toggle="tab" href="#ChampionAllKVP"><span id="ChampionAllKVPBadge" runat="server" class="badge">0</span> Vsi KVP-ji</a></li>
            </ul>
            <div class="tab-content">
                <div id="MyKVPs" class="tab-pane fade in active">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title">Pregled oddanih predlogov</h4>
                                </div>

                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelMyKVPDocuments" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelMyKVPDocuments_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelMyKVPDocuments" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" style="display: inline-block;" href="#myKVPsPanel"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="myKVPsPanel" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewKVPDocumentExporter" GridViewID="ASPxGridViewKVPDocument" runat="server">
                                    <Styles>
                                        <Header Wrap="True" />
                                    </Styles>
                                </dx:ASPxGridViewExporter>
                                <!-- Moji KVP -->
                                <dx:ASPxGridView ID="ASPxGridViewKVPDocument" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true" SettingsText-SearchPanelEditorNullText="Vnesi iskalni niz ..."
                                    ClientInstanceName="gridKVPDocument" SettingsBehavior-AutoFilterRowInputDelay="6000"
                                    Width="100%" KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    DataSourceID="XpoDataSourceKVPDOcument" OnCustomCallback="ASPxGridViewKVPDocument_CustomCallback" OnHtmlRowPrepared="ASPxGridViewKVPDocument_HtmlRowPrepared"
                                    OnDataBound="ASPxGridViewKVPDocument_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared" OnAfterPerformCallback="ASPxGridViews_AfterPerformCallback" OnCustomUnboundColumnData="ASPxGridViewKVPDocument_CustomUnboundColumnData"
                                    OnCustomButtonCallback="ASPxGridViewKVPDocument_CustomButtonCallback">
                                    <ClientSideEvents RowDblClick="RowDoubleClick" Init="gridKVPDocument_Init" EndCallback="MyKVPDocumentGrid_EndCallback" />
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
                                    <SettingsSearchPanel Visible="True"></SettingsSearchPanel>

                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
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

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
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

                                        <dx:GridViewDataTextColumn Caption="Problem"
                                            FieldName="OpisProblem" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Predlog izboljšave"
                                            FieldName="PredlogIzboljsave" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>



                                        <dx:GridViewDataTextColumn Width="90px" Name="Priloga" Caption="Priloga" ShowInCustomizationForm="True" FieldName="Priloga">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priloge"
                                            FieldName="Priloge" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="PrilogaBit" Visible="True"
                                            UnboundType="String" FieldName="PrilogaBit" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="false" AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                                        </dx:GridViewDataTextColumn>


                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewBandColumn Caption="Realizator">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Realizator.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" Settings-FilterMode="DisplayText">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>

                                                <dx:GridViewDataColumn FieldName="Realizator.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>

                                        <dx:GridViewDataTextColumn Caption="Presojevalec" FieldName="LastPresojaID.Lastname" Width="130px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. ideje"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>




                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="KVPRealizator" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled predlogov za realizacijo</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelKVPToRealize" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelKVPToRealize_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelKVPToRealize" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#kvpsToRealize"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="kvpsToRealize" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <!-- Kjer sem realizator KVP -->
                                <dx:ASPxGridViewExporter ID="ASPxGridViewKVPToRealizeExporter" GridViewID="ASPxGridViewKVPToRealize" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewKVPToRealize" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridKVPDocumentToRealize" SettingsBehavior-AutoFilterRowInputDelay="6000"
                                    Theme="Moderno" Width="100%" KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    DataSourceID="XpoDataSourceKVPDOcumentToRealize" OnCustomCallback="ASPxGridViewKVPToRealize_CustomCallback"
                                    OnDataBound="ASPxGridViewKVPToRealize_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared" OnAfterPerformCallback="ASPxGridViews_AfterPerformCallback" OnCustomUnboundColumnData="ASPxGridViewKVPDocument_CustomUnboundColumnData">
                                    <ClientSideEvents RowDblClick="RowDoubleClickKVPsToRealize" SelectionChanged="gridKVPDocumentRealize_SelectionChanged" Init="gridKVPDocumentToRealize_Init" />
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
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000"></SettingsBehavior>
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="50px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" />

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
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

                                        <dx:GridViewDataTextColumn Caption="Problem"
                                            FieldName="OpisProblem" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Predlog izboljšave"
                                            FieldName="PredlogIzboljsave" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priloge"
                                            FieldName="Priloge" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="PrilogaBit" Visible="True"
                                            UnboundType="String" FieldName="PrilogaBit" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="false" AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

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

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. ideje"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>



                                    </Columns>
                                </dx:ASPxGridView>

                                <div class="text-center small-margin-b small-margin-t">
                                    <dx:ASPxButton ID="btnRealizeSelected" runat="server" UseSubmitBehavior="false" AutoPostBack="false" ClientEnabled="false"
                                        ClientInstanceName="clientBtnRealizeSelected" OnClick="btnRealizeSelected_Click" Text="Realiziraj izbrane predloge">
                                        <Paddings PaddingLeft="25px" PaddingRight="25px" PaddingTop="0px" PaddingBottom="0px" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="KVPsToConfirm" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Potrjevanje oddanih predlogov</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelKVPsToConfirm" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelKVPsToConfirm_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelKVPsToConfirm" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#kvpsToConfirm"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="kvpsToConfirm" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <!-- Potrebno potrditi KVP -->
                                <dx:ASPxGridViewExporter ID="ASPxGridViewKVPsToConfirmExporter" GridViewID="ASPxGridViewKVPsToConfirm" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewKVPsToConfirm" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridKVPsToConfirm" SettingsBehavior-AutoFilterRowInputDelay="6000"
                                    DataSourceID="XpoDSKVPsToConfirm" Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    OnCustomCallback="ASPxGridViewKVPsToConfirm_CustomCallback"
                                    OnDataBound="ASPxGridViewKVPsToConfirm_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared" OnAfterPerformCallback="ASPxGridViews_AfterPerformCallback" OnCustomUnboundColumnData="ASPxGridViewKVPDocument_CustomUnboundColumnData">
                                    <ClientSideEvents RowDblClick="RowDoubleClickKVPsToConfirm" Init="gridKVPsToConfirm_Init" />
                                    <%--SelectionChanged="gridKVPsToConfirm_SelectionChanged"--%>
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True" GridLines="Both"
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
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
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

                                        <dx:GridViewDataTextColumn Caption="Problem"
                                            FieldName="OpisProblem" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Predlog izboljšave"
                                            FieldName="PredlogIzboljsave" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priloge"
                                            FieldName="Priloge" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="PrilogaBit" Visible="True"
                                            UnboundType="String" FieldName="PrilogaBit" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="false" AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

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
                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn Caption="Datum zakl. ideje"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>




                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="KVPsToCheck" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Preverjanje oddanih predlogov</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelKVPsToCheck" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelKVPsToCheck_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelKVPsToCheck" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#kvpsToCheck"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="kvpsToCheck" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <!-- KVP kjer sem presojevalec -->
                                <dx:ASPxGridViewExporter ID="ASPxGridViewKVPsToCheckExporter" GridViewID="ASPxGridViewKVPsToCheck" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewKVPsToCheck" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridKVPsToCheck" SettingsBehavior-AutoFilterRowInputDelay="6000"
                                    DataSourceID="XpoDSKVPsToCheck" Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    OnCustomCallback="ASPxGridViewKVPsToCheck_CustomCallback"
                                    OnDataBound="ASPxGridViewKVPsToCheck_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared" OnAfterPerformCallback="ASPxGridViews_AfterPerformCallback" OnCustomUnboundColumnData="ASPxGridViewKVPDocument_CustomUnboundColumnData">
                                    <ClientSideEvents RowDblClick="RowDoubleClickKVPsToCheck" Init="gridKVPsToCheck_Init" />
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True" GridLines="Both"
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
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
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

                                        <dx:GridViewDataTextColumn Caption="Problem"
                                            FieldName="OpisProblem" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Predlog izboljšave"
                                            FieldName="PredlogIzboljsave" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priloge"
                                            FieldName="Priloge" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="PrilogaBit" Visible="True"
                                            UnboundType="String" FieldName="PrilogaBit" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="false" AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

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
                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Datum zakl. ideje"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>




                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="KVPAuditor" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled predlogov za presojo</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelAuditorKVPs" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelAuditorKVPs_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelAuditorKVPs" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#KVPAuditorCollapse"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="KVPAuditorCollapse" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <!-- KVP kjer sem presojevalec-->
                                <dx:ASPxGridViewExporter ID="ASPxGridViewAuditorKVPsExporter" GridViewID="ASPxGridViewAuditorKVPs" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewAuditorKVPs" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridAuditorKVPs" SettingsBehavior-AutoFilterRowInputDelay="3000"
                                    Width="100%" KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding" OnAfterPerformCallback="ASPxGridViews_AfterPerformCallback"
                                    DataSourceID="XpoDataSourceAuditorKVPs" OnDataBound="ASPxGridViewAuditorKVPs_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared" OnCustomUnboundColumnData="ASPxGridViewKVPDocument_CustomUnboundColumnData">
                                    <ClientSideEvents RowDblClick="RowDoubleClickAuditorKVPs" Init="gridAuditorKVPs_Init" />
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="500"
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsPopup>
                                        <HeaderFilter Height="450px" />
                                    </SettingsPopup>
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
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

                                        <dx:GridViewDataTextColumn Caption="Problem"
                                            FieldName="OpisProblem" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Predlog izboljšave"
                                            FieldName="PredlogIzboljsave" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priloge"
                                            FieldName="Priloge" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="PrilogaBit" Visible="True"
                                            UnboundType="String" FieldName="PrilogaBit" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="false" AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

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
                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Datum zakl. ideje"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="RealizedKVP" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled realiziranih predlogov</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportToExcelRealizedKVP" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelRealizedKVP_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelRealizedKVP" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#RealizedKVPCollapse"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="RealizedKVPCollapse" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <!-- Realizirani KVP  -->
                                <dx:ASPxGridViewExporter ID="ASPxGridViewRealizedKVPExporter" GridViewID="ASPxGridViewRealizedKVP" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewRealizedKVP" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridRealizedKVPs" SettingsBehavior-AutoFilterRowInputDelay="6000"
                                    Width="100%" KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding" OnAfterPerformCallback="ASPxGridViews_AfterPerformCallback"
                                    DataSourceID="XPODSRealizedKVPs" OnDataBound="ASPxGridViewRealizedKVP_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared" OnCustomUnboundColumnData="ASPxGridViewKVPDocument_CustomUnboundColumnData">
                                    <ClientSideEvents RowDblClick="RowDoubleClickRealizedKVPs" Init="gridRealizedKVPs_Init" />
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
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
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

                                        <dx:GridViewDataTextColumn Caption="Problem"
                                            FieldName="OpisProblem" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Predlog izboljšave"
                                            FieldName="PredlogIzboljsave" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priloge"
                                            FieldName="Priloge" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="PrilogaBit" Visible="True"
                                            UnboundType="String" FieldName="PrilogaBit" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="false" AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

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
                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Datum zakl. ideje"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>


                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="ChampionAllKVP" class="tab-pane fade">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">



                            <div class="row2 align-item-centerV-startH">
                                <div class="col-xs-6 no-padding-left">
                                    <h4 class="panel-title" style="display: inline-block;">Pregled predlogov za presojo</h4>
                                </div>
                                <div class="col-xs-6 no-padding-right">
                                    <div class="row2 align-item-centerV-endH">
                                        <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExporttoExcelAllKVP" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExporttoExcelAllKVP_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelAllKVP" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-0">
                                            <a data-toggle="collapse" href="#ChampionAllKVPCollapse"></a>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                        <div id="ChampionAllKVPCollapse" class="panel-collapse collapse in">
                            <div class="panel-body horizontal-scroll">
                                <!-- Vsi KVP champion -->
                                <dx:ASPxGridViewExporter ID="ASPxGridViewAllKVPExport" GridViewID="ASPxGridViewChampionAllKVPs" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewChampionAllKVPs" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true"
                                    ClientInstanceName="gridChampionAllKVPs" SettingsBehavior-AutoFilterRowInputDelay="6000"
                                    Width="100%" KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding" OnAfterPerformCallback="ASPxGridViews_AfterPerformCallback"
                                    DataSourceID="XPODSChampionAllKVPs" OnDataBound="ASPxGridViewChampionAllKVPs_DataBound" OnHtmlDataCellPrepared="ASPxGridViewKVPDocument_HtmlDataCellPrepared" OnHtmlRowPrepared="ASPxGridViewKVPDocument_HtmlRowPrepared" OnCustomUnboundColumnData="ASPxGridViewKVPDocument_CustomUnboundColumnData">
                                    <ClientSideEvents RowDblClick="RowDoubleClickChampionAllKVPs" Init="gridAllKVPs_Init" />
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
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                    <Columns>

                                        <dx:GridViewDataDateColumn Caption="Datum"
                                            FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocument" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="#" FieldName="ZaporednaStevilka" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="140px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Equals" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="KVP Sk." FieldName="KVPSkupinaID.Koda" Width="80px" ExportWidth="80"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
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

                                        <dx:GridViewDataTextColumn Caption="Problem"
                                            FieldName="OpisProblem" ShowInCustomizationForm="True"
                                            Width="250px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Predlog izboljšave"
                                            FieldName="PredlogIzboljsave" ShowInCustomizationForm="True"
                                            Width="200px" ExportWidth="700">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Width="90px" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priloge"
                                            FieldName="Priloge" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="PrilogaBit" Visible="True"
                                            UnboundType="String" FieldName="PrilogaBit" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="false" AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataCheckColumn Caption="CIP"
                                            FieldName="PrihranekStroskiDA_NE" ShowInCustomizationForm="True"
                                            Width="90px">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataCheckColumn>

                                        <dx:GridViewDataTextColumn Caption="Status" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="LastStatusId.Naziv" ShowInCustomizationForm="True"
                                            Width="200px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Koda"
                                            FieldName="LastStatusId.Koda" ShowInCustomizationForm="True"
                                            Width="0px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

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
                                        <dx:GridViewDataTextColumn Caption="Presojevalec" FieldName="LastPresojaID.Lastname" Width="100px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tip" FieldName="idTip.Naziv" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Opis lokacije"
                                            FieldName="LokacijaID.Opis" ShowInCustomizationForm="True"
                                            Width="200px" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Datum zakl. ideje"
                                            FieldName="DatumZakljuceneIdeje" ShowInCustomizationForm="True"
                                            Width="125px" ExportWidth="120">
                                            <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Linija"
                                            FieldName="LinijaID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj"
                                            FieldName="StrojID.Opis" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Stroj številka"
                                            FieldName="StrojStevilka" ShowInCustomizationForm="True"
                                            Width="130px" Visible="true">
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
                                    <div class="col-xs-4" style="border-right: 1px solid #e1e1e1;">
                                        <div style="padding-bottom: 7px;"><span runat="server" id="KVPNumber" class="badge">X</span></div>
                                        <div>Vseh predlogov</div>
                                    </div>
                                    <div class="col-xs-4" style="border-right: 1px solid #e1e1e1;">
                                        <div style="padding-bottom: 7px;"><span runat="server" id="KVPCompleted" class="badge">X</span></div>
                                        <div>Zaključenih</div>
                                    </div>
                                    <div class="col-xs-4">
                                        <div style="padding-bottom: 7px;"><span runat="server" id="CollectedPoints" class="badge">X</span></div>
                                        <div>Zbrane točke v mesecu</div>
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
                                <div class="col-xs-8">
                                    <dx:ASPxLabel ID="lblSupervisor" runat="server" ForeColor="Black" Font-Bold="true" />
                                </div>
                            </div>
                            <div class="row2 small-padding-bottom">
                                <div class="col-xs-4">Champion: </div>
                                <div class="col-xs-8">
                                    <dx:ASPxLabel ID="lblChampion" runat="server" ForeColor="Black" Font-Bold="true" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="panel panel-default" style="margin-top: 15px;">
                    <div class="panel-heading">
                        <h4 class="panel-title" style="display: inline-block;">KVP skupina</h4>
                        <a data-toggle="collapse" data-target="#kvpGroup"></a>
                    </div>
                    <div id="kvpGroup" class="panel-collapse collapse in">
                        <div class="panel-body">
                            <div class="row2 small-padding-bottom">
                                <div class="col-xs-4">Naziv skupine: </div>
                                <div class="col-xs-8">
                                    <dx:ASPxLabel ID="lblKvpGroup" runat="server" ForeColor="Black" Font-Bold="true" />
                                </div>
                            </div>
                            <div class="row2 small-padding-bottom">
                                <div class="col-xs-4">Plan/Realizacija tekoči mesec: </div>
                                <div class="col-xs-8">
                                    <div>
                                        <span id="kvpRealCount" runat="server"></span>
                                        <span id="kvpRealPrecentage" runat="server"></span>
                                    </div>
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
                    OnClick="btnAddNewredCard_Click" AutoPostBack="false" UseSubmitBehavior="false" Width="180px" ClientEnabled="true">
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
                    <p>Vaš KVP predlog je bil uspešno posredovan vodji!</p>
                    <br />
                    <p>Hvala vam!</p>
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
            </div>

        </div>
    </div>

    <dx:XpoDataSource ID="XpoDataSourceKVPDOcument" runat="server" ServerMode="True"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is null">
    </dx:XpoDataSource>


    <dx:XpoDataSource ID="XpoDSKVPsToConfirm" runat="server"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSKVPsToCheck" runat="server"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDataSourceKVPDOcumentToRealize" runat="server"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDataSourceAuditorKVPs" runat="server" ServerMode="True"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[<KVPPresoje>][^.idKVPDocument=idKVPDocument.idKVPDocument]">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XPODSRealizedKVPs" runat="server" ServerMode="True"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is null">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XPODSChampionAllKVPs" runat="server" ServerMode="True"
        DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument" Criteria="[idTipRdeciKarton] is null">
    </dx:XpoDataSource>
</asp:Content>
