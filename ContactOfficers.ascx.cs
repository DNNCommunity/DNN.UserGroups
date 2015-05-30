//
// DotNetNuke® - http://www.dnnsoftware.com
// Copyright (c) 2002-2015
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
// THE SOFTWARE IS PROVIDED "AS IS", 5ITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common;

namespace DotNetNuke.Modules.UserGroups
{

    /// <summary>
    /// Class ContactOfficers.
    /// </summary>
    public partial class ContactOfficers : UserGroupsModuleBase 
    {

	    #region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
// ReSharper disable InconsistentNaming
		protected void Page_Load(object sender, EventArgs e) 
// ReSharper restore InconsistentNaming
		{
			try
			{
				if (Page.IsPostBack == false)
				{
					if (!String.IsNullOrEmpty(Request.QueryString["id"]))
					{
						var userGroupID = Convert.ToInt32(Request.QueryString["id"]);
						txtGroupID.Text = userGroupID.ToString();

						var cntUserGroup = new UserGroupController();
						var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

						lblGroupName.Text = objUserGroup.Name;

						if (!Request.IsAuthenticated)
						{
							Response.Redirect(Globals.AccessDeniedURL(GetLocalizedString("ModuleAccess.Error")), true);
						}
						else
						{
							// if the user is in this group or they are an admin, let them remain here.
							if (IsEditable || objUserGroup.UserGroupID == ProfileUsersGroupID)
							{
								
							}
							else
							{
								Response.Redirect(Globals.AccessDeniedURL(GetLocalizedString("ModuleAccess.Error")), true);
							}
						}
					}
					else
					{
						Response.Redirect(Globals.AccessDeniedURL(GetLocalizedString("ModuleAccess.Error")), true);
					}
				}
			}
			catch (Exception exc)
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
			var userGroupID = Convert.ToInt32(txtGroupID.Text);
			Response.Redirect(Util.ViewControlLink(TabId, ModuleId, userGroupID, PageScope.GroupProfile), true);
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
		protected void CmdSendClick(object sender, EventArgs e)
		{
			try 
			{
					var strMessage = "";

					if (txtMessage.Text == "")
					{
						strMessage = "Message Required! <br />";
					}

					if (strMessage.Length > 0)
					{
						lblMessage.Text = strMessage;
						lblMessage.Visible = true;
						return;
					}
					lblMessage.Visible = false;

					var userGroupID = Convert.ToInt32(Request.QueryString["id"]);
					var cntUserGroup = new UserGroupController();
					var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

					if (objUserGroup != null) 
					{
						var profileLink = Util.ViewControlLink(TabId, ModuleId, userGroupID, PageScope.GroupProfile);
						//Send message
						UserGroupController.SendNotifications(Entities.Users.UserController.GetCachedUser(PortalId, ProfileUser.Username), objUserGroup, PortalSettings, EmailType.ContactOfficers, txtMessage.Text, profileLink);
						Response.Redirect(Util.ViewControlLink(TabId, ModuleId, userGroupID, PageScope.GroupProfile), true);
					}
				//MessageKey = "MailSentSuccess";
			} 
			catch (Exception exc) 
			{
				Exceptions.LogException(exc);
			}
			//Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
		}

		#endregion

    }
}