<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpotlightSettings.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.SpotlightSettings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="Settings">
	<dl>
		<dt><dnn:label id="dnnlblMeetingMaxDayCount" runat="server" controlname="rntxtMeetingMaxDayCount" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox runat="server" ID="rntxtMeetingMaxDayCount" CssClass="right" Width="50px" MaxLength="3" MinValue="0" MaxValue="999" NumberFormat-DecimalDigits="0" />
		</dd>
		<dt><dnn:label id="dnnlblRequireMeetingAddress" runat="server" controlname="chkRequireMeetingAddress" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:CheckBox ID="chkRequireMeetingAddress" runat="server" CssClass="NormalTextBox right" />
		</dd>
		<dt><dnn:label id="dnnlblGroupPage" runat="server" controlname="rcbGroupPage" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadComboBox id="rcbGroupPage" runat="server" Width="254px" DataTextField="IndentedTabName" DataValueField="TabId" AutoPostBack="true" OnSelectedIndexChanged="RcbGroupPageSelectedIndexChanged" />
		</dd>
		<dt><dnn:label id="dnnlblModule" runat="server" controlname="rcbModule" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadComboBox id="rcbModule" runat="server" Width="254px" DataTextField="ModuleName" DataValueField="ModuleId" />
		</dd>
		<dt><dnn:label id="dnnlblTemplate" runat="server" controlname="txtTemplate" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Height="200px" CssClass="NormalTextBox right" MaxLength="2000" Width="250px" />
		</dd>
		<dt><dnn:label id="lblPageSize" runat="server" controlname="txtPageSize" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="txtPageSize" runat="server" CssClass="NormalTextBox right" MaxLength="2" Width="50px" MinValue="1" MaxValue="99" NumberFormat-DecimalDigits="0"  />
		</dd>
		<dt><dnn:label id="lblWidgetHeight" runat="server" controlname="txtWidgetHeight" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="txtWidgetHeight" runat="server" CssClass="NormalTextBox right" MaxLength="3" Width="50px" MinValue="20" MaxValue="999" NumberFormat-DecimalDigits="0"  />
		</dd>
		<dt><dnn:label id="lblWidgetWidth" runat="server" controlname="txtWidgetWidth" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="txtWidgetWidth" runat="server" CssClass="NormalTextBox right" MaxLength="3" Width="50px" MinValue="20" MaxValue="999" NumberFormat-DecimalDigits="0"  />
		</dd>
		<dt><dnn:label id="lblItemHeight" runat="server" controlname="txtItemHeight" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="txtItemHeight" runat="server" CssClass="NormalTextBox right" MaxLength="3" Width="50px" MinValue="20" MaxValue="999" NumberFormat-DecimalDigits="0"  />
		</dd>
		<dt><dnn:label id="lblItemWidth" runat="server" controlname="txtItemWidth" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="txtItemWidth" runat="server" CssClass="NormalTextBox right" MaxLength="3" Width="50px" MinValue="20" MaxValue="999" NumberFormat-DecimalDigits="0" />
		</dd>
		<dt><dnn:label id="lblFrameDuration" runat="server" controlname="txtFrameDuration" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="txtFrameDuration" runat="server" CssClass="NormalTextBox right" MaxLength="6" Width="50px" MinValue="100" MaxValue="100000" NumberFormat-DecimalDigits="0" />
		</dd>
		<dt><dnn:label id="lblScrollDuration" runat="server" controlname="txtScrollDuration" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="txtScrollDuration" runat="server" CssClass="NormalTextBox right" MaxLength="6" Width="50px" MinValue="100" MaxValue="100000" NumberFormat-DecimalDigits="0" />
		</dd>
	</dl>
</div>