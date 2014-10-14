<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageGroupMeetings.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.ManageGroupMeetings" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="ManageGroupMeetings">
	<div class="leader-actions">
		<asp:LinkButton ID="cmdAddEvent" runat="server" CssClass="CommandButton" resourcekey="cmdAddEvent" OnClick="CmdCancelClick" />
	</div>
	<div id="divID" runat="server" visible="false">
		<asp:label id="dnnlblUserGroupID" runat="server" />
		<asp:TextBox ID="txtUserGroupID" runat="server" />
	</div>	
	<div class="item-wrapper">
		<asp:Label ID="dnnlblGroup" runat="server" CssClass="SubHead" resourcekey="lblGroup" />
		<asp:Label ID="lblGroupName" runat="server" CssClass="NormalBold" />
	</div>
	<div class="div-grid">
		<telerik:RadGrid ID="rgMeetings" runat="server" AutoGenerateColumns="false" GridLines="None" Width="600px" OnItemCommand="RgMeetingsItemCommand" OnItemDataBound="RgMeetingsItemDataBound" >
			<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false"></ClientSettings>
			<MasterTableView DataKeyNames="MeetingID,UserGroupID" AllowNaturalSort="false">
				<Columns>
					<telerik:GridButtonColumn UniqueName="EditItem" CommandName="EditItem" ButtonType="ImageButton" ImageUrl="~/images/edit.gif" HeaderStyle-Width="26px" ItemStyle-Width="26px" />
					<telerik:GridButtonColumn UniqueName="DeleteItem" CommandName="DeleteItem" ButtonType="ImageButton" ImageUrl="~/images/delete.gif" HeaderStyle-Width="26px" ItemStyle-Width="26px" />
					<telerik:GridHyperLinkColumn UniqueName="Title" HeaderText="Title" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataTextField="Title" ItemStyle-Width="200" HeaderStyle-Width="200" />
					<telerik:GridBoundColumn UniqueName="Location" HeaderText="Location" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="Location" />
					<telerik:GridBoundColumn UniqueName="Date" HeaderText="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="MeetingDate" ItemStyle-Width="150" HeaderStyle-Width="150" />
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
	</div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdCancel" runat="server" CssClass="CommandButton" resourcekey="cmdCancel" OnClick="CmdCancelClick" />
	</div>
</div>