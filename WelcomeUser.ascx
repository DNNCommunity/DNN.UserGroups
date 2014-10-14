<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WelcomeUser.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.WelcomeUser" %>
<div class="WelcomeUser">
	<p><asp:Label ID="lblJoinMsg" runat="server" CssClass="Normal" /></p>
	<div class="item-wrapper">
		<asp:Label ID="lblMember" runat="server" CssClass="SubHead left" resourcekey="lblMember" />
		<asp:Label ID="lblUsername" runat="server" CssClass="Normal right" />
	</div>
	<div class="item-wrapper">
		<asp:Label ID="lblGroup" runat="server" CssClass="SubHead left" resourcekey="lblGroup" />
		<asp:Label ID="lblGroupName" runat="server" CssClass="Normal right" />
	</div>
	<div class="action-btns">
		<asp:HyperLink ID="hlViewGroup" runat="server" resourcekey="cmdViewGroup" CssClass="CommandButton"  />
	</div>
</div>