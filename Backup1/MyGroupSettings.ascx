<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyGroupSettings.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.MyGroupSettings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="Settings">
	<dl>
		<dt><dnn:label id="dnnlblGroupPage" runat="server" controlname="rcbGroupPage" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadComboBox id="rcbGroupPage" runat="server" Width="254px" DataTextField="IndentedTabName" DataValueField="TabId" AutoPostBack="true" OnSelectedIndexChanged="RcbGroupPageSelectedIndexChanged" />
		</dd>
		<dt><dnn:label id="dnnlblModule" runat="server" controlname="rcbModule" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadComboBox id="rcbModule" runat="server" Width="254px" DataTextField="ModuleName" DataValueField="ModuleId" />
		</dd>
		<dt><dnn:label id="dnnlblJoinTemplate" runat="server" controlname="txtJoinTemplate" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtJoinTemplate" runat="server" TextMode="MultiLine" Height="200px" CssClass="NormalTextBox right" MaxLength="2000" Width="250px" />
		</dd>
		<dt><dnn:label id="dnnlblGroupTemplate" runat="server" controlname="txtGroupTemplate" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtGroupTemplate" runat="server" TextMode="MultiLine" Height="200px" CssClass="NormalTextBox right" MaxLength="2000" Width="250px" />
		</dd>
	</dl>
</div>