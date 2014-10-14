<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupProfile.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.GroupProfile" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Literal ID="litMapScript" runat="server" />
<div class="usergroup-module">
	<div class="leader-actions">
		<asp:HyperLink ID="hlSendNews" runat="server" resourcekey="hlSendNews" CssClass="CommandButton" />
		<asp:HyperLink ID="hlEditGroup" runat="server" resourcekey="hlEditGroup" CssClass="CommandButton" />
		<asp:HyperLink ID="hlAddEvent" runat="server" CssClass="CommandButton" resourcekey="hlAddEvent" />
		<asp:HyperLink ID="hlMgUsers" runat="server" CssClass="CommandButton" resourcekey="hlMgUsers" />
	</div>
	<div class="ug-info">
		<h2><asp:Label ID="lblGroupName" runat="server" /></h2>
		<h3><asp:Label ID="lblLeader" runat="server" resourcekey="lblLeader" />&nbsp;<asp:HyperLink ID="hlLeader" runat="server" CssClass="leader-link" Target="_blank" /></h3>
		<h3><asp:Label ID="lblUrl" runat="server" resourcekey="lblUrl" />&nbsp;<asp:HyperLink ID="hlUrl" runat="server" CssClass="website-link" Target="_blank" /></h3>
		<h3><asp:Label ID="lblCountry" runat="server" />&nbsp;<asp:Label ID="lblLocation" runat="server" CssClass="location" /></h3>
		<h3><asp:Label ID="lblLanguage" runat="server" resourcekey="lblLanguage" />&nbsp;<asp:Label ID="lblLanguageValue" runat="server" CssClass="lang" /></h3>
		<div class="ug-description"><asp:Label ID="lblAbout" runat="server" /></div>
		<div id="map_canvas" class="ug-map"></div>
	</div>
	<div class="ug-actions">
		<div class="ug-connect">
			<span><asp:Label ID="lblConnect" runat="server" resourcekey="lblConnect" /></span>
			<asp:HyperLink ID="hlGroupProfile" runat="server" CssClass="dnn-link" ><asp:Image ID="imgGroupProfile" runat="server" resourcekey="imgGroupProfile" /></asp:HyperLink>
			<asp:HyperLink ID="hlFacebook" runat="server" CssClass="facebook-link" Target="_blank" ><asp:Image ID="imgFacebook" runat="server" resourcekey="imgFacebook" /></asp:HyperLink>
			<asp:HyperLink ID="hlTwitter" runat="server" CssClass="twitter-link" Target="_blank" ><asp:Image ID="imgTwitter" runat="server" resourcekey="imgTwitter" /></asp:HyperLink>
			<asp:HyperLink ID="hlLinkedIn" runat="server" CssClass="linkedin-link" Target="_blank" ><asp:Image ID="imgLinkedIn" runat="server" resourcekey="imgLinkedIn" /></asp:HyperLink>
		</div>
        	<telerik:RadGrid ID="rgMembers" runat="server" AutoGenerateColumns="false" GridLines="None" OnItemDataBound="RgMembersItemDataBound" Width="218px" AllowSorting="false" >
			<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false">
				<Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" ScrollHeight="380px" ></Scrolling>
			</ClientSettings>
			<MasterTableView DataKeyNames="UserID,Username,DisplayName,Officer,LeaderID" AllowNaturalSort="false" TableLayout="Fixed">
				<Columns>
					<telerik:GridHyperLinkColumn UniqueName="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Target="_blank" HeaderStyle-Width="150px" ItemStyle-Width="150px" />
					<telerik:GridImageColumn UniqueName="Officer" HeaderText="Officer" HeaderStyle-Width="35px" ItemStyle-Width="35px" ItemStyle-HorizontalAlign="Center" />
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
		<asp:HyperLink ID="hlJoin" runat="server" CssClass="join-btn" />
		<asp:HyperLink ID="hlContact" runat="server" CssClass="join-btn" resourcekey="hlContact"  />
    </div>
    <div class="ug-upcoming" id="divEvents" runat="server" visible="false">
		<h2><asp:Label ID="lblEvents" runat="server" resourcekey="lblEvents" /></h2>
		<telerik:RadGrid ID="rgMeetings" runat="server" AutoGenerateColumns="false" GridLines="None" Width="628px" OnItemCommand="RgMeetingsItemCommand" OnItemDataBound="RgMeetingsItemDataBound">
			<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false">
			</ClientSettings>
			<MasterTableView DataKeyNames="MeetingID,UserGroupID" AllowNaturalSort="false">
				<Columns>
					<telerik:GridButtonColumn UniqueName="EditItem" CommandName="EditItem" ButtonType="ImageButton" ImageUrl="~/images/edit.gif" HeaderStyle-Width="26px" ItemStyle-Width="26px" />
					<telerik:GridButtonColumn UniqueName="DeleteItem" CommandName="DeleteItem" ButtonType="ImageButton" ImageUrl="~/images/delete.gif" HeaderStyle-Width="26px" ItemStyle-Width="26px" />
					<telerik:GridHyperLinkColumn UniqueName="Title" HeaderText="Title" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200" HeaderStyle-Width="200" DataTextField="Title" />
					<telerik:GridBoundColumn UniqueName="Location" HeaderText="Location" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="Location" />
					<telerik:GridBoundColumn UniqueName="Date" HeaderText="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="MeetingDate" />
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
	</div>
	<div class="action-btns">
		<asp:HyperLink ID="hlGroupSearch" runat="server" resourcekey="hlGroupSearch" CssClass="CommandButton" />

	</div>
</div>