<%@ Page Title=" KVP skupine" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="KVPGroups.aspx.cs" Inherits="KVP_Obrazci.KVPGroups.KVPGroups" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function RowDoubleClick(s, e) {
            gridKVPGroups.GetRowValues(gridKVPGroups.GetFocusedRowIndex(), 'idKVPSkupina', OnGetRowValues);
        }
        function OnGetRowValues(value) {
            gridKVPGroups.PerformCallback('DblClick;' + value);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Vse KVP skupine</h4>
                    <a data-toggle="collapse" data-target="#kvpGroups"
                        href="#collapseOne"></a>
                </div>
                <div id="kvpGroups" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <dx:ASPxGridView ID="ASPxGridViewKVPGroups" runat="server" EnableCallbackCompression="true"
                            ClientInstanceName="gridKVPGroups"
                            Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                            KeyFieldName="idKVPSkupina" CssClass="gridview-no-header-padding"
                            DataSourceID="XpoDataSourceKVPGroups" OnCustomCallback="ASPxGridViewKVPGroups_CustomCallback"
                            OnDataBound="ASPxGridViewKVPGroups_DataBound">
                            <ClientSideEvents RowDblClick="RowDoubleClick" />
                            <Paddings Padding="0" />
                            <Settings ShowVerticalScrollBar="True"
                                ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400"
                                ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                            <SettingsPager PageSize="20" ShowNumericButtons="true">
                                <PageSizeItemSettings Visible="true" Items="20,40,60" Caption="Zapisi na stran : " AllItemText="Vsi">
                                </PageSizeItemSettings>
                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                            </SettingsPager>
                            <SettingsBehavior AllowFocusedRow="true" />
                            <Styles Header-Wrap="True">
                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                            </Styles>
                            <SettingsText EmptyDataRow="Trenutno ni podatka o skupinah. Dodaj novo." />
                            <Columns>

                                <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPSkupina" Width="40px"
                                    ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="15%"
                                    ReadOnly="true" ShowInCustomizationForm="True">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Naziv"
                                    FieldName="Naziv" ShowInCustomizationForm="True"
                                    Width="65%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <%--<dx:GridViewBandColumn Caption="Poterjevalec 1">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="Potrjevalec1.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" />
                                        <dx:GridViewDataColumn FieldName="Potrjevalec1.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" />
                                    </Columns>
                                </dx:GridViewBandColumn>

                                <dx:GridViewBandColumn Caption="Poterjevalec 2">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="Potrjevalec2.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" />
                                        <dx:GridViewDataColumn FieldName="Potrjevalec2.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" />
                                    </Columns>
                                </dx:GridViewBandColumn>

                                <dx:GridViewBandColumn Caption="Poterjevalec 3">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="Potrjevalec3.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" />
                                        <dx:GridViewDataColumn FieldName="Potrjevalec3.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" />
                                    </Columns>
                                </dx:GridViewBandColumn>--%>
                            </Columns>
                        </dx:ASPxGridView>
                    </div>
                </div>
                <dx:XpoDataSource ID="XpoDataSourceKVPGroups" runat="server" ServerMode="True"
                    DefaultSorting="idKVPSkupina asc" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPSkupina">
                </dx:XpoDataSource>
            </div>
        </div>
    </div>
    <div class="row2">
        <div class="col-xs-12">
            <div class="AddEditButtonsWrap">
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnAddNewKvpGroup" runat="server" Text="Nova KVP Skupina" RenderMode="Button" ImagePosition="Top"
                        AutoPostBack="false" UseSubmitBehavior="false" OnClick="btnAddNewKvpGroup_Click" Height="30" Width="50">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                        <%--<Image Url="Images/penPencile.png" UrlHottracked="Images/pencileHover.png" Width="25px" />--%>
                    </dx:ASPxButton>
                </span>
            </div>
        </div>
    </div>
</asp:Content>
