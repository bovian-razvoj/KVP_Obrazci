<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="NewsForm.aspx.cs" Inherits="KVP_Obrazci.Posts.NewsForm" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-sm-10">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Vsebina</h4>
                    <a data-toggle="collapse" data-target="#editNews"
                        href="#collapseOne"></a>
                </div>
                <div id="editNews" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row2 small-padding-bottom">
                            <div class="col-xs-12">
                                <dx:ASPxTextBox runat="server" ID="txtNewsTitle" ClientInstanceName="clientTxtNewsTitle"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" Font-Bold="true" NullText="Naslov novice">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents Init="SetFocus" />
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                        <div class="row2 small-padding-top">
                            <div class="col-xs-12">
                                <dx:ASPxHtmlEditor ID="ASPxHtmlEditorPost" runat="server" Width="100%" CssClass="text-box-input">
                                </dx:ASPxHtmlEditor>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Izvleček (uvodno besedilo)</h4>
                    <a data-toggle="collapse" data-target="#Excerpt"
                        href="#collapseOne"></a>
                </div>
                <div id="Excerpt" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row2 align-item-centerV-centerH">
                            <div class="col-sm-12 no-padding-left">
                                <dx:ASPxMemo ID="ASPxMemoExcerpt" runat="server" Width="100%" MaxLength="300"
                                    NullText="Izvleček novice..." Rows="5" HorizontalAlign="Left" BackColor="White"
                                    CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoExcerpt">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxMemo>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-2">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Podatki</h4>
                    <a data-toggle="collapse" data-target="#Settings"
                        href="#collapseOne"></a>
                </div>
                <div id="Settings" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row small-padding-bottom">
                            <div class="col-sm-12">
                                <div class="row2" style="align-items: center;">
                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="OBJAVI KO SHRANIM : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-1 no-padding-left">
                                        <dx:ASPxCheckBox ID="CheckBoxPublish" runat="server" ToggleSwitchDisplayMode="Always"></dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Kategorija</h4>
                    <a data-toggle="collapse" data-target="#categorie"
                        href="#collapseOne"></a>
                </div>
                <div id="categorie" class="panel-collapse collapse in">
                    <div class="panel-body">
                    </div>
                </div>
            </div>

            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Prikazna slika</h4>
                    <a data-toggle="collapse" data-target="#featuredImage"
                        href="#collapseOne"></a>
                </div>
                <div id="featuredImage" class="panel-collapse collapse in">
                    <div class="panel-body">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row2">
        <div class="AddEditButtonsElements clearFloatBtns big-margin-l">
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnConfirm" runat="server" Text="Shrani" AutoPostBack="false"
                    Height="30" Width="50" ValidationGroup="Confirm"
                    UseSubmitBehavior="false" OnClick="btnConfirm_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                    Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../Images/cancel.png" UrlHottracked="../Images/cancelHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>
</asp:Content>
