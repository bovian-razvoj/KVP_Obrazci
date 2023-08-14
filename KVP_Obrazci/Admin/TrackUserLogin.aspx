<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="TrackUserLogin.aspx.cs" Inherits="KVP_Obrazci.Admin.TrackUserLogin" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function FilterHistory_CheckedChanged(s, e) {
            if (s.GetChecked()) {
                $('#periodFilter').animate({
                    opacity: 1,
                    height: "toggle"
                }, 250);
            }
            else {
                $('#periodFilter').animate({
                    opacity: 0,
                    height: "toggle"
                }, 250);
            }
        }

        function dateEdit_Init(s, e) {
            s.SetDate(new Date());
        }

        function btnRefreshGrid_Click(s, e) {
            gridEmployees.PerformCallback('FilerByPeriod');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Aktivnost prijave zaposlenih</h4>
                    <a data-toggle="collapse" data-target="#userLoginActivity"
                        href="#collapseOne"></a>
                </div>
                <div id="userLoginActivity" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row2 large-padding-bottom">
                            <div class="col-xs-4">
                                <div class="row2 align-item-centerV-startH">
                                    <div class="col-xs-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel18" runat="server" Font-Size="12px" Text="PREGLEJ ZGODOVINO : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-xs-6 no-padding-left">
                                        <dx:ASPxCheckBox ID="FilterThroughHistoryCheckBox" runat="server" ToggleSwitchDisplayMode="Always">
                                            <ClientSideEvents CheckedChanged="FilterHistory_CheckedChanged" />
                                        </dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-7" id="periodFilter" style="display: none;">
                                <div class="row2 align-item-centerV-centerH">
                                    <div class="col-xs-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="DATUM OD : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-3 no-padding-left">
                                        <dx:ASPxDateEdit ID="DateEditDateFrom" runat="server" EditFormat="Date" Width="100%"
                                            CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="clientDateEditDateFrom">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                            <ClientSideEvents Init="dateEdit_Init" />
                                        </dx:ASPxDateEdit>
                                    </div>
                                    <div class="col-xs-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="DATUM DO : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-3 no-padding-left">
                                        <dx:ASPxDateEdit ID="DateEditDateTo" runat="server" EditFormat="Date" Width="100%"
                                            CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="clientDateEditDateTo">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                            <ClientSideEvents Init="dateEdit_Init" />
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
                        <div class="row2">
                            <div class="col-xs-12 no-padding-left no-padding-right">
                                <dx:ASPxGridView ID="ASPxGridViewEmployees" runat="server" AutoGenerateColumns="False" Settings-ShowHeaderFilterButton="true"
                                    EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridEmployees"
                                    Width="100%" KeyboardSupport="true" AccessKey="G"
                                    KeyFieldName="ActiveUserID" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                                    OnCustomCallback="ASPxGridViewEmployees_CustomCallback" OnCustomColumnDisplayText="ASPxGridViewEmployees_CustomColumnDisplayText"
                                    OnDataBinding="ASPxGridViewEmployees_DataBinding" OnHtmlDataCellPrepared="ASPxGridViewEmployees_HtmlDataCellPrepared">
                                    <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                        VerticalScrollableHeight="500" HorizontalScrollBarMode="Auto"
                                        VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="false" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="50,70,90" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" AutoFilterRowInputDelay ="8000" />
                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <SettingsText EmptyDataRow="Trenutno ni podatka o prijavljenih zaposlenih." />
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                    <SettingsBehavior AllowEllipsisInText="true" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ActiveUserID" Width="80px"
                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Ime" FieldName="UserID.Firstname" Width="260px"
                                            ReadOnly="true" ShowInCustomizationForm="True">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Priimek"
                                            FieldName="UserID.Lastname" ShowInCustomizationForm="True"
                                            Width="270px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Email"
                                            FieldName="UserID.Email" ShowInCustomizationForm="True"
                                            Width="25%" Visible="false">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Oddelek" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="UserID.DepartmentId.Name" ShowInCustomizationForm="True"
                                            Width="260px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataComboBoxColumn Caption="Vloga" SettingsHeaderFilter-Mode="CheckedList" FieldName="UserID.RoleID.Naziv" Width="180px">
                                            <Settings AllowAutoFilter="False" />
                                        </dx:GridViewDataComboBoxColumn>

                                        <dx:GridViewDataDateColumn FieldName="LogInDate" Caption="Datum prijave" Width="190px" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Prijavljen" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                            FieldName="IsActive" ShowInCustomizationForm="True"
                                            Width="110px">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataDateColumn FieldName="LastRequestTS" Caption="Zadnja zahteva" Width="190px" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList">
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Število zahtev" FieldName="RequestCount" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Čas seje (min)" FieldName="SessionExpireMin" Width="150px"
                                            ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
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

</asp:Content>
