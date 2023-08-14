<%@ Page Title="Urejanje rdečega kartona" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="KVPDocumentRedCardForm.aspx.cs" Inherits="KVP_Obrazci.KVPDocuments.KVPDocumentRedCardForm" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<%@ Register TagPrefix="widget" TagName="UploadAttachment" Src="~/Widgets/UploadAttachment.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>

        /*$(document).ready(function () {

            $('.nav-tabs a').on('show.bs.tab', function (e) {
                //alert("Tab " + event.target.hash + " is about to be shown");
                //$('.nav-tabs a[href="#KVPBasicData"]').tab('show');
                switch (event.target.hash) {
                    case '#KVPHistoryStatus':
                        clientCallbackPanelKVPDocumentForm.PerformCallback('GetHistoryStatuses');
                        break;
                }
            });

        });*/

        function CauseValidation(s, e) {
            var process = false;
            //var inputItems = [clientUsername, clientPass];
            var lookupItems = [lookUpType, lookUpProposer, lookUpLeaderTeam, lookUpTipRdeciKarton, lookUpStatusRdeciKarton, lookUpDepartment];
            var memoFields = [clientMemoProblemDesc, clientMemoImprovementProposition, clientMemoSavingsOrCosts];
            process = InputFieldsValidation(lookupItems, null, null, null, null, null);

            if (process) {
                e.processOnServer = true;
            }
            else
                e.processOnServer = false;
        }

        function lookUpTipRdeciKarton_ValueChanged(s, e) {
            s.GetGridView().GetRowValues(s.GetGridView().GetFocusedRowIndex(), 'Koda', OnGetRowValues);
        }
        function OnGetRowValues(value) {
            clientCallbackPanelKVPDocumentForm.PerformCallback("TipRdeciKarton;" + value);
        }

        function EndCallback_Panel(s, e) {
            if (s.cpRepairDate != "" && s.cpRepairDate != undefined) {
                dtRepairDate.SetDate(s.cpRepairDate);
                //dtRepairDate.SetEnabled(false);
                delete (s.cpRepairDate);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row">
        <div class="col-sm-12">
            <dx:ASPxCallbackPanel ID="CallbackPanelKVPDocumentForm" runat="server" Width="100%" OnCallback="CallbackPanelKVPDocumentForm_Callback"
                ClientInstanceName="clientCallbackPanelKVPDocumentForm">
                <ClientSideEvents EndCallback="EndCallback_Panel" />
                <PanelCollection>
                    <dx:PanelContent>
                        <ul class="nav nav-tabs" runat="server" id="navTabs">
                            <li class="active" runat="server" id="basicDataItem">
                                <a data-toggle="tab" href="#KVPBasicData">Vsebina KVP - Rdeči karton</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div id="KVPBasicData" class="tab-pane fade in active">
                                <div class="panel panel-default" style="margin-top: 10px; border-color: tomato">
                                    <div class="panel-heading" style="background-color: tomato; color: white; border-color: tomato;">
                                        <h4 class="panel-title" style="display: inline-block;">Osnovni podatki</h4>
                                        <a data-toggle="collapse" data-target="#demo" class="panel-collapse-arrow"
                                            href="#collapseOne"></a>
                                    </div>
                                    <div id="demo" class="panel-collapse collapse in">
                                        <div class="panel-body">
                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: flex-start;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="DATUM VNOSA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxDateEdit ID="DateEditDatumVnosa" runat="server" EditFormat="Date" Width="100%" Theme="Moderno"
                                                                CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientEnabled="false">
                                                                <FocusedStyle CssClass="focus-text-box-input" />
                                                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                                <DropDownButton Visible="true"></DropDownButton>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: center">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 75px;">
                                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="TIP : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupType" runat="server" ClientInstanceName="lookUpType"
                                                                KeyFieldName="idTip" TextFormatString="{1}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSTip">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Ime').Focus();}" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <GridViewStyles>
                                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                </GridViewStyles>
                                                                <GridViewProperties>
                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                        ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                                </GridViewProperties>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="Tip Id" FieldName="idTip" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="40%"
                                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Točke Realizator"
                                                                        FieldName="TockeRealizator" ShowInCustomizationForm="True"
                                                                        Width="30%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Točke Predlagatelj"
                                                                        FieldName="TockePredlagatelj" ShowInCustomizationForm="True"
                                                                        Width="30%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                </Columns>
                                                            </dx:ASPxGridLookup>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: flex-end;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="ODDELEK : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-6 no-padding-left">
                                                            <div class="col-sm-6 no-padding-left">
                                                                <dx:ASPxGridLookup ID="ASPxGridLookupDepartment" runat="server" ClientInstanceName="lookUpDepartment"
                                                                    KeyFieldName="Id" TextFormatString="{1}" CssClass="text-box-input" ClientEnabled="false"
                                                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                    OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSDepartment">
                                                                    <ClearButton DisplayMode="OnHover" />
                                                                    <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Name').Focus();}" />
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                    <GridViewStyles>
                                                                        <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                        <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                    </GridViewStyles>
                                                                    <GridViewProperties>
                                                                        <SettingsBehavior EnableRowHotTrack="True" />
                                                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                            ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                                    </GridViewProperties>
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" Width="80px"
                                                                            ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                        </dx:GridViewDataTextColumn>

                                                                        <dx:GridViewDataTextColumn Caption="Ime" FieldName="Name" Width="50%"
                                                                            ReadOnly="true" ShowInCustomizationForm="True">
                                                                        </dx:GridViewDataTextColumn>

                                                                        <dx:GridViewDataTextColumn Caption="Celotno ime"
                                                                            FieldName="FullName" ShowInCustomizationForm="True"
                                                                            Width="50%">
                                                                            <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                        </dx:GridViewDataTextColumn>

                                                                    </Columns>
                                                                </dx:ASPxGridLookup>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: flex-start">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="PREDLAGATELJ : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupProposer" runat="server" ClientInstanceName="lookUpProposer"
                                                                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />
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
                                                                    <dx:GridViewDataTextColumn Caption="Zaposlen Id" FieldName="Id" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
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
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: center">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="VODJA TEAMA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupLeaderTeam" runat="server" ClientInstanceName="lookUpLeaderTeam"
                                                                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />
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
                                                                    <dx:GridViewDataTextColumn Caption="Zaposlen Id" FieldName="Id" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
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
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: flex-end">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="REALIZATOR : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupRealizator" runat="server" ClientInstanceName="lookUpRealizator"
                                                                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />
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
                                                                    <dx:GridViewDataTextColumn Caption="Zaposlen Id" FieldName="Id" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
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
                                            </div>
                                            <div class="row small-padding-bottom">
                                                <div class="col-md-12">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="OPIS PROBLEMA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoProblemDesc" runat="server" Width="100%" MaxLength="300" Theme="Moderno"
                                                                NullText="Opis problema..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoProblemDesc">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row small-padding-bottom">
                                                <div class="col-md-12">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Size="12px" Text="PREDLOG IZBOLJŠAVE : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoImprovementProposition" runat="server" Width="100%" MaxLength="5000" Theme="Moderno"
                                                                NullText="Opis predloga izboljšave..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoImprovementProposition">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row large-padding-bottom">
                                                <div class="col-md-12">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Font-Size="12px" Text="PRIHRANEK/STROŠKI : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoSavingsOrCosts" runat="server" Width="100%" MaxLength="300" Theme="Moderno"
                                                                NullText="Opis prihrankov oz. stroškov..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoSavingsOrCosts">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-5 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Size="12px" Text="TIP RDEČI KARTON : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupTipRdeciKarton" runat="server" ClientInstanceName="lookUpTipRdeciKarton"
                                                                KeyFieldName="idTipRdeciKarton" TextFormatString="{2}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSRedCardType">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}"
                                                                    ValueChanged="lookUpTipRdeciKarton_ValueChanged" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <GridViewStyles>
                                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                </GridViewStyles>
                                                                <GridViewProperties>
                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                        ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                                </GridViewProperties>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="Tip Id" FieldName="idTipRdeciKarton" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="30%"
                                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Naziv"
                                                                        FieldName="Naziv" ShowInCustomizationForm="True"
                                                                        Width="70%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                </Columns>
                                                            </dx:ASPxGridLookup>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-5 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" Font-Size="12px" Text="DATUM POPRAVILA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxDateEdit ID="DateEditDatumPopravila" runat="server" EditFormat="Date" Width="100%" Theme="Moderno"
                                                                CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="dtRepairDate">
                                                                <FocusedStyle CssClass="focus-text-box-input" />
                                                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                                <DropDownButton Visible="true"></DropDownButton>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-4 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Font-Size="12px" Text="OPIS LOKACIJE : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-8 no-padding-left">
                                                            <dx:ASPxTextBox runat="server" ID="txtOpisLokacija" ClientInstanceName="clientTxtOpisLokacija"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-5 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel15" runat="server" Font-Size="12px" Text="STROJ ŠTEVILKA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxTextBox runat="server" ID="txtStrojStevilka" ClientInstanceName="clientTxtStrojStevilka"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-5 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel16" runat="server" Font-Size="12px" Text="STROJ : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxTextBox runat="server" ID="txtStroj" ClientInstanceName="clientTxtStroj"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-4 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="LINIJA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-8 no-padding-left">
                                                            <dx:ASPxTextBox runat="server" ID="txtLinija" ClientInstanceName="clientTxtLinija"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row large-padding-bottom">
                                                <div class="col-md-12">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-2 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" Font-Size="12px" Text="PREDLOG POPRAVILA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-10 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoPredlogPopravila" runat="server" Width="100%" MaxLength="500" Theme="Moderno"
                                                                NullText="Podaj predlog popravila..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoPropositionOfRepair">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-5 no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel14" runat="server" Font-Size="12px" Text="RDEČI KARTON STATUS : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupStatusRdeciKarton" runat="server" ClientInstanceName="lookUpStatusRdeciKarton"
                                                                KeyFieldName="idStatus" TextFormatString="{1}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthSmall" DataSourceID="XpoDSStatusOnlyRK">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Naziv').Focus();}" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <GridViewStyles>
                                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                </GridViewStyles>
                                                                <GridViewProperties>
                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                                    <SettingsPager ShowSeparators="True" AlwaysShowPager="false" ShowNumericButtons="false" NumericButtonCount="3" />
                                                                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" ShowPreview="false" ShowVerticalScrollBar="True"
                                                                        VerticalScrollableHeight="100"></Settings>
                                                                </GridViewProperties>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="Id" FieldName="idStatus" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Naziv"
                                                                        FieldName="Naziv" ShowInCustomizationForm="True"
                                                                        Width="100%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                </Columns>
                                                            </dx:ASPxGridLookup>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <dx:XpoDataSource ID="XpoDSTip" runat="server" ServerMode="True"
                                    DefaultSorting="idTip" TypeName="KVP_Obrazci.Domain.KVPOdelo.Tip">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSEmployee" runat="server" ServerMode="True"
                                    DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSStatus" runat="server" ServerMode="True"
                                    DefaultSorting="idStatus" TypeName="KVP_Obrazci.Domain.KVPOdelo.Status">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSRedCardType" runat="server" ServerMode="True"
                                    DefaultSorting="idTipRdeciKarton" TypeName="KVP_Obrazci.Domain.KVPOdelo.TipRdeciKarton">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSStatusOnlyRK" runat="server" ServerMode="True"
                                    DefaultSorting="idStatus" TypeName="KVP_Obrazci.Domain.KVPOdelo.Status" Criteria="[SamoRK] = 1">
                                </dx:XpoDataSource>


                                <dx:XpoDataSource ID="XpoDSDepartment" runat="server" ServerMode="True"
                                    DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Departments">
                                </dx:XpoDataSource>

                                <div>
                                    <span class="AddEditButtons">
                                        <dx:ASPxButton Theme="Moderno" ID="btnPrenosKVPForm" runat="server" Text="Prenos v KVP obrazec" AutoPostBack="false"
                                            Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnPrenosKVPForm_Click">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                        </dx:ASPxButton>
                                    </span>
                                </div>

                                <div class="AddEditButtonsElements clearFloatBtns">
                                    <div style="display: inline-block; position: relative; left: 30%">
                                        <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red"></dx:ASPxLabel>
                                    </div>
                                    <span class="AddEditButtons">
                                        <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                                            Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm"
                                            UseSubmitBehavior="false" OnClick="btnConfirm_Click">
                                            <ClientSideEvents Click="CauseValidation" />
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <ClientSideEvents Click="CauseValidation" />
                                        </dx:ASPxButton>
                                    </span>
                                    <span class="AddEditButtons">
                                        <dx:ASPxButton Theme="Moderno" ID="btnCancel" runat="server" Text="Preklići" AutoPostBack="false"
                                            Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                        </dx:ASPxButton>
                                    </span>
                                </div>
                            </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>
