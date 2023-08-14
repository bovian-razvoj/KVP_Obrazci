<%@ Page Title="Zaposleni" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="EmployeesSync.aspx.cs" Inherits="KVP_Obrazci.Employees.EmployeesSync" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function ClearMemoOnselect(s,e)
        {
            clientMemoNotes.SetText("");
        }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <div class="row2 align-item-centerV-startH">
                        <div class="col-xs-6 no-padding-left">
                            <h4 class="panel-title" style="display: inline-block;">Zaposleni</h4>
                        </div>
                        <div class="col-xs-6 no-padding-right">
                            <a data-toggle="collapse" href="#employees"></a>
                        </div>
                    </div>
                </div>
                <div id="employees" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <div class="row small-padding-bottom">
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center; justify-content: flex-start;">
                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" ForeColor ="Red" Font-Bold="true" Text="ZAPOSLEN : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-7 no-padding-left">
                                        <dx:ASPxGridLookup ID="ASPxGridLookupEmployee" runat="server" ClientInstanceName="lookUpEmployee"
                                            KeyFieldName="Id" TextFormatString="{1} - {2} {3}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                            OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" IncrementalFilteringMode="Contains">
                                            <ClearButton DisplayMode="OnHover" />
                                            <ClientSideEvents  ValueChanged="ClearMemoOnselect" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <GridViewStyles>
                                                <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                            </GridViewStyles>
                                            <GridViewProperties>
                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                                <SettingsPager PageSize ="50" ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                    ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                            </GridViewProperties>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Zaposlen Id" FieldName="Id" Width="80px"
                                                    ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Šifra" FieldName="ExternalId" Width="120px"
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
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <dx:ASPxLabel runat="server" ID="lblSyncWith" Text="sinhroniziraj z"></dx:ASPxLabel>
                            </div>
                            <div class="col-md-4">
                                <div class="row2" style="align-items: center; justify-content: center;">
                                    <div class="col-sm-0 big-margin-r">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" ForeColor ="Green" Font-Bold="true" Text="ZAPOSLEN : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col-sm-7 no-padding-left">
                                        <dx:ASPxGridLookup ID="ASPxGridLookupEmployee2" runat="server" ClientInstanceName="lookUpEmployee2"
                                            KeyFieldName="Id" TextFormatString="{1} - {2} {3}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                            OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" IncrementalFilteringMode="Contains">
                                            <ClearButton DisplayMode="OnHover" />
                                            
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <GridViewStyles>
                                                <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                            </GridViewStyles>
                                            <GridViewProperties>
                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                                <SettingsPager PageSize ="50" ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                    ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                            </GridViewProperties>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Zaposlen Id" FieldName="Id" Width="80px"
                                                    ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Šifra" FieldName="ExternalId" Width="120px"
                                                    ReadOnly="true" Visible="true" ShowInCustomizationForm="True" VisibleIndex="0">
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
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <dx:ASPxButton ID="btnSync" runat="server" Text="Sinhroniziraj" AutoPostBack="false"
                                    Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnSync_Click">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                </dx:ASPxButton>
                            </div>
                        </div>

                        <div class="row small-padding-bottom">
                            <div class="col-md-6">
                                <dx:ASPxMemo ID="memNotes" runat="server" Width="100%" MaxLength="5000"
                                    NullText="Podroben opis..." Rows="16" HorizontalAlign="Left" BackColor="White"
                                    CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoNotes">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxMemo>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
                DefaultSorting="ExternalId DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
            </dx:XpoDataSource>
        </div>
    </div>

</asp:Content>
