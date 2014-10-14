<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageGroupUsers.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.ManageGroupUsers" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="ManageGroupUsers">
	<div id="divID" runat="server" visible="false">
		<asp:label id="dnnlblUserGroupID" runat="server" />
		<asp:TextBox ID="txtUserGroupID" runat="server" />
	</div>	
	<div class="item-wrapper">
		<asp:Label ID="lblGroup" runat="server" CssClass="SubHead" resourcekey="lblGroup" />
		<asp:Label ID="lblGroupName" runat="server" CssClass="NormalBold" />
	</div>
	<div class="div-grid">
		<telerik:RadGrid ID="rgMembers" runat="server" AutoGenerateColumns="false" GridLines="None" Width="600px" OnItemDataBound="RgMembersItemDataBound" OnItemCommand="RgMembersItemCommand" >
			<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false"></ClientSettings>
			<MasterTableView DataKeyNames="UserID,Username,DisplayName,Officer,LeaderID,UserGroupID" AllowNaturalSort="false">
				<Columns>
					<telerik:GridButtonColumn UniqueName="DeleteItem" CommandName="DeleteItem" ButtonType="ImageButton" ImageUrl="~/images/delete.gif" HeaderStyle-Width="26px" ItemStyle-Width="26px" />
					<telerik:GridHyperLinkColumn UniqueName="Username" HeaderText="Username" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Target="_blank" DataTextField="Username" />
					<telerik:GridBoundColumn UniqueName="DisplayName" HeaderText="DisplayName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="DisplayName" />
					<telerik:GridBoundColumn UniqueName="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" DataField="Email" />
					<telerik:GridButtonColumn UniqueName="Officer" CommandName="Officer" HeaderText="Officer" ButtonType="ImageButton" HeaderStyle-Width="35px" ItemStyle-Width="35px" ItemStyle-HorizontalAlign="Center" />
				</Columns>
			</MasterTableView>
		</telerik:RadGrid>
	</div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdCancel" runat="server" CssClass="CommandButton" resourcekey="cmdCancel" OnClick="CmdCancelClick" />
	</div>
</div>