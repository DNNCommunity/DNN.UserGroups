<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.Settings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="Settings">
	<dl>
		<dt><dnn:label id="dnnlblAdminEmail" runat="server" controlname="txtAdminEmail" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtAdminEmail" runat="server" CssClass="NormalTextBox" Width="250" />
		</dd>
		<dt><dnn:label id="dnnlblEventModuleId" runat="server" controlname="txtEventModuleId" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtEventModuleId" runat="server" CssClass="NormalTextBox" Width="50" />
		</dd>
		<dt><dnn:label id="dnnlblMinGroupSize" runat="server" controlname="rntxtMinGroupSize" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="rntxtMinGroupSize" runat="server" ShowSpinButtons="true" MinValue="1" MaxValue="54" Width="54" />
		</dd>
		<dt><dnn:label id="dnnlblPublishMeetings" runat="server" controlname="txtMinUserGroupSize" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:CheckBox ID="chkPublishMeetings" runat="server" CssClass="NormalTextBox" />
		</dd>
		<dt><dnn:label id="dnnlblJoinMsg" runat="server" controlname="txtJoinMsg" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtJoinMsg" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" MaxLength="1000" Width="250" Height="100" />
		</dd>
		<dt><dnn:label id="dnnlblMapKey" runat="server" controlname="txtMapKey" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtMapKey" runat="server" CssClass="NormalTextBox" MaxLength="300" Width="250" />
		</dd>
		<dt><dnn:label id="dnnlblPageSize" runat="server" controlname="rntxtPageSize" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadNumericTextBox ID="rntxtPageSize" runat="server" ShowSpinButtons="true" MinValue="1" MaxValue="50" Width="54" />
		</dd>
		<dt><dnn:label id="dnnlblNameFormat" runat="server" controlname="rcbNameFormat" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadComboBox ID="rcbNameFormat" runat="server" Width="254" />
		</dd>
		<dt><dnn:label id="dnnlblCountrySelect" runat="server" controlname="txtCountrySelect" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtCountrySelect" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" MaxLength="2000" Width="250" Height="100" />
		</dd>
		<dt><dnn:label id="dnnlblRegionSelect" runat="server" controlname="txtRegionSelect" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtRegionSelect" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" MaxLength="2000" Width="250" Height="100" />
		</dd>
		<dt><dnn:label id="dnnlblLanguageSelect" runat="server" controlname="txtLanguageSelect" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtLanguageSelect" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" MaxLength="2000" Width="250" Height="100" />
		</dd>
		<dt><dnn:label id="dnnlblJoinSelect" runat="server" controlname="txtJoinSelect" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtJoinSelect" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" MaxLength="2000" Width="250" Height="100" />
		</dd>
		<dt><dnn:label id="dnnlblProfileProperty" runat="server" controlname="rcbProfileProperty" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadComboBox ID="rcbProfileProperty" runat="server" Width="254" DataTextField="PropertyName" DataValueField="PropertyDefinitionID" />
		</dd>
	</dl>
</div>