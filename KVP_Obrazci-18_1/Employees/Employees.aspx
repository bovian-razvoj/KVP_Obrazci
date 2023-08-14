<%@ Page Title="Zaposleni" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="KVP_Obrazci.Employees.Employees" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function RowDoubleClick(s, e) {
            gridEmployees.GetRowValues(gridEmployees.GetFocusedRowIndex(), 'Id', OnGetRowValues);
        }
        function OnGetRowValues(value) {
            gridEmployees.PerformCallback('DblClick;' + value);
        }

        function gridEmployees_OnSelectionChanged(s, e) {
            if (s.GetSelectedRowCount() > 0)
                clientBtnConfirmChanges.SetVisible(true);
            else {
                clientBtnConfirmChanges.SetVisible(false);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Zaposleni</h4>
                    <a data-toggle="collapse" data-target="#employees"
                        href="#collapseOne"></a>
                </div>
                <div id="employees" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <dx:ASPxGridView ID="ASPxGridViewEmployees" runat="server" AutoGenerateColumns="False" Settings-ShowHeaderFilterButton="true" 
                            EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridEmployees"
                            Width="100%" KeyboardSupport="true" AccessKey="G"
                            KeyFieldName="Id" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                            DataSourceID="XpoDSEmployees" OnCellEditorInitialize="ASPxGridViewEmployees_CellEditorInitialize"
                            OnBatchUpdate="ASPxGridViewEmployees_BatchUpdate" OnCustomCallback="ASPxGridViewEmployees_CustomCallback"
                            OnAutoFilterCellEditorInitialize="ASPxGridViewEmployees_AutoFilterCellEditorInitialize">
                            <ClientSideEvents RowDblClick="RowDoubleClick" SelectionChanged="gridEmployees_OnSelectionChanged" />
                            <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                VerticalScrollableHeight="500" HorizontalScrollBarMode="Auto"
                                VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="false" />
                            <SettingsPager PageSize="50" ShowNumericButtons="true">
                                <PageSizeItemSettings Visible="true" Items="50,70,90" Caption="Zapisi na stran : " AllItemText="Vsi">
                                </PageSizeItemSettings>
                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                            </SettingsPager>
                            <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" />
                            <Styles Header-Wrap="True">
                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                            </Styles>
                            <SettingsText EmptyDataRow="Trenutno ni podatka o zaposlenih. Dodaj novega."
                                CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" />
                            <SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="Click" />
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                            <SettingsBehavior AllowEllipsisInText="true" />
                            <Columns>
                                
                                <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="100px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" Visible="false" />
                                
                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ExternalId" Width="80px"
                                    ReadOnly="true" Visible="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Ime" FieldName="Firstname" Width="15%"
                                    ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Priimek"
                                    FieldName="Lastname" ShowInCustomizationForm="True"
                                    Width="25%" EditFormSettings-Visible="False">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="Email"
                                    FieldName="Email" ShowInCustomizationForm="True"
                                    Width="25%" EditFormSettings-Visible="False">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                               <%-- <dx:GridViewDataTextColumn Caption="Št. kartice"
                                    FieldName="Card" ShowInCustomizationForm="True"
                                    Width="25%" EditFormSettings-Visible="False">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>--%>

                                <dx:GridViewDataTextColumn Caption="Oddelek"  Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                    FieldName="DepartmentId.Name" ShowInCustomizationForm="True"
                                    Width="20%" EditFormSettings-Visible="False">
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataComboBoxColumn Caption="Vloga" SettingsHeaderFilter-Mode="CheckedList" FieldName="RoleID.Naziv" Width="200px">
                                    <Settings AllowAutoFilter="False" />
                                </dx:GridViewDataComboBoxColumn>

                                <dx:GridViewDataCheckColumn Caption="Upravičen do KVP" FieldName="UpravicenDoKVP" Width="110px">
                                    <BatchEditModifiedCellStyle Paddings-Padding="0"></BatchEditModifiedCellStyle>
                                    <CellStyle Paddings-Padding="0"></CellStyle>
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataCheckColumn>

                                <dx:GridViewDataCheckColumn Caption="Odstranjeni" FieldName="Deleted" Width="120px"
                                    EditFormSettings-Visible="False">
                                    <BatchEditModifiedCellStyle Paddings-Padding="0"></BatchEditModifiedCellStyle>
                                    <CellStyle Paddings-Padding="0"></CellStyle>
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataCheckColumn>

                                <dx:GridViewDataCheckColumn Caption="Upravičen do E-pošte" FieldName="UpravicenDoEPoste" Width="120px">
                                    <BatchEditModifiedCellStyle Paddings-Padding="0"></BatchEditModifiedCellStyle>
                                    <CellStyle Paddings-Padding="0"></CellStyle>
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataCheckColumn>

                                <dx:GridViewDataCheckColumn Caption="Dostop do eKVP" FieldName="eKVPPrijava" Width="120px">
                                    <BatchEditModifiedCellStyle Paddings-Padding="0"></BatchEditModifiedCellStyle>
                                    <CellStyle Paddings-Padding="0"></CellStyle>
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataCheckColumn>

                                <dx:GridViewDataCheckColumn Caption="Niso zaposleni več" FieldName="NotEmployedAnymore" Width="120px"
                                    EditFormSettings-Visible="False" Visible="false">
                                    <CellStyle Paddings-Padding="0"></CellStyle>
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataCheckColumn>

                                <dx:GridViewDataCheckColumn Caption="Sprememba imena" FieldName="NameChanged" Width="120px"
                                    EditFormSettings-Visible="False" Visible="false">
                                    <CellStyle Paddings-Padding="0"></CellStyle>
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataCheckColumn>

                                <dx:GridViewDataCheckColumn Caption="Zaposlen podvojen" FieldName="IsDuplicated" Width="120px"
                                    EditFormSettings-Visible="False" Visible="false">
                                    <CellStyle Paddings-Padding="0"></CellStyle>
                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                </dx:GridViewDataCheckColumn>
                            </Columns>
                            <%--<ClientSideEvents BatchEditEndEditing="OnBatchEditEndEditing" EndCallback="EndCallback_gridSelectedPositions" />--%>
                        </dx:ASPxGridView>
                    </div>
                </div>
            </div>
            <dx:XpoDataSource ID="XpoDSEmployees" runat="server" ServerMode="True"
                DefaultSorting="ExternalId DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
            </dx:XpoDataSource>
            <dx:XpoDataSource ID="XpoDSRoles" runat="server" ServerMode="True"
                DefaultSorting="VlogaID" TypeName="KVP_Obrazci.Domain.KVPOdelo.Vloga">
            </dx:XpoDataSource>
        </div>
    </div>
    <div class="row2">
        <div class="col-xs-12 align-item-centerV-centerH">
            <div class="AddEditButtonsWrap">
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnConfirmChanges" runat="server" Text="Potrdi spremembe" AutoPostBack="false"
                        Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirmChanges"
                        UseSubmitBehavior="false" ClientVisible="false" OnClick="btnConfirmChanges_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>
                </span>
                <span class="AddEditButtons">
                    <dx:ASPxButton Theme="Moderno" ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                        Height="30" Width="50" ValidationGroup="Confirm"
                        UseSubmitBehavior="false" OnClick="btnDelete_Click" ClientVisible="false">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>
                </span>
                <div class="AddEditButtonsElements clearFloatBtns">
                    <span class="AddEditButtons">
                        <dx:ASPxButton Theme="Moderno" ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                            Height="30" Width="50" ValidationGroup="Confirm"
                            UseSubmitBehavior="false" OnClick="btnAdd_Click" ClientVisible="false">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                        </dx:ASPxButton>
                    </span>
                    <span class="AddEditButtons">
                        <dx:ASPxButton Theme="Moderno" ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                            Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnEdit_Click">
                            <Paddings PaddingLeft="10" PaddingRight="10" />
                        </dx:ASPxButton>
                    </span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
