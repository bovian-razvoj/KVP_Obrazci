<%@ Page Title="Urejanje KVP skupine" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="KVPGroupsForm.aspx.cs" Inherits="KVP_Obrazci.KVPGroups.KVPGroupsForm" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function init_clientCallbackPanelEmployees(s, e) {
            $('#KVPGroupBasicData').addClass('active');
            $('#ctl00_ContentPlaceHolderMain_CallbackPanelEmployees_basicDataItem').addClass('active');
        }
        //EMPLOYEE
        function OnAddEmployee_Click(s, e) {
            var userActionAdd = '<%= (int)KVP_Obrazci.Common.Enums.UserAction.Add %>';
            var action = GetUrlQueryStrings()['action'];
            if (userActionAdd == action) {

                var process = CheckValidation();
                if (process)
                    e.processOnServer = true;
                else
                    e.processOnServer = false;
            }
            else {
                e.processOnServer = false;
                Employees_PerformCallback("AddEmployee");
            }
        }

        function OnRemoveEmployee_Click(s, e) {
            Employees_PerformCallback("RemoveEmployee");
        }

        //CHAMPION
        function OnAddChampion_Click(s, e) {
            Employees_PerformCallback("AddEmployeeChampion");
        }

        function OnRemoveChampion_Click(s, e) {
            Employees_PerformCallback("RemoveEmployeeChampion");
        }

        function Employees_PerformCallback(parameter) {
            var process = CheckValidation();
            if (process)
                clientCallbackPanelEmployees.PerformCallback(parameter);
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Employee':
                            clientPopUpEmployee.Hide();
                            clientCallbackPanelEmployees.PerformCallback("RefreshGrid;KVPGroupBasicData");
                            break;
                        case 'EmployeeChampion':
                            clientPopUpEmployee.Hide();
                            clientCallbackPanelEmployees.PerformCallback("RefreshGrid;Champion");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'Employee':
                            clientPopUpEmployee.Hide();
                    }
                    break;
            }
        }

        function CauseValidation(s, e) {
            var process = CheckValidation();

            if (process) {
                e.processOnServer = true;
            }
            else
                e.processOnServer = false;
        }

        function CheckValidation() {
            var process = false;
            var inputItems = [clientTxtKoda, clientTxtNaziv];
            var lookupItems = null;
            var isLookUpEmpty = false;


            /*if (!HasAuditorValue()) {
                lookupItems = [lookUpAuditor1, lookUpAuditor2, lookUpAuditor3];
            }
            else {
                $(lookUpAuditor1.GetInputElement()).parent().parent().removeClass("focus-text-box-input-error");
                $(lookUpAuditor2.GetInputElement()).parent().parent().removeClass("focus-text-box-input-error");
                $(lookUpAuditor3.GetInputElement()).parent().parent().removeClass("focus-text-box-input-error");
            }*/


            process = InputFieldsValidation(lookupItems, inputItems, null, null, null, null);

            return process;
        }

        function HasAuditorValue() {
            if (lookUpAuditor1.GetValue() != null) {
                return true;
            }
            else if (lookUpAuditor2.GetValue() != null) {
                return true;
            }
            else if (lookUpAuditor3.GetValue() != null) {
                return true;
            }

            return false;
        }

        function OnSelectionChanged_gridEmployees(s, e) {
            RemoveBtn_Enabled(s, clientBtnRemoveEmployee);
        }

        function OnSelectionChanged_gridEmployeesChampion(s, e) {
            RemoveBtn_Enabled(s, clientBtnRemoveChampion);
        }

        function RemoveBtn_Enabled(sender, removeBtn) {
            if (sender.GetSelectedRowCount() > 0)
                removeBtn.SetEnabled(true);
            else
                removeBtn.SetEnabled(false);
        }

        function CallbackPanelEmployees_EndCallback(s, e) {
            if (s.cpTabShow != "" && s.cpTabShow !== undefined) {

                $('.nav-tabs a[href="#' + s.cpTabShow + '"]').tab('show');
                //$('#' + s.cpTabShow).addClass('active');
                //$('.nav-tabs a[href="#' + s.cpTabShow + '"]').parent().addClass('active');
                delete (s.cpTabShow);
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelEmployees" ClientInstanceName="clientCallbackPanelEmployees" runat="server" Width="100%"
        OnCallback="CallbackPanelEmployees_Callback">
        <ClientSideEvents EndCallback="CallbackPanelEmployees_EndCallback" Init="init_clientCallbackPanelEmployees" />
        <PanelCollection>
            <dx:PanelContent>
                <ul class="nav nav-tabs" runat="server" id="navTabs">
                    <li runat="server" class="active" id="basicDataItem">
                        <a data-toggle="tab" href="#KVPGroupBasicData"><span class="glyphicon glyphicon-list-alt"> KVPSkupina</span></a>
                    </li>
                    <li runat="server" id="championItem">
                        <a data-toggle="tab" href="#Champion"><span runat="server" id="championBadge" class="badge">0</span> Champion</a>
                    </li>
                </ul>
                <div class="tab-content">
                    <div id="KVPGroupBasicData" class="tab-pane fade in active">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <h4 class="panel-title" style="display: inline-block;">Vsebina</h4>
                                <a data-toggle="collapse" data-target="#kvpGroup"
                                    href="#collapseOne"></a>
                            </div>
                            <div id="kvpGroup" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row small-padding-bottom">
                                        <div class="col-md-3">
                                            <div class="row2" style="align-items: center">
                                                <div class="col-sm-3 no-padding-right">
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="KODA SKUPINE : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-9 no-padding-left">
                                                    <dx:ASPxTextBox runat="server" ID="txtKoda" ClientInstanceName="clientTxtKoda"
                                                        CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="50">
                                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                        <ClientSideEvents Init="SetFocus" />
                                                    </dx:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-9">
                                            <div class="row2" style="align-items: center">
                                                <div class="col-sm-5 align-content-right">
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="NAZIV SKUPINE : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-7 no-padding-left">
                                                    <dx:ASPxTextBox runat="server" ID="txtNaziv" ClientInstanceName="clientTxtNaziv"
                                                        CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200">
                                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    </dx:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row small-padding-bottom">
                                        <div class="col-md-9">
                                            <div class="row2 small-padding-bottom">
                                                <div class="col-xs-10 no-padding-right">
                                                    <dx:ASPxGridView ID="ASPxGridViewEmployees" runat="server" EnableCallbackCompression="true"
                                                        ClientInstanceName="gridEmployees"
                                                        Width="100%" KeyboardSupport="true" AccessKey="G"
                                                        KeyFieldName="idKVPSkupina_Zaposleni" CssClass="gridview-no-header-padding"
                                                        DataSourceID="XpoDSGroupEmployee"
                                                        OnDataBound="ASPxGridViewEmployees_DataBound" OnCustomUnboundColumnData="ASPxGridViewEmployees_CustomUnboundColumnData">
                                                        <ClientSideEvents SelectionChanged="OnSelectionChanged_gridEmployees" />
                                                        <Paddings Padding="0" />
                                                        <Settings ShowVerticalScrollBar="True" ShowFooter="false"
                                                            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300"
                                                            ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                        <SettingsPager PageSize="30" ShowNumericButtons="true">
                                                            <PageSizeItemSettings Visible="true" Items="30,60,90" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                            </PageSizeItemSettings>
                                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                        </SettingsPager>
                                                        <SettingsBehavior AllowFocusedRow="true" AllowEllipsisInText="true" AutoFilterRowInputDelay ="8000"/>
                                                        <Styles Header-Wrap="True">
                                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                        </Styles>
                                                        <SettingsText EmptyDataRow="Trenutno ni podatka o zaposlenih. Dodaj novega." />
                                                        <Columns>

                                                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="None" Caption="Izberi" ShowClearFilterButton="true" />

                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="IdUser.Id" Width="80px"
                                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="IdUser.ExternalId" Width="80px"
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
                                                            
                                                            <dx:GridViewDataTextColumn Caption="Stevilo KVP v Letu"
                                                                 UnboundType="Integer" FieldName="StKVP" ShowInCustomizationForm="True"
                                                                Width="15%">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                    </dx:ASPxGridView>

                                                </div>
                                                <div class="col-xs-2 no-padding-right">
                                                    <div class="text-center small-margin-b">
                                                        <dx:ASPxButton ID="btnAddEmployee" runat="server" RenderMode="Button" UseSubmitBehavior="false" AutoPostBack="false"
                                                            ClientInstanceName="clientBtnAddEmployee" ToolTip="Dodaj zaposlenega" OnClick="btnAddEmployee_Click">
                                                            <Paddings PaddingLeft="25px" PaddingRight="25px" PaddingTop="0px" PaddingBottom="0px" />
                                                            <Image Url="../Images/plus.png" />
                                                            <ClientSideEvents Click="OnAddEmployee_Click" />
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div class="text-center">
                                                        <dx:ASPxButton ID="btnRemoveEmployee" runat="server" RenderMode="Button" UseSubmitBehavior="false" AutoPostBack="false"
                                                            ClientInstanceName="clientBtnRemoveEmployee" ToolTip="Odstrani zaposlenega" ClientEnabled="false">
                                                            <Paddings PaddingLeft="25px" PaddingRight="25px" PaddingTop="0px" PaddingBottom="0px" />
                                                            <Image Url="../Images/minus.png" />
                                                            <ClientSideEvents Click="OnRemoveEmployee_Click" />
                                                        </dx:ASPxButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 hidden">
                                            <div class="row2 small-padding-bottom" style="align-items: center">
                                                <div class="col-sm-5 no-padding-right" style="align-items: center;">
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="POTRJEVALEC 1 : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-7 no-padding-left">
                                                    <dx:ASPxGridLookup ID="ASPxGridLookupAuditor1" runat="server" ClientInstanceName="lookUpAuditor1"
                                                        KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                        OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee">
                                                        <ClearButton DisplayMode="OnHover" />
                                                        <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />
                                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                        <GridViewStyles>
                                                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                            <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                        </GridViewStyles>
                                                        <GridViewProperties>
                                                            <SettingsBehavior EnableRowHotTrack="True" />
                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AutoFilterRowInputDelay ="8000" />
                                                            <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                        </GridViewProperties>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="User Id" FieldName="Id" Width="80px"
                                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="30%"
                                                                ReadOnly="true" ShowInCustomizationForm="True">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
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

                                                            <dx:GridViewDataTextColumn Caption="Vloga"
                                                                FieldName="RoleID.Naziv" ShowInCustomizationForm="True"
                                                                Width="25%">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>
                                                    </dx:ASPxGridLookup>
                                                </div>
                                            </div>
                                            <div class="row2 small-padding-bottom" style="align-items: center">
                                                <div class="col-sm-5 no-padding-right" style="align-items: center;">
                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="POTRJEVALEC 2 : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-7 no-padding-left">
                                                    <dx:ASPxGridLookup ID="ASPxGridLookupAuditor2" runat="server" ClientInstanceName="lookUpAuditor2"
                                                        KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                        OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee">
                                                        <ClearButton DisplayMode="OnHover" />
                                                        <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />
                                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                        <GridViewStyles>
                                                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                            <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                        </GridViewStyles>
                                                        <GridViewProperties>
                                                            <SettingsBehavior EnableRowHotTrack="True" />
                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AutoFilterRowInputDelay ="8000"/>
                                                            <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                        </GridViewProperties>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="User Id" FieldName="Id" Width="80px"
                                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="30%"
                                                                ReadOnly="true" ShowInCustomizationForm="True">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
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

                                                            <dx:GridViewDataTextColumn Caption="Vloga"
                                                                FieldName="RoleID.Naziv" ShowInCustomizationForm="True"
                                                                Width="25%">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>
                                                    </dx:ASPxGridLookup>
                                                </div>
                                            </div>
                                            <div class="row2 small-padding-bottom" style="align-items: center">
                                                <div class="col-sm-5 no-padding-right" style="align-items: center;">
                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="POTRJEVALEC 3 : "></dx:ASPxLabel>
                                                </div>
                                                <div class="col-sm-7 no-padding-left">
                                                    <dx:ASPxGridLookup ID="ASPxGridLookupAuditor3" runat="server" ClientInstanceName="lookUpAuditor3"
                                                        KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                        Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                        OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee">
                                                        <ClearButton DisplayMode="OnHover" />
                                                        <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />
                                                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                        <GridViewStyles>
                                                            <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                            <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                        </GridViewStyles>
                                                        <GridViewProperties>
                                                            <SettingsBehavior EnableRowHotTrack="True" />
                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AutoFilterRowInputDelay ="8000"/>
                                                            <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                        </GridViewProperties>
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="User Id" FieldName="Id" Width="80px"
                                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="30%"
                                                                ReadOnly="true" ShowInCustomizationForm="True">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
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

                                                            <dx:GridViewDataTextColumn Caption="Vloga"
                                                                FieldName="RoleID.Naziv" ShowInCustomizationForm="True"
                                                                Width="25%">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>
                                                    </dx:ASPxGridLookup>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="Champion" class="tab-pane fade in">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <h4 class="panel-title" style="display: inline-block;">Dodaj champion-a</h4>
                                <a data-toggle="collapse" data-target="#championPanel"
                                    href="#collapseOne"></a>
                            </div>
                            <div id="championPanel" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <div class="row small-padding-bottom">
                                        <div class="col-md-9">
                                            <div class="row2 small-padding-bottom">
                                                <div class="col-xs-10 no-padding-right">
                                                    <dx:ASPxGridView ID="ASPxGridViewGroupEmployeeChampion" runat="server" EnableCallbackCompression="true"
                                                        ClientInstanceName="gridEmployeeChampion"
                                                        Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                                                        KeyFieldName="idKVPSkupina_Zaposleni" CssClass="gridview-no-header-padding"
                                                        DataSourceID="XpoDSGroupEmployeeChampion"
                                                        OnDataBound="ASPxGridViewGroupEmployeeChampion_DataBound">
                                                        <ClientSideEvents SelectionChanged="OnSelectionChanged_gridEmployeesChampion" />
                                                        <Paddings Padding="0" />
                                                        <Settings ShowVerticalScrollBar="True"
                                                            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300"
                                                            ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                        <SettingsPager PageSize="10" ShowNumericButtons="true">
                                                            <PageSizeItemSettings Visible="true" Items="10,20,30" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                            </PageSizeItemSettings>
                                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                        </SettingsPager>
                                                        <SettingsBehavior AllowFocusedRow="true" AllowEllipsisInText="true" AutoFilterRowInputDelay ="8000"/>
                                                        <Styles Header-Wrap="True">
                                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                        </Styles>
                                                        <SettingsText EmptyDataRow="Trenutno ni podatka o zaposlenih. Dodaj novega." />
                                                        <Columns>

                                                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" SelectAllCheckboxMode="None" Caption="Izberi" ShowClearFilterButton="true" />

                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPSkupina_Zaposleni" Width="80px"
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

                                                        </Columns>
                                                    </dx:ASPxGridView>
                                                </div>
                                                <div class="col-xs-2 no-padding-right">
                                                    <div class="text-center small-margin-b">
                                                        <dx:ASPxButton ID="btnAddChampion" runat="server" RenderMode="Button" UseSubmitBehavior="false" AutoPostBack="false"
                                                            ClientInstanceName="clientBtnAddChampion" ToolTip="Dodaj champion-a">
                                                            <Paddings PaddingLeft="25px" PaddingRight="25px" PaddingTop="0px" PaddingBottom="0px" />
                                                            <Image Url="../Images/plus.png" />
                                                            <ClientSideEvents Click="OnAddChampion_Click" />
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div class="text-center">
                                                        <dx:ASPxButton ID="btnRemoveChampion" runat="server" RenderMode="Button" UseSubmitBehavior="false" AutoPostBack="false"
                                                            ClientInstanceName="clientBtnRemoveChampion" ToolTip="Odstrani champion-a" ClientEnabled="false">
                                                            <Paddings PaddingLeft="25px" PaddingRight="25px" PaddingTop="0px" PaddingBottom="0px" />
                                                            <Image Url="../Images/minus.png" />
                                                            <ClientSideEvents Click="OnRemoveChampion_Click" />
                                                        </dx:ASPxButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row2">
                        <div class="col-xs-12 align-item-centerV-centerH">
                            <div class="AddEditButtonsElements clearFloatBtns">
                                <div style="display: inline-block; position: relative; left: 30%">
                                    <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red"></dx:ASPxLabel>
                                </div>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                                        Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm"
                                        UseSubmitBehavior="false" OnClick="btnConfirm_Click">
                                        <ClientSideEvents Click="CauseValidation" />
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="CauseValidation" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton Theme="Moderno" ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                    </dx:ASPxButton>
                                </span>
                            </div>
                        </div>
                    </div>

                    <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
                        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
                    </dx:XpoDataSource>

                    <dx:XpoDataSource ID="XpoDSGroupEmployee" runat="server" ServerMode="True"
                        DefaultSorting="idKVPSkupina_Zaposleni" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPSkupina_Users" Criteria="[idKVPSkupina] = ?">
                        <CriteriaParameters>
                            <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
                        </CriteriaParameters>
                    </dx:XpoDataSource>

                    <dx:XpoDataSource ID="XpoDSGroupEmployeeChampion" runat="server" ServerMode="True"
                        DefaultSorting="idKVPSkupina_Zaposleni" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPSkupina_Users" Criteria="[idKVPSkupina] = ? AND [Champion] = 1">
                        <CriteriaParameters>
                            <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
                        </CriteriaParameters>
                    </dx:XpoDataSource>
                    <dx:ASPxPopupControl ID="ASPxPopupControlEmployee" runat="server" ContentUrl="Employee_popup.aspx"
                        ClientInstanceName="clientPopUpEmployee" Modal="True" HeaderText="ZAPOSLENI"
                        CloseAction="CloseButton" Width="1000px" Height="580px" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                        AllowResize="true" ShowShadow="true"
                        OnWindowCallback="ASPxPopupControlEmployee_WindowCallback">
                        <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                        <ContentStyle BackColor="#F7F7F7">
                            <Paddings Padding="0px"></Paddings>
                        </ContentStyle>
                    </dx:ASPxPopupControl>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
