<%@ Page Title="Presojevalci" Language="C#" MasterPageFile="~/MasterPages/PopUp.Master" AutoEventWireup="true" CodeBehind="SendInfoMail_popup.aspx.cs" Inherits="KVP_Obrazci.KVPDocuments.SendInfoMail_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function btnSendInfoMailClick(s, e)
        {
            var memoFields = [clientMemoNotes];
            var lookupItems = [lookUpEmployee];

            var process = InputFieldsValidation(lookupItems, null, null, memoFields, null, null);

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row2 small-padding-bottom small-padding-top" style="align-items: center">
        <div class="col-xs-2 no-padding-right">
            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="PREJEMNIK : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-10 no-padding-left">
            <dx:ASPxGridLookup ID="ASPxGridLookupEmployee" runat="server" ClientInstanceName="lookUpEmployee"
                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="50%" Font-Size="14px"
                OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSEmployee" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                IncrementalFilteringMode="Contains">
                <ClearButton DisplayMode="OnHover" />
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                <GridViewStyles>
                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                </GridViewStyles>
                <GridViewProperties>
                    <SettingsBehavior EnableRowHotTrack="True" />
                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" />
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
                        Width="25%" Visible="false">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Oddelek"
                        FieldName="DepartmentId.Name" ShowInCustomizationForm="True"
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


    <div class="row2" style="align-items: center">
        <div class="col-xs-2 no-padding-right">
            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="BESEDILO : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-10 no-padding-left">
            <dx:ASPxMemo ID="ASPxMemoNotes" runat="server" Width="100%" MaxLength="5000"
                NullText="Podroben opis..." Rows="16" HorizontalAlign="Left" BackColor="White"
                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoNotes">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxMemo>
        </div>
    </div>

    <div class="row2 small-padding-top">
        <div class="col-xs-12" style="align-items: flex-end">
            <dx:ASPxButton ID="btnSendInfoMail" runat="server" Text="Pošlji" AutoPostBack="false"
                ClientInstanceName="clientBtnSendInfoMail" OnClick="btnSendInfoMail_Click" UseSubmitBehavior="false"
                Width="100px">
                <ClientSideEvents Click="btnSendInfoMailClick" />
            </dx:ASPxButton>
        </div>
    </div>
    <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
    </dx:XpoDataSource>
</asp:Content>
