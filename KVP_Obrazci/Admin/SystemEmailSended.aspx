<%@ Page Title="Poslana e-pošta" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="SystemEmailSended.aspx.cs" Inherits="KVP_Obrazci.Admin.SystemEmailSended" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/PopUp.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function RowDoubleClick(s, e) {
            gridSystemEmailMessage.GetRowValues(gridSystemEmailMessage.GetFocusedRowIndex(), 'SystemEmailMessageID;EmailBody', OnGetRowValuesAuditorKVPs);
        }

        function OnGetRowValuesAuditorKVPs(value) {
            clientSytemEmailMessageCallbackPanel.PerformCallback('DblClickShowEmailBody|' + value[0] + "|" + value[1]);
        }


        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'SystemEmailBody':
                            clientPopUpAuditors.Hide();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'SystemEmailBody':
                            clientPopUpAuditors.Hide();
                    }
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-xs-12">
            <div class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    <h4 class="panel-title" style="display: inline-block;">Pregled poslane pošte</h4>
                    <a data-toggle="collapse" data-target="#employees"
                        href="#collapseOne"></a>
                </div>
                <div id="employees" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <dx:ASPxCallbackPanel ID="SytemEmailMessageCallbackPanel" runat="server" OnCallback="SytemEmailMessageCallbackPanel_Callback" ClientInstanceName="clientSytemEmailMessageCallbackPanel">
                            <PanelCollection>
                                <dx:PanelContent>

                                    <dx:ASPxGridView ID="ASPxGridViewSystemEmailMessage" runat="server" AutoGenerateColumns="False" Settings-ShowHeaderFilterButton="true"
                                        EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridSystemEmailMessage"
                                        Width="100%" KeyboardSupport="true" AccessKey="G"
                                        KeyFieldName="SystemEmailMessageID" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                                        DataSourceID="XpoDSSystemEmailMessage" OnCustomColumnDisplayText="ASPxGridViewSystemEmailMessage_CustomColumnDisplayText"
                                        OnHtmlDataCellPrepared="ASPxGridViewSystemEmailMessage_HtmlDataCellPrepared">
                                        <ClientSideEvents RowDblClick="RowDoubleClick" />
                                        <Settings ShowVerticalScrollBar="True" ShowFilterBar="Auto" ShowFilterRow="True"
                                            VerticalScrollableHeight="500" HorizontalScrollBarMode="Auto"
                                            VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" ShowFooter="false" />
                                        <SettingsPager PageSize="50" ShowNumericButtons="true">
                                            <PageSizeItemSettings Visible="true" Items="50,70,90" Caption="Zapisi na stran : " AllItemText="Vsi">
                                            </PageSizeItemSettings>
                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                        </SettingsPager>
                                        <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" AllowSort="true" AutoFilterRowInputDelay="8000" />
                                        <Styles Header-Wrap="True">
                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                        </Styles>
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o sistemskih e-poštah." />
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                        <SettingsBehavior AllowEllipsisInText="true" />
                                        <Columns>

                                            <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="100px" SelectAllCheckboxMode="AllPages" Caption="Izberi" ShowClearFilterButton="true" Visible="false" />

                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="SystemEmailMessageID" Width="80px"
                                                ReadOnly="true" Visible="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Pošiljatelj" FieldName="EmailFrom" Width="15%"
                                                ReadOnly="true" ShowInCustomizationForm="True" Visible="False">
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Naslovnik"
                                                FieldName="EmailTo" ShowInCustomizationForm="True"
                                                Width="25%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Naslov e-pošte"
                                                FieldName="EmailSubject" ShowInCustomizationForm="True"
                                                Width="25%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Status" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                                FieldName="Status" ShowInCustomizationForm="True"
                                                Width="150px" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataDateColumn Caption="Datum vnosa"
                                                FieldName="ts" ShowInCustomizationForm="True"
                                                Width="180px" ExportWidth="120">
                                                <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataDateColumn>

                                            <dx:GridViewDataTextColumn Caption="Telo"
                                                FieldName="EmailBody" ShowInCustomizationForm="True"
                                                Width="25%" Visible="false">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                        </Columns>

                                    </dx:ASPxGridView>

                                    <dx:XpoDataSource ID="XpoDSSystemEmailMessage" runat="server" ServerMode="True"
                                        DefaultSorting="SystemEmailMessageID DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.SystemEmailMessage">
                                    </dx:XpoDataSource>

                                    <dx:ASPxPopupControl ID="ASPxPopupControlSystemEmailBody" runat="server" ContentUrl="SystemEmailBody_popup.aspx"
                                        ClientInstanceName="clientPopUpAuditors" Modal="True" HeaderText="TELO E-POŠTE"
                                        CloseAction="CloseButton" Width="750px" Height="510px" PopupHorizontalAlign="WindowCenter"
                                        PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                        AllowResize="true" ShowShadow="true"
                                        OnWindowCallback="ASPxPopupControlSystemEmailBody_WindowCallback">
                                        <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                        <ContentStyle BackColor="#F7F7F7">
                                            <Paddings Padding="0px"></Paddings>
                                        </ContentStyle>
                                    </dx:ASPxPopupControl>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
