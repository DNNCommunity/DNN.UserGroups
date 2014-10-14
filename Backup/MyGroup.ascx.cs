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
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Modules.UserGroups.Components.Entities;
using DotNetNuke.Services.Exceptions;
using Config = DotNetNuke.Modules.UserGroups.Common.Config;

namespace DotNetNuke.Modules.UserGroups
{

	/// <summary>
	/// 
	/// </summary>
    public partial class MyGroup : UserGroupsModuleBase 
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
					if (ProfileUsersGroupID > 0)
					{
						var cntUg = new UserGroupController();
						var objGroup = cntUg.GetCachedUserGroup(ProfileUsersGroupID, PortalId);

						litSpotlight.Text = objGroup == null ? FormatJoinContent() : FormatGroupContent(objGroup);	
					}
					else
					{
						litSpotlight.Text = FormatJoinContent();
					}

				}
			}
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

		#region Private Methods

		protected string FormatJoinContent() {
			var formatString = Config.JoinTemplate(ModuleId);

			// Template Tokens
			formatString = formatString.Replace("[JOINLINK]", Util.ViewControlLink(Config.MyGroupPage(ModuleId), Config.MyGroupModule(ModuleId), -1, PageScope.DirectorySearch));

			return formatString;
		}

		protected string FormatGroupContent(UserGroupInfo objGroup) {
			var formatString = Config.GroupTemplate(ModuleId);

			// Template Tokens
			formatString = formatString.Replace("[GROUPNAME]", objGroup.Name);
			formatString = formatString.Replace("[GROUPLINK]", Util.ViewControlLink(Config.MyGroupPage(ModuleId), Config.MyGroupModule(ModuleId), objGroup.UserGroupID, PageScope.GroupProfile));

			if (objGroup.DefaultLanguage != "") {
				formatString = formatString.Replace("[FLAG]", @"<img src=""" + ResolveUrl("~/images/Flags/" + objGroup.DefaultLanguage.Trim() + ".gif") + @""" alt=""Country Flag"" />");
			} else {
				formatString = formatString.Replace("[FLAG]", @"&nbsp;");
			}

			var cntMeeting = new MeetingController();
			var colMeetings = cntMeeting.GetMeetingsByUserGroup(objGroup.UserGroupID, DateTime.Now, 1, 0);

			if (colMeetings.Count > 0)
			{
				formatString = formatString.Replace("[MEETINGDATE]", colMeetings[0].MeetingDate.ToString());
				formatString = formatString.Replace("[MEETINGTITLE]", colMeetings[0].Title);
				formatString = formatString.Replace("[MEETINGLINK]", Util.ViewControlLink(Config.MyGroupPage(ModuleId), Config.MyGroupModule(ModuleId), colMeetings[0].MeetingID, PageScope.MeetingDetail));
			}
			else
			{
				formatString = formatString.Replace("[MEETINGDATE]", "");
				formatString = formatString.Replace("[MEETINGTITLE]", "No Meetings Scheduled");
				formatString = formatString.Replace("[MEETINGLINK]", "");
			}

			return formatString;
		}

		#endregion

    }
}