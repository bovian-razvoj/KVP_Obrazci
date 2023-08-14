<%@ Page Title="Pregled izplačil po obdobju" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="PayoutOverviewByPeriod.aspx.cs" Inherits="KVP_Obrazci.Payouts.PayoutOverviewByPeriod" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function btnRefreshGrid_Click(s, e) {
            var dateFields = [clientDateEditDateFrom, clientDateEditDateTo];
            var process = InputFieldsValidation(null, null, dateFields, null, null, null);
            if (process)
                clientPayoutOverViewByPeriodCallbackPanel.PerformCallback("GetData");
        }

        function clientPayoutOverViewByPeriodCallbackPanel_EndCallback(s, e) {
            if (s.cpGridViewRowCount != "" && s.cpGridViewRowCount !== undefined) {
                if (parseInt(s.cpGridViewRowCount) > 0)
                    clientBtnExportToExcel.SetEnabled(true);

                delete (s.cpGridViewRowCount);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="PayoutOverViewByPeriodCallbackPanel" runat="server" Width="100%" ClientInstanceName="clientPayoutOverViewByPeriodCallbackPanel"
        OnCallback="PayoutOverViewByPeriodCallbackPanel_Callback">
        <ClientSideEvents EndCallback="clientPayoutOverViewByPeriodCallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <div class="row2 align-item-centerV-startH">
                                    <div class="col-xs-6 no-padding-left">
                                        <h4 class="panel-title" style="display: inline-block;">Pregled izplačil po obdobju</h4>
                                    </div>
                                    <div class="col-xs-6 no-padding-right">
                                        <div class="row2 align-item-centerV-endH">
                                            <div class="col-xs-0 big-margin-r">
                                                <dx:ASPxButton ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click" RenderMode="Link" ClientEnabled="false"
                                                    AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcel" ToolTip="Izvozi v excel">
                                                    <DisabledStyle CssClass="icon-disabled" />
                                                    <HoverStyle CssClass="icon-hover" />
                                                    <Image Url="../Images/export_excel.png" Width="20px" />
                                                </dx:ASPxButton>
                                            </div>
                                            <div class="col-xs-0">
                                                <a data-toggle="collapse" data-target="#collapseOne"></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row small-padding-bottom small-padding-top">
                                        <div class="col-md-3">
                                            <div class="row2 align-item-centerV-centerH">
                                                <div class="col-sm-0 big-margin-r">
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="ZAPOSLEN : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-8 no-padding-left"">
                                                    <dx:ASPxGridLookup ID="ASPxGridLookupEmployee" runat="server" ClientInstanceName="lookUpEmployee"
                                                        KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                        OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" IncrementalFilteringMode="Contains">
                                                        <ClearButton DisplayMode="OnHover" />
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
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row2 align-item-centerV-centerH">
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
                                            <div class="row2 align-item-centerV-centerH">
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
                                    </div>
                                    <div class="col-md-3 hidden-sm"></div>
                                </div>

                                <div class="row small-padding-bottom">
                                    <div class="col-xs-12">
                                        <dx:ASPxGridViewExporter ID="PayoutOverviewByPeriodExporter" GridViewID="ASPxGridViewPayoutOverViewByPeriod" runat="server"></dx:ASPxGridViewExporter>
                                        <dx:ASPxGridView ID="ASPxGridViewPayoutOverViewByPeriod" runat="server" EnableCallbackCompression="true"
                                            ClientInstanceName="gridPayoutOverViewByPeriod" AutoGenerateColumns="true"
                                            Width="100%" KeyFieldName="IdPregleda" CssClass="gridview-no-header-padding"
                                            OnDataBinding="ASPxGridViewPayoutOverViewByPeriod_DataBinding" OnCustomCallback="ASPxGridViewPayoutOverViewByPeriod_CustomCallback"
                                            OnAfterPerformCallback="ASPxGridViewPayoutOverViewByPeriod_AfterPerformCallback">
                                            <Paddings Padding="5px" />
                                            <Settings ShowVerticalScrollBar="True" HorizontalScrollBarMode="Auto"
                                                ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                                ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                            <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                </PageSizeItemSettings>
                                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                            </SettingsPager>
                                            <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                            <Styles Header-Wrap="True">
                                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                            </Styles>
                                            <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />

                                        </dx:ASPxGridView>
                                    </div>
                                </div>

                                <div class="row small-padding-bottom">
                                    <div class="col-md-2 hidden-sm"></div>
                                    <div class="col-md-8">

                                        <div class="well well-lg small-margin-l small-margin-r">
                                            <div class="table-responsive">
                                                <table class="table table-hover" id="SummaryContentTable" runat="server">
                                                </table>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-md-2 hidden-sm"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
    </dx:XpoDataSource>
</asp:Content>
