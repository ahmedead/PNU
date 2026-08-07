<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LabelMessage.ascx.cs" Inherits="Portal.Main.WebApp.Controls.Common.LabelMessage" %>
<asp:Repeater runat="server" ID="rptMessages">
    <ItemTemplate>
		<div class="<%# Eval("DivCss") %>">
			
			<p><%# Eval("Message") %></p>
		</div>
    </ItemTemplate>
</asp:Repeater>
