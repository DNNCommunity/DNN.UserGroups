<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Spotlight.ascx.cs" Inherits="DotNetNuke.Modules.UserGroups.Spotlight" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="GroupSpotlight">
	<telerik:RadRotator ID="rrGroup" runat="server" ScrollDuration="2000" FrameDuration="2000" RotatorType="AutomaticAdvance" >
		<ItemTemplate>
			<asp:Literal ID="litSpotlight" runat="server" Text='<%# FormatContent(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "UserGroupID"))) %>' />
		</ItemTemplate>
	</telerik:RadRotator>
</div>