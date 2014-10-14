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
using Config = DotNetNuke.Modules.UserGroups.Common.Config;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.UserGroups.Common;

namespace DotNetNuke.Modules.UserGroups 
{

	/// <summary>
	/// 
	/// </summary>
	public partial class WelcomeUser : UserGroupsModuleBase 
	{

		#region Event Handlers

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
					Response.Redirect(DotNetNuke.Common.Globals.AccessDeniedURL(GetLocalizedString("ModuleAccess.Error")), true);
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

						var invalidGroup = true;
						var joinMsg = Localization.GetString("GroupNotFound", LocalResourceFile);
						var viewLinkText = Localization.GetString("cmdCancel", LocalResourceFile);
						var viewLinkUrl = DotNetNuke.Common.Globals.NavigateURL();

						if (groupID != -1)
						{
							var cntGroup = new UserGroupController();
							var objGroup = cntGroup.GetCachedUserGroup(groupID, ModuleId);

							if (objGroup != null)
							{
								lblGroupName.Text = objGroup.Name;

								if (ProfileUsersGroupID == groupID)
								{
									joinMsg = Localization.GetString("CurrentMember", LocalResourceFile);
								}
								else if (ProfileUsersGroupID > 0)
								{
									joinMsg = Localization.GetString("AnotherGroupMember", LocalResourceFile);
								}
								else
								{
									invalidGroup = false;
									var cntUg = new UserGroupController();
									cntUg.JoinUserGroup(objGroup, Entities.Users.UserController.GetCachedUser(PortalId, ProfileUser.Username), PortalSettings, TabId);

									//// Audit (Currently, we have nothign in for adding a user to a group)
									//var cntAudit = new AuditController();
									//var objAudit = new AuditInfo
									//{
									//     ImpactedUserID = ProfileUserID,
									//     CurrentUserGroupID = objGroup.UserGroupID,
									//     ReasonID = Convert.ToInt32(rcbReason.SelectedValue),
									//     Notes = txtReasonOther.Text.Trim(),
									//     CreatedByUserID = UserId,
									//     CreatedOnDate = DateTime.Now
									//};

									//cntAudit.AddAudit(objAudit);

									joinMsg = Config.JoinMessage(ModuleId);
									viewLinkText = Localization.GetString("cmdViewGroup", LocalResourceFile);
									viewLinkUrl = Util.ViewControlLink(TabId, ModuleId, objGroup.UserGroupID, PageScope.GroupProfile);
								}
							}
						} 

						if (invalidGroup)
						{	
							lblGroupName.Text = @"N/A";
						}

						lblUsername.Text = ProfileUserAlias;
						lblJoinMsg.Text = joinMsg;
						hlViewGroup.Text = viewLinkText;
						hlViewGroup.NavigateUrl = viewLinkUrl;
					}
				}
			} 
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

	}
}