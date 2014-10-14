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

namespace DotNetNuke.Modules.UserGroups
{

	/// <summary>
	/// 
	/// </summary>
	public partial class MeetingDetail : UserGroupsModuleBase
    {

	   #region Event Handlers

		/// <summary>
		/// Initialize the control by first registering any scripts for Ajax. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>This requires the control having 'SupportsPartialRendering' set to true in the manifest file.</remarks>
// ReSharper disable InconsistentNaming
		protected void Page_Init(Object sender, EventArgs e) {
// ReSharper restore InconsistentNaming
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
							var meetingID = Convert.ToInt32(Request.QueryString["id"]);
							BindMeeting(meetingID);
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
				catch (Exception exc) 
				{
					Exceptions.ProcessModuleLoadException(this, exc);
				}
		}

		/// <summary>
		/// Will navigate the user to the group profile's home page. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdGroupProfileClick(object sender, EventArgs e) 
		{
		   Response.Redirect(Util.ViewControlLink(TabId, ModuleId, Convert.ToInt32(txtGroupID.Text), PageScope.GroupProfile), true);
		}

		#endregion

		#region Private Methods

		void BindMeeting(int meetingID)
		{
			var cntMeeting = new MeetingController();
			var objMeeting = cntMeeting.GetMeeting(meetingID);

			var cntGroup = new UserGroupController();
			var objGroup = cntGroup.GetCachedUserGroup(objMeeting.UserGroupID, PortalId);

			if (objGroup == null) return;
			GroupName.Text = objGroup.Name;

			txtGroupID.Text = objMeeting.UserGroupID.ToString();
			Title.Text = objMeeting.Title;
			Description.Text = objMeeting.Description;
			Location.Text = objMeeting.Location;

			var address = "";
			if (objMeeting.Location != "")
			{
				address = objMeeting.Location;
			}

			litMapScript.Text = Util.SetMap(objGroup, address);
			MeetingDate.Text = objMeeting.MeetingDate.ToString();
		}

		#endregion

    }
}