<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsPost.ascx.cs" Inherits="KVP_Obrazci.Widgets.NewsPost" %>

<div class="well" style="border-top: 2px solid #DCDCDC; margin: 10px; background-color: #fff; padding: 0;">
    <div style="border-bottom: 1px solid #e1e1e1; padding: 2px 0 0 5px">
        <h5 runat="server" id="postTitle"></h5>
    </div>
    <div style="padding: 15px;">
        <p style="font-size: 12px;" id="metaDataParagraph" runat="server">
        </p>
        <p id="bodyPost" runat="server">
        </p>
    </div>
</div>
