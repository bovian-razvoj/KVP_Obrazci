<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="KVP_Obrazci.Posts.News" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function clientCardViewPosts_CardDblClick(s, e) {
            clientCardViewPosts.PerformCallback('CardDoubleClick');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Novice</h4>
                    <a data-toggle="collapse" data-target="#employees"
                        href="#collapseOne"></a>
                </div>
                <div id="employees" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <dx:ASPxCardView ID="ASPxCardViewPosts" runat="server" Width="100%" DataSourceID="XPODSPosts" EnableCardsCache="false" ClientInstanceName="clientCardViewPosts"
                            KeyFieldName="PrispevkiID" AutoGenerateColumns="False" OnCustomCallback="ASPxCardViewPosts_CustomCallback" OnHtmlCardPrepared="ASPxCardViewPosts_HtmlCardPrepared"
                            OnCustomColumnDisplayText="ASPxCardViewPosts_CustomColumnDisplayText">
                            <ClientSideEvents CardDblClick="clientCardViewPosts_CardDblClick" />
                            <Paddings Padding="0" />
                            <Settings ShowFilterBar="Auto" VerticalScrollableHeight="500"
                                VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" LayoutMode="table" />
                            <SettingsPager ShowNumericButtons="true">
                                <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                </PageSizeItemSettings>
                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                            </SettingsPager>
                            <SettingsPopup>
                                <HeaderFilter Height="150px" />
                            </SettingsPopup>
                            <SettingsBehavior AllowEllipsisInText="true" AllowFocusedCard="true" AllowSelectByCardClick="true" AllowSelectSingleCardOnly="true" />
                            <Styles Header-Wrap="True">
                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                <Card Height="1px" />
                                <FocusedCard CssClass="focus-text-box-input">                                    
                                </FocusedCard>
                            </Styles>     
                                                   
                            <Columns>
                                <dx:CardViewTextColumn FieldName="PrispevkiID" Caption="PrispevkiID" />
                                <dx:CardViewTextColumn FieldName="Objavljen" Caption="Objavljeno" />
                                <dx:CardViewTextColumn FieldName="KategorijaID.Naziv" Caption="Kategorija" />
                                <dx:CardViewTextColumn FieldName="AvtorID.Firstname" Caption="Avtor" />
                                <dx:CardViewTextColumn FieldName="Naslov" Caption="Naslov" />
                                <dx:CardViewTextColumn FieldName="Izvlecek" Caption="Izvleček" />
                                <dx:CardViewImageColumn FieldName="PrikaznaSlika" Settings-AllowSort="False">
                                    <PropertiesImage ImageWidth="150" />
                                </dx:CardViewImageColumn>
                                <dx:CardViewDateColumn FieldName="DatumVnosa" PropertiesDateEdit-DisplayFormatString="dd. MMMM yyyy - hh:mm" />
                            </Columns>
                            <CardLayoutProperties ColCount="2">
                                <Items>
                                    <dx:CardViewColumnLayoutItem ColumnName="PrikaznaSlika" ShowCaption="False" RowSpan="7" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <dx:CardViewColumnLayoutItem ColumnName="Naslov" ShowCaption="False" VerticalAlign="Top" HorizontalAlign="Center" />
                                    <dx:CardViewColumnLayoutItem ColumnName="Izvlecek" HorizontalAlign="Center" ShowCaption="False">
                                        <ParentContainerStyle Font-Bold="true" />
                                    </dx:CardViewColumnLayoutItem>
                                    <dx:EmptyLayoutItem Height="5" />
                                    <dx:CardViewColumnLayoutItem ColumnName="KategorijaID.Naziv" HorizontalAlign="Center" />
                                    <dx:CardViewColumnLayoutItem ColumnName="AvtorID.Firstname" HorizontalAlign="Center" />
                                    <dx:CardViewColumnLayoutItem ColumnName="Objavljen" VerticalAlign="Top" HorizontalAlign="Center" />
                                    <dx:CardViewColumnLayoutItem ColumnName="DatumVnosa" ShowCaption="False" HorizontalAlign="Center" />
                                </Items>
                            </CardLayoutProperties>
                            <Settings ShowHeaderPanel="true" />
                            <SettingsPager SettingsTableLayout-ColumnCount="3" SettingsTableLayout-RowsPerPage="2" EnableAdaptivity="true" />
                        </dx:ASPxCardView>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row2">
        <div class="col-xs-12 align-item-centerV-centerH">
            <div class="AddEditButtonsWrap">
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                        Height="30" Width="50" ValidationGroup="Confirm" OnClick="btnDelete_Click" UseSubmitBehavior="false">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                        <Image Url="../Images/trash.png" UrlHottracked="../Images/trashHover.png" />
                    </dx:ASPxButton>
                </span>
                <div class="AddEditButtonsElements clearFloatBtns big-margin-l">
                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                            Height="30" Width="50" ValidationGroup="Confirm"
                            UseSubmitBehavior="false" OnClick="btnAdd_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/add.png" UrlHottracked="../Images/addHover.png" />
                        </dx:ASPxButton>
                    </span>
                    <span class="AddEditButtons">
                        <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                            Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnEdit_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                            <Image Url="../Images/edit.png" UrlHottracked="../Images/editHover.png" />
                        </dx:ASPxButton>
                    </span>
                </div>
            </div>
        </div>
    </div>

    <dx:XpoDataSource ID="XPODSPosts" runat="server" ServerMode="true"
        DefaultSorting="PrispevkiID" TypeName="KVP_Obrazci.Domain.KVPOdelo.Prispevek">
    </dx:XpoDataSource>
</asp:Content>
