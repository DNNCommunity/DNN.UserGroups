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
using DotNetNuke.Services.Exceptions;

namespace DotNetNuke.Modules.UserGroups
{

	/// <summary>
	/// 
	/// </summary>
    public partial class Spotlight : UserGroupsModuleBase 
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
					rrGroup.Height = Config.SpotlightHeight(ModuleId);
					rrGroup.ItemHeight = Config.SpotlightItemHeight(ModuleId);
					rrGroup.Width = Config.SpotlightWidth(ModuleId);
					rrGroup.ItemWidth = Config.SpotlightItemWidth(ModuleId);
					rrGroup.PauseOnMouseOver = true;
					rrGroup.FrameDuration = Config.SpotlightFrameDuration(ModuleId);
					rrGroup.ScrollDuration = Config.SpotlightScrollDuration(ModuleId);
					//rrGroup.ScrollDirection =;
					//rrGroup.RotatorType = Telerik.Web.UI.RotatorType.AutomaticAdvance; //Buttons, ButtonsOver, SlideShow, SlideShowButtons
					BindRotator();
				}
			}
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="groupID"></param>
	    /// <returns></returns>
		protected string FormatContent(int groupID) {
			var cntGroup = new UserGroupController();
			var objGroup = cntGroup.GetCachedUserGroup(groupID, PortalId);

			var formatString = Config.SpotlightTemplate(ModuleId);

			var cntMeeting = new MeetingController();
			var colMeetings = cntMeeting.GetMeetingsByUserGroup(objGroup.UserGroupID, DateTime.Now, 1, 0);

			if (colMeetings.Count > 0)
			{
				// Template Tokens
				formatString = formatString.Replace("[GROUPNAME]", objGroup.Name);
				formatString = formatString.Replace("[GROUPLINK]", Util.ViewControlLink(Config.SpotlightGroupPage(ModuleId), Config.SpotlightGroupModule(ModuleId), objGroup.UserGroupID, PageScope.GroupProfile));

				if (objGroup.DefaultLanguage != "")
				{
					formatString = formatString.Replace("[FLAG]", @"<img src=""" + ResolveUrl("~/images/Flags/" + objGroup.DefaultLanguage.Trim() + ".gif") + @""" alt=""Country Flag"" />");
				}
				else
				{
					formatString = formatString.Replace("[FLAG]", @"&nbsp;");
				}

				formatString = formatString.Replace("[MEETINGDATE]", colMeetings[0].MeetingDate.ToString());
				formatString = formatString.Replace("[MEETINGTITLE]", colMeetings[0].Title);
				formatString = formatString.Replace("[MEETINGLINK]", Util.ViewControlLink(Config.SpotlightGroupPage(ModuleId), Config.SpotlightGroupModule(ModuleId), colMeetings[0].MeetingID, PageScope.MeetingDetail));
			}
			else
			{
				formatString = "";
			}

			return formatString;
		}

	    /// <summary>
	    /// 
	    /// </summary>
		private void BindRotator() {
			var cntGroup = new MeetingController();
			var colGroups = cntGroup.SpotlightSearch(PortalId, Config.SpotlightPageSize(ModuleId), 0, Config.MeetingMaxDayCount(ModuleId), Config.RequireMeetingAddress(ModuleId));

			rrGroup.DataSource = colGroups;
			rrGroup.DataBind();
		}

    }
}