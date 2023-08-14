<%@ Page Title="Domov KVP" Language="C#" MasterPageFile="~/MasterPages/WelcomeSite.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="KVP_Obrazci.Home" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var isInCallback = false;
        function OnClosePopupEventHandler_Prijava(param, url) {
            switch (param) {
                case 'Potrdi':
                    Prijava_Popup.Hide();
                    window.location.assign(url);//"../Default.aspx"
                    break;
                case 'Prekliči':
                    Prijava_Popup.Hide();
                    break;
            }
        }


        function CauseValidation(s, e) {
            var procees = false;
            var inputItems = [clientUsername, clientPass];

            procees = InputFieldsValidation(null, inputItems, null, null);

            if (procees) {
                clientLoadingPanel.Show();
                clientLoginCallback.PerformCallback("SignInUserCredentials");
            }
        }

        function EndLoginCallback(s, e) {
            clientLoadingPanel.Hide();

            clientUsername.SetText("");
            clientPass.SetText("");

            if (s.cpResult != "" && s.cpResult !== undefined) {
                clientErrorLabel.SetText(s.cpResult);
                clientErrorLabelKey.SetText(s.cpResult);
                delete (s.cpResult);
            }
            else
                window.location.assign('Home.aspx');//"../Default.aspx"
        }
        function ClearText(s, e) {
            clientErrorLabel.SetText("");
            clientErrorLabelKey.SetText("");

            $(clientUsername.GetInputElement()).parent().parent().parent().removeClass("focus-text-box-input-error");
            $(clientPass.GetInputElement()).parent().parent().parent().removeClass("focus-text-box-input-error")
        }

        function OnValueChanged_txtToken(s, e) {
            clientLoadingPanel.Show();
            clientLoginCallback.PerformCallback(s.GetText());
        }
        function onClick_Forward(s, e) {
            $("#ctl00_ContentPlaceHolderMain_ASPxFormLayoutLogin_0").hide();
            $("#ctl00_ContentPlaceHolderMain_ASPxFormLayoutLogin_1").show(function () {
                clientToken.Focus();
            });
        }

        function onClick_Backward(s, e) {
            $("#ctl00_ContentPlaceHolderMain_ASPxFormLayoutLogin_1").hide();
            $("#ctl00_ContentPlaceHolderMain_ASPxFormLayoutLogin_0").show(function () {
                clientUsername.Focus();
            });
        }
        //$(document).keydown(function () {
        //   // alert("Handler for .keydown() called."+this.value);
        //});
        //$(document).on('keyup', function (e) {

        //    var userInput = e.target.value;
        //    alert(userInput);

        //});

        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolderMain_FormLayoutWrap').keypress(function (event) {
                var key = event.which;
                if (key == 13) {
                    CauseValidation(this, event);
                    clientUsername.GetInputElement().blur();
                    clientPass.GetInputElement().blur();
                    return false;
                }
            });

            var newkvpnoleaderex = GetUrlQueryStrings()['newkvpnoleaderex'];
            var unhandledException = GetUrlQueryStrings()['unhandledExp'];
            var sessionEnd = GetUrlQueryStrings()['sessionExpired'];
            var messageType = GetUrlQueryStrings()['messageType'];

            if (unhandledException) {
                $("#unhandledExpModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.unhandledExp;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
            else if (newkvpnoleaderex) {
                $("#newkvpnoleaderex").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.newkvpnoleaderex;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
            else if (sessionEnd) {
                $("#sessionEndModal").modal("show");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.sessionExpired;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
            else if (messageType !== undefined) {
                var value = "";

                switch (messageType) {
                    case "1":
                        var resource = '<%= KVP_Obrazci.Resources.HandledException.res_01 %>';
                        value = resource;
                        break;
                    case "2":
                        value = '<%= KVP_Obrazci.Resources.HandledException.res_02 %>';
                        break;
                    default:
                        break;
                }

                $("#handledExpBodyText").append(value);
                $("#handledExpModal").modal("show");

                //we delete messageType query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.messageType;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }

            if (!isInCallback) {
                clientGenerateHelperButtonsCallbackPanel.PerformCallback('CreateButtons');
                isInCallback = true;
            }

       <%--     var k = '<%=ConfigurationManager.AppSettings["bgColor"].ToString() %>'
            alert(k);--%>
        });

        function GenerateHelperButtonsCallbackPanel_EndCallback(s, e)
        {
            isInCallback = false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="FormLayoutWrap" runat="server" style="display: flex; width: 50%; margin: 0 auto; overflow: hidden; padding: 10px; border: 1px solid #e1e1e1; border-radius: 3px; box-shadow: 5px 10px 18px #e1e1e1; background-color: whitesmoke; margin-top: 30px;">
        <dx:ASPxFormLayout ID="ASPxFormLayoutLogin" runat="server">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="500" />
            <Items>
                <dx:LayoutGroup Name="LOGIN" GroupBoxDecoration="HeadingLine" Caption="Prijava" UseDefaultPaddings="false" GroupBoxStyle-Caption-BackColor="WhiteSmoke">
                    <Items>
                        <dx:LayoutItem Caption="Error label caption" Name="ErrorLabelCaption" ShowCaption="False"
                            CaptionSettings-VerticalAlign="Middle">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxLabel ID="ErrorLabel" runat="server" Text="" ForeColor="Red"
                                        ClientInstanceName  ="clientErrorLabel">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Uporabniško ime" Name="Username" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="20px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="txtUsername" runat="server"
                                        CssClass="text-box-input" ClientInstanceName="clientUsername"
                                        AutoCompleteType="Disabled">
                                        <FocusedStyle CssClass="focus-text-box-input" />
                                        <ClientSideEvents Init="SetFocus" GotFocus="ClearText" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Geslo" Name="Password" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="10px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="txtPassword" runat="server"
                                        CssClass="text-box-input" Password="true" ClientInstanceName="clientPass">
                                        <ClientSideEvents GotFocus="ClearText" />
                                        <FocusedStyle CssClass="focus-text-box-input" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Zapomni si geslo" Name="RememberMe" Paddings-PaddingTop="10px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="rememberMeCheckBox" runat="server" ToggleSwitchDisplayMode="Always" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Name="SignUp"  ShowCaption="False" Paddings-PaddingTop="20px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <div class="row">
                                        <div class="col-xs-6">
                                            <dx:ASPxButton ID="ASPxButton7" runat="server" Text="Ključek" Width="100"
                                                AutoPostBack="false" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="onClick_Forward" />
                                                <Image Url="Images/key.png" UrlHottracked="Images/keyHoover.png" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-xs-6 text-right">
                                            <dx:ASPxButton ID="ASPxButton8" runat="server" Text="PRIJAVA" Width="100"
                                                AutoPostBack="false" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="CauseValidation" />
                                                 <Image Url="Images/lock2.png" UrlHottracked="Images/lock2Hoover.png" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>

                                    <dx:ASPxCallback ID="LoginCallback" runat="server" OnCallback="LoginCallback_Callback"
                                        ClientInstanceName="clientLoginCallback">
                                        <ClientSideEvents EndCallback="EndLoginCallback" />
                                    </dx:ASPxCallback>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>

                <dx:LayoutGroup Name="LOGIN" GroupBoxDecoration="HeadingLine" Caption="Prijava s ključkom" UseDefaultPaddings="false" GroupBoxStyle-Caption-BackColor="WhiteSmoke" ClientVisible="false">
                    <Items>
                         <dx:LayoutItem Caption="Error label caption" Name="ErrorLabelCaption" ShowCaption="False"
                            CaptionSettings-VerticalAlign="Middle">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxLabel ID="lblErrorKey" runat="server" Text="" ForeColor="Red"
                                        ClientInstanceName  ="clientErrorLabelKey">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Ključek" Name="Password" CaptionSettings-VerticalAlign="Middle" Paddings-PaddingBottom="10px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="ASPxTextBox2" runat="server"
                                        CssClass="text-box-input" Password="true" ClientInstanceName="clientToken">
                                        <ClientSideEvents ValueChanged="OnValueChanged_txtToken" />
                                        <FocusedStyle CssClass="focus-text-box-input" />
                                    </dx:ASPxTextBox>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Name="SignUp" HorizontalAlign="Right" ShowCaption="False" Paddings-PaddingTop="20px">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>

                                    <dx:ASPxButton ID="ASPxButton6" runat="server" Text="Uporabniško ime/Geslo" Width="100"
                                        AutoPostBack="false" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="onClick_Backward" />
                                        <Image Url="Images/lock2.png" UrlHottracked="Images/lock2Hoover.png" />
                                    </dx:ASPxButton>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
        <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="clientLoadingPanel" runat="server" Modal="true">
        </dx:ASPxLoadingPanel>
    </div>

    <div id="HomeContent" runat="server">
        <h2>KVP Sistem</h2>
        <div class="row">
            <div class="col-sm-6">
                <div class="well" style="background-color: #ffffff; padding: 0;">
                    <div style="border-bottom: 1px solid #e1e1e1; padding: 2px 0 0 5px">
                        <h4 style="margin-top: 10px;">Obvestila</h4>
                    </div>
                    <asp:MultiView ID="MultiViewPost" runat="server">
                    </asp:MultiView>
                    <br />
                    <div style="display: flex; justify-content: center;">
                        <dx:ASPxPager ID="ASPxPager" runat="server" ItemsPerPage="1" OnPageIndexChanged="ASPxPager_PageIndexChanged">
                            <LastPageButton Visible="True" />
                            <FirstPageButton Visible="True" />
                            <Summary Position="Inside" Text="Stran {0} of {1} " />
                        </dx:ASPxPager>
                    </div>
                    <%--<div class="well" style="border-top: 2px solid #DCDCDC; margin: 10px; background-color: #fff; padding: 0;">
                        <div style="border-bottom: 1px solid #e1e1e1; padding: 2px 0 0 5px">
                            <h5>Uvajanje eKVP sistema</h5>
                        </div>
                        <div style="padding: 15px;">
                            <p style="font-size: 12px;">
                                <strong>1.1.2019:8:00 - uvodno obvestilo</strong>
                            </p>
                            <p>
                                Z letošnjim letom uvajamo elektronsko aplikacijo za obravnavo in sledenje vaših KVP predlogov. Iz stare baze so bili preneseni vsi potrjeni in nerealizirani predlogi od 1.1.2016 dalje.
Nove lahko še vedno pišete na obrazec in oddate v nabiralnik, svojemu nadrejenemu ali championu. Lahko pa uporabite svojo prijavo v e-KVP in predlog direktno vpišete v sistem. 
Sistem omogoča tudi enostavno sledenje statusu vaših predlogov in stanju točk realiziranih KVPjev.
                                <br>
                                <br>
                                Veselimo se vaših novih idej

                            </p>
                        </div>
                    </div>--%>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="well" style="background-color: #ffffff;">
                    <div class="panel-group">
                        <div class="panel panel-default">
                            <div class="panel-heading">Opravila</div>
                            <div class="panel-body"> 
                                <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Nov KVP" RenderMode="Button" ImagePosition="Top" CssClass="instruction-buttons tasks-buttons small-margin-t"
                                    AutoPostBack="false" UseSubmitBehavior="false" Width="180px">
                                    <ClientSideEvents Click="function(s,e){ window.location.assign('KVPDocuments/KVPDocumentForm.aspx?action=1&recordId=0'); }" />
                                    <Image Url="Images/add.png" UrlHottracked="Images/addHover.png" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Nov Rdeči karton" RenderMode="Button" ImagePosition="Top" CssClass="instruction-buttons tasks-buttons small-margin-t"
                                    AutoPostBack="false" UseSubmitBehavior="false" Width="180px" ClientEnabled="true">
                                    <ClientSideEvents Click="function(s,e){ window.location.assign('KVPDocuments/KVPDocumentRedCardForm.aspx?action=1&recordId=0'); }" />
                                    <Image Url="Images/redcard.png" UrlHottracked="Images/redcardHoover.png" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton5" runat="server" Text="Moji KVP-ji" RenderMode="Button" ImagePosition="Top" CssClass="instruction-buttons tasks-buttons small-margin-t"
                                    AutoPostBack="false" UseSubmitBehavior="false" Width="180px">
                                    <ClientSideEvents Click="function(s,e){ window.location.assign('Dashboard.aspx'); }" />
                                    <Image Url="Images/presoja.png" UrlHottracked="Images/presojaHoover.png" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton9" runat="server" Text="Moji RK-ji" RenderMode="Button" ImagePosition="Top" CssClass="instruction-buttons tasks-buttons small-margin-t"
                                    AutoPostBack="false" UseSubmitBehavior="false" Width="180px" ClientEnabled="true">
                                    <ClientSideEvents Click="function(s,e){ window.location.assign('DashboardRedCards.aspx'); }" />
                                    <Image Url="Images/myRedCard.png" UrlHottracked="Images/presojaHoover.png" />
                                </dx:ASPxButton>

                            </div>
                        </div>
                        <div class="panel panel-default" style="display: none;" id="kodeksChanges" runat="server">
                            <div class="panel-heading">Spremembe CODEKS</div>
                            <div class="panel-body">
                                <div class="row2 big-margin-b">
                                    <div class="col-lg-4 align-item-centerV-centerH">
                                        <dx:ASPxButton ID="btnNewEmployees" runat="server" Text="<span class='badge' style='margin-bottom: 8px; color:white'>X</span> <br /> Novi zaposleni"
                                            RenderMode="Button" EncodeHtml="false" Font-Bold="true" ClientVisible="false" Width="180px" Height="70px"
                                            AutoPostBack="false" UseSubmitBehavior="false" BackColor="#FF8762" CssClass="no-background-image" ForeColor="White" Border-BorderColor="#FFBEAE">
                                            <HoverStyle BackgroundImage-ImageUrl="//" BackColor="#FFBEAE"></HoverStyle>
                                            <ClientSideEvents Click="function(s,e){ window.location.assign('/KVPGroups/EmployeesKVPGroups.aspx?newEmployees=true'); }" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-lg-4 align-item-centerV-centerH">
                                        <dx:ASPxButton ID="btnDeletedEmployees" runat="server" Text="<span class='badge' style='margin-bottom: 8px; color:white'>X</span> <br /> Novi zaposleni"
                                            RenderMode="Button" EncodeHtml="false" Font-Bold="true" ClientVisible="false" Width="180px" Height="70px"
                                            AutoPostBack="false" UseSubmitBehavior="false" BackColor="#8AD0EA" CssClass="no-background-image" ForeColor="White" Border-BorderColor="#8AD0EA">
                                            <HoverStyle BackgroundImage-ImageUrl="//" BackColor="#B6E0EE"></HoverStyle>
                                            <ClientSideEvents Click="function(s,e){ window.location.assign('/KVPGroups/EmployeesKVPGroups.aspx?activeTab=#EmployeesKVPGroupRemove'); }" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-lg-4 align-item-centerV-centerH">
                                        <dx:ASPxButton ID="btnEmployeeLeft" runat="server" Text="<span class='badge' style='margin-bottom: 8px; color:white'>X</span> <br /> Zapustili <br /> podjetje"
                                            RenderMode="Button" EncodeHtml="false" Font-Bold="true" ClientVisible="false" Width="180px" Height="70px"
                                            AutoPostBack="false" UseSubmitBehavior="false" BackColor="#FDA50F" CssClass="no-background-image" ForeColor="White" Border-BorderColor="#FDA50F">
                                            <HoverStyle BackgroundImage-ImageUrl="//" BackColor="#EB9605"></HoverStyle>
                                            <ClientSideEvents Click="function(s,e){ window.location.assign('/Employees/Employees.aspx?notEmployedAnymore=true'); }" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>

                                <div class="row2">
                                    <div class="col-lg-4 align-item-centerV-centerH">
                                        <dx:ASPxButton ID="btnEmployeeNameChanged" runat="server" Text="<span class='badge' style='margin-bottom: 8px; color:white'>X</span> <br /> Spremembe <br/>osebnih podatkov"
                                            RenderMode="Button" EncodeHtml="false" Font-Bold="true" ClientVisible="false" Width="180px" Height="70px"
                                            AutoPostBack="false" UseSubmitBehavior="false" BackColor="#B197DE" CssClass="no-background-image" ForeColor="White" Border-BorderColor="#B197DE">
                                            <HoverStyle BackgroundImage-ImageUrl="//" BackColor="#D1C5E9"></HoverStyle>
                                            <ClientSideEvents Click="function(s,e){ window.location.assign('/Employees/Employees.aspx?nameChanged=true'); }" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-lg-4 align-item-centerV-centerH">
                                        <dx:ASPxButton ID="btnEmployeeDuplicated" runat="server" Text="<span class='badge' style='margin-bottom: 8px; color:white'>X</span> <br /> Zaposleni podvojeni"
                                            RenderMode="Button" EncodeHtml="false" Font-Bold="true" ClientVisible="false" Width="180px" Height="70px"
                                            AutoPostBack="false" UseSubmitBehavior="false" BackColor="#6DC7BD" CssClass="no-background-image" ForeColor="White" Border-BorderColor="#6DC7BD">
                                            <HoverStyle BackgroundImage-ImageUrl="//" BackColor="#8AD2C7"></HoverStyle>
                                            <ClientSideEvents Click="function(s,e){ window.location.assign('/Employees/Employees.aspx?isDuplicated=true'); }" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="panel panel-default">
                            <div class="panel-heading">Navodila</div>
                            <div class="panel-body">
                                <dx:ASPxCallbackPanel runat="server" ID="GenerateHelperButtonsCallbackPanel" OnCallback="GenerateHelperButtonsCallbackPanel_Callback"
                                    ClientInstanceName="clientGenerateHelperButtonsCallbackPanel">
                                    <ClientSideEvents EndCallback="GenerateHelperButtonsCallbackPanel_EndCallback" />
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <%-- ADD Buttons based on uploaded documents --%>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Kako napišem KVP?" RenderMode="Button" ImagePosition="Top" CssClass="instruction-buttons"
                                    AutoPostBack="false" UseSubmitBehavior="false" Width="180px" Visible="false">
                                    <Image Url="Images/questionMark.png" UrlHottracked="Images/questionMarkHover.png" Width="25px" />
                                    <ClientSideEvents Click="function(s,e){ window.open('/Documents/e-KVP sistem - pdf.pdf', '_blank'); }" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Kaj je in kaj ni KVP?" RenderMode="Button" ImagePosition="Top" CssClass="instruction-buttons"
                                    AutoPostBack="false" UseSubmitBehavior="false" Width="180px" Visible="false">
                                    <Image Url="Images/questionMark.png" UrlHottracked="Images/questionMarkHover.png" Width="25px" />
                                    <ClientSideEvents Click="function(s,e){ window.open('/Documents/kaj_je_in_kaj_ni_KVP.pdf', '_blank'); }" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Unhandled exception Modal -->
    <div id="unhandledExpModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: red; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">error_outline</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Napaka!</h3>
                    <p>Sistem je naletel na napako. Naša ekipa razvijalcev je že dobila obvestilo o napaki in je v čakalni vrsti za odpravljanje. Za to se vam iskreno opravičujemo in vas lepo pozdravljamo.</p>
                </div>
            </div>

        </div>
    </div>

    <!-- new kvp no leader exception Modal -->
    <div id="newkvpnowleaderex" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: red; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">error_outline</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Pozor!</h3>
                    <p>Sistem je naletel težavo, za vašega uporabnika ni nastavljenega vsaj enega vodje. PROSIM obvestite skrbnika sistema eKVP</p>
                </div>
            </div>

        </div>
    </div>

    <!-- Session end Modal -->
    <div id="sessionEndModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: yellow; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">warning</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Potek seje!</h3>
                    <p>Zaradi neaktivnosti vas je sistem samodejno odjavil.</p>
                </div>
            </div>

        </div>
    </div>

    <!-- Handled exception Modal -->
    <div id="handledExpModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header text-center" style="background-color: #bce8f1; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <div><i class="material-icons" style="font-size: 48px; color: white">error_outline</i></div>
                </div>
                <div class="modal-body text-center">
                    <h3>Opozorilo!</h3>
                    <p id="handledExpBodyText"></p>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
