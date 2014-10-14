<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageGroup.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.ManageGroup" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="ManageGroup">
	<div class="leader-actions">
		<asp:LinkButton ID="cmdManageUsers" runat="server" CssClass="CommandButton" resourcekey="cmdManageUsers" OnClick="CmdManageUsersClick" />
		<asp:LinkButton ID="cmdManageMeetings" runat="server" CssClass="CommandButton" resourcekey="cmdManageMeetings" OnClick="CmdManageMeetingsClick" />
	</div>
	<div id="divUserGroupID" runat="server" visible="false">
		<asp:label id="lblUserGroupID" runat="server" />
		<asp:TextBox ID="txtUserGroupID" runat="server" />
	</div>	
	<div class="item-wrapper">
		<dnn:label id="dnnlblName" runat="server" controlname="txtName" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtName" runat="server" CssClass="NormalTextBox right long" MaxLength="50" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblCountry" runat="server" controlname="lblCountry" suffix=":" CssClass="SubHead" />
		<telerik:RadComboBox ID="rcbCountry" runat="server" DataTextField="Text" DataValueField="Value" Width="254" AutoPostBack="true" OnSelectedIndexChanged="RcbCountrySelectedIndexChanged" CssClass="right" />
		<asp:TextBox ID="txtCountry" runat="server" CssClass="NormalTextBox right long" Visible="false" MaxLength="50" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblRegion" runat="server" controlname="lblRegion" suffix=":" CssClass="SubHead" />
		<telerik:RadComboBox ID="rcbRegion" runat="server" DataTextField="Text" DataValueField="Value" Width="254" CssClass="right" />
		<asp:TextBox ID="txtRegion" runat="server" CssClass="NormalTextBox right long" Visible="false" MaxLength="50" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblCity" runat="server" controlname="txtCity" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtCity" runat="server" CssClass="NormalTextBox right long" MaxLength="50" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblLanguageID" runat="server" controlname="rcbLanguageID" suffix=":" CssClass="SubHead" />
		<telerik:RadComboBox ID="rcbLanguageID" runat="server" DataTextField="Language" DataValueField="LanguageID" Width="254" CssClass="right" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblMeetingAddy" runat="server" controlname="rtMeetingAddress" suffix=":" CssClass="SubHead" />
		<telerik:RadTextBox TextMode="MultiLine" MaxLength="250" ID="rtMeetingAddress" runat="server" CssClass="NormalTextBox right long" Width="250px" />
	</div>
	<div class="item-wrapper" visible="false" runat="server" id="divLat">
		<dnn:label id="dnnlblLatitude" runat="server" controlname="txtLatitude" suffix=":" CssClass="SubHead" />
		<telerik:RadNumericTextBox ID="rntxtLatitude" runat="server" ShowSpinButtons="false" Width="250" CssClass="right" MinValue="-90" MaxValue="90" Type="Number" />
	</div>	
	<div class="item-wrapper" visible="false" runat="server" id="divLong">
		<dnn:label id="dnnlblLongitude" runat="server" controlname="txtLongitude" suffix=":" CssClass="SubHead" />
		<telerik:RadNumericTextBox ID="rntxtLongitude" runat="server" ShowSpinButtons="false" Width="250" CssClass="right" MinValue="-180" MaxValue="180" Type="Number" />
	</div>	
	<div class="item-wrapper">
		<dnn:label id="dnnlblUrl" runat="server" controlname="txtUrl" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtUrl" runat="server" CssClass="NormalTextBox right long" MaxLength="100" />
	</div>
	<div class="item-wrapper" id="divLogo" runat="server" visible="false">
		<dnn:label id="dnnlblLogo" runat="server" controlname="txtLogo" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtLogo" runat="server" CssClass="NormalTextBox right long" MaxLength="100" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblLinkedInUrl" runat="server" controlname="txtLinkedInUrl" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtLinkedInUrl" runat="server" CssClass="NormalTextBox right long" MaxLength="100"  />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblFacebookUrl" runat="server" controlname="txtFacebookUrl" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtFacebookUrl" runat="server" CssClass="NormalTextBox right long" MaxLength="100" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblTwitterUrl" runat="server" controlname="txtTwitterUrl" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtTwitterUrl" runat="server" CssClass="NormalTextBox right long" MaxLength="100" />
	</div>
	<div id="divMembers" runat="server" class="item-wrapper">
		<dnn:label id="dnnlblMembers" runat="server" controlname="lblMembers" suffix=":" CssClass="SubHead" />
		<asp:Label ID="lblMembers" runat="server" CssClass="Normal right long" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblAbout" runat="server" controlname="rtAbout" suffix=":" CssClass="SubHead" />
		<telerik:RadTextBox TextMode="MultiLine" MaxLength="200" ID="rtAbout" runat="server" CssClass="NormalTextBox right long narrow" Width="250px" />
	</div>
	<div class="item-wrapper">
		<dnn:label id="dnnlblDefaultLanguage" runat="server" controlname="txtFlag" suffix=":" CssClass="SubHead" />
		<asp:TextBox ID="txtFlag" runat="server" CssClass="NormalTextBox right long" MaxLength="10" />
	</div>
	<div id="divActive" runat="server" class="item-wrapper">
		<dnn:label id="dnnlblActive" runat="server" controlname="chkActive" suffix=":" CssClass="SubHead" />
		<asp:CheckBox ID="chkActive" runat="server" CssClass="NormalTextBox right long" />
	</div>
	<div id="divLeader" runat="server" class="item-wrapper">
		<dnn:label id="dnnlblLeaderID" runat="server" controlname="rcbLeaderID" suffix=":" CssClass="SubHead" />
		<telerik:RadComboBox ID="rcbLeaderID" runat="server" DataTextField="Username" DataValueField="UserID" Width="254" CssClass="right" />
		<asp:TextBox ID="txtLeaderID" runat="server" Visible="false" />
		<asp:TextBox ID="txtLeaderUsername" runat="server" CssClass="NormalTextBox right long" MaxLength="200" />
		<asp:LinkButton ID="cmdAdd" runat="server" CssClass="CommandButton right" resourcekey="cmdAdd" OnClick="CmdAddUserClick" />
	</div>
	<div id="divCreated" runat="server" class="item-wrapper">
		<dnn:label id="dnnlblCreatedOnDate" runat="server" controlname="lblCreatedOnDate" suffix=":" CssClass="SubHead" />
		<asp:Label ID="lblCreatedOnDate" runat="server" CssClass="Normal right long" />
	</div>
	<div id="divModified" runat="server" class="item-wrapper">
		<dnn:label id="dnnlblLastModifiedOnDate" runat="server" controlname="lblLastModifiedOnDate" suffix=":" CssClass="SubHead" />
		<asp:Label ID="lblLastModifiedOnDate" runat="server" CssClass="Normal right long" />
	</div>
	<div id="divModifiedUser" runat="server" class="item-wrapper">
		<dnn:label id="dnnlblLastModifiedUser" runat="server" controlname="lblLastModifiedUser" suffix=":" CssClass="SubHead" />
		<asp:Label ID="lblLastModifiedUser" runat="server" CssClass="Normal right long" />
	</div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdSave" runat="server" CssClass="CommandButton primary-action" resourcekey="cmdSave" OnClick="CmdSaveClick" />
		<asp:LinkButton ID="cmdDelete" runat="server" CssClass="CommandButton secondary-action" resourcekey="cmdDelete" OnClick="CmdDeleteClick" />
		<asp:LinkButton ID="cmdCancel" runat="server" CssClass="CommandButton secondary-action" resourcekey="cmdCancel" OnClick="CmdCancelClick" />
	</div>
	<div class="messages">
		<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" />
	</div>
</div>