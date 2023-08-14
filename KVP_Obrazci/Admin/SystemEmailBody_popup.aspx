<%@ Page Title="Telo sporočila" Language="C#" MasterPageFile="~/MasterPages/PopUp.Master" AutoEventWireup="true" CodeBehind="SystemEmailBody_popup.aspx.cs" Inherits="KVP_Obrazci.Admin.SystemEmailBody_popup" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function btnConfirmPopUp_Click(s, e) {            
            e.processOnServer = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row2 small-padding-bottom small-padding-top" style="align-items: center">
        <div class="col-xs-12 no-padding-left">
            <dx:ASPxHtmlEditor ID="ASPxHtmlEditorEmailBody" runat="server" Width="100%">
                <Settings AllowContextMenu="False" AllowDesignView="false" AllowHtmlView="false" AllowPreview="true"></Settings>
            </dx:ASPxHtmlEditor>
        </div>
    </div>

    <div class="row2 small-padding-top">
        <div class="col-xs-12" style="align-items: flex-end">
            <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Potrdi" AutoPostBack="false"
                ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopUp_Click" UseSubmitBehavior="false"
                Width="100px">
                <ClientSideEvents Click="btnConfirmPopUp_Click" />
            </dx:ASPxButton>
        </div>
    </div>
  
</asp:Content>
