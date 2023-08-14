<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopUp.ascx.cs" Inherits="KVP_Obrazci.Widgets.PopUp" %>
<script type="text/javascript">

    function ConfirmBtn_Click(s, e)
    {
        clientLoadingPanel.Show();
        clientPupUpNotification.Hide();
        clientCallbackPopUp.PerformCallback('Confirm');
    }

    function CancelBtn_Click(s, e)
    {
        //clientLoadingPanel.Show();
        clientPupUpNotification.Hide();
        //clientCallbackPopUp.PerformCallback('Cancel');
    }

    function CallbackPopUp_EndCallback(s, e)
    {
        clientLoadingPanel.Hide();
    }
</script>

<div class="">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="clientLoadingPanel" Modal="true">
    </dx:ASPxLoadingPanel>
    <dx:ASPxPopupControl ID="PupUpNotification" runat="server" Width="400" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="clientPupUpNotification"
        HeaderText="Obvestilo" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true"
        OnWindowCallback="PupUpNotification_WindowCallback" ContentStyle-Paddings-PaddingLeft="0" ContentStyle-Paddings-PaddingRight="0">
        <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" Width="100%" Height="100%">
                                <Items>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                
                                                <dx:ASPxHeadline ID="Title" runat="server" />

                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                
                                                <dx:ASPxLabel ID="Description" runat="server" />
                                            
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                    <dx:LayoutItem ShowCaption="False" Paddings-PaddingTop="19">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxButton ID="btOK" runat="server" Text="OK" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px"
                                                    OnClick="btOK_Click" UseSubmitBehavior="false">
                                                   <%-- <ClientSideEvents Click="ConfirmBtn_Click" />--%>
                                                </dx:ASPxButton>
                                                <dx:ASPxButton ID="btCancel" runat="server" Text="Prekliči" Width="80px" AutoPostBack="False" Style="float: left; margin-right: 8px"
                                                    OnClick="btCancel_Click" UseSubmitBehavior="false">
                                                    <ClientSideEvents Click="CancelBtn_Click" />
                                                </dx:ASPxButton>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>

                                </Items>
                            </dx:ASPxFormLayout>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
                <div>
                    
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxCallback ID="CallbackPopUp" runat="server" ClientInstanceName="clientCallbackPopUp" 
        OnCallback="CallbackPopUp_Callback">
        <ClientSideEvents EndCallback="CallbackPopUp_EndCallback" />
    </dx:ASPxCallback>
</div>
