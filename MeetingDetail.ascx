<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeetingDetail.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.MeetingDetail" %>
<asp:Literal ID="litMapScript" runat="server" />
<div class="MeetingDetail">
	<div id="divGroupID" runat="server" visible="false">
		<asp:TextBox ID="txtGroupID" runat="server" />
	</div>	
	<div class="item-wrapper">
		<asp:Label id="lblGroupName" runat="server" CssClass="NormalBold" resourcekey="lblGroupName" />
		<asp:Label ID="GroupName" runat="server" CssClass="long" />
	</div>
	<div class="item-wrapper">
		<asp:Label id="lblTitle" runat="server" CssClass="NormalBold" resourcekey="lblTitle" />
		<asp:Label ID="Title" runat="server" CssClass="long" />
	</div>
	<div class="item-wrapper">
		<asp:Label id="lblDescription" runat="server" CssClass="NormalBold" resourcekey="lblDescription" />
		<asp:Label ID="Description" runat="server" />
	</div>
	<div class="item-wrapper">
		<asp:Label id="lblLocation" runat="server" CssClass="NormalBold" resourcekey="lblLocation" />
		<asp:Label ID="Location" runat="server" />
	</div>
	<div class="item-wrapper">
		<div id="map_canvas" class="ug-map"></div>
	</div>
	<div class="item-wrapper">
		<asp:Label id="lblMeetingDate" runat="server" CssClass="NormalBold" resourcekey="lblMeetingDate" />
		<asp:Label ID="MeetingDate" runat="server" />
	</div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdGroupProfile" runat="server" resourcekey="cmdGroupProfile" CssClass="CommandButton" OnClick="CmdGroupProfileClick" />
	</div>
</div>