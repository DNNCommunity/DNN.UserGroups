<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMeeting.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.EditMeeting" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Literal ID="litMapScript" runat="server" />
<div class="EditMeeting">
	<div id="divEventID" runat="server" visible="false">
		<asp:label id="lblEventID" runat="server" />
		<asp:TextBox ID="txtEventID" runat="server" />
		<asp:label id="lblMeetingID" runat="server" />
		<asp:TextBox ID="txtMeetingID" runat="server" />
	</div>	
	<dl>
		<dt><dnn:label id="dnnlblTitle" runat="server" controlname="txtTitle" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="NormalTextBox" Width="250px" />
		</dd>
		<dt><dnn:label id="dnnlblDescription" runat="server" controlname="txtDescription" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<asp:TextBox ID="txtDescription" runat="server" CssClass="NormalTextBox" TextMode="MultiLine" Width="248px" Height="100px" />
		</dd>
		<dt><dnn:label id="dnnlblMeetingDate" runat="server" controlname="txtMeetingDate" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadDateTimePicker ID="rdtMeetingDate" runat="server" />
		</dd>
		<dt><dnn:label id="dnnlblLocation" runat="server" controlname="rtLocation" suffix=":" CssClass="SubHead" /></dt>
		<dd>
			<telerik:RadTextBox TextMode="MultiLine" MaxLength="200" ID="rtLocation" runat="server" CssClass="NormalTextBox" Width="250px" Height="50px" />
		</dd>
		<dt></dt>
		<dd>
			<asp:LinkButton ID="cmdViewMap" runat="server" resourcekey="cmdViewMap" CssClass="CommandButton secondary-action" OnClick="CmdViewMap_Click" />
		</dd>
	</dl>
	<div class="map-wrapper">
		<div id="map_canvas" class="ug-map"></div>
	</div>
	<div class="action-btns">
		<asp:LinkButton ID="cmdDelete" runat="server" resourcekey="cmdDelete" CssClass="CommandButton secondary-action" OnClick="CmdDelete_Click" />
		<asp:LinkButton ID="cmdSave" runat="server" resourcekey="cmdSave" CssClass="CommandButton primary-action" OnClick="CmdSave_Click" />
		<asp:LinkButton ID="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="CommandButton secondary-action" OnClick="CmdCancel_Click" />
	</div>
	<div class="messages">
		<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" />
	</div>
</div>