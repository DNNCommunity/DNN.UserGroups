<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaveGroup.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.LeaveGroup" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="LeaveGroup">
	<div class="item-wrapper">
		<asp:Label ID="lblMember" runat="server" CssClass="SubHead left med" resourcekey="lblMember" />
		<asp:Label ID="lblUsername" runat="server" CssClass="Normal right long" />
	</div>
	<div class="item-wrapper">
		<asp:Label ID="lblGroup" runat="server" CssClass="SubHead left med" resourcekey="lblGroup" />
		<asp:Label ID="lblGroupName" runat="server" CssClass="Normal right long" />
	</div>
	<div class="item-wrapper">
		<asp:Label ID="lblReason" runat="server" CssClass="SubHead left med" resourcekey="lblReason" />
		<telerik:RadComboBox ID="rcbReason" runat="server" DataTextField="Reason" DataValueField="ReasonID" Width="254" AutoPostBack="true" OnSelectedIndexChanged="RcbReasonSelectedIndexChanged" CssClass="right long" />
	</div>
	<div class="item-wrapper" runat="server" id="divReasonOther" visible="false">
		<asp:Label ID="lblReasonOther" runat="server" CssClass="SubHead left med" resourcekey="rtReasonOther" />
		<telerik:RadTextBox TextMode="MultiLine" MaxLength="250" ID="rtReasonOther" runat="server" CssClass="NormalTextBox right long narrow" Width="250px" />
	</div>
	<div class="messages"><asp:Label ID="lblMsg" runat="server" CssClass="NormalRed" /></div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdRemove" runat="server" resourcekey="cmdRemove" CssClass="CommandButton primary-action" OnClick="CmdRemoveClick" visible="false" />
		<asp:LinkButton ID="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="CommandButton secondary-action" OnClick="CmdCancelClick" />
	</div>
</div>