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
using DotNetNuke.Modules.UserGroups.Components.Entities;
using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.UserGroups.Components;

namespace DotNetNuke.Modules.UserGroups
{

	/// <summary>
	/// 
	/// </summary>
	public partial class EditMeeting : UserGroupsModuleBase
    {

	   #region Event Handlers

		/// <summary>
		/// Initialize the control by first registering any scripts for Ajax. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>This requires the control having 'SupportsPartialRendering' set to true in the manifest file.</remarks>
		protected void Page_Init(Object sender, EventArgs e) {
			Framework.jQuery.RequestRegistration();

			var strMapScript = @"<script src=""http://maps.google.com/maps?file=api&amp;v=2&amp;key=" + Config.MapKey(ModuleId) + @"&sensor=false"" type=""text/javascript""></script>";

			Page.ClientScript.RegisterClientScriptBlock(GetType(), "mapsRegister", strMapScript);
			Page.ClientScript.RegisterClientScriptInclude("maps", TemplateSourceDirectory + "/scripts/jquery.googlemaps1.01.js"); //jquery.googlemaps.pack.1.01.js

			if (Framework.AJAX.IsInstalled()) {
				Framework.AJAX.RegisterScriptManager();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e) 
		{
			try 
				{
					if (Page.IsPostBack == false) 
					{
						if (!String.IsNullOrEmpty(Request.QueryString["UserGroupID"])) 
						{
							var userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
							var cntUserGroup = new UserGroupController();
							var objGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

							if (objGroup != null)
							{
								if (objGroup.LeaderID == UserId || IsEditable || CurrentUserIsOfficer) 
								{
									BindMeeting(objGroup);
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
		protected void CmdDelete_Click(object sender, EventArgs e) {
			if (String.IsNullOrEmpty(Request.QueryString["UserGroupID"])) return;
			var userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
			var cntMeeting = new MeetingController();
			cntMeeting.DeleteUserGroupMeeting(Convert.ToInt32(txtMeetingID.Text), userGroupID, ModuleId);

			Response.Redirect(Globals.NavigateURL(), true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdSave_Click(object sender, EventArgs e) 
		{
			if (!String.IsNullOrEmpty(Request.QueryString["UserGroupID"]))
			{
				var strMessage = "";

				if (txtTitle.Text == "") {
					strMessage = Localization.GetString("valTitle", LocalResourceFile);
				}

				if (strMessage.Length > 0) {
					lblMessage.Text = strMessage;
					lblMessage.Visible = true;
					return;
				}
				lblMessage.Visible = false;

				var userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
				var cntUserGroup = new UserGroupController();
				var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);
				var profileUrl = Util.ViewControlLink(TabId, ModuleId, objUserGroup.UserGroupID, PageScope.GroupProfile);

				{
					var cntMeet = new MeetingController();

					if (txtMeetingID.Text == "-1")
					{
						cntMeet.AddUserGroupMeeting(objUserGroup, txtTitle.Text, txtDescription.Text, rtLocation.Text, rdtMeetingDate.SelectedDate.Value, ModuleId, PortalId, profileUrl);
					}
					else
					{
						cntMeet.UpdateUserGroupMeeting(Convert.ToInt32(txtMeetingID.Text), objUserGroup, txtTitle.Text, txtDescription.Text, rtLocation.Text, rdtMeetingDate.SelectedDate.Value, ModuleId, PortalId, Convert.ToInt32(txtEventID.Text), profileUrl);
					}				
					Response.Redirect(Globals.NavigateURL(), true);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdCancel_Click(object sender, EventArgs e) {
		   Response.Redirect(Globals.NavigateURL(), true);
		}

		protected void CmdViewMap_Click(object sender, EventArgs e) {
			var userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
			var cntUserGroup = new UserGroupController();
			var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

			litMapScript.Text = Util.SetMap(objUserGroup, rtLocation.Text);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userGroup"></param>
		void BindMeeting(UserGroupInfo userGroup)
		{
			if (!String.IsNullOrEmpty(Request.QueryString["MeetingID"]))
			{
				var meetingID = Convert.ToInt32(Request.QueryString["MeetingID"]);

				var cntMeeting = new MeetingController();
				var objMeeting = cntMeeting.GetMeeting(meetingID);

				if (objMeeting != null)
				{
					txtDescription.Text = objMeeting.Description;
					txtEventID.Text = objMeeting.EventID.ToString();
					rtLocation.Text = objMeeting.Location;
					txtTitle.Text = objMeeting.Title;
					txtMeetingID.Text = objMeeting.MeetingID.ToString();
					rdtMeetingDate.SelectedDate = objMeeting.MeetingDate;
					cmdDelete.Visible = true;
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

					if (objMeeting.Location != "") {
						litMapScript.Text = Util.SetMap(userGroup, objMeeting.Location);
					}
					else
					{
						litMapScript.Text = Util.SetMap(userGroup, "");
					}
				}
				else
				{
					IsNewMeeting(userGroup);
				}
			}
			else
			{
				IsNewMeeting(userGroup);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void IsNewMeeting(UserGroupInfo userGroup)
		{
			rdtMeetingDate.MinDate = DateTime.Now;
			txtEventID.Text = "-1";
			txtMeetingID.Text = "-1";
			cmdDelete.Visible = false;
			rtLocation.Text = userGroup.MeetingAddress;

			if (userGroup.MeetingAddress != "")
			{
				litMapScript.Text = Util.SetMap(userGroup, "");
			}
		}

		#endregion

    }
}