<%@ Page Title="Zaposleni KVP skupina" Language="C#" MasterPageFile="~/MasterPages/PopUp.Master" AutoEventWireup="true" CodeBehind="Employee_popup.aspx.cs" Inherits="KVP_Obrazci.KVPGroups.Employee_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnSelectionChanged_gridEmployees(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirm.SetEnabled(true);
            else
                clientBtnConfirm.SetEnabled(false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 99%;">
        <dx:ASPxGridView ID="ASPxGridViewEmployees" runat="server" EnableCallbackCompression="true"
            ClientInstanceName="gridEmployees"
            Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
            KeyFieldName="Id" CssClass="gridview-no-header-padding"
            DataSourceID="XpoDSUsers">
            <ClientSideEvents SelectionChanged="OnSelectionChanged_gridEmployees" />
            <Paddings Padding="0" />
            <Settings ShowVerticalScrollBar="True"
                ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300"
                ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
            <SettingsPager PageSize="20" ShowNumericButtons="true">
                <PageSizeItemSettings Visible="true" Items="20,30,50" Caption="Zapisi na stran : " AllItemText="Vsi">
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

                <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="50px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" />

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

            </Columns>
        </dx:ASPxGridView>
        <div class="row2 small-padding-top">
            <div class="col-xs-12" style="align-items: flex-end">
                <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Potrdi" AutoPostBack="false"
                    ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopUp_Click"
                    ClientEnabled="false" Width="100px">
                </dx:ASPxButton>
            </div>
        </div>
        <dx:XpoDataSource ID="XpoDSUsers" runat="server" ServerMode="True"
            DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users" Criteria="Not [<KVPSkupina_Users>][^.Id=IdUser] and [Deleted]=0">
        </dx:XpoDataSource>
    </div>
</asp:Content>
