<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="VersionDetails.aspx.cs" Inherits="KVP_Obrazci.CompanySettings.VersionDetails" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function CauseValidation(s, e) {
            var process = false;
            var inputItems = [clientTxtVersionNumber];
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
        <div class="col-md-7">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Opis verzij</h4>
                    <a data-toggle="collapse" data-target="#content"
                        href="#collapseOne"></a>
                </div>
                <div id="content" class="panel-collapse collapse in">
                    <div class="panel-body" style="max-height:650px; overflow:scroll;">
                        <div runat="server" id="versionDetails"></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-5" runat="server" id="newVersionCol">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Nova verzija</h4>
                    <a data-toggle="collapse" data-target="#content"
                        href="#collapseOne"></a>
                </div>
                <div id="content" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row large-padding-bottom">
                            <div class="col-md-12">
                                <div class="row2 align-item-centerV-startH">
                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="STEVILKA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-8 no-padding-left">
                                        <dx:ASPxTextBox runat="server" ID="txtVersionNumber" ClientInstanceName="clientTxtVersionNumber"
                                            CssClass="text-box-input" Font-Size="13px" Font-Bold="true" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row large-padding-bottom">
                            <div class="col-md-12">
                                <div class="row2 align-item-centerV-startH">
                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="OPIS : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-12 no-padding-left">
                                        <dx:ASPxHtmlEditor ID="HtmlEditorVersionDetails" runat="server" Width="100%" CssClass="text-box-input"></dx:ASPxHtmlEditor>
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


</asp:Content>
