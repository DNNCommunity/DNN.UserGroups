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
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;

//using DotNetNuke.Modules.UserGroups.Common;

namespace DotNetNuke.Modules.UserGroups.Components.Entities
{

	/// <summary>
	/// This is our Info class that represents columns in our data store that are associated with the UserGroups_UserGroups table.
	/// </summary>
	public class UserGroupInfo : DotNetNuke.Entities.Content.ContentItem
    {

		//#region Private Members
		private string _groupProfileUrl;
		//private UserGroupStatus _status = UserGroupStatus.Inactive;

		//#endregion

		public string Name { get; set; }

		public string Country { get; set; }

		public string Region { get; set; }

		public string City { get; set; }

		public string Url { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int Members { get; set; }

		public string LeaderDisplayName { get; set; }

		public string LeaderProfilePhoto { get; set; }

		public string TwitterUrl { get; set; }

		public string LinkedInUrl { get; set; }

		public string FacebookUrl { get; set; }

		public string LanguageName { get; set; }


		public int UserGroupID { get; set; }

		public int PortalID { get; set; }

		public int LeaderID { get; set; }

		public string LeaderUsername { get; set; }

		/// <summary>
		/// Legacy
		/// </summary>
		public string Logo { get; set; }

		public string About { get; set; }

		public bool Active { get; set; }

		public int LanguageID { get; set; }

		/// <summary>
		/// This was added for flag integration. Eventually, I want to use the core control (used in admin's add language UI - DnnLanguageComboBox, in 5.5+). 
		/// </summary>
		public string DefaultLanguage { get; set; }

#pragma warning disable 108,114
		public int CreatedByUserID { get; set; }
#pragma warning restore 108,114

#pragma warning disable 108,114
		public DateTime CreatedOnDate { get; set; }
#pragma warning restore 108,114

#pragma warning disable 108,114
		public int LastModifiedByUserID { get; set; }
#pragma warning restore 108,114

#pragma warning disable 108,114
		public DateTime LastModifiedOnDate { get; set; }
#pragma warning restore 108,114

		public string MeetingAddress { get; set; }

		public int TotalRecords { get; set; }

		public string GroupProfileUrl {
			get {
				// we use 1074 because content items didn't originally exist (where tabid is stored) and this is the tab we want on dnn.com
				_groupProfileUrl = Common.Util.ViewControlLink(TabID < 1 ? 1074 : TabID, ModuleID, UserGroupID, PageScope.MeetingDetail);
				return _groupProfileUrl;
			}
			set { _groupProfileUrl = value; }
		}

		public List<BaseMeetingInfo> UpcomingMeetings {
			get {
				var cntMeeting = new MeetingController();
				var colMeetings = cntMeeting.GetMeetingsByUserGroupService(Convert.ToInt32(UserGroupID), DateTime.Now, 5, 0);

				return colMeetings;
			}
		}

		public override void Fill(System.Data.IDataReader dr) {
			base.Fill(dr);

			Name = Null.SetNullString(dr["Name"]);
			Country = Null.SetNullString(dr["Country"]);
			Region = Null.SetNullString(dr["Region"]);
			City = Null.SetNullString(dr["City"]);
			Url = Null.SetNullString(dr["Url"]);
			Latitude = Null.SetNullSingle(dr["Latitude"]);
			Longitude = Null.SetNullSingle(dr["Longitude"]);
			Members = Null.SetNullInteger(dr["Members"]);
			LeaderDisplayName = Null.SetNullString(dr["LeaderDisplayName"]);
			//LeaderProfilePhoto = Null.SetNullString(dr["LeaderProfilePhoto"]);
			TwitterUrl = Null.SetNullString(dr["TwitterUrl"]);
			LinkedInUrl = Null.SetNullString(dr["LinkedInUrl"]);
			FacebookUrl = Null.SetNullString(dr["FacebookUrl"]);
			LanguageName = Null.SetNullString(dr["LanguageName"]);
			UserGroupID = Null.SetNullInteger(dr["UserGroupID"]);
			PortalID = Null.SetNullInteger(dr["PortalID"]);
			LeaderID = Null.SetNullInteger(dr["LeaderID"]);
			LeaderUsername = Null.SetNullString(dr["LeaderUsername"]);
			Logo = Null.SetNullString(dr["Logo"]);
			About = Null.SetNullString(dr["About"]);
			Active = Null.SetNullBoolean(dr["Active"]);
			LanguageID = Null.SetNullInteger(dr["LanguageID"]);
			CreatedByUserID = Null.SetNullInteger(dr["CreatedByUserID"]);
			CreatedOnDate = Null.SetNullDateTime(dr["CreatedOnDate"]);
			LastModifiedByUserID = Null.SetNullInteger(dr["LastModifiedByUserID"]);
			LastModifiedOnDate = Null.SetNullDateTime(dr["LastModifiedOnDate"]);
			MeetingAddress = Null.SetNullString(dr["MeetingAddress"]);
			DefaultLanguage = Null.SetNullString(dr["DefaultLanguage"]);
			TotalRecords = Null.SetNullInteger(dr["TotalRecords"]);
		}
    }
}