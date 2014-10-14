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
using System.Data;
using DotNetNuke.Framework;

namespace DotNetNuke.Modules.UserGroups.Components.Data
{

	/// <summary>
	/// An abstract class for the data access layer (Thus, this is our abstract data provider).
	/// </summary>
	public abstract class DataProvider
	{

		#region Shared/Static Methods

		// singleton reference to the instantiated object 
		private static DataProvider _objProvider;

		// constructor
		static DataProvider() 
		{
			CreateProvider();
		}

		// dynamically create provider
		private static void CreateProvider()
		{
			_objProvider = (DataProvider)Reflection.CreateObject("data", "DotNetNuke.Modules.UserGroups.Components.Data", "");
		}

		// return the provider
		public static DataProvider Instance()
		{
			return _objProvider;
		}

		#endregion

		#region Abstract methods

		public abstract int AddUserGroup(int portalID, int leaderID, string name, string country, string region, string city, string url, string logo, bool active, double latitude, double longitude, string twitterUrl, string linkedInUrl, string facebookUrl, string about, int languageID, int createdByUserID, DateTime createdOnDate, int moduleID, string meetingAddress, string defaultLanguage);

		public abstract void UpdateUserGroup(int userGroupID, int portalID, int leaderID, string name, string country, string region, string city, string url, string logo, bool active, double latitude, double longitude, string twitterUrl, string linkedInUrl, string facebookUrl, string about, int languageID, int lastModifiedByUserID, DateTime lastModifiedOnDate, string meetingAddress, int contentItemID, string defaultLanguage);

		public abstract void DeleteUserGroup(int userGroupID, int moduleID);

		public abstract IDataReader SearchGroups(int portalId, int pageIndex, int pageSize, string country, string region, string city, string name, int languageID, int propertyDefinitionID);

		public abstract IDataReader SearchGroupsByLocation(int portalId, float latitude, float longitude, int distance, bool inKilometers, int propertyDefinitionID);

		public abstract IDataReader GetSitemapUrLs(int portalID);

		public abstract IDataReader GetUserGroup(int userGroupID);

		public abstract IDataReader GetGroupOfficers(int userGroupID);

		public abstract IDataReader GetFile(int userID, int fileID);

		// Regions
		public abstract IDataReader GetGroupRegions(int portalId);

		public abstract IDataReader GetGroupRegionsByCountry(int portalId, string country);

		// User Group Users
		public abstract IDataReader GetUsersByUserGroup(int userGroupID);

		public abstract IDataReader GetUserGroupUser(int userID, int portalID);

		public abstract void JoinUserGroup(int userGroupID, int userID);

		public abstract void LeaveUserGroup(int userGroupID, int userID);

		public abstract void UpdateOfficer(int userGroupID, int userID, bool isOfficer);

		public abstract void UpdateLeader(int userGroupID, int newLeaderID, int oldLeaderID);

		//Languages
		public abstract int AddLanguage(int portalID, string language);

		public abstract IDataReader GetLanguages(int portalID);

		//Reasons
		public abstract IDataReader GetActiveReasons(int portalID);

		//Meetings
		public abstract int AddUserGroupMeeting(int userGroupID, int eventID, string title, string description, string location, DateTime meetingDate);

		public abstract void UpdateUserGroupMeeting(int meetingID, int userGroupID, int eventID, string title, string description, string location, DateTime meetingDate);

		public abstract IDataReader GetUpcommingGroupMeetings(int userGroupID, DateTime startDate, int pageSize, int pageIndex);

		public abstract IDataReader SpotlightSearch(int portalID, int pageSize, int pageIndex, int nextDays, bool requireMeetingLocation);

		public abstract IDataReader GetAllMeetingsByDate(int portalID, DateTime startDate, DateTime endDate, string region, int pageIndex, int pageSize);

		public abstract IDataReader GetMeeting(int meetingID);

		public abstract void DeleteMeeting(int meetingID, int userGroupID);

		//Audit
		public abstract int AddAudit(int impactedUserID, int currentUserGroupID, int reasonID, string notes, int createdByUserID, DateTime createdOnDate);

		#endregion
	}
}