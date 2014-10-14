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
using System.Web.UI.WebControls;
using DotNetNuke.Common;

namespace DotNetNuke.Modules.UserGroups 
{

	/// <summary>
	/// 
	/// </summary>
	public partial class ManageGroupUsers : UserGroupsModuleBase {

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
								// we need logic to make sure this is admin, leaders, officers only!
								if (IsEditable || objGroup.LeaderID == UserId || CurrentUserIsOfficer)
								{
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
		protected void RgMembersItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e) 
		{
			if (!(e.Item is Telerik.Web.UI.GridDataItem)) return;
			var userKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"];
			var officer = (bool)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Officer"];
			var leaderID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LeaderID"];
			var dataItem = (Telerik.Web.UI.GridDataItem)e.Item;

			var imgDelete = ((ImageButton)(dataItem)["DeleteItem"].Controls[0]);
			imgDelete.AlternateText = Localization.GetString("Delete", LocalResourceFile);
			imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile);

			var hlName = ((HyperLink)(dataItem)["Username"].Controls[0]);
			hlName.NavigateUrl = Globals.UserProfileURL(userKeyID);

			var imgOfficer = ((ImageButton)(dataItem)["Officer"].Controls[0]);
			imgOfficer.CommandArgument = userKeyID.ToString();

			if (officer) 
			{
				imgOfficer.ImageUrl = TemplateSourceDirectory + "/images/check.png";
				imgOfficer.AlternateText = Localization.GetString("IsOfficer", LocalResourceFile);
				imgOfficer.ToolTip = Localization.GetString("IsOfficer", LocalResourceFile);
			} 
			else 
			{
				if (userKeyID == leaderID) 
				{
					imgOfficer.ImageUrl = TemplateSourceDirectory + "/images/check.png";
					imgOfficer.AlternateText = Localization.GetString("IsOfficer", LocalResourceFile);
					imgOfficer.ToolTip = Localization.GetString("IsOfficer", LocalResourceFile);
				     imgOfficer.Enabled = false;
				} 
				else 
				{
					imgOfficer.ImageUrl = TemplateSourceDirectory + "/images/unchecked.png";
					imgOfficer.AlternateText = Localization.GetString("NotOfficer", LocalResourceFile);
					imgOfficer.ToolTip = Localization.GetString("NotOfficer", LocalResourceFile);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgMembersItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e) {
			if (
				(!(e.Item.ItemType == Telerik.Web.UI.GridItemType.Item |
				   e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem))) return;

			var keyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"];
			var userGroupID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];

			switch (e.CommandName) 
			{
				case "DeleteItem":
					Response.Redirect(Util.RemoveUserLink(TabId, ModuleId, keyID), true);
					break;
				case "Officer":
					var keyOfficer = (bool)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Officer"];
					var keyLeaderID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LeaderID"];

					if (keyID != keyLeaderID)
					{
						var cntUgUser = new UserGroupUserController();
						cntUgUser.UpdateOfficer(userGroupID, keyID, !keyOfficer, PortalId);
						BindGrid(userGroupID, true);
					}
					break;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Localizes the grid header colums.
		/// </summary>
		private void SetLocalization() {
			foreach (Telerik.Web.UI.GridColumn gc in rgMembers.MasterTableView.Columns) {
				if (gc.HeaderText != "") {
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupID"></param>
		/// <param name="bindIt"></param>
		private void BindGrid(int groupID, bool bindIt)
		{
			var cntUgUsers = new UserGroupUserController();
			var colUsers = cntUgUsers.GetCachedUsersByUserGroup(groupID);

		    	rgMembers.DataSource = colUsers;
		    	rgMembers.Visible = true;

		    	if (bindIt)
		    	{
		    		rgMembers.DataBind();
		    	}
		}

		#endregion

	}
}