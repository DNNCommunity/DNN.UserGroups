<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeetingFinder.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.MeetingFinder" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="MeetingFinder">
	<div>
		<asp:Label ID="lblMeetingStart" runat="server" resourcekey="lblMeetingStart" CssClass="NormalBold" />
		<telerik:RadDatePicker ID="rdpMeetingStart" runat="server" Width="100"  />
		<asp:Label ID="lblMeetingEnd" runat="server" resourcekey="lblMeetingEnd" CssClass="NormalBold" />
		<telerik:RadDatePicker ID="rdpMeetingEnd" runat="server" Width="100" />
		<asp:Label ID="lblRegion" runat="server" resourcekey="lblRegion" CssClass="NormalBold" />
		<telerik:RadComboBox ID="rcbRegion" runat="server" Width="175" DataTextField="Region" DataValueField="Region" />
		<asp:LinkButton ID="cmdGo" runat="server" Text="Go" OnClick="CmdGo_Click" CssClass="CommandButton" />
	</div>
	<div class="div-grid">
		<telerik:RadGrid ID="rgMeetings" runat="server" AutoGenerateColumns="false" GridLines="None" Width="600px" OnItemDataBound="RgMeetings_ItemDataBound" AllowPaging="true" OnPageIndexChanged="RgMeetings_PageChange">
			<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
			</ClientSettings>
			<MasterTableView DataKeyNames="MeetingID,UserGroupID,MapUrl" AllowNaturalSort="false">
				<Columns>
					<telerik:GridHyperLinkColumn UniqueName="Name" HeaderText="Group" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataTextField="Name" />
					<telerik:GridBoundColumn UniqueName="Title" HeaderText="Title" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="Title" ItemStyle-Width="200" HeaderStyle-Width="200" />
					<telerik:GridHyperLinkColumn UniqueName="Location" HeaderText="Location" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataTextField="Location" Target="_blank" />
					<telerik:GridBoundColumn UniqueName="Date" HeaderText="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="MeetingDate" />
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
	</div>
	<div class="action-btns">
		<asp:HyperLink ID="hlGroupSearch" runat="server" resourcekey="hlGroupSearch" CssClass="CommandButton primary-action" />
	</div>
</div>