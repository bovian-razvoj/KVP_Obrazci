<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PopUp.Master" AutoEventWireup="true" CodeBehind="Line_popup.aspx.cs" Inherits="KVP_Obrazci.CodeList.Line.Line_popup" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidateInput(s, e)
        {
            var inputItems = [clientTxtSort, clientTxtDesc];//clientTxtCode,
            var process = InputFieldsValidation(null, inputItems, null, null, null, null);

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row2 small-padding-bottom small-padding-top align-item-centerV-startH">
        <div class="col-xs-0 big-margin-r">
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="SORT : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-6 no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtSort" ClientInstanceName="clientTxtSort"
                CssClass="text-box-input" Font-Size="13px" Width="100%" Font-Bold="true" AutoCompleteType="Disabled" BackColor="LightGray" ReadOnly="true">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                <ClientSideEvents KeyPress="isNumberKey_int" />
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row2 small-padding-bottom small-padding-top align-item-centerV-startH hidden">
        <div class="col-xs-0 big-margin-r">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="KODA : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-5 no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtCode" ClientInstanceName="clientTxtCode" MaxLength="50"
                CssClass="text-box-input" Font-Size="13px" Width="100%" Font-Bold="true" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>

    <div class="row2 small-padding-bottom small-padding-top align-item-centerV-startH">
        <div class="col-xs-0 big-margin-r">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="OPIS : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-10 no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtDesc" ClientInstanceName="clientTxtDesc" MaxLength="150"
                CssClass="text-box-input" Font-Size="13px" Width="100%" Font-Bold="true" AutoCompleteType="Disabled">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>
    
    <div class="row2 small-padding-top align-item-centerV-endH">
        <div class="col-xs-12">
            <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Shrani" AutoPostBack="false"
                ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopUp_Click" UseSubmitBehavior="false"
                Width="100px">
                <ClientSideEvents Click="ValidateInput" />
            </dx:ASPxButton>
        </div>
    </div>
</asp:Content>
