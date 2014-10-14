//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2010
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.UserGroups.Common;
using System.Web.UI.WebControls;

namespace DotNetNuke.Modules.UserGroups 
{

	/// <summary>
	/// 
	/// </summary>
	public partial class ManageGroupMeetings : UserGroupsModuleBase {

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
// ReSharper disable InconsistentNaming
		protected void Page_Init(object sender, EventArgs e) {
// ReSharper restore InconsistentNaming
			if (Framework.AJAX.IsInstalled()) {
				Framework.AJAX.RegisterScriptManager();
			}

			if (!Page.IsPostBack) {
				SetLocalization();
			}
		}

		/// <summary>
		/// Runs when the page loads, determins if the user can join the group (if it exists and other business rules), then  adds user to new group (if applicable) and sets page up with information about what just happend.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>This class and managegroup class are only interfaces a user can be added to a group.</remarks>
// ReSharper disable InconsistentNaming
		protected void Page_Load(object sender, EventArgs e) 
// ReSharper restore InconsistentNaming
		{
			try 
			{
				if (!IsAuthenticated)
				{
					Response.Redirect(Util.AccessDeniedLink(), true);
				} 
				else 
				{
					if (!Page.IsPostBack) 
					{
						var groupID = -1;
						if (Request.QueryString["id"] != null) 
						{
							groupID = Convert.ToInt32(Request.QueryString["id"]);
						}

						if (groupID != -1)
						{
							var cntGroup = new UserGroupController();
							var objGroup = cntGroup.GetCachedUserGroup(groupID, ModuleId);

							if (objGroup != null)
							{
								if (IsEditable || objGroup.LeaderID == UserId || CurrentUserIsOfficer) 
									{
										//leader or officer attempting to remove user
										lblGroupName.Text = objGroup.Name;
										txtUserGroupID.Text = objGroup.UserGroupID.ToString();
										BindGrid(groupID, true);
									}
									else
									{
										Response.Redirect(Util.AccessDeniedLink(), true);
									}
							}
							else
							{
								Response.Redirect(Util.AccessDeniedLink(), true);
							}
						}
						else
						{
							Response.Redirect(Util.AccessDeniedLink(), true);
						}
					}
				}
			} 
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdCancelClick(object sender, EventArgs e) 
		{
			Response.Redirect(Util.ManageGroupLink(TabId, ModuleId, Convert.ToInt32(txtUserGroupID.Text)), true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgMeetingsItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e) {
			if (!(e.Item is Telerik.Web.UI.GridDataItem)) return;
			var keyMeetingID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MeetingID"];
			//var groupKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];
			var dataItem = (Telerik.Web.UI.GridDataItem)e.Item;

			var imgEdit = ((ImageButton)(dataItem)["EditItem"].Controls[0]);
			imgEdit.AlternateText = Localization.GetString("Edit", LocalResourceFile);
			imgEdit.ToolTip = Localization.GetString("Edit", LocalResourceFile);

			var imgDelete = ((ImageButton)(dataItem)["DeleteItem"].Controls[0]);
			imgDelete.CommandArgument = keyMeetingID.ToString();
			imgDelete.AlternateText = Localization.GetString("Delete", LocalResourceFile);
			imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile);
			imgDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

			var hlMeetingDetail = ((HyperLink)(dataItem)["Title"].Controls[0]);
			hlMeetingDetail.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, keyMeetingID, PageScope.MeetingDetail);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgMeetingsItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e) {
			if (
				(!(e.Item.ItemType == Telerik.Web.UI.GridItemType.Item |
				   e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))) return;

			var keyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MeetingID"];
			var userGroupID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];

			switch (e.CommandName) 
			{
				case "EditItem":
					Response.Redirect(Util.EditMeeting(TabId, ModuleId, userGroupID, keyID), true);
					break;
				case "DeleteItem":
					var cntMeeting = new MeetingController();
					cntMeeting.DeleteUserGroupMeeting(keyID, userGroupID, ModuleId);
					BindGrid(userGroupID, true);
					break;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Localizes the grid header colums.
		/// </summary>
		private void SetLocalization() {
			foreach (Telerik.Web.UI.GridColumn gc in rgMeetings.MasterTableView.Columns) {
				if (gc.HeaderText != "") {
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile);
				}
			}
		}

		/// <summary>
		/// Binds a list of all meetings for management reasons.
		/// </summary>
		/// <param name="userGroupID"></param>
		/// <param name="bindIt"></param>
		private void BindGrid(int userGroupID, bool bindIt) 
		{
			if (userGroupID > 0) 
			{
				var cntMeeting = new MeetingController();
				var colMeetings = cntMeeting.GetMeetingsByUserGroup(userGroupID, DateTime.Now, 100, 0);

				rgMeetings.DataSource = colMeetings;

				if (bindIt)
				{
					rgMeetings.DataBind();
				}
			}
		}

		#endregion

	}
}