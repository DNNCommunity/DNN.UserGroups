<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactOfficers.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.ContactOfficers" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="ContactOfficers">
	<div class="item-wrapper" id="divID" visible="false" runat="server">
		<asp:TextBox ID="txtGroupID" runat="server" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblGroupName" runat="server" controlname="lblGroupName" suffix=":" CssClass="SubHead" />
		<asp:Label ID="lblGroupName" runat="server" CssClass="NormalBold right long" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblMessage" runat="server" controlname="txtMessage" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="NormalTextBox right long narrow" />
	</div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdSendMessage" runat="server" resourcekey="cmdSendMessage" CssClass="CommandButton primary-action" OnClick="CmdSendClick" />
		<asp:LinkButton ID="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="CommandButton secondary-action" OnClick="CmdCancelClick" />
	</div>
	<div class="messages">
		<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" />
	</div>
</div>