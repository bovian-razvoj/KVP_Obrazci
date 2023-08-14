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

        function GeneratePayout(s, e)
        {
            gridPayouts.PerformCallback('StartPayoutProcedure');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Izplačila</h4>
                    <a data-toggle="collapse" data-target="#employees"
                        href="#collapseOne"></a>
                    <div class="col-xs-0 big-margin-r">
                                            <dx:ASPxButton ID="btnExportPayout" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportPayouts_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportPayout" ToolTip="Izvozi v excel">
                                                <DisabledStyle CssClass="icon-disabled" />
                                                <HoverStyle CssClass="icon-hover" />
                                                <Image Url="../Images/export_excel.png" Width="20px" />
                                            </dx:ASPxButton>
                                        </div>
                </div>
                <div id="employees" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row2">
                            <div class="col-xs-12">
                                <div class="row2 small-padding-bottom" style="align-items: center; justify-content: center;">
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
                                <dx:ASPxGridViewExporter ID="ASPxGridViewExporterPayouts" GridViewID="ASPxGridViewPayouts" runat="server"></dx:ASPxGridViewExporter>
                                <dx:ASPxGridView ID="ASPxGridViewPayouts" runat="server" AutoGenerateColumns="False" EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridPayouts"
                                    Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="idIzplacilla" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                                    DataSourceID="XpoDSPayouts" OnCustomCallback="ASPxGridViewPayouts_CustomCallback" >
                                    <ClientSideEvents EndCallback="GridPayouts_EndCallback" />
                                    <Settings   ShowFilterRow="True"
                                        VerticalScrollableHeight="450" HorizontalScrollBarMode="Auto"
                                        VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" GridLines="Both" 
                                         ShowTitlePanel="True" ShowHorizontalScrollBar="True" UseFixedTableLayout="True" ShowHeaderFilterButton="true" ShowFilterRowMenu="true"  />
                                    
                                    
                                    
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o izplačilih." />
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                    <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="IdUser.ExternalId" Width="80px"
                                            ReadOnly="true" Visible="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewBandColumn Caption="Zaposlen">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="IdUser.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" />
                                                <dx:GridViewDataColumn FieldName="IdUser.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" />
                                            </Columns>
                                        </dx:GridViewBandColumn>
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

                                    </Columns>
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <dx:XpoDataSource ID="XpoDSPayouts" runat="server" ServerMode="True"
            DefaultSorting="idIzplacilla" TypeName="KVP_Obrazci.Domain.KVPOdelo.Izplacila">
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
