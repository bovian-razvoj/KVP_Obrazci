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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <ul class="nav nav-tabs">
                <li id="basicDataItem" class="active"><a data-toggle="tab" href="#BasicData">Vsebina</a></li>
                <li id="credentialsDataItem"><a data-toggle="tab" href="#CredentialsData">Dostop</a></li>
                <li id="payoutsItem"><a data-toggle="tab" href="#Payouts"><span id="PayoutsBadge" runat="server" class="badge">0</span> Izplačila</a></li>
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
                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
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

                <div id="Payouts" class="tab-pane fade in">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <h4 class="panel-title" style="display: inline-block;">Izplačila zaposlenega</h4>
                            <a data-toggle="collapse" data-target="#contentPayouts"
                                href="#collapseOne"></a>
                        </div>
                        <div id="contentPayouts" class="panel-collapse collapse in">
                            <div class="panel-body">
                            </div>
                        </div>
                    </div>
                </div>

                <dx:XpoDataSource ID="XpoDSRoles" runat="server" ServerMode="True"
                    DefaultSorting="VlogaID" TypeName="KVP_Obrazci.Domain.KVPOdelo.Vloga">
                </dx:XpoDataSource>
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
                                    <ClientSideEvents Click="CauseValidation" />
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


            </div>
        </div>
    </div>
</asp:Content>
