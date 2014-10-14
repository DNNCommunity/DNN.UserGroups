<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DirectorySearch.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.DirectorySearch" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="DirectorySearch">
	<div id="div-filters">
		<div id="divCountry" runat="server" class="left">
			<asp:Label ID="lblCountry" runat="server" resourcekey="lblCountry" CssClass="SubHead" />
			<telerik:RadComboBox ID="rcbCountry" runat="server" DataTextField="Text" DataValueField="Value" Width="200" AutoPostBack="true" OnSelectedIndexChanged="RcbCountrySelectedIndexChanged" />
		</div>
		<div id="divRegion" runat="server" class="right">
			<asp:Label ID="lblRegion" runat="server" resourcekey="lblRegion" CssClass="SubHead" />
			<telerik:RadComboBox ID="rcbRegion" runat="server" DataTextField="Text" DataValueField="Value" Width="200" AutoPostBack="true" OnSelectedIndexChanged="RcbRegionSelectedIndexChanged" />
		</div>
		<div id="divLanguage" runat="server" class="right">
			<asp:Label ID="lblLanguage" runat="server" resourcekey="lblLanguage" CssClass="SubHead" />
			<telerik:RadComboBox ID="rcbLanguage" runat="server" DataTextField="Language" DataValueField="LanguageID" Width="200" AutoPostBack="true" OnSelectedIndexChanged="RcbLanguageSelectedIndexChanged" />
		</div>
	</div>
	<div id="divAdmin" runat="server" class="div-admin">
		<div class="left">
			<asp:Label ID="lblGroupName" runat="server" resourcekey="lblGroupName" CssClass="SubHead" />
			<asp:TextBox ID="txtGroupName" runat="server" MaxLength="50" CssClass="NormalTextBox" Width="166" />
		</div>
		<div class="right">
			<asp:CheckBox ID="chkStatus" runat="server" CssClass="NormalTextBox" resourcekey="chkStatus" />
		</div>
		<div>
			<br /><br />
			<asp:LinkButton ID="cmdSearch" runat="server" CssClass="CommandButton" resourcekey="cmdSearch" OnClick="CmdSearchClick" />
		</div>
	</div>
	<div id="div-grid">
		<telerik:RadGrid ID="rgUserGroups" runat="server" AutoGenerateColumns="false" GridLines="None" Width="620px" OnItemDataBound="RgUserGroupsItemDataBound" OnItemCommand="RgUserGroupsItemCommand" >
			<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false">
			</ClientSettings>
			<MasterTableView DataKeyNames="UserGroupID,LeaderID,LeaderUsername,LeaderDisplayName" AllowNaturalSort="false">
				<Columns>
					<telerik:GridButtonColumn UniqueName="EditItem" CommandName="EditItem" ButtonType="ImageButton" ImageUrl="~/images/edit.gif" HeaderStyle-Width="26px" ItemStyle-Width="26px" />
					<telerik:GridHyperLinkColumn UniqueName="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Target="_blank" DataTextField="Name" />
					<telerik:GridBoundColumn UniqueName="City" HeaderText="City" DataField="City" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
					<telerik:GridBoundColumn UniqueName="Members" HeaderText="Members" DataField="Members" HeaderStyle-Width="32px" ItemStyle-Width="32px" />
					<telerik:GridHyperLinkColumn UniqueName="Leader" HeaderText="Leader" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Target="_blank" />
					<telerik:GridHyperLinkColumn UniqueName="Join" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
					<telerik:GridCheckBoxColumn UniqueName="Active" HeaderText="Active" DataField="Active"  HeaderStyle-Width="32px" ItemStyle-Width="32px" /> 
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
	</div>
	<div class="instructions">
		<asp:Literal ID="litInstructions" runat="server" />
	</div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdAddGroup" runat="server" CssClass="CommandButton" resourcekey="cmdAddGroup" OnClick="CmdAddGroupClick" visible="false" />
		<asp:LinkButton runat="server" ID="cmdMyGroup" CssClass="CommandButton" resourcekey="cmdMyGroup" OnClick="CmdMyGroupClick"  />
	</div>
</div>