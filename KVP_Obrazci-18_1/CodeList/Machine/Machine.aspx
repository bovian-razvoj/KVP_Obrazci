<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Machine.aspx.cs" Inherits="KVP_Obrazci.CodeList.Machine.Machine" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function RowDoubleClick(s, e) {

        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Machine':
                            clientPopUpMachine.Hide();
                            gridMachine.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (KVPType) {
                        case 'Machine':
                            clientPopUpMachine.Hide();
                    }
                    break;
            }
        }

        function btnDelete_Click(s, e) {
            clientCallbackPanelMachine.PerformCallback('DeleteMachine');
        }

        function btnAdd_Click(s, e) {
            clientCallbackPanelMachine.PerformCallback('AddMachine');
            //clientPopUpMachine.Show();
        }

        function gridMachine_BatchEditRowValidating(s, e) {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelMachine" runat="server" Width="100%" OnCallback="CallbackPanelMachine_Callback" ClientInstanceName="clientCallbackPanelMachine">
        <PanelCollection>
            <dx:PanelContent>
                <div class="row2">
                    <div class="col-xs-6">
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-heading">
                                <h4 class="panel-title" style="display: inline-block;">Stroji</h4>
                                <a data-toggle="collapse" data-target="#Machine"
                                    href="#collapseOne"></a>
                            </div>
                            <div id="Machine" class="panel-collapse collapse in">
                                <div class="panel-body">
                                    <dx:ASPxGridView ID="ASPxGridViewMachine" runat="server" AutoGenerateColumns="False" Settings-ShowHeaderFilterButton="true"
                                        EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridMachine"
                                        Width="100%" KeyboardSupport="true" AccessKey="G"
                                        KeyFieldName="idStroj" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                                        DataSourceID="XpoDSMachine"  OnBatchUpdate="ASPxGridViewMachine_BatchUpdate">
                                        <ClientSideEvents RowDblClick="RowDoubleClick" BatchEditRowValidating="gridMachine_BatchEditRowValidating" />
                                        <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                            VerticalScrollableHeight="350" HorizontalScrollBarMode="Auto"
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
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o lokacijah. Dodaj novo."
                                            CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" />
                                        <SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="DblClick" />
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                        <SettingsBehavior AllowEllipsisInText="true" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="idMachine" Width="80px"
                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Sort" Settings-AllowHeaderFilter="True"
                                                FieldName="Sort" ShowInCustomizationForm="True"
                                                Width="20%">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                <PropertiesTextEdit>
                                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true">
                                                        <RegularExpression ErrorText="Vpiši sort" ValidationExpression="^[0-9]+$" />
                                                    </ValidationSettings>
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Stroškovno mesto" Settings-AllowHeaderFilter="True"
                                                FieldName="Koda" ShowInCustomizationForm="True"
                                                Width="20%">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                <PropertiesTextEdit MaxLength="50">
                                                    <ValidationSettings  Display="Dynamic" RequiredField-IsRequired="true">
                                                    </ValidationSettings>
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Opis" FieldName="Opis" Width="60%" ShowInCustomizationForm="True">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                <PropertiesTextEdit MaxLength="150">
                                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" >
                                                    </ValidationSettings>
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>

                                        </Columns>

                                    </dx:ASPxGridView>

                                    <dx:ASPxPopupControl ID="ASPxPopupControlMachine" runat="server" ContentUrl="Machine_popup.aspx"
                                        ClientInstanceName="clientPopUpMachine" Modal="True" HeaderText="STROJ"
                                        CloseAction="CloseButton" Width="680px" Height="270px" PopupHorizontalAlign="WindowCenter"
                                        PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                        AllowResize="true" ShowShadow="true"
                                        OnWindowCallback="ASPxPopupControlMachine_WindowCallback">
                                        <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                        <ContentStyle BackColor="#F7F7F7">
                                            <Paddings Padding="0px"></Paddings>
                                        </ContentStyle>
                                    </dx:ASPxPopupControl>
                                </div>
                            </div>
                        </div>
                        <dx:XpoDataSource ID="XpoDSMachine" runat="server" ServerMode="True"
                            DefaultSorting="idStroj" TypeName="KVP_Obrazci.Domain.KVPOdelo.Stroj">
                        </dx:XpoDataSource>

                    </div>
                </div>
                <div class="row2">
                    <div class="col-xs-6 align-item-centerV-centerH">
                        <div class="AddEditButtonsWrap" style="width: 100%;">
                            <span class="AddEditButtons">
                                <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false"
                                    Height="30" Width="50"
                                    UseSubmitBehavior="false">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <Image Url="../../Images/trash.png" UrlHottracked="../../Images/trashHover.png" />
                                    <ClientSideEvents Click="btnDelete_Click" />
                                </dx:ASPxButton>
                            </span>
                            <div class="AddEditButtonsElements clearFloatBtns">
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false"
                                        Height="30" Width="50"
                                        UseSubmitBehavior="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                        <ClientSideEvents Click="btnAdd_Click" />
                                    </dx:ASPxButton>
                                </span>
                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../../Images/edit.png" UrlHottracked="../../Images/editHover.png" />
                                    </dx:ASPxButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
