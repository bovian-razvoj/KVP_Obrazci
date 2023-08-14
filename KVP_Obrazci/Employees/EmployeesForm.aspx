<%@ Page Title="Urejanje zaposlenih" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="EmployeesForm.aspx.cs" Inherits="KVP_Obrazci.Employees.EmployeesForm" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function CauseValidation(s, e) {
            var process = false;
            var lookupItems = [lookUpRoles];
            var inputItems = [clientTxtFirstname, clientTxtLastname, clientTxtDepartment];
            process = InputFieldsValidation(lookupItems, null, null, null, null, null);

            if (process) {
                e.processOnServer = true;
            }
            else
                e.processOnServer = false;
        }

        function myFunction() {
            var x = document.getElementById("ctl00_ContentPlaceHolderMain_txtPassword_I");

            if (x.type === "password") {
                x.type = "text";
                $('.icon-show-hide-pass').css("background-image", "url('/Images/hidePass.png')");
            } else {
                x.type = "password";
                $('.icon-show-hide-pass').css("background-image", "url('/Images/showPass.png')");
            }
        }

        $(document).ready(function () {
            $('[data-toggle="popover"]').popover();

            $("[data-toggle='popover']").on('shown.bs.popover', function () {
                setTimeout(function () {
                    $('#columnPopover').popover('hide');
                }, 4000);
            });
        });

        function gridKVPDocument_Init(s, e) {
            SetEnableExportBtn(s, clientBtnExportToExcelKVPPayouts);
        }

        function SetEnableExportBtn(sender, button) {
            if (sender.GetVisibleRowsOnPage() > 0)
                button.SetEnabled(true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <ul class="nav nav-tabs">
                <li id="basicDataItem" class="active"><a data-toggle="tab" href="#BasicData">Vsebina</a></li>
                <li id="credentialsDataItem"><a data-toggle="tab" href="#CredentialsData">Dostop</a></li>
                <li id="payoutsItem"><a data-toggle="tab" href="#KVPPayouts">KVP-ji za izplačila</a></li>
            </ul>
            <div class="tab-content">
                <div id="BasicData" class="tab-pane fade in active">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <h4 class="panel-title" style="display: inline-block;">Osnovni podatki</h4>
                            <a data-toggle="collapse" data-target="#content"
                                href="#collapseOne"></a>
                        </div>
                        <div id="content" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <div class="row small-padding-bottom">
                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r" style="margin-right: 50px;">
                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="ID : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-10 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtPersonalID" ClientInstanceName="clientTxtPersonalID"
                                                    CssClass="text-box-input" Font-Size="13px" Width="40%" BackColor="LightGray" ReadOnly="true">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r" style="margin-right: 30px;">
                                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="ŽETON : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-10 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtCard" ClientInstanceName="clientTxtCard"
                                                    CssClass="text-box-input" Font-Size="13px" Width="40%" BackColor="LightGray" ReadOnly="true">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row small-padding-bottom">
                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r" style="margin-right: 42px;">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="IME : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-10 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtFirstname" ClientInstanceName="clientTxtFirstname"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" BackColor="LightGray" ReadOnly="true">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <ClientSideEvents Init="SetFocus" />
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="PRIIMEK : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-10 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtLastname" ClientInstanceName="clientTxtLastname"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" BackColor="LightGray" ReadOnly="true">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r" style="margin-right: 25px;">
                                                <dx:ASPxLabel ID="ASPxLabel14" runat="server" Font-Size="12px" Text="VLOGA : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-10 no-padding-left">
                                                <dx:ASPxGridLookup ID="ASPxGridLookupRoles" runat="server" ClientInstanceName="lookUpRoles"
                                                    KeyFieldName="VlogaID" TextFormatString="{1}" CssClass="text-box-input"
                                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                    OnLoad="ASPxGridLookupLoad_WidthSmall" DataSourceID="XpoDSRoles">
                                                    <ClearButton DisplayMode="OnHover" />
                                                    <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}" />
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <GridViewStyles>
                                                        <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                        <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                    </GridViewStyles>
                                                    <GridViewProperties>
                                                        <SettingsBehavior EnableRowHotTrack="True" />
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AutoFilterRowInputDelay="8000" />
                                                        <SettingsPager ShowSeparators="True" AlwaysShowPager="false" ShowNumericButtons="false" NumericButtonCount="3" />
                                                        <Settings ShowFilterRow="false" ShowFilterRowMenu="false" ShowPreview="false" ShowVerticalScrollBar="True"
                                                            VerticalScrollableHeight="100"></Settings>
                                                    </GridViewProperties>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Id" FieldName="VlogaID" Width="80px"
                                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn Caption="Naziv"
                                                            FieldName="Naziv" ShowInCustomizationForm="True"
                                                            Width="100%">
                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                        </dx:GridViewDataTextColumn>

                                                    </Columns>
                                                </dx:ASPxGridLookup>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row small-padding-bottom hidden">

                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Font-Size="12px" Text="KVP SKUPINA : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-9 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtKVPGroup" ClientInstanceName="clientTxtKVPGroup"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" BackColor="LightGray" ReadOnly="true">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row large-padding-bottom">
                                    <div class="col-md-12">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Font-Size="12px" Text="OPOMBE : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-11 no-padding-left">
                                                <dx:ASPxMemo ID="ASPxMemoCustomField1" runat="server" Width="100%" MaxLength="255" Theme="Moderno"
                                                    NullText="Dodaten opis..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                    CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoCustomField1">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxMemo>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel panel-info">
                                            <div class="panel-heading"><strong>Oddelek</strong></div>
                                            <div class="panel-body">
                                                <div class="row small-padding-bottom">
                                                    <div class="col-md-3">
                                                        <div class="row2" style="align-items: center">
                                                            <div class="col-sm-0 big-margin-r">
                                                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="ODDELEK : "></dx:ASPxLabel>
                                                            </div>
                                                            <div class="col-md-8 no-padding-left">
                                                                <dx:ASPxTextBox runat="server" ID="txtDepartment" ClientInstanceName="clientTxtDepartment"
                                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="#d9edf7" ReadOnly="true">
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="row2 align-item-centerV-centerH">
                                                            <div class="col-sm-0 big-margin-r">
                                                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Size="12px" Text="VODJA : "></dx:ASPxLabel>
                                                            </div>
                                                            <div class="col-md-8 no-padding-left">
                                                                <dx:ASPxTextBox runat="server" ID="txtDepartmentHead" ClientInstanceName="clientTxtDepartmentHead"
                                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="#d9edf7" ReadOnly="true">
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="row2 align-item-centerV-centerH">
                                                            <div class="col-sm-0 big-margin-r">
                                                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Font-Size="12px" Text="NAMESTNIK VODJA : "></dx:ASPxLabel>
                                                            </div>
                                                            <div class="col-md-8 no-padding-left">
                                                                <dx:ASPxTextBox runat="server" ID="txtDepartmentDeputy" ClientInstanceName="clientTxtDepartmentDeputy"
                                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="#d9edf7" ReadOnly="true">
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-3">
                                                        <div class="row2 align-item-centerV-endH">
                                                            <div class="col-sm-0 big-margin-r">
                                                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Font-Size="12px" Text="NADREJENI ODDELEK : "></dx:ASPxLabel>
                                                            </div>
                                                            <div class="col-md-7 no-padding-left">
                                                                <dx:ASPxTextBox runat="server" ID="txtParentID" ClientInstanceName="clientTxtParentID"
                                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="#d9edf7" ReadOnly="true">
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel panel-warning">
                                            <div class="panel-heading"><strong>KVP skupina</strong></div>
                                            <div class="panel-body">
                                                <div class="row small-padding-bottom">
                                                    <div class="col-md-3">
                                                        <div class="row2" style="align-items: center">
                                                            <div class="col-sm-0 big-margin-r">
                                                                <dx:ASPxLabel ID="ASPxLabel15" runat="server" Font-Size="12px" Text="NAZIV : "></dx:ASPxLabel>
                                                            </div>
                                                            <div class="col-md-8 no-padding-left">
                                                                <dx:ASPxTextBox runat="server" ID="txtKVPGroupName" ClientInstanceName="clientTxtKVPGroupName"
                                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="#fcf8e3" ReadOnly="true">
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="row2 align-item-centerV-centerH">
                                                            <div class="col-sm-0 big-margin-r">
                                                                <dx:ASPxLabel ID="ASPxLabel16" runat="server" Font-Size="12px" Text="CHAMPION : "></dx:ASPxLabel>
                                                            </div>
                                                            <div class="col-md-8 no-padding-left">
                                                                <dx:ASPxTextBox runat="server" ID="txtKVPGroupChampion" ClientInstanceName="clientTxtKVPGroupChampion"
                                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="#fcf8e3" ReadOnly="true">
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div id="CredentialsData" class="tab-pane fade in">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <h4 class="panel-title" style="display: inline-block;">Dostopni podatki</h4>
                            <a data-toggle="collapse" data-target="#contentCredentials"
                                href="#collapseOne"></a>
                        </div>
                        <div id="contentCredentials" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <div class="row small-padding-bottom">
                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="UPO. IME : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-10 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtUsername" ClientInstanceName="clientTxtUsername"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Size="12px" Text="GESLO : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-8 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtPassword" ClientInstanceName="clientTxtPassword"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%" Password="true">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxTextBox>

                                            </div>
                                            <div class="col-sm-1">
                                                <div class="icon-show-hide-pass" onclick="myFunction()"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row2" style="align-items: center">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="EMAIL : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-10 no-padding-left" id="columnPopover" data-toggle="popover" title="Več e-naslovov" data-content="Če želite dodati več elektronskih naslovov jih ločite z vejico (,)"
                                                data-placement="top">
                                                <dx:ASPxTextBox runat="server" ID="txtEmail" ClientInstanceName="clientTxtEmail" AutoCompleteType="Disabled"
                                                    CssClass="text-box-input" Font-Size="13px" Width="100%">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <ClientSideEvents LostFocus="function(s,e){$('#columnPopover').popover('hide')}" GotFocus="function(s,e){$('#columnPopover').popover('show')}" />
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row small-padding-bottom">
                                    <div class="col-xs-12 align-item-centerV-centerH text-center">
                                        <span class="AddEditButtons">
                                            <dx:ASPxButton ID="btnSendCredentials" runat="server" Text="Pošlji dostopne podatke" AutoPostBack="false"
                                                Height="30" Width="50" UseSubmitBehavior="false" ClientEnabled="false" OnClick="btnSendCredentials_Click">
                                                <Paddings PaddingLeft="10" PaddingRight="10" />
                                            </dx:ASPxButton>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="KVPPayouts" class="tab-pane fade in">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <h4 class="panel-title" style="display: inline-block;">KVP za izplačila</h4>
                            <div class="col-xs-6 no-padding-right">
                            </div>
                            <div class="row2 align-item-centerV-endH">
                                <div class="col-xs-0 big-margin-r">
                                    <dx:ASPxButton ID="btnExportToExcelKVPPayouts" runat="server" RenderMode="Link" ClientEnabled="false" OnClick="btnExportToExcelKVPPayouts_Click"
                                        AutoPostBack="false" UseSubmitBehavior="false" ClientInstanceName="clientBtnExportToExcelKVPPayouts" ToolTip="Izvozi v excel">
                                        <DisabledStyle CssClass="icon-disabled" />
                                        <HoverStyle CssClass="icon-hover" />
                                        <Image Url="../Images/export_excel.png" Width="20px" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                            <a data-toggle="collapse" data-target="#contentPayouts"
                                href="#collapseOne"></a>

                        </div>
                        <div id="contentPayouts" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewKVPPayoutsExporter" GridViewID="ASPxGridViewKVPPayouts" runat="server">
                                    <Styles>
                                        <Header Wrap="True" />
                                    </Styles>
                                </dx:ASPxGridViewExporter>
                                <!-- Moji KVP -->
                                <dx:ASPxGridView ID="ASPxGridViewKVPPayouts" runat="server" EnableCallbackCompression="true" Settings-ShowHeaderFilterButton="true" SettingsText-SearchPanelEditorNullText="Vnesi iskalni niz ..."
                                    ClientInstanceName="gridKVPPayouts" SettingsBehavior-AutoFilterRowInputDelay="6000"
                                    Width="100%" KeyFieldName="idKVPDocument" CssClass="gridview-no-header-padding"
                                    DataSourceID="XpoDataSourceKVPPayout">
                                    <ClientSideEvents Init="gridKVPDocument_Init" />
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

                                        <dx:GridViewDataTextColumn Width="90px" Name="Priloga" Caption="Priloga" ShowInCustomizationForm="True">
                                            <CellStyle Paddings-Padding="0" HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="CheckBoxAttachment" runat="server" ReadOnly="true" Checked='<%# GetChecked(Eval("Priloge")) %>'>
                                                </dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
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
            </div>
        </div>
        <dx:XpoDataSource ID="XpoDataSourceKVPPayout" runat="server" ServerMode="True"
            DefaultSorting="DatumVnosa DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocument">
        </dx:XpoDataSource>

        <dx:XpoDataSource ID="XpoDSRoles" runat="server" ServerMode="True"
            DefaultSorting="VlogaID" TypeName="KVP_Obrazci.Domain.KVPOdelo.Vloga">
        </dx:XpoDataSource>
    </div>

    <div class="row2">
            <div class="col-xs-12 align-item-centerV-centerH">
                <div class="AddEditButtonsElements clearFloatBtns">
                    <div style="display: inline-block; position: relative; left: 30%">
                        <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red"></dx:ASPxLabel>
                    </div>
                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                            Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm"
                            UseSubmitBehavior="false" OnClick="btnConfirm_Click">
                            <ClientSideEvents Click="CauseValidation" />
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                        </dx:ASPxButton>
                    </span>
                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                            Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                        </dx:ASPxButton>
                    </span>
                </div>
            </div>
        </div>
</asp:Content>
