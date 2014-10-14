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
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.UserGroups
{

	/// <summary>
	/// 
	/// </summary>
	public partial class LeaveGroup : UserGroupsModuleBase {

		#region Event Handlers

		/// <summary>
		/// Sets the control up for user input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
						//var currentGroupID = -1;
						var invalidSecurity = true;
						var invalidAction = true;
						var invalidMsg = "";
						var groupName = "N/A";

						var cntGroup = new UserGroupController();
						var objCurrentGroup = cntGroup.GetCachedUserGroup(ProfileUsersGroupID, ModuleId);

						if (objCurrentGroup != null)
						{
							if (ProfileUserID == UserId) 
							{
								// user is attempting to remove themselves
								invalidSecurity = false;
							} 
							else 
							{
								if (IsEditable) 
								{
									// admin attempting to remove user
									invalidSecurity = false;
								} 
								else 
								{
									var cntUserGroup = new UserGroupUserController();
									var objMember = cntUserGroup.GetCachedMember(UserId, PortalId);

									if (objMember != null)
									{
										if (objCurrentGroup.LeaderID == UserId || objMember.Officer) {
											//leader or officer attempting to remove user
											invalidSecurity = false;
										}
									}

								}
							}

							if (invalidSecurity) 
							{
								Response.Redirect(Util.AccessDeniedLink(), true);
							}

							// @ this point, we know end user has valid security and we are working with a valid group
							if (objCurrentGroup.LeaderID == ProfileUserID)
							{
								// user we are attempting to remove is the leader
								invalidMsg = Localization.GetString("CurrentLeader", LocalResourceFile);
							}
							else
							{
								invalidAction = false;
								var newGroupID = -1;

								if (Request.QueryString["ngid"] != null) 
								{
									newGroupID = Convert.ToInt32(Request.QueryString["ngid"]);
								}

								if (newGroupID > 0)
								{
									invalidMsg = Localization.GetString("NewGroupNext", LocalResourceFile);
								}
							}
							groupName = objCurrentGroup.Name;
						}
						else
						{
							invalidMsg = Localization.GetString("GroupNotFound", LocalResourceFile);
						}

						BindReasons();

						if (invalidAction) 
						{
							rcbReason.Enabled = false;
						}
						lblGroupName.Text = groupName;
						lblUsername.Text = ProfileUserAlias;

						if (invalidMsg != "")
						{
							lblMsg.Visible = true;
							lblMsg.Text = invalidMsg;
						}
						else
						{
							lblMsg.Visible = false;
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
		/// Confirms the end user has selected a valid reason, then removes the profile user from the group. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdRemoveClick(object sender, EventArgs e) 
		{
			if (rcbReason.SelectedIndex > 0)
			{
				var newGroupID = -1;

				if (Request.QueryString["ngid"] != null) 
				{
					newGroupID = Convert.ToInt32(Request.QueryString["ngid"]);
				}

				var cntUg = new UserGroupController();
				var objUserGroup = cntUg.GetCachedUserGroup(ProfileUsersGroupID, ModuleId);

				if (objUserGroup != null)
				{
					cntUg.LeaveUserGroup(objUserGroup, Entities.Users.UserController.GetCachedUser(PortalId, ProfileUser.Username), PortalSettings, TabId);
					// Audit
					var cntAudit = new AuditController();
					var objAudit = new AuditInfo
					               	{
					               		ImpactedUserID = ProfileUserID,
					               		CurrentUserGroupID = objUserGroup.UserGroupID,
					               		ReasonID = Convert.ToInt32(rcbReason.SelectedValue),
					               		Notes = rtReasonOther.Text.Trim(),
					               		CreatedByUserID = UserId,
					               		CreatedOnDate = DateTime.Now
					               	};

					cntAudit.AddAudit(objAudit);
				}

				Response.Redirect(
					newGroupID > 0
						? Util.ViewControlLink(TabId, ModuleId, newGroupID, PageScope.WelcomeUser)
						: DotNetNuke.Common.Globals.NavigateURL(), true);
			}
			else
			{
				lblMsg.Text = Localization.GetString("InvalidReason", LocalResourceFile);
			}
		}

		/// <summary>
		/// Determines visibility of the "other reason" row when the reason combobox is changed. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RcbReasonSelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
		{
			cmdRemove.Visible = rcbReason.SelectedIndex != 0;

			divReasonOther.Visible = rcbReason.SelectedItem.Text == @"Other";
		}

		/// <summary>
		/// Redirects the user back to the group profile page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdCancelClick(object sender, EventArgs e) 
		{
			Response.Redirect(Util.ViewControlLink(TabId, ModuleId, ProfileUsersGroupID, PageScope.GroupProfile), true);
		}

		#endregion

	    #region Private Methods

		/// <summary>
		/// Binds a list of reasons to the combobox. 
		/// </summary>
		private void BindReasons()
		{
			var cntReason = new ReasonController();
			var colReasons =  cntReason.GetActiveReasons(PortalId);

			if (colReasons != null)
			{
				rcbReason.DataSource = colReasons;
				rcbReason.DataBind();
			}

			rcbReason.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", "-1"));
		}

	    #endregion

	}
}