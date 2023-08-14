<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/WelcomeSite.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="KVP_Obrazci.Credentials.ChangePassword" %>

<%@ MasterType VirtualPath="~/MasterPages/WelcomeSite.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function btnChangePassword_Click(s, e) {
            var inputItems = [clientPassword, clientReEnterPass];
            var process = InputFieldsValidation(null, inputItems, null, null, null, null);

            if (process) {
                if (clientPassword.GetText() === clientReEnterPass.GetText())
                {
                    clientChangePasswordCallback.PerformCallback('ChangePassword');
                }
                else
                    $("#expModal").modal("show");
            }
        }

        function EndChangePasswordCallback(s, e) {
            if (s.cpErrorResult != "" && s.cpErrorResult !== undefined) {
                clientErrorLabel.SetText(s.cpErrorResult);
                delete (s.cpErrorResult);
            }
            else if (s.cpResult != "" && s.cpResult !== undefined) {
                $("#successModal").modal("show");
                delete (s.cpResult);
                var delay = 3000;
                setTimeout(function () { window.location.assign('../Home.aspx'); }, delay);
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <div class="col-md-4 hidden-sm hidden-xs"></div>
        <div class="col-md-4 align-item-centerV-centerH" style="border: 1px solid #e1e1e1; border-radius: 3px; box-shadow: 5px 10px 18px #e1e1e1; background-color: whitesmoke; margin-top: 30px;">
            <dx:ASPxFormLayout ID="ASPxFormLayoutLogin" runat="server">
                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="500" />
                <Items>
                    <dx:LayoutGroup Name="Password" GroupBoxDecoration="HeadingLine" Caption="Nastavitev gesla" UseDefaultPaddings="false" GroupBoxStyle-Caption-BackColor="WhiteSmoke">
                        <Items>
                            <dx:LayoutItem Caption="Error label caption" Name="ErrorLabelCaption" ShowCaption="False"
                                CaptionSettings-VerticalAlign="Middle">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxLabel ID="ErrorLabel" runat="server" Text="" ForeColor="Red"
                                            ClientInstanceName="clientErrorLabel">
                                        </dx:ASPxLabel>
                                        <div class="alert alert-info">
                                            Pozdravljeni 
                                            <dx:ASPxLabel ID="lblFullName" runat="server" Text="" Font-Bold="true" />. Vaše uporabniško ime je že dodeljeno.
                                            Prosimo, da si nastavite še geslo za dostop do eKVP sistema.
                                        </div>

                                        <dx:ASPxHiddenField ID="hiddenUserID" ClientInstanceName="clientHiddenUserID" runat="server"></dx:ASPxHiddenField>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Uporabniško ime" Name="Username" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="20px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtUsername" runat="server"
                                            CssClass="text-box-input" ClientInstanceName="clientUsername"
                                            AutoCompleteType="Disabled" ReadOnly="true" BackColor="LightGray">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Vnesi geslo" Name="Password" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="20px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtPassword" runat="server"
                                            CssClass="text-box-input" ClientInstanceName="clientPassword"
                                            AutoCompleteType="Disabled" Password="true">
                                            <ClientSideEvents Init="SetFocus" />
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Ponovi geslo" Name="reEnterPassword" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="10px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtReEnterPassword" runat="server"
                                            CssClass="text-box-input" Password="true" ClientInstanceName="clientReEnterPass">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Name="SignUp" HorizontalAlign="Right" ShowCaption="False" Paddings-PaddingTop="20px">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButton ID="btnChangePassword" runat="server" Text="Nastavi geslo" Width="100"
                                            AutoPostBack="false" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="btnChangePassword_Click" />
                                        </dx:ASPxButton>
                                        <dx:ASPxCallback ID="ChangePasswordCallback" runat="server" OnCallback="ChangePasswordCallback_Callback"
                                            ClientInstanceName="clientChangePasswordCallback">
                                            <ClientSideEvents EndCallback="EndChangePasswordCallback" />
                                        </dx:ASPxCallback>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
            </dx:ASPxFormLayout>
        </div>
        <div class="col-md-4 hidden-sm hidden-xs"></div>
    </div>

    <!-- Modal -->
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
                    <p>Uspešno ste si nastavili geslo!</p>
                    <p>Sistem vas bo preusmeril na prijavno stran!</p>
                </div>
                <%--<div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>--%>
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
                    <p>Vnešena gesla se ne ujemata. Ponovno vpišite geslo.</p>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
