<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="KVPArchive.aspx.cs" Inherits="KVP_Obrazci.Admin.KVPArchive" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div class="row">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">KVP Arhiv</h4>
                    <a data-toggle="collapse" data-target="#kvpArchive"></a>
                </div>
                <div id="kvpArchive" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <dx:ASPxGridView ID="ASPxGridViewKVPDocumentArchive" runat="server" AutoGenerateColumns="False"
                            EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridKVPDocumentArchive"
                            Theme="Moderno" Width="100%" KeyboardSupport="true" AccessKey="G"
                            KeyFieldName="idKVPDocumentArh" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                            DataSourceID="XpoDSKVPDocumentArchive">
                            <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                VerticalScrollableHeight="600" HorizontalScrollBarMode="Auto"
                                VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="false" />
                            <SettingsPager PageSize="50" ShowNumericButtons="true">
                                <PageSizeItemSettings Visible="true" Items="50,70,90" Caption="Zapisi na stran : " AllItemText="Vsi">
                                </PageSizeItemSettings>
                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                            </SettingsPager>
                            <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" AutoFilterRowInputDelay ="8000" />
                            <Styles Header-Wrap="True">
                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                            </Styles>
                            <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                            <SettingsBehavior AllowEllipsisInText="true" />
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPDocumentArh" Width="80px"
                                    ReadOnly="true" ShowInCustomizationForm="True">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Številka KVP" FieldName="StevilkaKVP" Width="20%"
                                    ReadOnly="true" ShowInCustomizationForm="True">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="KVP Skupina"
                                    FieldName="KVPSKupina" ShowInCustomizationForm="True"
                                    Width="15%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Datum vnosa"
                                    FieldName="DatumVnosa" ShowInCustomizationForm="True"
                                    Width="15%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Predlagatelj"
                                    FieldName="Predlagatelj" ShowInCustomizationForm="True"
                                    Width="20%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="OpisProblem"
                                    FieldName="OpisProblem" ShowInCustomizationForm="True"
                                    Width="40%">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                            </Columns>
                            <Templates>
                                <DetailRow>
                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-startH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Vodja odobritve: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%#(Eval("VodjaZaOdobritevIdeje") == DBNull.Value || Eval("VodjaZaOdobritevIdeje") == "") ? " / " : Eval("VodjaZaOdobritevIdeje") %>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-centerH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Presoja: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("Presoja") == DBNull.Value || Eval("Presoja") == "") ? " / " : Eval("Presoja")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-endH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Realizator: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("Realizator") == DBNull.Value || Eval("Realizator") == "") ? " / " : Eval("Realizator")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-12">
                                            <div class="row2 align-item-centerV-startH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Opis problema: 
                                                </div>
                                                <div class="col-sm-11 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("OpisProblem") == DBNull.Value || Eval("OpisProblem") == "") ? " / " : Eval("OpisProblem")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-startH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Zaključena ideja: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("DatumZakljuceneIdeje") == DBNull.Value || Eval("DatumZakljuceneIdeje") == "") ? " / " : Eval("DatumZakljuceneIdeje")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-centerH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Sprejel: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("Sprejel") == DBNull.Value || Eval("Sprejel") == "") ? " / " : Eval("Sprejel")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-endH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Realiziral: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("Realiziral") == DBNull.Value || Eval("Realiziral") == "") ? " / " : Eval("Realiziral")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>

                                    <div class="row small-padding-bottom">
                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-startH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Zavrnil: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("Zavrnil") == DBNull.Value || Eval("Zavrnil") == "") ? " / " : Eval("Zavrnil")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="row2 align-item-centerV-centerH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Tip ideje: 
                                                </div>
                                                <div class="col-sm-4 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("TipIdeje") == DBNull.Value || Eval("TipIdeje") == "") ? " / " : Eval("TipIdeje")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="row2 align-item-centerV-startH">
                                                <div class="col-sm-0 big-margin-r">
                                                    Opombe: 
                                                </div>
                                                <div class="col-sm-11 no-padding-left">
                                                    <div class="well well-sm no-margin-b">
                                                        <dx:ASPxLabel runat="server" Text='<%# (Eval("Opombe") == DBNull.Value || Eval("Opombe") == "") ? " / " : Eval("Opombe")%>' Font-Bold="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </DetailRow>
                            </Templates>
                            <SettingsDetail ShowDetailRow="true" />
                        </dx:ASPxGridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <dx:XpoDataSource ID="XpoDSKVPDocumentArchive" runat="server" ServerMode="True"
        DefaultSorting="idKVPDocumentArh" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPDocumentArh">
    </dx:XpoDataSource>
</asp:Content>
