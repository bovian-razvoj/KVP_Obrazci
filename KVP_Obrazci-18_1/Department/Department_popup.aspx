<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PopUp.Master" AutoEventWireup="true" CodeBehind="Department_popup.aspx.cs" Inherits="KVP_Obrazci.Department.Department_popup" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function bntPopUpConfirm_Click(s, e) {
            var lookupItems = [lookUpDepartmentHead];//, lookUpDepartmentHeadDeputy, lookUpDepartment
            var process = InputFieldsValidation(lookupItems, null, null, null, null, null);

            if (process)
                e.processOnServer = true;
            else
                e.processOnServer = false;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row2 small-padding-bottom small-padding-top" style="align-items: center">
        <div class="col-xs-3 no-padding-right">
            <dx:ASPxLabel ID="ASPxLabel16" runat="server" Font-Size="12px" Text="SKUPINA : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-9 no-padding-left">
            <dx:ASPxTextBox runat="server" ID="txtGroupName" ClientInstanceName="clientTxtGroupName"
                CssClass="text-box-input" Font-Size="13px" Width="100%" ClientEnabled="false" ReadOnly="true"
                BackColor="LightGray" Font-Bold="true">
                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
            </dx:ASPxTextBox>
        </div>
    </div>
    <div class="row2 small-padding-bottom align-item-centerV-startH">
        <div class="col-xs-3 no-padding-right">
            <dx:ASPxLabel ID="ASPxLabel23" runat="server" Font-Size="12px" Text="VODJA ODDLEKA : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-8 no-padding-left">
            <dx:ASPxGridLookup ID="ASPxGridLookupDepartmentHead" runat="server" ClientInstanceName="lookUpDepartmentHead"
                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
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
                    <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" Width="80px"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="30%"
                        ReadOnly="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Priimek"
                        FieldName="Lastname" ShowInCustomizationForm="True"
                        Width="30%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <%--<dx:GridViewDataTextColumn Caption="Email"
                                    FieldName="Email" ShowInCustomizationForm="True"
                                    Width="25%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>--%>

                    <dx:GridViewDataTextColumn Caption="Oddelek"
                        FieldName="DepartmentId.Name" ShowInCustomizationForm="True"
                        Width="20%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Vloga"
                        FieldName="RoleID.Naziv" ShowInCustomizationForm="True"
                        Width="20%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>

    <div class="row2 small-padding-bottom align-item-centerV-startH">
        <div class="col-xs-3 no-padding-right">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="NAMESTNIK VODJE : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-8 no-padding-left">
            <dx:ASPxGridLookup ID="ASPxGridLookupDepartmentHeadDeputy" runat="server" ClientInstanceName="lookUpDepartmentHeadDeputy"
                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
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
                    <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" Width="80px"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True" VisibleIndex="0">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="30%"
                        ReadOnly="true" ShowInCustomizationForm="True">
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Priimek"
                        FieldName="Lastname" ShowInCustomizationForm="True"
                        Width="30%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <%--<dx:GridViewDataTextColumn Caption="Email"
                                    FieldName="Email" ShowInCustomizationForm="True"
                                    Width="25%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Oddelek"
                                    FieldName="DepartmentId.Name" ShowInCustomizationForm="True"
                                    Width="15%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>--%>

                    <dx:GridViewDataTextColumn Caption="Vloga"
                        FieldName="RoleID.Naziv" ShowInCustomizationForm="True"
                        Width="15%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>

    <div class="row2 small-padding-bottom align-item-centerV-startH">
        <div class="col-xs-3 no-padding-right">
            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="NADREJENI ODDELEK : "></dx:ASPxLabel>
        </div>
        <div class="col-xs-8 no-padding-left">
            <dx:ASPxGridLookup ID="ASPxGridLookupDepartment" runat="server" ClientInstanceName="lookUpDepartment"
                KeyFieldName="Id" TextFormatString="{1}" CssClass="text-box-input"
                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                OnLoad="ASPxGridLookupLoad_WidthLarge" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                IncrementalFilteringMode="Contains" OnDataBinding="ASPxGridLookupDepartment_DataBinding">
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
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="Id" Width="80px"
                        ReadOnly="true" Visible="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                    </dx:GridViewDataTextColumn>

                    <%-- <dx:GridViewDataTextColumn Caption="Deleted" FieldName="Deleted" Width="15%"
                                    ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                </dx:GridViewDataTextColumn>--%>

                    <dx:GridViewDataTextColumn Caption="Ime skupine"
                        FieldName="Name" ShowInCustomizationForm="True"
                        Width="25%" EditFormSettings-Visible="False">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <%--<dx:GridViewDataTextColumn Caption="Polno ime skupine"
                                    FieldName="FullName" ShowInCustomizationForm="True"
                                    Width="25%" EditFormSettings-Visible="False">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Koda" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                    FieldName="Code" ShowInCustomizationForm="True"
                                    Width="20%" EditFormSettings-Visible="False">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>--%>

                    <dx:GridViewDataTextColumn Caption="Vodja oddelka" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                        FieldName="DepartmentHeadId" ShowInCustomizationForm="True"
                        Width="20%" EditFormSettings-Visible="False" Visible="false">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Vodja"
                        UnboundType="String" FieldName="DepartmentHeadName" ShowInCustomizationForm="True"
                        Width="25%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Namestnik vodje oddelka" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                        FieldName="DepartmentHeadDeputyId" ShowInCustomizationForm="True"
                        Width="20%" EditFormSettings-Visible="False" Visible="false">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Namestnik vodje"
                        UnboundType="String" FieldName="DepartmentHeadDeputyName" ShowInCustomizationForm="True"
                        Width="25%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="ParentId" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                        FieldName="ParentId" ShowInCustomizationForm="True"
                        Width="20%" EditFormSettings-Visible="False" Visible="false">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Nadrejeni oddelek"
                        UnboundType="String" FieldName="DepartmentSupName" ShowInCustomizationForm="True"
                        Width="25%">
                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridLookup>
        </div>
    </div>

    <div class="row2 small-padding-top small-padding-bottom">
        <div class="col-xs-12" style="align-items: flex-end">
            <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Potrdi" AutoPostBack="false"
                ClientInstanceName="clientBtnConfirm" UseSubmitBehavior="false"
                Width="100px" OnClick="btnConfirmPopUp_Click">
                <ClientSideEvents Click="bntPopUpConfirm_Click" />
            </dx:ASPxButton>
        </div>
    </div>

    <dx:XpoDataSource ID="XpoDSDepartments" runat="server" ServerMode="True"
        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Departments">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
    </dx:XpoDataSource>
</asp:Content>
