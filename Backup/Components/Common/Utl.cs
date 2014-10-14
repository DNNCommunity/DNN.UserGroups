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
using DotNetNuke.Common;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Entities;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.UserGroups.Components.Common
{

    /// <summary>
    /// The Constants helper class provides a central area for Module Constants
    /// </summary>
    public static class Util
    {

		/// <summary>
		/// Formats url's to have http:// in front. This is used for manage group fields: (logo, url, twitterurl, facebookurl, linkedinurl) currently. 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string FormatUrl(string url)
		{
			var text = url;
			if (!String.IsNullOrEmpty(url) && !url.ToLower().StartsWith("http://"))
			{
				text = "http://" + url;
			}
			return text;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="moduleID"></param>
		/// <param name="keyID"></param>
		/// <param name="control"></param>
		/// <returns></returns>
		public static string ViewControlLink(int tabId, int moduleID, int keyID, PageScope control)
		{
			var url = keyID != -1 ? Globals.NavigateURL(tabId, "", "view=" + (int)control, "id=" + keyID) : Globals.NavigateURL(tabId, "", "view=" + (int)control);
			return url;
		}

    	/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="moduleID"></param>
		/// <param name="userID"></param>
		/// <returns></returns>
		public static string RemoveUserLink(int tabId, int moduleID, int userID) 
		{
			return Globals.NavigateURL(tabId, "", "view=" + (int)PageScope.RemoveUser, "u=" + userID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="moduleID"></param>
		/// <param name="newGroupID"></param>
		/// <returns></returns>
		public static string SwitchGroupsLink(int tabId, int moduleID, int newGroupID) 
		{
			return Globals.NavigateURL(tabId, "", "view=" + (int)PageScope.RemoveUser, "ngid=" + newGroupID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="moduleID"></param>
		/// <param name="groupID"></param>
		/// <returns></returns>
		public static string ManageGroupLink(int tabId, int moduleID, int groupID)
		{
			return Globals.NavigateURL(tabId, "Manage", "mid=" + moduleID, "UserGroupID=" + groupID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="moduleID"></param>
		/// <param name="userGroupID"></param>
		/// <returns></returns>
		public static string ManageUsersLink(int tabId, int moduleID, int userGroupID) 
		{
			return Globals.NavigateURL(tabId, "ManageUsers", "mid=" + moduleID, "id=" + userGroupID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="moduleID"></param>
		/// <param name="userGroupID"></param>
		/// <returns></returns>
		public static string ManageMeetingsLink(int tabId, int moduleID, int userGroupID) 
		{
			return Globals.NavigateURL(tabId, "ManageMeetings", "mid=" + moduleID, "id=" + userGroupID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="moduleID"></param>
		/// <param name="userGroupID"></param>
		/// <param name="meetingID"></param>
		/// <returns></returns>
		public static string EditMeeting(int tabId, int moduleID, int userGroupID, int meetingID)
		{
			return meetingID == -1 ? Globals.NavigateURL(tabId, "EditMeeting", "mid=" + moduleID, "UserGroupID=" + userGroupID) : Globals.NavigateURL(tabId, "EditMeeting", "mid=" + moduleID, "UserGroupID=" + userGroupID, "MeetingID=" + meetingID);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static string AccessDeniedLink()
		{
			return Globals.AccessDeniedURL(Localization.GetString("ModuleAccess.Error"));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static string SendMessageLink(int tabId, int moduleID, int userGroupID) 
		{
			return Globals.NavigateURL(tabId, "SendMessage", "mid=" + moduleID, "UserGroupID=" + userGroupID);
		}

    		/// <summary>
    		/// 
    		/// </summary>
    		/// <param name="objGroup"></param>
    		/// <param name="address"></param>
    		public static string SetMap(UserGroupInfo objGroup, string address) {
			//var strLat = "33.706063";
			//var strLong = "-84.385986";
			string strDisplayScritp;

			//               if ((objGroup.Latitude != 0) & (objGroup.Longitude != 0)) {
			//                    strLat = objGroup.Latitude.ToString();
			//                    strLong = objGroup.Longitude.ToString();

			//                    strDisplayScritp = @"<script type=""text/javascript"">
			//									$(document).ready(function() {
			//										$('#map_canvas').googleMaps({
			//												latitude: " + strLat + ", longitude: " + strLong + "}); }); </script>";
			//               } else {
			// see if we can get lat/long elsewhere

			if (address != "")
			{
				strDisplayScritp = @"<script type=""text/javascript"">
		    						$(document).ready(function() {
		    							$('#map_canvas').googleMaps({
		    									geocode: '" + address + "' , scroll: false }); }); </script>";
			}
			else
			{
				if (objGroup.MeetingAddress != "") {
					//strLat = objGroup.Latitude.ToString();
					//strLong = objGroup.Longitude.ToString();

					strDisplayScritp = @"<script type=""text/javascript"">
		    						$(document).ready(function() {
		    							$('#map_canvas').googleMaps({
		    									geocode: '" + objGroup.MeetingAddress + "' , scroll: false }); }); </script>";
				} else {
					//see if we can get lat/long elsewhere
					strDisplayScritp =
						@"<script type=""text/javascript"">
					 $(document).ready(function() { 
						$('#map_canvas').googleMaps({
							geocode: '" + objGroup.City + " " + objGroup.Region + "' , scroll: false }); }); </script>";
				}
			}


			return strDisplayScritp;
		}

    }
}