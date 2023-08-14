<%@ Page Title="Urejanje rdečega kartona" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="KVPDocumentRedCardForm.aspx.cs" Inherits="KVP_Obrazci.KVPDocuments.KVPDocumentRedCardForm" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<%@ Register TagPrefix="widget" TagName="UploadAttachment" Src="~/Widgets/UploadAttachment.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var firstShow = true;
        $(document).ready(function () {

            $("#modal-btn-save").on("click", function () {
                $("#saveKVPModal").modal('hide');
                firstShow = false;
                clientBtnConfirm.DoClick();
            });

            $("#modal-btn-submit").on("click", function () {
                $("#saveKVPModal").modal('hide');
                firstShow = false;
                clientBtnConfirm.DoClick();
            });

            $("#modal-btn-submit-add-attachment").on("click", function () {
                clientLoadingPanel.Show();
                clientCallbackPanelKVPDocumentForm.PerformCallback('SaveAndAddAttachment');
            });

            var submitKVPSuccess = GetUrlQueryStrings()['successMessage'];

            if (submitKVPSuccess) {
                $("#successModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.successMessage;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
                var stRK_val = GetUrlQueryStrings()['stRK'];
                stRK_val = stRK_val.toString().replace('%20', ' ');
                stRK_val = stRK_val.toString().replace('%20', ' ');

                $("#stRK").append(stRK_val); 
            }

            $('#RCManualNumberingContainer').popover('show');

            $("[data-toggle='popover']").on('shown.bs.popover', function () {
                setTimeout(function () {
                    $('#RCManualNumberingContainer').popover('hide');
                }, 4000);
            });

            SetRCNumberingTextBoxVisible(clientRCNumberingType.GetValue());

            var saveKVPAndOpenAttachmentTab = GetUrlQueryStrings()['addAttachment']
            if (saveKVPAndOpenAttachmentTab) {
                //$('.nav-tabs a[href="#Attachments"]').tab('show');
                //$('.nav-tabs a[href="#Attachments"]').tab('show');
                $('#RCBasicData').removeClass('active');
                $('#Attachments').addClass('active');


                $('#ctl00_ContentPlaceHolderMain_CallbackPanelKVPDocumentForm_basicDataItem').removeClass('active');
                $('#ctl00_ContentPlaceHolderMain_CallbackPanelKVPDocumentForm_attachmentsItem').addClass('active');

                var params = QueryStringsToObject();
                delete params.addAttachment;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
        });

        $(document).on('show.bs.tab', '.nav-tabs a', function (e) {
            switch (event.target.hash) {
                case '#RCHistoryStatus':
                    clientLoadingPanel.Show();
                    clientCallbackPanelKVPDocumentForm.PerformCallback('GetHistoryStatuses');
                    break;
                case '#Attachments':
                    $('[data-toggle="popover"]').popover({ html: true });
                    break;
            }
            //e.preventDefault();
        });

        function CauseValidation(s, e) {
            var process = false;
            var inputItems = [];
            var lookupItems = [lookUpProposer, lookUpLeaderTeam, lookUpTipRdeciKarton, lookUpStatusRdeciKarton, lookUpDepartment, lookUpLocation];
            var memoFields = [clientMemoProblemDesc];
            var userActionAdd = '<%= (int)KVP_Obrazci.Common.Enums.UserAction.Add %>';
            var action = GetUrlQueryStrings()['action'];
            var isLookupMachineEmpty = (lookUpMachine.GetText() == null || lookUpMachine.GetText() == "");
            var isLookupLineEmpty = (lookUpLine.GetText() == null || lookUpLine.GetText() == "");

            process = InputFieldsValidation(lookupItems, inputItems, null, memoFields, null, null);

            if (clientRCNumberingType.GetValue() == "Manual") {
                if (clientTxtRCNumberManual.GetText() == "" || clientTxtRCNumberManual.GetText().toUpperCase() == "P-") {
                    $(clientTxtRCNumberManual.GetInputElement()).parent().parent().parent().addClass("focus-text-box-input-error");
                    process = false;
                }
            }

            //obvezno polje je ali stroj ali linija
            if (isLookupMachineEmpty && isLookupLineEmpty) {
                $(lookUpMachine.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                $(lookUpLine.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                process = false;
            }

            if (userActionAdd == action && firstShow && process) {
                $("#saveKVPModal").modal('show');
                e.processOnServer = false;

                $('.nav-tabs a[href="#RCBasicData"]').tab('show');
            }
            else {
                if (process) {
                    clientLoadingPanel.Show();
                    e.processOnServer = true;
                }
                else {
                    firstShow = true;
                    e.processOnServer = false;
                    $('.nav-tabs a[href="#RCBasicData"]').tab('show');
                }
            }
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
            else if (s.cpStatusHistory != "" && s.cpStatusHistory !== undefined) {

                var obj = jQuery.parseJSON(s.cpStatusHistory);

                $('#my-timeline').roadmap(obj, {
                    eventsPerSlide: 6,
                    slide: 1,
                    prevArrow: '<i class="material-icons">keyboard_arrow_left</i>',
                    nextArrow: '<i class="material-icons">keyboard_arrow_right</i>'
                });
                $('.nav-tabs a[href="#RCHistoryStatus"]').tab('show');
                delete (s.cpStatusHistory);
            }
            else if (s.cpPrintID != "" && s.cpPrintID != undefined)
            {
                window.open(s.cpPrintID, '_blank');
                delete (s.cpPrintID);
            }

            SetRCNumberingTextBoxVisible(clientRCNumberingType.GetValue());
            clientLoadingPanel.Hide();
        }

        function RCNumberingType_ValueChanged(s, e) {
            var value = s.GetValue();

            SetRCNumberingTextBoxVisible(value);
        }

        function lookUpProposer_ValueChanged(s, e) {
            lookUpProposer.GetGridView().GetRowValues(lookUpProposer.GetGridView().GetFocusedRowIndex(), 'DepartmentId.DepartmentHeadId;DepartmentId.Id', OnGetRowValuesDepartmentHeadID);
        }

        function OnGetRowValuesDepartmentHeadID(value) {

            var proposerValue = lookUpProposer.GetValue();

            lookUpDepartment.SetValue(value[1]);

            if (proposerValue == value[0]) {
                clientLoadingPanel.Show();
                clientCallbackPanelKVPDocumentForm.PerformCallback('ProposalTheSameAsLeader');
            }
            else
                lookUpLeaderTeam.SetValue(value[0]);
        }

        function btnAddComment_Click(s, e) {
            var memoFields = [clientMemoNotesLeader];
            var lookupItems = null;
            var userActionAdd = '<%= (int)KVP_Obrazci.Common.Enums.UserAction.Add %>';
            var action = GetUrlQueryStrings()['action'];
            var isLookupMachineEmpty = (lookUpMachine.GetText() == null || lookUpMachine.GetText() == "");
            var isLookupLineEmpty = (lookUpLine.GetText() == null || lookUpLine.GetText() == "");

            if (userActionAdd == action) {
                memoFields.push(clientMemoProblemDesc);
                lookupItems = [lookUpProposer, lookUpLeaderTeam, lookUpTipRdeciKarton, lookUpStatusRdeciKarton, lookUpDepartment, lookUpLocation];
            }



            var process = InputFieldsValidation(lookupItems, null, null, memoFields, null, null);

            if (clientRCNumberingType.GetValue() == "Manual") {
                if (clientTxtRCNumberManual.GetText() == "" || clientTxtRCNumberManual.GetText().toUpperCase() == "P-") {
                    $(clientTxtRCNumberManual.GetInputElement()).parent().parent().parent().addClass("focus-text-box-input-error");
                    process = false;
                }
            }

            if (userActionAdd == action) {
                var lookupMachineText = lookUpMachine.GetText();
                var lookupLineText = lookUpLine.GetText();

                //obvezno polje je ali stroj ali linija
                if (isLookupMachineEmpty && isLookupLineEmpty) {
                    $(lookUpMachine.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                    $(lookUpLine.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                    process = false;
                }
            }

            if (process) {
                gridKVPComments.PerformCallback('CommentAdd');
            }
        }

        function btnSendMail_Click(s, e) {
            clientCallbackPanelKVPDocumentForm.PerformCallback('InfoMailPopup');
        }

        function SetRCNumberingTextBoxVisible(value) {
            if (value == "Manual") {
                $("#RCManualNumberingContainer").removeClass("hidden");
                $("#RCSystemNumberingContainer").addClass("hidden");
            }
            else {
                $("#RCManualNumberingContainer").addClass("hidden");
                $("#RCSystemNumberingContainer").removeClass("hidden");
            }
        }

        function btnbtnSetRealizationRC_Click(s, e) {
            CheckValidtionForRealization(e);
        }

        function btnRealizeRC_Click(s, e) {
            CheckValidtionForRealization(e);
        }

        function CheckValidtionForRealization(e) {
            var process = false;
            var inputItems = [];
            var lookupItems = [lookUpProposer, lookUpLeaderTeam, lookUpTipRdeciKarton, lookUpStatusRdeciKarton, lookUpDepartment, lookUpLocation, lookUpRealizator];
            var memoFields = [clientMemoProblemDesc, clientMemoActvity];
            var isLookupMachineEmpty = (lookUpMachine.GetText() == null || lookUpMachine.GetText() == "");
            var isLookupLineEmpty = (lookUpLine.GetText() == null || lookUpLine.GetText() == "");

            process = InputFieldsValidation(lookupItems, inputItems, null, memoFields, null, null);

            if (clientRCNumberingType.GetValue() == "Manual") {
                if (clientTxtRCNumberManual.GetText() == "" || clientTxtRCNumberManual.GetText().toUpperCase() == "P-") {
                    $(clientTxtRCNumberManual.GetInputElement()).parent().parent().parent().addClass("focus-text-box-input-error");
                    process = false;
                }
            }

            //obvezno polje je ali stroj ali linija
            if (isLookupMachineEmpty && isLookupLineEmpty) {
                $(lookUpMachine.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                $(lookUpLine.GetInputElement()).parent().parent().addClass("focus-text-box-input-error");
                process = false;
            }

            if (process) {
                clientLoadingPanel.Show();
                e.processOnServer = true;
            }
            else {
                firstShow = true;
                e.processOnServer = false;
                $('.nav-tabs a[href="#RCBasicData"]').tab('show');
            }
        }

        function btnCompleteRC_Click(s, e) { }
        function txtRCNumberManual_Init(s, e) {
            var userActionAdd = '<%= (int)KVP_Obrazci.Common.Enums.UserAction.Add %>';
            var action = GetUrlQueryStrings()['action'];

            if (userActionAdd == action) {
                s.SetText('P-');
            }
        }

        function OnClosePopUpHandler(command, sender, url) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'InfoMail':
                            clientPopUpSendInfoMail.Hide();
                            $("#successModal .modal-body p").empty();
                            $("#successModal .modal-body p").append("Vaše sporočilo je bilo poslano.");
                            $("#successModal").modal("show");
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'InfoMail':
                            clientPopUpSendInfoMail.Hide();
                            break;
                    }
                    break;
            }
        }

        function txtRCNumberManual_TextChanged(s, e) {
            var index = s.GetText().indexOf("P-");
            if (index < 0)
                s.SetText("P-");
        }

        function CheckBoxSecurity_CheckedChanged(s, e) {
            clientCallbackPanelKVPDocumentForm.PerformCallback('SecurityCheckBoxChanged');
        }

        function ASPxGridViewKVPComments_EndCallBack(s, e) {
            if (s.cp_error != "" && s.cp_error != undefined) {
                $('#expModal').modal('show');
                delete (s.cp_error);
            }
        }

        function btnPrintRedCard_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelKVPDocumentForm.PerformCallback("PrintRedCard");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row">
        <div class="col-sm-12">
            <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="clientLoadingPanel"
                Modal="True">
            </dx:ASPxLoadingPanel>
            <dx:ASPxCallbackPanel ID="CallbackPanelKVPDocumentForm" runat="server" Width="100%" OnCallback="CallbackPanelKVPDocumentForm_Callback"
                ClientInstanceName="clientCallbackPanelKVPDocumentForm">
                <SettingsLoadingPanel Enabled="false" />
                <ClientSideEvents EndCallback="EndCallback_Panel" />
                <PanelCollection>
                    <dx:PanelContent>
                        <ul class="nav nav-tabs" runat="server" id="navTabs">
                            <li class="active" runat="server" id="basicDataItem">
                                <a data-toggle="tab" href="#RCBasicData">Vsebina Rdeči karton</a>
                            </li>
                            <li runat="server" id="historyStatusItem">
                                <a data-toggle="tab" href="#RCHistoryStatus"><span runat="server" id="historyStatusBadge" class="badge">0</span> Zgodovina statusov</a>
                            </li>
                            <li runat="server" id="attachmentsItem">
                                <a data-toggle="tab" href="#Attachments"><span runat="server" id="attachmentsBadge" class="badge">0</span> Priloge</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div id="RCBasicData" class="tab-pane fade in active">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading" style="background-color: #f5f5f5; color: #333; border-color: #ddd;">
                                        <h4 class="panel-title" style="display: inline-block;">Osnovni podatki</h4>
                                        <a data-toggle="collapse" data-target="#demo" class="panel-collapse-arrow"
                                            href="#collapseOne"></a>
                                    </div>
                                    <div id="demo" class="panel-collapse collapse in" style="background-color: #f3b2b2">
                                        <div class="panel-body">
                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-startH">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 30px;">
                                                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Font-Size="12px" Text="ŠTEVILKA RK : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left hidden" id="RCManualNumberingContainer" data-toggle="popover" title="Papirna oblika RK" data-content="Številka rdečega kartona v paprini obliki mora imete predpono (P-)!"
                                                            data-placement="top">
                                                            <dx:ASPxTextBox runat="server" ID="txtRCNumberManual" ClientInstanceName="clientTxtRCNumberManual"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled"
                                                                BackColor="LightGray" Font-Bold="true">
                                                                <ClientSideEvents Init="txtRCNumberManual_Init"
                                                                    LostFocus="function(s,e){$('#RCManualNumberingContainer').popover('hide')}"
                                                                    GotFocus="function(s,e){$('#RCManualNumberingContainer').popover('show')}"
                                                                    UserInput="txtRCNumberManual_TextChanged" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left" id="RCSystemNumberingContainer">
                                                            <dx:ASPxTextBox runat="server" ID="txtRCNumberSystem" ClientInstanceName="clientTxtRCNumberSystem"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" ClientEnabled="false" ReadOnly="true" AutoCompleteType="Disabled"
                                                                BackColor="LightGray" Font-Bold="true">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-centerH">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 35px;">
                                                            <dx:ASPxLabel ID="lblRedCardNumbering" runat="server" Font-Size="12px" Text="ŠTEVILČENJE RK : " ClientVisible="false"></dx:ASPxLabel>
                                                        </div>

                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxComboBox ID="RCNumberingType" runat="server" CssClass="text-box-input" Width="100%" ClientVisible="false"
                                                                ClientInstanceName="clientRCNumberingType">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <ClientSideEvents ValueChanged="RCNumberingType_ValueChanged" />
                                                                <Items>
                                                                    <dx:ListEditItem Text="Ročno" Value="Manual" />
                                                                    <dx:ListEditItem Text="Sistem" Value="System" />
                                                                </Items>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-endH">
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
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" IncrementalFilteringMode="Contains">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <ClientSideEvents ValueChanged="lookUpProposer_ValueChanged" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <GridViewStyles>
                                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                </GridViewStyles>
                                                                <GridViewProperties>
                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
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

                                                                    <dx:GridViewDataTextColumn Caption="VodjaID"
                                                                        FieldName="DepartmentId.DepartmentHeadId" ShowInCustomizationForm="True"
                                                                        Width="15%" Visible="false">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="OddelekID"
                                                                        FieldName="DepartmentId.Id" ShowInCustomizationForm="True"
                                                                        Width="15%" Visible="false">
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                            </dx:ASPxGridLookup>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: center">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 41px;">
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
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
                                                    <div class="row2 align-item-centerV-endH">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="ODDELEK : "></dx:ASPxLabel>
                                                        </div>
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AutoFilterRowInputDelay="8000" />
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
                                            <div class="row medium-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-startH">
                                                        <div class="col-xs-0 big-margin-r no-padding-right" style="margin-right: 62px;">
                                                            <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Size="12px" Text="TIP RK : "></dx:ASPxLabel>
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AutoFilterRowInputDelay="8000" />
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
                                                    <div class="row2 align-item-centerV-centerH">
                                                        <div class="col-xs-0 big-margin-r no-padding-right">
                                                            <dx:ASPxLabel ID="ASPxLabel13" runat="server" Font-Size="12px" Text="TERMIN ZA IZVEDBO : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxDateEdit ID="DateEditDatumPopravila" runat="server" EditFormat="Date" Width="100%"
                                                                CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientInstanceName="dtRepairDate">
                                                                <FocusedStyle CssClass="focus-text-box-input" />
                                                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                                <DropDownButton Visible="true"></DropDownButton>
                                                            </dx:ASPxDateEdit>
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
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
                                            <div class="row medium-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-startH">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 43px;">
                                                            <dx:ASPxLabel ID="ASPxLabel20" runat="server" Font-Size="12px" Text="LOKACIJA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupLocation" runat="server" ClientInstanceName="lookUpLocation"
                                                                KeyFieldName="idLokacija" TextFormatString="{3}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSLocation" IncrementalFilteringMode="Contains">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <GridViewStyles>
                                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                </GridViewStyles>
                                                                <GridViewProperties>
                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                                                    <SettingsPager PageSize="100" ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />

                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                        ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                                </GridViewProperties>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idLokacija" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Sort" FieldName="Sort" Width="20%"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Koda"
                                                                        FieldName="Koda" ShowInCustomizationForm="True"
                                                                        Width="30%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Opis"
                                                                        FieldName="Opis" ShowInCustomizationForm="True"
                                                                        Width="70%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                </Columns>
                                                            </dx:ASPxGridLookup>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-centerH">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 86px;">
                                                            <dx:ASPxLabel ID="ASPxLabel21" runat="server" Font-Size="12px" Text="LINIJA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupLine" runat="server" ClientInstanceName="lookUpLine"
                                                                KeyFieldName="idLinija" TextFormatString="{3}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSLine" IncrementalFilteringMode="Contains">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <GridViewStyles>
                                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                </GridViewStyles>
                                                                <GridViewProperties>
                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                                                    <SettingsPager PageSize="100" ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                        ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                                </GridViewProperties>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idLinija" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Sort" FieldName="Sort" Width="20%"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Koda"
                                                                        FieldName="Koda" ShowInCustomizationForm="True"
                                                                        Width="30%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Opis"
                                                                        FieldName="Opis" ShowInCustomizationForm="True"
                                                                        Width="70%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                </Columns>
                                                            </dx:ASPxGridLookup>


                                                            <dx:ASPxTextBox runat="server" ID="ASPxTextBox1" ClientInstanceName="clientTxtLinija"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200" ClientVisible="false">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-endH">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel22" runat="server" Font-Size="12px" Text="STROJ : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupMachine" runat="server" ClientInstanceName="lookUpMachine"
                                                                KeyFieldName="idStroj" TextFormatString="{3}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthMedium" DataSourceID="XpoDSMachine" IncrementalFilteringMode="Contains">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                <GridViewStyles>
                                                                    <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                    <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                                </GridViewStyles>
                                                                <GridViewProperties>
                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                                                    <SettingsPager PageSize="100" ShowSeparators="True" AlwaysShowPager="True" ShowNumericButtons="true" NumericButtonCount="3" />
                                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                        ShowHorizontalScrollBar="true" VerticalScrollableHeight="200"></Settings>
                                                                </GridViewProperties>
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="idStroj" Width="80px"
                                                                        ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Sort" FieldName="Sort" Width="20%"
                                                                        ReadOnly="true" ShowInCustomizationForm="True">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Koda"
                                                                        FieldName="Koda" ShowInCustomizationForm="True"
                                                                        Width="30%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="Opis"
                                                                        FieldName="Opis" ShowInCustomizationForm="True"
                                                                        Width="70%">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                </Columns>
                                                            </dx:ASPxGridLookup>


                                                            <dx:ASPxTextBox runat="server" ID="ASPxTextBox2" ClientInstanceName="clientTxtStroj"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200" ClientVisible="false">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>

                                                        <div class="col-sm-3 no-padding-left">
                                                            <dx:ASPxTextBox runat="server" ID="txtStrojStevilka" ClientInstanceName="clientTxtStrojStevilka"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200" NullText="Številka stroja">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4 hidden">
                                                    <div class="row2 align-item-centerV-endH">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel23" runat="server" Font-Size="12px" Text="STROJ ŠTEVILKA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-md-6">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="OPIS NAPAKE : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoFailureDesc" runat="server" Width="100%" MaxLength="5000" Theme="Moderno"
                                                                NullText="Opis problema..." Rows="5" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoProblemDesc">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Size="12px" Text="AKTIVNOST : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoActivity" runat="server" Width="100%" MaxLength="5000" Theme="Moderno"
                                                                NullText="Opis predloga izboljšave..." Rows="5" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoActvity">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-sm-12">
                                                    <div class="row2" style="align-items: center;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="VARNOST : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-1 no-padding-left">
                                                            <dx:ASPxCheckBox ID="CheckBoxSecurity" runat="server" ToggleSwitchDisplayMode="Always">
                                                                <ClientSideEvents CheckedChanged="CheckBoxSecurity_CheckedChanged" />
                                                            </dx:ASPxCheckBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom hidden">
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AutoFilterRowInputDelay="8000" />
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

                                            <div class="row large-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-startH large-padding-bottom">
                                                        <div class="col-sm-0 big-margin-r small-margin-b">
                                                            <dx:ASPxLabel ID="lblNoteLeader" runat="server" Font-Size="12px" Text="OPOMBA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoNotesLeader" runat="server" Width="100%" MaxLength="2000"
                                                                NullText="Dodatne opombe..." Rows="5"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoNotesLeader">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                    <div class="row2 align-item-centerV-startH">
                                                        <div class="col-sm-0">
                                                            <dx:ASPxButton ID="btnAddComment" runat="server" RenderMode="Link" AutoPostBack="false" ToolTip="Dodaj opombo"
                                                                ClientInstanceName="clientBtnAddComment" UseSubmitBehavior="false" CssClass="hoverButtonAdd">
                                                                <ClientSideEvents Click="btnAddComment_Click" />
                                                                <Image Url="../Images/addComment.png" UrlHottracked="../Images/addCommentHover2.png" UrlDisabled="../Images/addCommentDisabled.png" />
                                                            </dx:ASPxButton>
                                                        </div>
                                                        <div class="col-sm-0">
                                                            <dx:ASPxButton ID="btnSendMail" runat="server" RenderMode="Link" AutoPostBack="false" ToolTip="Pošlji mail"
                                                                ClientInstanceName="clientBtnSendMail" UseSubmitBehavior="false" CssClass="hoverButtonAdd">
                                                                <ClientSideEvents Click="btnSendMail_Click" />
                                                                <Image Url="../Images/sendMail.png" UrlHottracked="../Images/sendMailHover.png" />
                                                            </dx:ASPxButton>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-8">
                                                    <dx:ASPxGridView ID="ASPxGridViewKVPComments" runat="server" EnableCallbackCompression="true"
                                                        ClientInstanceName="gridKVPComments"
                                                        Width="100%" KeyFieldName="KVPKomentarjiID" CssClass="gridview-no-header-padding"
                                                        DataSourceID="XpoDSKVPComment" OnCustomCallback="ASPxGridViewKVPComments_CustomCallback"
                                                        OnHtmlRowPrepared="ASPxGridViewKVPComments_HtmlRowPrepared"
                                                        OnCustomButtonCallback="ASPxGridViewKVPComments_CustomButtonCallback">
                                                        <ClientSideEvents EndCallback="ASPxGridViewKVPComments_EndCallBack" />
                                                        <Paddings Padding="0" />
                                                        <Settings ShowVerticalScrollBar="True"
                                                            VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                        <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                        </SettingsPager>
                                                        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" AutoFilterRowInputDelay="8000" />
                                                        <Styles Header-Wrap="True">
                                                            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                        </Styles>
                                                        <SettingsText EmptyDataRow="Trenutno ni opomb. Dodajte novo." />
                                                        <SettingsDetail ShowDetailRow="true" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn ButtonRenderMode="Image" Width="80px" Caption="Briši">
                                                                <CustomButtons>
                                                                    <dx:GridViewCommandColumnCustomButton Image-ToolTip="Izbriši komentar" ID="DeleteComment">
                                                                        <Image Url="../Images/trash.png" />
                                                                        <Styles Style-CssClass="hoverButtonTrash2">
                                                                        </Styles>
                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:GridViewCommandColumn>

                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="KVPKomentarjiID" Width="80px"
                                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn Caption="EmploeeId" FieldName="UserId.Id" Width="80px"
                                                                ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewBandColumn Caption="Lastnik">
                                                                <Columns>
                                                                    <dx:GridViewDataColumn FieldName="UserId.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" Settings-FilterMode="DisplayText">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataColumn>

                                                                    <dx:GridViewDataColumn FieldName="UserId.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataColumn>
                                                                </Columns>
                                                            </dx:GridViewBandColumn>

                                                            <dx:GridViewDataTextColumn Caption="Komentar"
                                                                FieldName="Opombe" ShowInCustomizationForm="True"
                                                                Width="200px">
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataDateColumn Caption="Datum"
                                                                FieldName="ts" ShowInCustomizationForm="True"
                                                                Width="150px">
                                                                <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy - hh:mm" />
                                                                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataDateColumn>
                                                        </Columns>
                                                        <Templates>
                                                            <DetailRow>
                                                                <div class="row small-padding-bottom">
                                                                    <div class="col-sm-12">
                                                                        <div class="row2 align-item-centerV-startH">
                                                                            <div class="col-sm-0 big-margin-r">
                                                                                Komentar: 
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
                                                    </dx:ASPxGridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <dx:ASPxPopupControl ID="ASPxPopupControlSendInfoMail" runat="server" ContentUrl="SendInfoMail_popup.aspx"
                                    ClientInstanceName="clientPopUpSendInfoMail" Modal="True" HeaderText="POŠLJI INFO NA MAIL"
                                    CloseAction="CloseButton" Width="680px" Height="470px" PopupHorizontalAlign="WindowCenter"
                                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                    AllowResize="true" ShowShadow="true"
                                    OnWindowCallback="ASPxPopupControlSendInfoMail_WindowCallback">
                                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                    <ContentStyle BackColor="#F7F7F7">
                                        <Paddings Padding="0px"></Paddings>
                                    </ContentStyle>
                                </dx:ASPxPopupControl>

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
                                    DefaultSorting="idTipRdeciKarton" TypeName="KVP_Obrazci.Domain.KVPOdelo.TipRdeciKarton" Criteria="[Koda] <> 'VARNOST'">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSStatusOnlyRK" runat="server" ServerMode="True"
                                    DefaultSorting="idStatus" TypeName="KVP_Obrazci.Domain.KVPOdelo.Status" Criteria="[SamoRK] = 1">
                                </dx:XpoDataSource>


                                <dx:XpoDataSource ID="XpoDSDepartment" runat="server" ServerMode="True"
                                    DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Departments">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSLocation" runat="server" ServerMode="True"
                                    DefaultSorting="idLokacija" TypeName="KVP_Obrazci.Domain.KVPOdelo.Lokacija">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSLine" runat="server" ServerMode="True"
                                    DefaultSorting="Opis" TypeName="KVP_Obrazci.Domain.KVPOdelo.Linija">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSMachine" runat="server" ServerMode="True"
                                    DefaultSorting="idStroj" TypeName="KVP_Obrazci.Domain.KVPOdelo.Stroj" Criteria="[Active] = 1">
                                </dx:XpoDataSource>

                                <dx:XpoDataSource ID="XpoDSKVPComment" runat="server" ServerMode="True"
                                    DefaultSorting="ts DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPKomentarji" Criteria="[KVPDocId.idKVPDocument] = ?">
                                    <CriteriaParameters>
                                        <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
                                    </CriteriaParameters>
                                </dx:XpoDataSource>


                            </div>
                            <div id="RCHistoryStatus" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <h4 class="panel-title" style="display: inline-block;">Zgodovina statusov</h4>
                                    </div>
                                    <div class="panel-body" style="background-color: #f3b2b2">
                                        <div id="my-timeline"></div>
                                    </div>
                                </div>
                            </div>
                            <div id="Attachments" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <h4 class="panel-title" style="display: inline-block;">Seznam prilog</h4>
                                    </div>
                                    <div class="panel-body" id="active-drop-zone" style="background-color: #f3b2b2">
                                        <widget:UploadAttachment ID="RedCardAttachments" runat="server" OnPopulateAttachments="RedCardAttachments_PopulateAttachments" OnUploadComplete="RedCardAttachments_UploadComplete"
                                            OnDeleteAttachments="RedCardAttachments_DeleteAttachments" OnDownloadAttachments="RedCardAttachments_DownloadAttachments"
                                            WebsiteDocumentContainerID="ctl00_ContentPlaceHolderMain_CallbackPanelKVPDocumentForm_RedCardAttachments_documentContainer"
                                            ReplaceFileInDestination="false" RandomFileName="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="AddEditButtonsWrap">

                                <span class="AddEditButtons">
                                    <dx:ASPxButton Theme="Moderno" ID="btnPrenosKVPForm" runat="server" Text="Prenos v KVP obrazec" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnPrenosKVPForm_Click" ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSetRealizationRC" runat="server" Text="Oddaj v realizacijo" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" ClientVisible="false" OnClick="btnSetRealizationRC_Click">
                                        <ClientSideEvents Click="btnbtnSetRealizationRC_Click" />
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/EvaluateKVP.png" UrlHottracked="../Images/EvaluateKVP_Hover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnRealizeRC" runat="server" Text="Realiziraj RK" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnRealizeRC_Click"
                                        ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="btnRealizeRC_Click" />
                                        <Image Url="../Images/authorizeKVP.png" UrlHottracked="../Images/authorizeKVPHoover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnCompleteRC" runat="server" Text="Zaključi RK" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCompleteRC_Click"
                                        ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="btnCompleteRC_Click" />
                                        <Image Url="../Images/zakljuci.png" UrlHottracked="../Images/zakljuciHoover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSubmitRedCard" runat="server" Text="Oddaj RK" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnSubmitRedCard_Click"
                                        ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="CauseValidation" />
                                        <Image Url="../Images/EvaluateKVP.png" UrlHottracked="../Images/EvaluateKVP_Hover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnPrintRedCard" runat="server" Text="Natisni RK" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="btnPrintRedCard_Click" />
                                        <Image Url="../Images/print.png" UrlHottracked="../Images/printHover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <div class="AddEditButtonsElements clearFloatBtns">
                                    <div style="display: inline-block; position: relative; left: 30%">
                                        <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red"></dx:ASPxLabel>
                                    </div>
                                    <span class="AddEditButtons">
                                        <dx:ASPxButton Theme="Moderno" ID="btnConfirm" runat="server" Text="Shrani" AutoPostBack="false"
                                            Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm"
                                            UseSubmitBehavior="false" OnClick="btnConfirm_Click" ClientVisible="false">
                                            <ClientSideEvents Click="CauseValidation" />
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../Images/edit.png" UrlHottracked="../Images/editHoover.png" />
                                        </dx:ASPxButton>
                                    </span>
                                    <span class="AddEditButtons">
                                        <dx:ASPxButton Theme="Moderno" ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                                            Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../Images/cancel.png" UrlHottracked="../Images/cancelHover.png" />
                                        </dx:ASPxButton>
                                    </span>
                                </div>
                            </div>

                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </div>

    <div id="successModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: #47c9a2; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="fa fa-check-square-o" style="font-size: 60px; color: white"></i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Odlično!</h3>
                    <p>Vaš Rdeči karton je bil uspešno posredovan TPM Administratorju!</p>
                    <p>Za hitrejšo odpravo napake napišite št. RK na rdeč kartonček in tega namestite na mesto napake.</p>
                    <p>Št. rdečega kartona:</p>
                    <b id="stRK"></b>
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
            </div>

        </div>
    </div>

    <div class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false" id="saveKVPModal">
        <div class="vertical-alignment-helper">
            <div class="modal-dialog modal-sm vertical-align-center">
                <div class="modal-content">
                    <div class="modal-header kvp-model-header">
                        <button type="button" class="close" data-dismiss="modal" style="color: white; opacity: 0.8">&times;</button>
                        <h4 class="modal-title" id="myModalLabel" style="color: white;">Rdeči karton</h4>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row2">
                            <div class="col-xs-2 align-item-centerV-centerH">
                                <i class="fa fa-balance-scale" style="font-size: 30px; color: #3C8DBC;"></i>
                            </div>
                            <div class="col-xs-10 text-right">
                                <p>Vaš rdeči karton boste oddali v pregled.</p>
                                <br />
                                <p>Z spodnjim gumbom potrdite vašo odločitev.</p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="display: grid;">
                        <%--<button type="button" class="btn btn-default" id="modal-btn-save">Shrani RK</button>--%>
                        <button type="button" class="btn btn-primary" id="modal-btn-submit" style="margin-bottom: 10px;">Oddaj RK</button>
                        <button type="button" class="btn btn-info" id="modal-btn-submit-add-attachment" style="margin-bottom: 10px;">Oddaj RK in dodaj prilogo</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- exception Modal -->
    <div id="expModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: red; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">error_outline</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Napaka!</h3>
                    <p>Številka rdečega kartona že obstaja. Vnesi drugo.</p>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
