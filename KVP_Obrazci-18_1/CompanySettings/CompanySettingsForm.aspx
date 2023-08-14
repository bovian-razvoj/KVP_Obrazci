<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CompanySettingsForm.aspx.cs" Inherits="KVP_Obrazci.CompanySettings.CompanySettingsForm" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function CauseValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtKVPNumber, clientTxtPayout, clientTxtQuotient];
            process = InputFieldsValidation(null, inputItems, null, null, null, null);

            if (process) {
                e.processOnServer = true;
            }
            else
                e.processOnServer = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <ul class="nav nav-tabs">
                <li id="basicDataItem" class="active"><a data-toggle="tab" href="#BasicSettings"><i class="fa fa-gears"></i>Nastavitve za podjetje</a></li>
            </ul>
            <div class="tab-content">
                <div id="BasicSettings" class="tab-pane fade in active">
                    <div class="panel panel-default" style="margin-top: 10px;">
                        <div class="panel-heading">
                            <h4 class="panel-title" style="display: inline-block;">Osnovni podatki</h4>
                            <a data-toggle="collapse" data-target="#content"
                                href="#collapseOne"></a>
                        </div>
                        <div id="content" class="panel-collapse collapse in">
                            <div class="panel-body">
                                <div class="row large-padding-bottom">
                                    <div class="col-md-4">
                                        <div class="row2 align-item-centerV-startH">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="ZAČETNA KVP ŠTEVILKA : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-7 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtKVPNumber" ClientInstanceName="clientTxtKVPNumber"
                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="LightGray" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <ClientSideEvents KeyPress="isNumberKey_int" />
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row2 align-item-centerV-centerH">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="IZPLAČILO (€) : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-9 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtPayout" ClientInstanceName="clientTxtPayout"
                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="LightGray" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row2 align-item-centerV-endH">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="KOLIČNIK (točke) : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-9 no-padding-left">
                                                <dx:ASPxTextBox runat="server" ID="txtQuotient" ClientInstanceName="clientTxtQuotient"
                                                    CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" BackColor="LightGray" AutoCompleteType="Disabled">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                    <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row large-padding-bottom">
                                    <div class="col-md-4" style="justify-content: center">
                                        <div class="row2 align-item-centerV-startH">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="OMOGOČI POŠILJANJE E-POŠTE : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-6 no-padding-left">
                                                <dx:ASPxCheckBox ID="CheckBoxPosiljanjePoste" ClientInstanceName="ToggleCheckBox" runat="server" ToggleSwitchDisplayMode="Always">
                                                </dx:ASPxCheckBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-8">
                                        <div class="row2 align-item-centerV-endH">
                                            <div class="col-sm-0 big-margin-r">
                                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="OPOMBE : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-sm-11 no-padding-left">
                                                <dx:ASPxMemo ID="ASPxMemoOpombe" runat="server" Width="100%" MaxLength="5000"
                                                    NullText="Zapiski in opombe za podjetja..." Rows="8" HorizontalAlign="Left" BackColor="White"
                                                    CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoProblemDesc">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxMemo>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row small-padding-bottom">
                                    <div class="col-xs-12 text-center">
                                        <dx:ASPxButton ID="btnConfirm" runat="server" Text="Shrani spremembe" AutoPostBack="false"
                                            Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm"
                                            UseSubmitBehavior="false" OnClick="btnConfirm_Click">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <ClientSideEvents Click="CauseValidation" />
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
</asp:Content>
