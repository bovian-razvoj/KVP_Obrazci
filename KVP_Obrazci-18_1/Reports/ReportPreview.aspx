<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ReportPreview.aspx.cs" Inherits="KVP_Obrazci.Reports.ReportPreview" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.1.Web.WebForms, Version=18.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function customizeActions(s, e) {
            e.Actions.push({
                text: "Izhod",
                imageClassName: "exit-logo-html5",
                disabled: ko.observable(false),
                visible: true,
                clickAction: function () {
                    history.back();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="row2">
        <dx:ASPxWebDocumentViewer ID="WebReportViewer" runat="server" ColorScheme="dark" Width="100%">
            <ClientSideEvents Init="function(s, e) {s.previewModel.reportPreview.zoom(1);}" CustomizeMenuActions="customizeActions" />
        </dx:ASPxWebDocumentViewer>
    </div>
</asp:Content>
