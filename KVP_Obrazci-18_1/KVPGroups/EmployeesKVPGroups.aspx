<%@ Page Title="Zaposleni na KVP skupini" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="EmployeesKVPGroups.aspx.cs" Inherits="KVP_Obrazci.KVPGroups.EmployeesKVPGroups" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var activeTabName = GetUrlQueryStrings()['activeTab'];

            if (activeTabName != "") {
                $('.nav-tabs a[href="' + activeTabName + '"]').tab('show');
                var params = QueryStringsToObject();
                delete params.activeTab;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
        });

        function gridEmployees_OnSelectionChanged(s, e) {
            if (s.GetSelectedRowCount() > 0)
                lookUpKVPGroups.SetEnabled(true);
            else {
                lookUpKVPGroups.SetEnabled(false);
                clientBtnConfirm.SetEnabled(false);
            }
        }

        function lookUpKVPGroups_ValueChanged(s, e) {
            if (gridEmployees.GetSelectedRowCount() > 0 && s.GetValue() != null) {
                clientBtnConfirm.SetEnabled(true);
            }
            else {
                clientBtnConfirm.SetEnabled(false);
            }
        }

        function btnConfirm_Click(s, e) {
            var process = CheckValidation();
            if (process) {
                clientCallbackPanelEmployeesKVPGroups.PerformCallback("AddEmployeToKVPGroup");
            }
        }

        function CheckValidation() {
            var process = false;
            var lookupItems = [lookUpKVPGroups];

            process = InputFieldsValidation(lookupItems, null, null, null, null, null);

            if (gridEmployees.GetSelectedRowCount() < 0)
                process = false;

            return process;
        }

        function CallbackPanelEmployeesKVPGroups_EndCallback(s, e) {
            //gridEmployees.Refresh();
        }

        function gridEmployeesToRemove_OnSelectionChanged(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnRemoveFromKVPGroup.SetEnabled(true);
            else {
                clientBtnRemoveFromKVPGroup.SetEnabled(false);
            }
        }

        function clientBtnRemoveFromKVPGroup_Click(s, e) {
            clientCallbackPanelEmployeesKVPGroups.PerformCallback("RemoveEmployeFromKVPGroup");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelEmployeesKVPGroups" runat="server" Width="100%" ClientInstanceName="clientCallbackPanelEmployeesKVPGroups"
        OnCallback="CallbackPanelEmployeesKVPGroups_Callback">
        <ClientSideEvents EndCallback="CallbackPanelEmployeesKVPGroups_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <ul class="nav nav-tabs" runat="server" id="navTabs">
                    <li runat="server" class="active" id="employeesKVPGroupAddItem">
                        <a data-toggle="tab" href="#EmployeesKVPGroupAdd"><span class="glyphicon glyphicon-list-alt"></span>Zaposleni in kvp skupine</a>
                    </li>
                    <li runat="server" id="employeesKVPGroupRemoveItem">
                        <a data-toggle="tab" href="#EmployeesKVPGroupRemove"><span runat="server" id="championBadge" class="badge">0</span> Odstrani zaposlene iz skupine</a>
                    </li>
                </ul>
                <div class="tab-content">
                    <div id="EmployeesKVPGroupAdd" class="tab-pane fade in active">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <h4 class="panel-title" style="display: inline-block;">Vsebina</h4>
                                <a data-toggle="collapse" data-target="#tabCollapseOne"
                                    href="#collapseOne"></a>
                            </div>
                            <div id="tabCollapseOne" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-9">
                                            <dx:ASPxGridView ID="ASPxGridViewEmployees" runat="server" EnableCallbackCompression="true"
                                                ClientInstanceName="gridEmployees"
                                                Width="100%" KeyboardSupport="true" AccessKey="G"
                                                KeyFieldName="Id" CssClass="gridview-no-header-padding"
                                                DataSourceID="XpoDSUsers">
                                                <ClientSideEvents SelectionChanged="gridEmployees_OnSelectionChanged" />
                                                <Paddings Padding="0" />
                                                <Settings ShowVerticalScrollBar="True"
                                                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                                    ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                    <PageSizeItemSettings Visible="true" Items="50,70,90" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                    </PageSizeItemSettings>
                                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                </SettingsPager>
                                                <SettingsBehavior AllowFocusedRow="true" AllowEllipsisInText="true" />
                                                <Styles Header-Wrap="True">
                                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                </Styles>
                                                <SettingsText EmptyDataRow="Trenutno ni podatka o zaposlenih. Dodaj novega." />
                                                <Columns>

                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="100px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" />

                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ExternalId" Width="80px"
                                                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="25%"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Priimek"
                                                        FieldName="Lastname" ShowInCustomizationForm="True"
                                                        Width="25%">
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

                                                    <dx:GridViewDataTextColumn Caption="Vloga"
                                                        FieldName="RoleID.Naziv" ShowInCustomizationForm="True"
                                                        Width="15%">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="NewEmployee"
                                                        FieldName="NewEmployee" ShowInCustomizationForm="True"
                                                        Width="15%" Visible="false">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                            </dx:ASPxGridView>
                                        </div>

                                        <div class="col-sm-3">
                                            <dx:ASPxGridLookup ID="ASPxGridLookupKVPGroups" runat="server" ClientInstanceName="lookUpKVPGroups"
                                                KeyFieldName="idKVPSkupina" TextFormatString="{2}" CssClass="text-box-input" ClientEnabled="false"
                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="80%" Font-Size="14px"
                                                OnLoad="ASPxGridLookupLoad_WidthSmall" DataSourceID="XpoDSKVPGroups" IncrementalFilteringMode="Contains">
                                                <ClearButton DisplayMode="OnHover" />
                                                <ClientSideEvents GotFocus="function(s,e) { s.ShowDropDown(); }" DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}" ValueChanged="lookUpKVPGroups_ValueChanged" />
                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                <GridViewStyles>
                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                </GridViewStyles>
                                                <GridViewProperties>
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                    <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                        ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                </GridViewProperties>
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="KVPGroup Id" FieldName="idKVPSkupina" Width="80px"
                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="30%"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Naziv"
                                                        FieldName="Naziv" ShowInCustomizationForm="True"
                                                        Width="70%">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                </Columns>
                                            </dx:ASPxGridLookup>
                                        </div>

                                    </div>
                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-12 text-center">
                                            <dx:ASPxButton ID="btnConfirm" runat="server" Text="Dodaj v skupino" AutoPostBack="false"
                                                ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm"
                                                ClientEnabled="false" Width="150px">
                                                <ClientSideEvents Click="btnConfirm_Click" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="EmployeesKVPGroupRemove" class="tab-pane fade in">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <h4 class="panel-title" style="display: inline-block;">Vsebina</h4>
                                <a data-toggle="collapse" data-target="#tabCollapseTwo"
                                    href="#collapseOne"></a>
                            </div>
                            <div id="tabCollapseTwo" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-9">
                                            <dx:ASPxGridView ID="ASPxGridViewEmployeesToRemove" runat="server" EnableCallbackCompression="true"
                                                ClientInstanceName="gridEmployeesToRemove"
                                                Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                                                KeyFieldName="idKVPSkupina_Zaposleni" CssClass="gridview-no-header-padding"
                                                DataSourceID="XpoDSUsersToRemove">
                                                <ClientSideEvents SelectionChanged="gridEmployeesToRemove_OnSelectionChanged" />
                                                <Paddings Padding="0" />
                                                <Settings ShowVerticalScrollBar="True"
                                                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                                    ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                    <PageSizeItemSettings Visible="true" Items="50,70,90" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                    </PageSizeItemSettings>
                                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                </SettingsPager>
                                                <SettingsBehavior AllowFocusedRow="true" AllowEllipsisInText="true" />
                                                <Styles Header-Wrap="True">
                                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                </Styles>
                                                <SettingsText EmptyDataRow="Trenutno ni podatka o zaposlenih." />
                                                <Columns>

                                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="100px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" />

                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="IdUser.Id" Width="80px"
                                                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Ime" FieldName="IdUser.Firstname" Width="25%"
                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Priimek"
                                                        FieldName="IdUser.Lastname" ShowInCustomizationForm="True"
                                                        Width="25%">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Email"
                                                        FieldName="IdUser.Email" ShowInCustomizationForm="True"
                                                        Width="25%">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Oddelek"
                                                        FieldName="IdUser.DepartmentId.Name" ShowInCustomizationForm="True"
                                                        Width="15%">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="Vloga"
                                                        FieldName="IdUser.RoleID.Naziv" ShowInCustomizationForm="True"
                                                        Width="15%">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn Caption="KVP skupina"
                                                        FieldName="idKVPSkupina.Naziv" ShowInCustomizationForm="True"
                                                        Width="25%">
                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                    </dx:GridViewDataTextColumn>

                                                </Columns>
                                            </dx:ASPxGridView>
                                        </div>
                                    </div>
                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-12 text-center">
                                            <dx:ASPxButton ID="btnRemoveFromKVPGroup" runat="server" Text="Odstrani iz skupine" ClientEnabled="false"
                                                ClientInstanceName="clientBtnRemoveFromKVPGroup" AutoPostBack="false" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="clientBtnRemoveFromKVPGroup_Click" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:XpoDataSource ID="XpoDSUsers" runat="server" ServerMode="True"
        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users" Criteria="Not [<KVPSkupina_Users>][^.Id=IdUser] and Deleted=0">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSKVPGroups" runat="server" ServerMode="True"
        DefaultSorting="idKVPSkupina" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPSkupina">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSUsersToRemove" runat="server" ServerMode="True"
        DefaultSorting="idKVPSkupina" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPSkupina_Users" Criteria="[<Users>][^.IdUser=Id] AND IdUser.Deleted = 1">
    </dx:XpoDataSource>
</asp:Content>
