<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Department.aspx.cs" Inherits="KVP_Obrazci.Department.Department" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function RowDoubleClick(s, e) {

            clientCallbackPanelDepartment.PerformCallback('OpenPopup');
        }

        function OnClosePopUpHandler(command, sender, url) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Department':
                            clientPopUpDepartments.Hide();
                            gridDepartment.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'Department':
                            clientPopUpDepartments.Hide();
                    }
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanelDepartment" runat="server" Width="100%" ClientInstanceName="clientCallbackPanelDepartment" OnCallback="CallbackPanelDepartment_Callback">
        <PanelCollection>
            <dx:PanelContent>
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
                                    <dx:ASPxGridView ID="ASPxGridViewDepartment" runat="server" AutoGenerateColumns="False" Settings-ShowHeaderFilterButton="true"
                                        EnableTheming="True" EnableCallbackCompression="true" ClientInstanceName="gridDepartment"
                                        Width="100%" KeyboardSupport="true" AccessKey="G"
                                        KeyFieldName="Id" Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Paddings-PaddingLeft="3px" Paddings-PaddingRight="3px"
                                        DataSourceID="XpoDSDepartments" OnCustomUnboundColumnData="ASPxGridViewDepartment_CustomUnboundColumnData">
                                        <ClientSideEvents RowDblClick="RowDoubleClick" />
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
                                        <SettingsText EmptyDataRow="Trenutno ni podatka o oddelkih."
                                            CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" />
                                        <%--<SettingsEditing Mode="Batch" BatchEditSettings-StartEditAction="Click" />--%>
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                        <SettingsBehavior AllowEllipsisInText="true" />
                                        <Columns>
                                            <%--<dx:GridViewDataTextColumn Caption="ID" FieldName="Id" Width="80px"
                                    ReadOnly="true" Visible="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                </dx:GridViewDataTextColumn>--%>

                                            <dx:GridViewDataTextColumn Caption="Deleted" FieldName="Deleted" Width="15%"
                                                ReadOnly="true" ShowInCustomizationForm="True" EditFormSettings-Visible="False">
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Ime skupine"
                                                FieldName="Name" ShowInCustomizationForm="True"
                                                Width="25%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Polno ime skupine"
                                                FieldName="FullName" ShowInCustomizationForm="True"
                                                Width="25%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Koda" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                                FieldName="Code" ShowInCustomizationForm="True"
                                                Width="20%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Vodja"
                                                UnboundType="String" FieldName="DepartmentHeadName" ShowInCustomizationForm="True"
                                                Width="25%">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="DepartmentHeadId" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                                FieldName="DepartmentHeadId" ShowInCustomizationForm="True" Visible="false"
                                                Width="20%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Namestnik vodje"
                                                UnboundType="String" FieldName="DepartmentHeadDeputyName" ShowInCustomizationForm="True"
                                                Width="25%">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="DepartmentHeadDeputyId" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                                FieldName="DepartmentHeadDeputyId" ShowInCustomizationForm="True" Visible="false"
                                                Width="20%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="Nadrejeni oddelek"
                                                UnboundType="String" FieldName="DepartmentSupName" ShowInCustomizationForm="True"
                                                Width="25%">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn Caption="ParentId" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList"
                                                FieldName="ParentId" ShowInCustomizationForm="True" Visible="false"
                                                Width="20%" EditFormSettings-Visible="False">
                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                        </Columns>
                                    </dx:ASPxGridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <dx:ASPxPopupControl ID="ASPxPopupControlDepartment" runat="server"
                    ClientInstanceName="clientPopUpDepartments" Modal="True" HeaderText="UREJANJE ODDELKA" ContentUrl="Department_popup.aspx"
                    CloseAction="CloseButton" Width="710px" Height="400px" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                    AllowResize="true" ShowShadow="true"
                    OnWindowCallback="ASPxPopupControlDepartment_WindowCallback">
                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                    <ContentStyle BackColor="#F7F7F7">
                        <Paddings Padding="0px"></Paddings>
                    </ContentStyle>
                </dx:ASPxPopupControl>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:XpoDataSource ID="XpoDSDepartments" runat="server" ServerMode="True"
        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Departments">
    </dx:XpoDataSource>

    <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
        DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
    </dx:XpoDataSource>
</asp:Content>
