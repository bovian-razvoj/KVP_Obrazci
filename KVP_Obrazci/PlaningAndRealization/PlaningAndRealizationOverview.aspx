<%@ Page Title="Planiranje in realizacija" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PlaningAndRealizationOverview.aspx.cs" Inherits="KVP_Obrazci.PlaningAndRealization.PlaningAndRealizationOverview" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function RowDoubleClick(s, e) {

        }
        function RowDoubleClickRealization(s, e) {

        }

        function ComboBoxYear_ValueChanged(s, e) {
            cbpCheckPlanAndRealizationForYear.PerformCallback('ValueChanged');
        }

        function RowDoubleClickKVPsPercentage(s, e) { }

        function gridPlaning_Init(s, e) {
            SetEnableExportBtn(s, clientbtnExportCilji);
        }

        function gridRealization_Init(s, e) {
            SetEnableExportBtn(s, clientbtnExportRealizacija);
        }

        function gridKVPsPercentage_Init(s, e) {
            SetEnableExportBtn(s, clientbtnExportPrecentage);
        }

        function gridPlaning_OnBatchStartEditing(s, e) {
            var isChampion = '<%= KVP_Obrazci.Helpers.PrincipalHelper.IsUserChampion() %>';
            if (isChampion === 'True')
                e.cancel = true;
        }
        function gridPlaning_EndCallback(s, e) {
            // gridPlaning.Refresh();
        }

        function gridPlaning_StartEditing(s, e) {
            var disableEditing = '<%= KVP_Obrazci.Helpers.PrincipalHelper.IsUserChampion()%>' || '<%= KVP_Obrazci.Helpers.PrincipalHelper.IsUserLeader()%>';
            if (disableEditing == 'True')
                disableEditing = true;
            else
                disableEditing = false;

            e.cancel = disableEditing;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="cbpCheckPlanAndRealizationForYear" runat="server" ClientInstanceName="cbpCheckPlanAndRealizationForYear" OnCallback="cbpCheckPlanAndRealizationForYear_Callback">
        <PanelCollection>
            <dx:PanelContent>
                <div class="row2">
                    <div class="col-sm-0 big-margin-r">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="18px" Text="LETO : "></dx:ASPxLabel>
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
                    <div class="col-sm-2 no-padding-left">

                        <dx:ASPxButton ID="btnOpenYear" runat="server" Text="Odpri leto" OnClick="btnOpenYear_Click" AutoPostBack="false"
                            Height="30" Width="50" UseSubmitBehavior="false" BackColor="#79a63f" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/refresh.png"></Image>
                        </dx:ASPxButton>


                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <ul class="nav nav-tabs" runat="server" id="navTabs">
                            <li class="active" runat="server" id="PlaningItem">
                                <a data-toggle="tab" href="#Planing">Cilj</a>
                            </li>
                            <li runat="server" id="RealizationItem">
                                <a data-toggle="tab" href="#Realization">Podani KVP</a>
                            </li>
                            <li runat="server" id="PercentageItem">
                                <a data-toggle="tab" href="#Percentage">Realizacija</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div id="Planing" class="tab-pane fade in active">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <div class="row2 align-item-centerV-startH">
                                            <div class="col-xs-6 no-padding-left">
                                                <h4 class="panel-title" style="display: inline-block;">KVP Planiranje</h4>
                                            </div>
                                            <div class="col-xs-6 no-padding-right">
                                                <div class="row2 align-item-centerV-endH">
                                                    <div class="col-xs-0 big-margin-r">
                                                        <dx:ASPxButton ID="btnExportCilji" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportCilji_Click"
                                                            AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportCilji" ToolTip="Izvozi v excel">
                                                            <DisabledStyle CssClass="icon-disabled" />
                                                            <HoverStyle CssClass="icon-hover" />
                                                            <Image Url="../Images/export_excel.png" Width="20px" />
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div class="col-xs-0">
                                                        <a data-toggle="collapse" data-target="#collapsePlaning" href="#collapseOne"></a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="collapsePlaning" class="panel-collapse collapse in">
                                        <div class="panel-body">
                                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporterPlaning" GridViewID="ASPxGridViewPlaning" runat="server"></dx:ASPxGridViewExporter>
                                            <dx:ASPxGridView ID="ASPxGridViewPlaning" runat="server" AutoGenerateColumns="False"
                                                EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridPlaning"
                                                Width="100%" KeyboardSupport="true" AccessKey="G" KeyFieldName="idPlanRealizacija"
                                                OnBatchUpdate="ASPxGridViewPlaning_BatchUpdate" OnCustomCallback="ASPxGridViewPlaning_CustomCallback" OnDataBinding="ASPxGridViewPlaning_DataBinding" OnHtmlRowPrepared="ASPxGridViewPlaning_HtmlRowPrepared" OnHtmlDataCellPrepared="ASPxGridViewKVPsPercentage_HtmlDataCellPrepared">
                                                <ClientSideEvents EndCallback="gridPlaning_EndCallback" BatchEditStartEditing="gridPlaning_StartEditing" />
                                                <Paddings PaddingTop="0" PaddingBottom="0" PaddingLeft="3px" PaddingRight="3px" />

                                                <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                                    VerticalScrollableHeight="560" HorizontalScrollBarMode="Auto"
                                                    VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />

                                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                </SettingsPager>

                                                <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" AutoFilterRowInputDelay="8000" />

                                                <Styles Header-Wrap="True">
                                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                </Styles>

                                                <SettingsText EmptyDataRow="Trenutno ni podatka o planiranju. Dodaj novega."
                                                    CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" />

                                                <SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="Click" />

                                                <%-- <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />--%>

                                                <SettingsBehavior AllowEllipsisInText="true" />

                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idPlanRealizacija"
                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP skupina kdoa" FieldName="idKVPSkupina.Koda" Width="20%"
                                                        ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False" Visible="false">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP skupina" FieldName="idKVPSkupina.Naziv" Width="300px"
                                                        ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Aktivnost" FieldName="idKVPSkupina.Aktivnost" Width="20" Visible="false"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Leto"
                                                        FieldName="Leto" ShowInCustomizationForm="True"
                                                        Width="80px" EditFormSettings-Visible="False">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Januar"
                                                        FieldName="Plan_Jan" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Februar"
                                                        FieldName="Plan_Feb" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Marec"
                                                        FieldName="Plan_Mar" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="April"
                                                        FieldName="Plan_Apr" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Maj"
                                                        FieldName="Plan_Maj" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Junij"
                                                        FieldName="Plan_Jun" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Julij"
                                                        FieldName="Plan_Jul" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Avgust"
                                                        FieldName="Plan_Avg" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="September"
                                                        FieldName="Plan_Sep" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Oktober"
                                                        FieldName="Plan_Okt" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="November"
                                                        FieldName="Plan_Nov" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="December"
                                                        FieldName="Plan_Dec" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings Display="Dynamic" RegularExpression-ValidationExpression="^[0-9]*$" RegularExpression-ErrorText="Vnesi številke" />
                                                        </PropertiesTextEdit>
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                </Columns>


                                                <%--<ClientSideEvents BatchEditEndEditing="OnBatchEditEndEditing" EndCallback="EndCallback_gridSelectedPositions" />--%>
                                            </dx:ASPxGridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="Realization" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <div class="col-xs-6 no-padding-left">
                                            <h4 class="panel-title" style="display: inline-block;">KVP Realiziranje</h4>
                                        </div>
                                        <div class="col-xs-6 no-padding-right">
                                            <div class="row2 align-item-centerV-endH">
                                                <div class="col-xs-0 big-margin-r">
                                                    <dx:ASPxButton ID="btnExportRealizacija" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportRealizacija_Click"
                                                        AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportRealizacija" ToolTip="Izvozi v excel">
                                                        <DisabledStyle CssClass="icon-disabled" />
                                                        <HoverStyle CssClass="icon-hover" />
                                                        <Image Url="../Images/export_excel.png" Width="20px" />
                                                    </dx:ASPxButton>
                                                </div>
                                                <div class="col-xs-0">
                                                    <a data-toggle="collapse" data-target="#collapseRealization" href="#collapseOne"></a>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div id="collapseRealization" class="panel-collapse collapse in">
                                        <div class="panel-body">
                                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporterrealization" GridViewID="ASPxGridViewRealization" runat="server"></dx:ASPxGridViewExporter>
                                            <dx:ASPxGridView ID="ASPxGridViewRealization" runat="server" AutoGenerateColumns="False"
                                                EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridRealization"
                                                Width="100%" KeyboardSupport="true" AccessKey="G" KeyFieldName="idPlanRealizacija"
                                                OnCustomCallback="ASPxGridViewRealization_CustomCallback" OnDataBinding="ASPxGridViewRealization_DataBinding" OnHtmlRowPrepared="ASPxGridViewRealization_HtmlRowPrepared" OnHtmlDataCellPrepared="ASPxGridViewKVPsPercentage_HtmlDataCellPrepared">
                                                <ClientSideEvents RowDblClick="RowDoubleClickRealization" />

                                                <Paddings PaddingTop="0" PaddingBottom="0" PaddingLeft="3px" PaddingRight="3px" />

                                                <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                                    VerticalScrollableHeight="560"
                                                    VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="false" />

                                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                </SettingsPager>

                                                <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" AutoFilterRowInputDelay="8000" />

                                                <Styles Header-Wrap="True">
                                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                </Styles>

                                                <SettingsText EmptyDataRow="Trenutno ni podatka o planiranju. Dodaj novega." />

                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />

                                                <SettingsBehavior AllowEllipsisInText="true" />

                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idPlanRealizacija"
                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP skupina" FieldName="idKVPSkupina.Naziv" Width="20%"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Aktivnost" FieldName="idKVPSkupina.Aktivnost" Width="20" Visible="false"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Leto"
                                                        FieldName="Leto" ShowInCustomizationForm="True"
                                                        Width="80px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Januar"
                                                        FieldName="Real_Jan" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Februar"
                                                        FieldName="Real_Feb" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Marec"
                                                        FieldName="Real_Mar" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="April"
                                                        FieldName="Real_Apr" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Maj"
                                                        FieldName="Real_Maj" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Junij"
                                                        FieldName="Real_Jun" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Julij"
                                                        FieldName="Real_Jul" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Avgust"
                                                        FieldName="Real_Avg" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="September"
                                                        FieldName="Real_Sep" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Oktober"
                                                        FieldName="Real_Okt" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="November"
                                                        FieldName="Real_Nov" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="December"
                                                        FieldName="Real_Dec" ShowInCustomizationForm="True"
                                                        Width="100px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                </Columns>
                                            </dx:ASPxGridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="Percentage" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <div class="col-xs-6 no-padding-left">
                                            <h4 class="panel-title" style="display: inline-block;">KVP Odstotki</h4>
                                        </div>
                                        <div class="col-xs-6 no-padding-right">
                                            <div class="row2 align-item-centerV-endH">
                                                <div class="col-xs-0 big-margin-r">
                                                    <dx:ASPxButton ID="btnExportPercentage" runat="server" RenderMode="Link" ClientEnabled="true" OnClick="btnExportPercentage_Click"
                                                        AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientbtnExportPercentage" ToolTip="Izvozi v excel">
                                                        <DisabledStyle CssClass="icon-disabled" />
                                                        <HoverStyle CssClass="icon-hover" />
                                                        <Image Url="../Images/export_excel.png" Width="20px" />
                                                    </dx:ASPxButton>
                                                </div>
                                                <div class="col-xs-0">
                                                    <a data-toggle="collapse" data-target="#collapseRealization" href="#collapseOne"></a>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div id="collapsePercentage" class="panel-collapse collapse in">
                                        <div class="panel-body">
                                            <dx:ASPxGridViewExporter ID="ASPxGridViewExportPercentage" GridViewID="ASPxGridViewKVPsPercentage" runat="server"></dx:ASPxGridViewExporter>
                                            <dx:ASPxGridView ID="ASPxGridViewKVPsPercentage" runat="server" AutoGenerateColumns="False"
                                                EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridKVPsPercentage"
                                                Width="100%" KeyboardSupport="true" AccessKey="G" KeyFieldName="idPlanRealizacija"
                                                OnCustomCallback="ASPxGridViewKVPsPercentage_CustomCallback" OnDataBinding="ASPxGridViewKVPsPercentage_DataBinding" OnHtmlRowPrepared="ASPxGridViewKVPsPercentage_HtmlRowPrepared" OnHtmlDataCellPrepared="ASPxGridViewKVPsPercentage_HtmlDataCellPrepared">
                                                <ClientSideEvents RowDblClick="RowDoubleClickKVPsPercentage" />

                                                <Paddings PaddingTop="0" PaddingBottom="0" PaddingLeft="3px" PaddingRight="3px" />

                                                <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                                    VerticalScrollableHeight="560"
                                                    VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="false" />

                                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                </SettingsPager>

                                                <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" AutoFilterRowInputDelay="8000" />

                                                <Styles Header-Wrap="True">
                                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                </Styles>

                                                <SettingsText EmptyDataRow="Trenutno ni podatka o planiranju. Dodaj novega." />

                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />

                                                <SettingsBehavior AllowEllipsisInText="true" />

                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idPlanRealizacija"
                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP skupina" FieldName="idKVPSkupina.Naziv" Width="50%"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Aktivnost" FieldName="idKVPSkupina.Aktivnost" Width="20" Visible="false"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Leto"
                                                        FieldName="Leto" ShowInCustomizationForm="True"
                                                        Width="80px">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Januar (%)"
                                                        FieldName="Odst_Jan" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Februar (%)"
                                                        FieldName="Odst_Feb" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Marec (%)"
                                                        FieldName="Odst_Mar" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="April (%)"
                                                        FieldName="Odst_Apr" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Maj (%)"
                                                        FieldName="Odst_Maj" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Junij (%)"
                                                        FieldName="Odst_Jun" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Julij (%)"
                                                        FieldName="Odst_Jul" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Avgust (%)"
                                                        FieldName="Odst_Avg" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="September (%)"
                                                        FieldName="Odst_Sep" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Oktober (%)"
                                                        FieldName="Odst_Okt" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="November (%)"
                                                        FieldName="Odst_Nov" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="December (%)"
                                                        FieldName="Odst_Dec" ShowInCustomizationForm="True"
                                                        Width="125px" PropertiesTextEdit-DisplayFormatString="n2">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                </Columns>
                                            </dx:ASPxGridView>
                                        </div>
                                    </div>
                                </div>
                            </div>




                            <dx:XpoDataSource ID="XpoDSPlaning" runat="server" ServerMode="True"
                                DefaultSorting="idPlanRealizacija" TypeName="KVP_Obrazci.Domain.KVPOdelo.PlanRealizacija" Criteria="[idKVPSkupina.Aktivnost]  = 1">
                            </dx:XpoDataSource>
                        </div>
                        <dx:ASPxButton ID="btnRefresh" runat="server" Text="Osveži" OnClick="btnRefresh_Click" AutoPostBack="false"
                            Height="30" Width="50" UseSubmitBehavior="false" BackColor="#79a63f">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/refresh.png"></Image>
                        </dx:ASPxButton>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>

