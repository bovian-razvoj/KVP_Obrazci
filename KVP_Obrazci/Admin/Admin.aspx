<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="KVP_Obrazci.Admin.Admin" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <div class="AddEditButtonsWrap">
                <span class="AddEditButtons">
                    <dx:ASPxButton ID="btnRun" runat="server" Text="Zaženi" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnRun_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnZeInsertirane" runat="server" Text="Zaženi že dodane" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnZeInsertirane_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <br />
                    <dx:ASPxButton ID="btnUporabnik" runat="server" Text="Uporabnik" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUporabnik_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnPresoja" runat="server" Text="Presoja" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnPresoja_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnVodja" runat="server" Text="Vodja" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnVodja_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnRealizator" runat="server" Text="Realizator" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnRealizator_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnMergeKVP" runat="server" Text="Merge KVP" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnMergeKVP_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnUpdateLok" runat="server" Text="Update Loka, Stroj, Linija" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUpdateLok_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnSetPresoje" runat="server" Text="Update presoje" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnSetPresoje_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnSetDatumZakljuceneIdeje" runat="server" Text="Update zakl. ideja" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnSetDatumZakljuceneIdeje_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnSetDatumZakljuceneIdejeRK" runat="server" Text="Update zakl. ideja RK" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnSetDatumZakljuceneIdejeRK_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <br />
                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Size="12px" Text="V bazi : "></dx:ASPxLabel>
                    <dx:ASPxTextBox runat="server" ID="txtImeVBazi"
                        CssClass="text-box-input" Font-Size="13px" Width="100%">
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        <ClientSideEvents Init="SetFocus" />
                    </dx:ASPxTextBox>
                    <br />
                    <br />
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="Zamenjaj z : "></dx:ASPxLabel>
                    <dx:ASPxTextBox runat="server" ID="txtMenjaj"
                        CssClass="text-box-input" Font-Size="13px" Width="100%">
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                        <ClientSideEvents Init="SetFocus" />
                    </dx:ASPxTextBox>

                    <%-- <dx:ASPxButton ID="btnUpdateTocke" runat="server" Text="Update točke" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUpdateTocke_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnUpdateUserName" runat="server" Text="Posodobitev uporabnikov" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUpdateUserName_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnInsertPlan" runat="server" Text="Uvoz Plan Realizacija" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnInsertPlan_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnUpdateKVPSkupina" runat="server" Text="Update KVP doc skupina" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUpdateKVPSkupina_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                     <dx:ASPxButton ID="btnIzplacila" runat="server" Text="Update Izplačila ime in priimek = null" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnIzplacila_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                     <dx:ASPxButton ID="btnIzplacilaUpdateMesec" runat="server" Text="Update Mesec izplačila" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnIzplacilaUpdateMesec_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                     <dx:ASPxButton ID="btnCheckStatusOnKVP" runat="server" Text="Preveri KVP Status" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCheckStatusOnKVP_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                         <dx:ASPxButton ID="btnUpdateIzplacilaMarec" runat="server" Text="Update izplačila - MAREC" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUpdateIzplacilaMarec_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnPreveriIzplacila" runat="server" Text="Preveri izplacila" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnPreveriIzplacila_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                     <dx:ASPxButton ID="btnUpdateZapadliRK" runat="server" Text="Update zapadli RK" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUpdateZapadliRK_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>--%>

                    <dx:ASPxButton ID="btnDeleteDuplicateUsers" runat="server" Text="Zbriši podvojene" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnDeleteDuplicateUsers_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnGenerateListItemsEmail" runat="server" Text="Sistemski email" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnGenerateListItemsEmail_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnPreveriIzplacila" runat="server" Text="Preveri izplacila" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnPreveriIzplacila_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

             
                    <dx:ASPxButton ID="btnIzplacila" runat="server" Text="Update Izplačila ime in priimek = null" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnIzplacila_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                     <dx:ASPxButton ID="bntSendMail" runat="server" Text="Uskladitev točk P in R" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="bntUskladitev_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnAnalizaVsehKVPInTock" runat="server" Text="Analiza vseh KVP točk" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnAnalizaVsehKVPInTock_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                     <dx:ASPxButton ID="btnCheckPointSumWithPayout" runat="server" Text="Preveri KVP točk z izplačilom" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnCheckPointSumWithPayout_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>

                     <dx:ASPxButton ID="btnUpdateDecember" runat="server" Text="Posodobi točke december" AutoPostBack="false"
                        Height="30" Width="50" UseSubmitBehavior="false" OnClick="btnUpdateDecember_Click">
                        <Paddings PaddingLeft="10" PaddingRight="10" />
                    </dx:ASPxButton>


                    <br />
                    <dx:ASPxMemo ID="memNotes" runat="server" Width="100%" MaxLength="500000"
                        NullText="Podroben opis..." Rows="16" HorizontalAlign="Left" BackColor="White"
                        CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoProblemDesc">
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                    </dx:ASPxMemo>

                    <br />
                    <dx:ASPxMemo ID="memRezultat" runat="server" Width="100%" MaxLength="500000"
                        NullText="Podroben opis..." Rows="16" HorizontalAlign="Left" BackColor="White"
                        CssClass="text-box-input" Font-Size="14px" ClientInstanceName="clientMemoProblemDesc">
                        <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                    </dx:ASPxMemo>

                </span>
            </div>
        </div>
    </div>

</asp:Content>
