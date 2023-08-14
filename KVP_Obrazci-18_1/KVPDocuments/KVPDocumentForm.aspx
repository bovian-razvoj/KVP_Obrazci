<%@ Page Title="Urejanje KVP predloga" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="KVPDocumentForm.aspx.cs" Inherits="KVP_Obrazci.KVPDocuments.KVPDocumentForm" %>

<%@ Register Assembly="DevExpress.Xpo.v18.1, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<%@ Register TagPrefix="widget" TagName="UploadAttachment" Src="~/Widgets/UploadAttachment.ascx" %>

<%@ Register TagPrefix="widget" TagName="PopUp" Src="~/Widgets/PopUp.ascx" %>

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
                var userChampionRole = '<%= KVP_Obrazci.Helpers.PrincipalHelper.IsUserChampion() %>';
                var isUserChampion = (userChampionRole == 'True');
                $("#saveKVPModal").modal('hide');
                if (isUserChampion)
                    clientBtnSubmitProposalToLeader.DoClick();
                else
                    clientBtnSubmitProposalToChampion.DoClick();
            });

            var submitKVPSuccess = GetUrlQueryStrings()['successMessage'];

            if (submitKVPSuccess) {
                $("#successModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.successMessage;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
        });

        function CauseValidation(s, e) {
            var process = false;

            var lookupItems = [lookUpType, lookUpProposer, lookUpLeaderTeam, lookUpDepartment];
            var memoFields = [clientMemoProblemDesc, clientMemoImprovementProposition];

            process = InputFieldsValidation(lookupItems, null, null, memoFields, null, null);

            var userActionAdd = '<%= (int)KVP_Obrazci.Common.Enums.UserAction.Add %>';
            var action = GetUrlQueryStrings()['action'];

            if (userActionAdd == action && firstShow && process) {
                $("#saveKVPModal").modal('show');
                e.processOnServer = false;
            }
            else {
                if (process) {
                    clientLoadingPanel.Show();
                    e.processOnServer = true;
                }
                else
                    e.processOnServer = false;
            }
        }

        function SaveStatus_Click(s, e) {
            var process = false;
            var lookupItems = [lookUpStatus];
            process = InputFieldsValidation(lookupItems, null, null, null, null, null);
            if (process) {
                clientLoadingPanel.Show();
                clientCallbackPanelKVPDocumentForm.PerformCallback('SaveStatus');
            }
        }

        function CallbackPanelKVPDocumentForm_EndCallback(s, e) {
            if (s.cpStatusHistory != "" && s.cpStatusHistory !== undefined) {

                var obj = jQuery.parseJSON(s.cpStatusHistory);

                $('#my-timeline').roadmap(obj, {
                    eventsPerSlide: 6,
                    slide: 1,
                    prevArrow: '<i class="material-icons">keyboard_arrow_left</i>',
                    nextArrow: '<i class="material-icons">keyboard_arrow_right</i>'
                });
                $('.nav-tabs a[href="#KVPHistoryStatus"]').tab('show');
                delete (s.cpStatusHistory);
            }
            else if (s.cpRejectArguments != "" && s.cpRejectArguments !== undefined) {
                $("#rejectModal").modal("show");

                $("#modalBodyText").empty();
                $("#modalBodyText").append(s.cpRejectArguments);
                delete (s.cpRejectArguments);
            }
            else if (s.cpBtnNextPreviousKVPDocID != "" && s.cpBtnNextPreviousKVPDocID !== undefined) {
                var recordId = GetUrlQueryStrings()['recordId'];
                var params = QueryStringsToObject();
                params.recordId = s.cpBtnNextPreviousKVPDocID;

                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);

                delete (s.cpBtnNextPreviousKVPDocID);
            }

            clientLoadingPanel.Hide();
        }

        $(document).on('show.bs.tab', '.nav-tabs a', function (e) {
            switch (event.target.hash) {
                case '#KVPHistoryStatus':
                    clientLoadingPanel.Show();
                    clientCallbackPanelKVPDocumentForm.PerformCallback('GetHistoryStatuses');
                    break;
                case '#Attachments':
                    $('[data-toggle="popover"]').popover({ html: true });
                    break;
            }
            //e.preventDefault();
        });

        var isPostbackInitiated = false;
        function btnSubmitProposal_Click(s, e) {
            var memoFields = [clientMemoProblemDesc, clientMemoImprovementProposition];
            var lookupItems = [lookUpProposer, lookUpType, lookUpLeaderTeam];
            var process = InputFieldsValidation(lookupItems, null, null, memoFields, null, null);

            var elementName = s.name.substring(s.name.lastIndexOf('_') + 1, s.name.length);

            if (process && !isPostbackInitiated) {
                clientLoadingPanel.Show();
                isPostbackInitiated = true;
                switch (elementName) {
                    case clientBtnSubmitProposalToLeader.name.substring(clientBtnSubmitProposalToLeader.name.lastIndexOf('_') + 1, clientBtnSubmitProposalToLeader.name.length):
                        clientCallbackPanelKVPDocumentForm.PerformCallback('SubmitProposalToLeader');
                        break;
                    case clientBtnSubmitProposalToChampion.name.substring(clientBtnSubmitProposalToChampion.name.lastIndexOf('_') + 1, clientBtnSubmitProposalToChampion.name.length):
                        clientCallbackPanelKVPDocumentForm.PerformCallback('SubmitProposalToChampion');
                        break;
                }
            }
        }

        function ConfirmKVP_Click(s, e) {
            var process = false;
            var lookupItems = [lookUpProposer, lookUpRealizator, lookUpLeaderTeam];
            process = InputFieldsValidation(lookupItems, null, null, null, null, null);
            if (process)
            {
                clientLoadingPanel.Show();
                e.processOnServer = true;
            }
            else
            {
                $('.nav-tabs a[href="#KVPBasicData"]').tab('show');
                e.processOnServer = false;
            }
        }

        function OnClosePopUpHandler(command, sender, url) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'Auditor':
                            clientPopUpAuditors.Hide();
                            window.location.assign(url);
                            break;
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
                        case 'Auditor':
                            clientPopUpAuditors.Hide();
                            case'InfoMail':
                                clientPopUpSendInfoMail.Hide();
                                break;
                    }
                    break;
            }
        }

        function ShowAuditorPopUp(s, e) {
            var lookupItems = [lookUpProposer, lookUpLeaderTeam];

            var process = InputFieldsValidation(lookupItems, null, null, null, null, null);
            if (process) {
                clientLoadingPanel.Show();
                clientCallbackPanelKVPDocumentForm.PerformCallback('ShowAuditorPopUp');
            }
            else
                $('.nav-tabs a[href="#KVPBasicData"]').tab('show');
        }

        function ShowRealizationPopUp(s, e) {

            var dateFields = [clientDateEditRealizationDate];
            var lookupItems = [lookUpProposer, lookUpLeaderTeam, lookUpRealizator];

            var process = InputFieldsValidation(lookupItems, null, dateFields, null, null, null);
            if (process) {
                clientLoadingPanel.Show();
                clientCallbackPanelKVPDocumentForm.PerformCallback('ShowRealizationPopUp');
            }
            else
                $('.nav-tabs a[href="#KVPBasicData"]').tab('show');
        }

        function btnRejectKVP_Click(s, e) {
            clientPopUpRejectKVPArguments.Show();
        }

        //Reject popup btn confirm
        function bntPopUpConfirm_Click(s, e) {
            var memoFields = [clientMemoArgumentDesc, clientMemoImprovementProposition];
            //var lookupItems = [lookUpProposer, lookUpInformedEmployee];
            var lookupItems = [lookUpProposer, lookUpLeaderTeam];

            var process = InputFieldsValidation(lookupItems, null, null, memoFields, null, null);
            if (process)
                clientLoadingPanel.Show();
            e.processOnServer = process;
        }

        function SubmitKVPAndOpenNew(s, e) {
            var memoFields = [clientMemoProblemDesc, clientMemoImprovementProposition];
            var lookupItems = [lookUpProposer, lookUpType, lookUpLeaderTeam];
            var process = InputFieldsValidation(lookupItems, null, null, memoFields, null, null);

            if (process) 
                clientLoadingPanel.Show();

            e.processOnServer = process;
        }

        function SubmitKVPRejectAndOpenNew(s, e) {
            var memoFields = [clientMemoProblemDesc, clientMemoImprovementProposition];
            var lookupItems = [lookUpProposer, lookUpType, lookUpLeaderTeam];
            var process = InputFieldsValidation(lookupItems, null, null, memoFields, null, null);
            if (process) {
                clientPopUpRejectKVPArguments.Show();
                clientHfSaveAndReject.Set("SubmitKVPRejectAndOpenNew", true);
            }
            e.processOnServer = false;
        }

        function btnRejectArgumentsNotification_Click(s, e) {
            clientLoadingPanel.Show();
            clientCallbackPanelKVPDocumentForm.PerformCallback('RejectArguments');
        }

        function btnNextKVP_Click(s, e) {
            clientLoadingPanel.Show();
            e.processOnServer = true;
        }

        function btnPreviousKVP_Click(s, e) {
            clientLoadingPanel.Show();
            e.processOnServer = true;
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
            var userActionAdd = '<%= (int)KVP_Obrazci.Common.Enums.UserAction.Add %>';
            var action = GetUrlQueryStrings()['action'];

            if (userActionAdd == action) {
                memoFields.push(clientMemoImprovementProposition);
                memoFields.push(clientMemoProblemDesc);
            }

            var process = InputFieldsValidation(null, null, null, memoFields, null, null);

            if (process) {
                gridKVPComments.PerformCallback('CommentAdd');
            }
        }

        function clientBtnCompleteKVP_Click(s, e) {
            var lookupItems = [lookUpProposer, lookUpLeaderTeam, lookUpRealizator];

            var process = InputFieldsValidation(lookupItems, null, null, null, null, null);

            if (process)
            {
                clientLoadingPanel.Show();
                e.processOnServer = true;
            }
            else
            {
                $('.nav-tabs a[href="#KVPBasicData"]').tab('show');
                e.processOnServer = false;
            }
        }

        function btnSendMail_Click(s, e)
        {
             clientCallbackPanelKVPDocumentForm.PerformCallback('InfoMailPopup');
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
                <ClientSideEvents EndCallback="CallbackPanelKVPDocumentForm_EndCallback" />
                <PanelCollection>
                    <dx:PanelContent>
                        <ul class="nav nav-tabs" runat="server" id="navTabs">
                            <li class="active" runat="server" id="basicDataItem">
                                <a data-toggle="tab" href="#KVPBasicData">Vsebina KVP</a>
                            </li>
                            <li runat="server" id="cipItem">
                                <a data-toggle="tab" href="#CIP">CIP</a>
                            </li>
                            <li runat="server" id="historyStatusItem">
                                <a data-toggle="tab" href="#KVPHistoryStatus"><span runat="server" id="historyStatusBadge" class="badge">0</span> Zgodovina statusov</a>
                            </li>
                            <li runat="server" id="changeStatusItem" class="hidden">
                                <a data-toggle="tab" href="#ChangeSatus">Spreminjanje statusa</a>
                            </li>
                            <li runat="server" id="overviewItem">
                                <a data-toggle="tab" href="#Overview"><span runat="server" id="auditorsBadge" class="badge">0</span> Presoje</a>
                            </li>
                            <li runat="server" id="attachmentsItem">
                                <a data-toggle="tab" href="#Attachments"><span runat="server" id="attachmentsBadge" class="badge">0</span> Priloge</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div id="KVPBasicData" class="tab-pane fade in active">
                                <div class="row">
                                    <div class="col-xs-6 align-item-centerV-startH">
                                        <span class="AddEditButtons">
                                            <dx:ASPxButton ID="btnPreviousKVP" runat="server" Text="Prejšnji" AutoPostBack="false"
                                                Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnPreviousKVP_Click">
                                                <Paddings PaddingLeft="0" PaddingRight="6" />
                                                <ClientSideEvents Click="btnPreviousKVP_Click" />
                                                <Image Url="../Images/icon-previous-left.png" UrlHottracked="../Images/icon-previous-left.png" />
                                            </dx:ASPxButton>
                                        </span>
                                    </div>
                                    <div class="col-xs-6 align-item-centerV-endH">
                                        <span class="AddEditButtons pull-right">
                                            <dx:ASPxButton ID="btnNextKVP" runat="server" Text="Naslednji" AutoPostBack="false" ImagePosition="Right"
                                                Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnNextKVP_Click">
                                                <Paddings PaddingLeft="6" PaddingRight="0" />
                                                <ClientSideEvents Click="btnNextKVP_Click" />
                                                <Image Url="../Images/icon-next-right.png" UrlHottracked="../Images/icon-next-right.png" />
                                            </dx:ASPxButton>
                                        </span>
                                    </div>
                                </div>
                                <div class="panel panel-default" style="margin-top: 10px;" runat="server" id="KVPBasicPanel">
                                    <div class="panel-heading" runat="server" id="KVPBasicPanelHeading">

                                        <h4 class="panel-title" style="display: inline-block;">Osnovni podatki</h4>

                                        <dx:ASPxButton ID="btnRejectArgumentsNotification" runat="server" AutoPostBack="False" AllowFocus="False" RenderMode="Link"
                                            CssClass="glyphicon glyphicon-exclamation-sign reject-KVP-arguments"
                                            Font-Size="25px" ForeColor="White"
                                            ClientVisible="false">
                                            <ClientSideEvents Click="btnRejectArgumentsNotification_Click" />
                                        </dx:ASPxButton>
                                        <a data-toggle="collapse" data-target="#demo"
                                            href="#collapseOne"></a>
                                    </div>
                                    <div id="demo" class="panel-collapse collapse in">
                                        <div class="panel-body">
                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-startH">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right:25px">
                                                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Font-Size="12px" Text="ŠTEVILKA KVP : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxTextBox runat="server" ID="txtKVPStevilka" ClientInstanceName="clientTxtKVPNumber"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" ClientEnabled="false" ReadOnly="true"
                                                                BackColor="LightGray" Font-Bold="true">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: flex-start;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="DATUM VNOSA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxDateEdit ID="DateEditDatumVnosa" runat="server" EditFormat="Date" Width="100%"
                                                                CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientEnabled="false">
                                                                <FocusedStyle CssClass="focus-text-box-input" />
                                                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                                <DropDownButton Visible="true"></DropDownButton>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: center;">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 78px;">
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

                                            <div class="row small-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2" style="align-items: center; justify-content: flex-start;">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right:18px">
                                                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="PREDLAGATELJ : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupProposer" runat="server" ClientInstanceName="lookUpProposer"
                                                                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSProposer" IncrementalFilteringMode="Contains">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <%-- DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" --%>
                                                                <ClientSideEvents ValueChanged="lookUpProposer_ValueChanged" />
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

                                                                    <dx:GridViewDataTextColumn Caption="VodjaID"
                                                                        FieldName="DepartmentId.DepartmentHeadId" ShowInCustomizationForm="True"
                                                                        Width="15%" Visible="false">
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                    </dx:GridViewDataTextColumn>

                                                                    <dx:GridViewDataTextColumn Caption="OddelekID"
                                                                        FieldName="DepartmentId.Id" ShowInCustomizationForm="True"
                                                                        Width="15%" Visible="false">
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
                                                    <div class="row2" style="align-items: center; justify-content: center;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="VODJA TIMA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupLeaderTeam" runat="server" ClientInstanceName="lookUpLeaderTeam"
                                                                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" IncrementalFilteringMode="Contains">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <%--<ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />--%>
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
                                                    <div class="row2" style="align-items: center; justify-content: flex-end;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="REALIZATOR : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                            <dx:ASPxGridLookup ID="ASPxGridLookupRealizator" runat="server" ClientInstanceName="lookUpRealizator"
                                                                KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                                Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                                OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSRealizator" IncrementalFilteringMode="Contains">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <%-- <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Firstname').Focus();}" />--%>
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
                                            </div>

                                            <div class="row small-padding-bottom hidden">
                                                <div class="col-md-12">
                                                    <div class="row2 align-item-centerV-startH">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel22" runat="server" Font-Size="12px" Text="LOKACIJA OPIS : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-11 no-padding-left">
                                                            <dx:ASPxTextBox runat="server" ID="txtOpisLokacija" ClientInstanceName="clientTxtOpisLokacija"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row medium-padding-bottom">
                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-startH">
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 48px;">
                                                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" Font-Size="12px" Text="LOKACIJA : "></dx:ASPxLabel>
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" />
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
                                                        <div class="col-sm-0 big-margin-r" style="margin-right: 62px;">
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" />
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


                                                            <dx:ASPxTextBox runat="server" ID="txtLinija" ClientInstanceName="clientTxtLinija"
                                                                CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="200" ClientVisible="false">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="row2 align-item-centerV-endH">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel20" runat="server" Font-Size="12px" Text="STROJ : "></dx:ASPxLabel>
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
                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" />
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


                                                            <dx:ASPxTextBox runat="server" ID="txtStroj" ClientInstanceName="clientTxtStroj"
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
                                                            <dx:ASPxLabel ID="ASPxLabel19" runat="server" Font-Size="12px" Text="STROJ ŠTEVILKA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-7 no-padding-left">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-md-6">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-0 big-margin-r small-margin-b">
                                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Size="12px" Text="OPIS PROBLEMA : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoProblemDesc" runat="server" Width="100%" MaxLength="5000"
                                                                NullText="Opis problema..." Rows="5" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoProblemDesc">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="row2" style="align-items: center">
                                                        <div class="col-sm-0 big-margin-r small-margin-b">
                                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Size="12px" Text="PREDLOG IZBOLJŠAVE : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoImprovementProposition" runat="server" Width="100%" MaxLength="5000"
                                                                NullText="Opis predloga izboljšave..." Rows="5" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoImprovementProposition">
                                                                <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            </dx:ASPxMemo>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom hidden">
                                                <div class="col-md-12">
                                                    <div class="row2" style="align-items: center; justify-content: flex-start;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="ASPxLabel9" runat="server" Font-Size="12px" Text="PRIHRANEK/STROŠKI : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-12 no-padding-left">
                                                            <dx:ASPxMemo ID="ASPxMemoSavingsOrCosts" runat="server" Width="100%" MaxLength="500"
                                                                NullText="Opis prihrankov oz. stroškov..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                                CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoSavingsOrCosts">
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
                                                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" Font-Size="12px" Text="PRIHRANEK/STROŠKI : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-1 no-padding-left">
                                                            <dx:ASPxCheckBox ID="CheckBoxPrihranekStroski" runat="server" ToggleSwitchDisplayMode="Always"></dx:ASPxCheckBox>
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
                                                        <Paddings Padding="0" />
                                                        <Settings ShowVerticalScrollBar="True"
                                                            VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                        <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                        </SettingsPager>
                                                        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
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

                                            <div class="row small-padding-bottom">
                                                <div class="col-sm-12">
                                                    <div class="row2" style="align-items: center;">
                                                        <div class="col-sm-0 big-margin-r">
                                                            <dx:ASPxLabel ID="lblDatumRealizacija" runat="server" Font-Size="12px" ClientVisible="false" Text="DATUM REALIZACIJE : "></dx:ASPxLabel>
                                                        </div>
                                                        <div class="col-sm-1 no-padding-left">
                                                            <dx:ASPxDateEdit ID="DateEditDatumRealizacije" runat="server" EditFormat="Date" Width="100%" ClientEnabled="false"
                                                                CssClass="text-box-input date-edit-padding" Font-Size="13px" ClientVisible="false" ClientInstanceName="clientDateEditRealizationDate">
                                                                <ClearButton DisplayMode="OnHover" />
                                                                <FocusedStyle CssClass="focus-text-box-input" />
                                                                <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                                                <DropDownButton Visible="true"></DropDownButton>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row small-padding-bottom">
                                                <div class="col-sm-12">
                                                    <div class="row2 align-item-centerV-endH">
                                                        <div class="col-sm-11"></div>
                                                        <div class="col-sm-0">
                                                            <dx:ASPxButton ID="btnDelete" runat="server" RenderMode="Link" AutoPostBack="false" ToolTip="Briši KVP" ClientVisible="false"
                                                                ClientInstanceName="clientBtnDelete" OnClick="btnDelete_Click" UseSubmitBehavior="false" CssClass="hoverButtonTrash">
                                                                <Image Url="../Images/trash.png" UrlHottracked="../Images/trashHover.png"></Image>
                                                            </dx:ASPxButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="CIP" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <h4 class="panel-title" style="display: inline-block;">CIP</h4>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row small-padding-bottom">
                                            <div class="col-md-12">
                                                <div class="row small-padding-bottom">
                                                    <div class="col-md-12">
                                                        <div class="row2" style="align-items: center">
                                                            <div class="col-sm-1 no-padding-right hidden">
                                                                <dx:ASPxLabel ID="ASPxLabel15" runat="server" Font-Size="12px" Text="CIP : "></dx:ASPxLabel>
                                                            </div>
                                                            <div class="col-sm-12 no-padding-left">
                                                                <dx:ASPxMemo ID="ASPxMemoCIPOpombe" runat="server" Width="100%" MaxLength="600"
                                                                    NullText="Opombe CIP..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                                    CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoCIPOpombe">
                                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                                </dx:ASPxMemo>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="KVPHistoryStatus" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <h4 class="panel-title" style="display: inline-block;">Zgodovina statusov</h4>
                                    </div>
                                    <div class="panel-body">
                                        <div id="my-timeline"></div>

                                        <div class="row small-padding-bottom small-padding-top">
                                            <div class="col-md-4">
                                                <div class="row2 align-item-centerV-startH">
                                                    <div class="col-sm-0 big-margin-r">
                                                        <dx:ASPxLabel ID="ASPxLabel24" runat="server" Font-Size="12px" Text="PRETEČENI DNEVI : "></dx:ASPxLabel>
                                                    </div>
                                                    <div class="col-sm-7 no-padding-left">
                                                        <dx:ASPxTextBox runat="server" ID="txtElapsedDays" ClientInstanceName="clientTxtElapsedDays"
                                                            CssClass="text-box-input" Font-Size="13px" Width="30%" ClientEnabled="false" ReadOnly="true"
                                                            BackColor="LightGray" Font-Bold="true">
                                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                        </dx:ASPxTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="ChangeSatus" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <h4 class="panel-title" style="display: inline-block;">Spremeni status</h4>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row small-padding-bottom">
                                            <div class="col-md-4">
                                                <div class="row2" style="align-items: center; justify-content: flex-start;">
                                                    <div class="col-sm-0 big-margin-r">
                                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Size="12px" Text="TRENUTNI STATUS : "></dx:ASPxLabel>
                                                    </div>
                                                    <div class="col-sm-6 no-padding-left">
                                                        <dx:ASPxTextBox runat="server" ID="txtCurrentStatus" ClientInstanceName="clientTxtCurrentStatus"
                                                            CssClass="text-box-input" Font-Size="13px" Width="100%">
                                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            <ClientSideEvents Init="SetFocus" />
                                                        </dx:ASPxTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="row2" style="align-items: center; justify-content: center;">
                                                    <div class="col-sm-0 big-margin-r">
                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" Font-Size="12px" Text="STATUS : "></dx:ASPxLabel>
                                                    </div>
                                                    <div class="col-sm-7 no-padding-left">
                                                        <dx:ASPxGridLookup ID="ASPxGridLookupStatus" runat="server" ClientInstanceName="lookUpStatus"
                                                            KeyFieldName="idStatus" TextFormatString="{1}" CssClass="text-box-input"
                                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                            OnLoad="ASPxGridLookupLoad_WidthSmall" DataSourceID="XpoDSStatus">
                                                            <ClearButton DisplayMode="OnHover" />
                                                            <ClientSideEvents DropDown="function(s,e){s.GetGridView().GetAutoFilterEditor('Koda').Focus();}" />
                                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                            <GridViewStyles>
                                                                <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                                                <FilterBarClearButtonCell></FilterBarClearButtonCell>
                                                            </GridViewStyles>
                                                            <GridViewProperties>
                                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowEllipsisInText="true" />
                                                                <SettingsPager ShowSeparators="True" AlwaysShowPager="False" ShowNumericButtons="False" NumericButtonCount="3" />
                                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowPreview="True" ShowVerticalScrollBar="True"
                                                                    VerticalScrollableHeight="200"></Settings>
                                                            </GridViewProperties>
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Status Id" FieldName="idStatus" Width="80px"
                                                                    ReadOnly="true" Visible="false" ShowInCustomizationForm="True" VisibleIndex="0">
                                                                </dx:GridViewDataTextColumn>

                                                                <%--<dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="50%"
                                                                    ReadOnly="true" ShowInCustomizationForm="True">
                                                                </dx:GridViewDataTextColumn>--%>

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
                                            <%--<div class="col-md-4">
                                                <div class="row2" style="align-items: center; justify-content: flex-end;">
                                                    <div class="col-sm-0 big-margin-r">
                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" Font-Size="12px" Text="REALIZATOR : "></dx:ASPxLabel>
                                                    </div>
                                                    <div class="col-sm-7 no-padding-left">
                                                        <dx:ASPxGridLookup ID="ASPxGridLookupRealizatorOnStatus" runat="server" ClientInstanceName="lookUpRealizatorOnStatus"
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
                                            </div>--%>
                                        </div>
                                        <div class="row small-padding-bottom">
                                            <div class="col-md-12">
                                                <div class="row2" style="align-items: center">
                                                    <div class="col-sm-0 big-margin-r">
                                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" Font-Size="12px" Text="OPOMBE : "></dx:ASPxLabel>
                                                    </div>
                                                    <div class="col-sm-12 no-padding-left">
                                                        <dx:ASPxMemo ID="ASPxMemoOpombe" runat="server" Width="100%" MaxLength="300"
                                                            NullText="Opombe..." Rows="3" HorizontalAlign="Left" BackColor="White"
                                                            CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoOpombe">
                                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                        </dx:ASPxMemo>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row2" style="align-items: flex-start">
                                                    <div class="col-sm-12">
                                                        <div class="AddEditButtonsElements clearFloatBtns">
                                                            <div style="display: inline-block; position: relative; left: 30%">
                                                                <dx:ASPxLabel ID="ASPxLabel14" runat="server" ForeColor="Red"></dx:ASPxLabel>
                                                            </div>
                                                            <span class="AddEditButtons">
                                                                <dx:ASPxButton ID="btnConfirmStatus" runat="server" Text="Shrani status" AutoPostBack="false"
                                                                    Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirmStatus"
                                                                    UseSubmitBehavior="false">
                                                                    <ClientSideEvents Click="SaveStatus_Click" />
                                                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                                                </dx:ASPxButton>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="Overview" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <h4 class="panel-title" style="display: inline-block;">Presoje</h4>
                                    </div>
                                    <div class="panel-body horizontal-scroll">
                                        <dx:ASPxGridView ID="ASPxGridViewKVPAuditors" runat="server" EnableCallbackCompression="true"
                                            ClientInstanceName="gridKVPAuditors"
                                            Width="100%" KeyFieldName="idKVPPresoje" CssClass="gridview-no-header-padding"
                                            DataSourceID="XpoDataSourceKVPAuditors">
                                            <Paddings Padding="0" />
                                            <Settings ShowVerticalScrollBar="True"
                                                ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300"
                                                ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                            <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                <PageSizeItemSettings Visible="true" Items="10,20,30,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                </PageSizeItemSettings>
                                                <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                            </SettingsPager>
                                            <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="true" AllowEllipsisInText="true" />
                                            <Styles Header-Wrap="True">
                                                <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                            </Styles>
                                            <SettingsText EmptyDataRow="Trenutno ni podatka o predlogih. Dodaj novega." />
                                            <Columns>

                                                <dx:GridViewDataTextColumn Caption="ID" FieldName="idKVPPresoje" Width="80px"
                                                    ReadOnly="true" Visible="false" ShowInCustomizationForm="True">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewBandColumn Caption="Presojevalec">
                                                    <Columns>
                                                        <dx:GridViewDataColumn FieldName="Presojevalec.Firstname" Caption="Ime" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="120px" />
                                                        <dx:GridViewDataColumn FieldName="Presojevalec.Lastname" Caption="Priimek" ReadOnly="true" Visible="true" ShowInCustomizationForm="True" Width="160px" />
                                                    </Columns>
                                                </dx:GridViewBandColumn>

                                                <dx:GridViewDataTextColumn Caption="Opombe" FieldName="Opomba" Width="500px"
                                                    ReadOnly="true" Visible="true" ShowInCustomizationForm="True">
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataDateColumn Caption="Datum"
                                                    FieldName="ts" ShowInCustomizationForm="True"
                                                    Width="100px">
                                                    <PropertiesDateEdit DisplayFormatString="dd. MMMM yyyy" />
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                </dx:GridViewDataDateColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                    </div>
                                </div>
                            </div>
                            <div id="Attachments" class="tab-pane fade in">
                                <div class="panel panel-default" style="margin-top: 10px;">
                                    <div class="panel-heading">
                                        <h4 class="panel-title" style="display: inline-block;">Seznam prilog</h4>
                                    </div>
                                    <div class="panel-body" id="active-drop-zone">
                                        <widget:UploadAttachment ID="test" runat="server" OnPopulateAttachments="test_PopulateAttachments" OnUploadComplete="test_UploadComplete"
                                            OnDeleteAttachments="test_DeleteAttachments" OnDownloadAttachments="test_DownloadAttachments" />
                                    </div>
                                </div>
                            </div>


                            <dx:ASPxPopupControl ID="ASPxPopupControlAuditors" runat="server" ContentUrl="KVPAuditor_popup.aspx"
                                ClientInstanceName="clientPopUpAuditors" Modal="True" HeaderText="PRESOJEVALCI"
                                CloseAction="CloseButton" Width="680px" Height="470px" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                AllowResize="true" ShowShadow="true"
                                OnWindowCallback="ASPxPopupControlAuditors_WindowCallback">
                                <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                <ContentStyle BackColor="#F7F7F7">
                                    <Paddings Padding="0px"></Paddings>
                                </ContentStyle>
                            </dx:ASPxPopupControl>

                            <dx:ASPxPopupControl ID="ASPxPopupControlRejectKVPArguments" runat="server"
                                ClientInstanceName="clientPopUpRejectKVPArguments" Modal="True" HeaderText="RAZLOG ZAVRNITVE"
                                CloseAction="CloseButton" Width="680px" Height="445px" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                AllowResize="true" ShowShadow="true"
                                OnWindowCallback="ASPxPopupControlRejectKVPArguments_WindowCallback">
                                <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                <ContentStyle BackColor="#F7F7F7">
                                    <Paddings Padding="0px"></Paddings>
                                </ContentStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl>
                                        <div class="row2 small-padding-bottom small-padding-top" style="align-items: center">
                                            <div class="col-xs-3 no-padding-right">
                                                <dx:ASPxLabel ID="ASPxLabel16" runat="server" Font-Size="12px" Text="ARGUMENTI : "></dx:ASPxLabel>
                                            </div>
                                            <div class="col-xs-9 no-padding-left">
                                                <dx:ASPxMemo ID="ASPxMemoRejectKVPArguments" runat="server" Width="100%" MaxLength="500"
                                                    NullText="Podroben opis zavrnitve..." Rows="16" HorizontalAlign="Left" BackColor="White"
                                                    CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoArgumentDesc">
                                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                                </dx:ASPxMemo>
                                            </div>
                                        </div>

                                        <div class="row2 small-padding-bottom align-item-centerV-startH">
                                            <div class="col-xs-3 no-padding-right">
                                                <dx:ASPxLabel ID="ASPxLabel23" runat="server" Font-Size="12px" Text="OBVEŠČENA OSEBA : "></dx:ASPxLabel>
                                                <%--<dx:ASPxLabel ID="ASPxLabel25" runat="server" Font-Size="12px" Text="(ni obvezen podatek)"></dx:ASPxLabel>--%>
                                            </div>
                                            <div class="col-sm-7 no-padding-left">
                                                <dx:ASPxGridLookup ID="ASPxGridLookupInformedEmployee" runat="server" ClientInstanceName="lookUpInformedEmployee"
                                                    KeyFieldName="Id" TextFormatString="{1} {2}" CssClass="text-box-input"
                                                    Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="14px"
                                                    OnLoad="ASPxGridLookupLoad_WidthLarge" DataSourceID="XpoDSEmployee" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
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

                                        <div class="row2 small-padding-top small-padding-bottom">
                                            <div class="col-xs-12" style="align-items: flex-end">
                                                <dx:ASPxButton ID="btnConfirmPopUp" runat="server" Text="Potrdi" AutoPostBack="false"
                                                    ClientInstanceName="clientBtnConfirm" OnClick="btnConfirmPopUp_Click" UseSubmitBehavior="false"
                                                    Width="100px">
                                                    <ClientSideEvents Click="bntPopUpConfirm_Click" />
                                                </dx:ASPxButton>
                                            </div>
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

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

                            <dx:XpoDataSource ID="XpoDSDepartment" runat="server" ServerMode="True"
                                DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Departments">
                            </dx:XpoDataSource>

                            <dx:XpoDataSource ID="XpoDataSourceKVPAuditors" runat="server" ServerMode="True"
                                DefaultSorting="idKVPPresoje DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPPresoje" Criteria="[idKVPDocument.idKVPDocument] = ?">
                                <CriteriaParameters>
                                    <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
                                </CriteriaParameters>
                            </dx:XpoDataSource>

                            <dx:XpoDataSource ID="XpoDSProposer" runat="server" ServerMode="True"
                                DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
                            </dx:XpoDataSource>

                            <dx:XpoDataSource ID="XpoDSRealizator" runat="server" ServerMode="True"
                                DefaultSorting="Id" TypeName="KVP_Obrazci.Domain.KVPOdelo.Users">
                            </dx:XpoDataSource>

                            <dx:XpoDataSource ID="XpoDSKVPComment" runat="server" ServerMode="True"
                                DefaultSorting="ts DESC" TypeName="KVP_Obrazci.Domain.KVPOdelo.KVPKomentarji" Criteria="[KVPDocId.idKVPDocument] = ?">
                                <CriteriaParameters>
                                    <asp:QueryStringParameter Name="RecordID" QueryStringField="recordId" DefaultValue="-1" />
                                </CriteriaParameters>
                            </dx:XpoDataSource>

                            <dx:XpoDataSource ID="XpoDSLocation" runat="server" ServerMode="True"
                                DefaultSorting="idLokacija" TypeName="KVP_Obrazci.Domain.KVPOdelo.Lokacija">
                            </dx:XpoDataSource>

                            <dx:XpoDataSource ID="XpoDSLine" runat="server" ServerMode="True"
                                DefaultSorting="Opis" TypeName="KVP_Obrazci.Domain.KVPOdelo.Linija">
                            </dx:XpoDataSource>

                            <dx:XpoDataSource ID="XpoDSMachine" runat="server" ServerMode="True"
                                DefaultSorting="idStroj" TypeName="KVP_Obrazci.Domain.KVPOdelo.Stroj">
                            </dx:XpoDataSource>

                            <div class="AddEditButtonsWrap">

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnPrenosRdeciKarton" runat="server" Text="Prenos v rdeči karton" AutoPostBack="false" ClientEnabled="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnPrenosRdeciKarton_Click"
                                        ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/redcard.png" UrlHottracked="../Images/redcardHoover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnRejectKVP" runat="server" Text="Zavrni KVP" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" BackColor="Tomato"
                                        ClientVisible="false">
                                        <ClientSideEvents Click="btnRejectKVP_Click" />
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/reject.png" UrlHottracked="../Images/rejectHoover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnConfrimKVP" runat="server" Text="Potrdi KVP" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" BackColor="#79a63f" OnClick="btnConfrimKVP_Click"
                                        ClientVisible="false">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="ConfirmKVP_Click" />
                                        <Image Url="../Images/confirm.png" UrlHottracked="../Images/confirmHoover.png" />
                                    </dx:ASPxButton>
                                </span>



                                <span class="AddEditButtons">
                                    <widget:PopUp ID="NotificationWindow" runat="server" OnConfirmBtnClick="NotificationWindow_ConfirmBtnClick" OnCancelBtnClick="NotificationWindow_CancelBtnClick" />
                                    <dx:ASPxButton ID="btnSubmitProposalToLeader" runat="server" Text="Oddaj KVP predlog" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="clientBtnSubmitProposalToLeader">
                                        <ClientSideEvents Click="btnSubmitProposal_Click" />
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/EvaluateKVP.png" UrlHottracked="../Images/EvaluateKVP_Hover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSubmitProposalToChampion" runat="server" Text="Oddaj KVP predlog v preverjanje" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" ClientVisible="false" ClientInstanceName="clientBtnSubmitProposalToChampion">
                                        <ClientSideEvents Click="btnSubmitProposal_Click" />
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/EvaluateKVP.png" UrlHottracked="../Images/EvaluateKVP_Hover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnSubmitProposalAndNewKVP" runat="server" Text="Oddaj KVP predlog in odpri novega" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" ClientVisible="false" OnClick="btnSubmitProposalAndNewKVP_Click">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="SubmitKVPAndOpenNew" />
                                        <Image Url="../Images/EvaluateKVP.png" UrlHottracked="../Images/EvaluateKVP_Hover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnAddAuditor" runat="server" Text="Dodaj presojevalca" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false"
                                        ClientVisible="false">
                                        <ClientSideEvents Click="ShowAuditorPopUp" />
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/presoja.png" UrlHottracked="../Images/presojaHoover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <widget:PopUp ID="PopUpRealizationConfirmation" runat="server" OnConfirmBtnClick="PopUpRealizationConfirmation_ConfirmBtnClick" OnCancelBtnClick="PopUpRealizationConfirmation_CancelBtnClick" />
                                    <dx:ASPxButton ID="btnRealize" runat="server" Text="Realiziraj KVP" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false"
                                        ClientVisible="false">
                                        <ClientSideEvents Click="ShowRealizationPopUp" />
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <Image Url="../Images/authorizeKVP.png" UrlHottracked="../Images/authorizeKVPHoover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <span class="AddEditButtons">
                                    <dx:ASPxButton ID="btnCompleteKVP" runat="server" Text="Zaključi KVP" AutoPostBack="false"
                                        Height="30" Width="50" UseSubmitBehavior="false" ClientVisible="false" OnClick="btnCompleteKVP_Click"
                                        ClientInstanceName="clientBtnCompleteKVP">
                                        <Paddings PaddingLeft="10" PaddingRight="10" />
                                        <ClientSideEvents Click="clientBtnCompleteKVP_Click" />
                                        <Image Url="../Images/zakljuci.png" UrlHottracked="../Images/zakljuciHoover.png" />
                                    </dx:ASPxButton>
                                </span>

                                <div class="AddEditButtonsElements clearFloatBtns">
                                    <div style="display: inline-block; position: relative; left: 30%">
                                        <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red"></dx:ASPxLabel>
                                    </div>
                                    <span class="AddEditButtons">
                                        <dx:ASPxHiddenField runat="server" ID="hfSaveAndReject" ClientInstanceName="clientHfSaveAndReject" />
                                        <dx:ASPxButton ID="btnSaveAndReject" runat="server" Text="Shrani in Zavrni" AutoPostBack="false"
                                            Height="30" Width="50" ValidationGroup="Confirm" ClientVisible="false" ClientInstanceName="clientbtnSaveAndReject"
                                            UseSubmitBehavior="false" OnClick="btnSaveAndReject_Click">
                                            <Image Url="../Images/reject.png" UrlHottracked="../Images/rejectHoover.png" />
                                            <ClientSideEvents Click="SubmitKVPRejectAndOpenNew" />
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                        </dx:ASPxButton>
                                    </span>
                                    <span class="AddEditButtons">
                                        <dx:ASPxButton ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                                            Height="30" Width="50" ValidationGroup="Confirm" ClientInstanceName="clientBtnConfirm"
                                            UseSubmitBehavior="false" OnClick="btnConfirm_Click">
                                            <ClientSideEvents Click="CauseValidation" />
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                        </dx:ASPxButton>
                                    </span>
                                    <span class="AddEditButtons">
                                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
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

    <!-- Modal -->
    <div id="rejectModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: tomato; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="fa fa-exclamation-circle" style="font-size: 60px; color: white"></i></div>
                </div>
                <div class="modal-body text-center" id="modalBodyText">
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
                        <h4 class="modal-title" id="myModalLabel" style="color: white;">KVP predlog</h4>
                    </div>
                    <div class="modal-body text-center">
                        <div class="row2">
                            <div class="col-xs-2 align-item-centerV-centerH">
                                <i class="fa fa-balance-scale" style="font-size: 30px; color: #3C8DBC;"></i>
                            </div>
                            <div class="col-xs-10 text-right">
                                <p>Vaš KVP predlog lahko shranite oz. oddate v pregled.</p>
                                <p>Z spodnjimi gumbi izberite vašo odločitev.</p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" id="modal-btn-save">Shrani KVP</button>
                        <button type="button" class="btn btn-primary" id="modal-btn-submit" style="padding: 6px 30px;">Oddaj KVP</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- success Modal -->
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
                    <p>Vaš KVP predlog je bil uspešno posredovan vodji!</p>
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
            </div>

        </div>
    </div>
</asp:Content>
