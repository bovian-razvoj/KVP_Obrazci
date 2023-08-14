<%@ Page Title="Izplačila" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PayoutOverview.aspx.cs" Inherits="KVP_Obrazci.Payouts.PayoutOverview" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var performCallback = false;
        var idInterval;
        function ComboBoxMonth_ValueChanged(s, e) {
            if (!performCallback) {
                performCallback = true;
                //idInterval = setInterval(IntervalComplete, 200);
                gridPayouts.PerformCallback('ValueChanged');
            }
        }
        function ComboBoxYear_ValueChanged(s, e) {
            if (!performCallback) {
                performCallback = true;
                //idInterval = setInterval(IntervalComplete, 200);
                gridPayouts.PerformCallback('ValueChanged');
            }
        }

        function IntervalComplete() {
            gridPayouts.PerformCallback('ValueChanged');
        }

        function GridPayouts_EndCallback(s, e) {
            //clearInterval(idInterval);
            performCallback = false;
            // gridPayouts.Refresh();
        }

        function gridPayouts_Init(s, e) {
            SetEnableExportBtn(s, clientbtnExportCilji);
        }

        function GeneratePayout(s, e) {
            gridPayouts.PerformCallback('StartPayoutProcedure');
        }

        function lookUpEmployee_ValueChanged(s, e) {
            if (!performCallback) {
                performCallback = true;
                gridPayouts.PerformCallback('ValueChanged');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <div class="row2 align-item-centerV-startH">
                        <div class="col-xs-6 no-padding-left">
                            <h4 class="panel-title" style="display: inline-block;">Izplačila</h4>
                        </div>
                        <div class="col-xs-6 no-padding-right">
                            <div class="row2 align-item-centerV-endH">
                                <div class="col-xs-0 big-margin-r">
                                    <dx:ASPxButton ID="btnExportPayout" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportPayouts_Click"
                                        AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportPayout" ToolTip="Izvozi v excel">
                                        <DisabledStyle CssClass="icon-disabled" />
                                        <HoverStyle CssClass="icon-hover" />
                                        <Image Url="../Images/export_excel.png" Width="20px" />
                                    </dx:ASPxButton>
                                </div>
                                <div class="col-xs-0">
                                    <a data-toggle="collapse" data-target="#employees" href="#collapseOne"></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="employees" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row2">
                            <div class="col-xs-12">
                                <div class="row2 medium-padding-bottom" style="align-items: center; justify-content: center;">
                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="ZAPOSLEN : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-2 no-padding-left" style="margin-right: 90px;">
                                        <dx:ASPxGridLookup ID="ASPxGridLookupEmployee" runat="server" ClientInstanceName="lookUpEmployee"
                                            KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                            OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" IncrementalFilteringMode="Contains">
                                            <ClearButton DisplayMode="OnHover" />
                                            <ClientSideEvents ValueChanged="lookUpEmployee_ValueChanged" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <GridViewStyles>
                                                <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                            </GridViewStyles>
                                            <GridViewProperties>
                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                                <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                    ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                            </GridViewProperties>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Zaposlen Id" FieldName="Id" Width="80px"
                                                    ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="30%"
                                                    ReadOnly="true" ShowInCustomizationForm="True">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Priimek"
                                                    FieldName="Lastname" ShowInCustomizationForm="True"
                                                    Width="30%">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Email"
                                                    FieldName="Email" ShowInCustomizationForm="True"
                                                    Width="25%">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Oddelek"
                                                    FieldName="DepartmentId.Name" ShowInCustomizationForm="True"
                                                    Width="15%">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="VodjaID"
                                                    FieldName="DepartmentId.DepartmentHeadId" ShowInCustomizationForm="True"
                                                    Width="15%" Visible="false">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="OddelekID"
                                                    FieldName="DepartmentId.Id" ShowInCustomizationForm="True"
                                                    Width="15%" Visible="false">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Vloga"
                                                    FieldName="RoleID.Naziv" ShowInCustomizationForm="True"
                                                    Width="15%">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

                                            </Columns>
                                        </dx:ASPxGridLookup>
                                    </div>

                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="MESEC : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-2 no-padding-left">
                                        <dx:ASPxComboBox ID="ComboBoxMonth" runat="server" ValueType="System.String" DropDownStyle="DropDownList"
                                            IncrementalFilteringMode="StartsWith" EnableSynchronization="False" CssClass="text-box-input" ClientInstanceName="clientComboBoxMonth">
                                            <ClearButton DisplayMode="OnHover" />
                                            <ClientSideEvents ValueChanged="ComboBoxMonth_ValueChanged" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <Items>
                                                <dx:ListEditItem Text="Januar" Value="1" />
                                                <dx:ListEditItem Text="Februar" Value="2" />
                                                <dx:ListEditItem Text="Marec" Value="3" />
                                                <dx:ListEditItem Text="April" Value="4" />
                                                <dx:ListEditItem Text="Maj" Value="5" />
                                                <dx:ListEditItem Text="Junij" Value="6" />
                                                <dx:ListEditItem Text="Julij" Value="7" />
                                                <dx:ListEditItem Text="Avgust" Value="8" />
                                                <dx:ListEditItem Text="September" Value="9" />
                                                <dx:ListEditItem Text="Oktober" Value="10" />
                                                <dx:ListEditItem Text="November" Value="11" />
                                                <dx:ListEditItem Text="December" Value="12" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </div>
                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="LETO : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-2 no-padding-left">
                                        <dx:ASPxComboBox ID="ComboBoxYear" runat="server" ValueType="System.String" DropDownStyle="DropDownList"
                                            IncrementalFilteringMode="StartsWith" EnableSynchronization="False" CssClass="text-box-input" ClientInstanceName="clientComboBoxYear">
                                            <ClearButton DisplayMode="OnHover" />
                                            <ClientSideEvents ValueChanged="ComboBoxYear_ValueChanged" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <Items>
                                                <dx:ListEditItem Text="2018" Value="2018" />
                                                <dx:ListEditItem Text="2019" Value="2019" />
                                                <dx:ListEditItem Text="2020" Value="2020" />
                                                <dx:ListEditItem Text="2021" Value="2021" />
                                                <dx:ListEditItem Text="2022" Value="2022" />
                                                <dx:ListEditItem Text="2023" Value="2023" />
                                                <dx:ListEditItem Text="2024" Value="2024" />
                                                <dx:ListEditItem Text="2025" Value="2025" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row2">
                            <div class="col-xs-12">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewExporterPayouts" GridViewID="ASPxGridViewPayouts" runat="server">
                                    <Styles>
                                        <Header Wrap="True" />
                                    </Styles>
                                </dx:ASPxGridViewExporter>

                                <dx:ASPxGridView ID="ASPxGridViewPayouts" runat="server" AutoGenerateColumns="False" EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridPayouts"
                                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="idIzplacilla" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                                    DataSourceID="XpoDSPayouts" OnCustomCallback="ASPxGridViewPayouts_CustomCallback">
                                    <ClientSideEvents EndCallback="GridPayouts_EndCallback" />
                                    <Settings ShowFilterRow="True"
                                        VerticalScrollableHeight="450" HorizontalScrollBarMode="Auto"
                                        VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" GridLines="Both"
                                        ShowTitlePanel="True" ShowHorizontalScrollBar="True" UseFixedTableLayout="True" ShowHeaderFilterButton="true" ShowFilterRowMenu="true" />



                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" AutoFilterRowInputDelay="8000" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o izplačilih." />
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="IdUser.ExternalId" Width="80px"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False" ExportWidth="500">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Zaposlen"
                                            FieldName="ImePriimek" ShowInCustomizationForm="True"
                                            Width="15%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Mesec"
                                            FieldName="Mesec" ShowInCustomizationForm="True"
                                            Width="10%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Prenos iz prej. meseca"
                                            FieldName="PrenosIzPrejsnjegaMeseca" ShowInCustomizationForm="True"
                                            Width="25%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Točke predlagatelj"
                                            FieldName="PredlagateljT" ShowInCustomizationForm="True"
                                            Width="25%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Točke realizator"
                                            FieldName="RealizatorT" ShowInCustomizationForm="True"
                                            Width="20%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Vsota točk"
                                            FieldName="VsotaT" ShowInCustomizationForm="True"
                                            Width="20%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Izplacilo v mesecu"
                                            FieldName="IzplaciloVMesecu" ShowInCustomizationForm="True"
                                            Width="20%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Prenos točk v naslednji mesec"
                                            FieldName="PrenosTvNaslednjiMesec" ShowInCustomizationForm="True"
                                            Width="25%">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                         <dx:GridViewDataTextColumn Caption="Opombe"
                                            FieldName="Notes" ShowInCustomizationForm="True"
                                            Width="25%">
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
        <dx:XpoDataSource ID="XpoDSPayouts" runat="server" ServerMode="True"
            DefaultSorting="idIzplacilla" TypeName="KVP_Obrazci.Domain.KVPOdelo.Izplacila" Criteria="[IdUser.Deleted] = 0">
        </dx:XpoDataSource>

        <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
            DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users" Criteria="[Deleted] = 0">
        </dx:XpoDataSource>
    </div>

    <div class="AddEditButtonsWrap">
        <span class="AddEditButtons">
            <dx:ASPxButton ID="btnGeneratePayouts" runat="server" Text="Generiraj izplačila za tekoči mesec" AutoPostBack="false"
                Height="30" Width="50" UseSubmitBehavior="false">
                <Paddings PaddingLeft="10" PaddingRight="10" />
                <ClientSideEvents Click="GeneratePayout" />
            </dx:ASPxButton>
        </span>
    </div>

    <div class="AddEditButtonsWrap">
        <span class="AddEditButtons">
            <dx:ASPxButton ID="btnCreateCSV" runat="server" Text="Izplačila CSV" AutoPostBack="false"
                Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCreateCSV_Click">
                <Paddings PaddingLeft="10" PaddingRight="10" />
            </dx:ASPxButton>
        </span>
    </div>
</asp:Content>
