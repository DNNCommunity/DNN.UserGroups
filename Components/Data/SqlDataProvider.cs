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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace DotNetNuke.Modules.UserGroups.Components.Data
{

	/// <summary>
	/// SQL Server implementation of the abstract DataProvider class
	/// </summary>
	public class SqlDataProvider : DataProvider {

		#region Private Members

		private const string ModuleQualifier = "UserGroups_";
		private const string ProviderType = "data";

		private readonly string _connectionString;
		private readonly string _databaseOwner;
		private readonly string _objectQualifier;
		private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
          private readonly string _providerPath;

		#endregion

		#region Constructors

		public SqlDataProvider()
		{
			// Read the configuration specific information for this provider
			var objProvider = (Provider) _providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

			// This code handles getting the connection string from either the connectionString / appsetting section and uses the connectionstring section by default if it exists.  
			// Get Connection string from web.config
			_connectionString = Config.GetConnectionString();

			// If above funtion does not return anything then connectionString must be set in the dataprovider section.
			if (_connectionString == string.Empty)
			{
				// Use connection string specified in provider
				_connectionString = objProvider.Attributes["connectionString"];
			}

			_providerPath = objProvider.Attributes["providerPath"];

			_objectQualifier = objProvider.Attributes["objectQualifier"];
			if (_objectQualifier != string.Empty & _objectQualifier.EndsWith("_") == false)
			{
				_objectQualifier += "_";
			}

			_databaseOwner = objProvider.Attributes["databaseOwner"];
			if (_databaseOwner != string.Empty & _databaseOwner.EndsWith(".") == false)
			{
				_databaseOwner += ".";
			}
		}

		#endregion

		#region Properties

		public string ConnectionString
		{
			get { return _connectionString; }
		}

		public string ProviderPath
		{
			get { return _providerPath; }
		}

		public string ObjectQualifier
		{
			get { return _objectQualifier; }
		}

		public string DatabaseOwner
		{
			get { return _databaseOwner; }
		}

		private string NamePrefix
		{
			get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
		}

		#endregion

		#region Private Methods

		private static object GetNull(object field)
		{
			return Null.GetNull(field, DBNull.Value);
		}

		#endregion

		#region Public Methods

		public override int AddUserGroup(int portalID, int leaderID, string name, string country, string region, string city, string url, string logo, bool active, double latitude, double longitude, string twitterUrl, string linkedInUrl, string facebookUrl, string about, int languageID, int createdByUserID, DateTime createdOnDate, int moduleID, string meetingAddress, string defaultLanguage) 
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "UserGroup_Add", portalID, GetNull(leaderID), name, country, GetNull(region), GetNull(city), GetNull(url), GetNull(logo), active, GetNull(latitude), GetNull(longitude), GetNull(twitterUrl), GetNull(linkedInUrl), GetNull(facebookUrl), GetNull(about), GetNull(languageID), createdByUserID, createdOnDate, moduleID, GetNull(meetingAddress), GetNull(defaultLanguage)));
		}

		public override void UpdateUserGroup(int userGroupID, int portalID, int leaderID, string name, string country, string region, string city, string url, string logo, bool active, double latitude, double longitude, string twitterUrl, string linkedInUrl, string facebookUrl, string about, int languageID, int lastModifiedByUserID, DateTime lastModifiedOnDate, string meetingAddress, int contentItemID, string defaultLanguage) 
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UserGroup_Update", userGroupID, portalID, leaderID, name, country, region, city, GetNull(url), GetNull(logo), active, GetNull(latitude), GetNull(longitude), GetNull(twitterUrl), GetNull(linkedInUrl), GetNull(facebookUrl), GetNull(about), languageID, GetNull(lastModifiedByUserID), GetNull(lastModifiedOnDate), GetNull(meetingAddress), contentItemID, GetNull(defaultLanguage));
		}

		public override void DeleteUserGroup(int userGroupID, int moduleID) 
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteUserGroup", userGroupID);
		}

		public override IDataReader SearchGroups(int portalId, int pageIndex, int pageSize, string country, string region, string city, string name, int languageID, int propertyDefinitionID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "SearchGroups", portalId, pageIndex, pageSize, GetNull(country), GetNull(region), GetNull(city), GetNull(name), languageID, propertyDefinitionID);
		}

		public override IDataReader SearchGroupsByLocation(int portalId, float latitude, float longitude, int distance, bool inKilometers, int propertyDefinitionID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "SearchGroupsByLocation", portalId, latitude, longitude, distance, inKilometers, propertyDefinitionID);
		}

		public override IDataReader GetSitemapUrLs(int portalID) 
		{
		     return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetSitemapURLs", portalID);
		}

		public override IDataReader GetUserGroup(int userGroupID) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "UserGroup_Get", userGroupID);
		}

		public override IDataReader GetFile(int userID, int fileID) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetAvatar", userID, fileID);
		}

		// Regions
		public override IDataReader GetGroupRegions(int portalID) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "Region_Get", portalID);
		}

		public override IDataReader GetGroupRegionsByCountry(int portalID, string country) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "Region_GetByCountry", portalID, country);
		}

		// User Group Users
		public override IDataReader GetUsersByUserGroup(int userGroupID) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetUsersByUserGroup", userGroupID);
		}

		public override IDataReader GetUserGroupUser(int userID, int portalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GroupUser_Get", userID, portalID);
		}

		public override void JoinUserGroup(int userGroupID, int userID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "JoinUserGroup", userGroupID, userID);
		}

		public override void LeaveUserGroup(int userGroupID, int userID) {
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UserGroup_Leave", userGroupID, userID);
		}

		public override void UpdateOfficer(int userGroupID, int userID, bool isOfficer) {
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UserGroup_OfficerUpdate", userGroupID, userID, isOfficer);
		}

		public override void UpdateLeader(int userGroupID, int newLeaderID, int oldLeaderID) {
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UserGroup_LeaderUpdate", userGroupID, newLeaderID, oldLeaderID);
		}

		public override IDataReader GetGroupOfficers(int userGroupID) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "Users_GetOfficers", userGroupID);
		}

		//Languages
		public override int AddLanguage(int portalID, string language) {
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "LanguageAdd", portalID, language));
		}

		public override IDataReader GetLanguages(int portalID) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "LanguagesGet", portalID);
		}

		//Reasons
		public override IDataReader GetActiveReasons(int portalID) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "ReasonsGetActive", portalID);
		}

		//Meetings
		public override int AddUserGroupMeeting(int userGroupID, int eventID, string title, string description, string location, DateTime meetingDate) {
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "Meetings_Add", userGroupID, eventID, title, description, location, meetingDate));
		}

		public override void UpdateUserGroupMeeting(int meetingID, int userGroupID, int eventID, string title, string description, string location, DateTime meetingDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "Meetings_Update", meetingID, userGroupID, eventID, title, description, location, meetingDate);
		}

		public override IDataReader SpotlightSearch(int portalID, int pageSize, int pageIndex, int nextDays, bool requireMeetingLocation) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "Spotlight_Search", portalID, pageSize, pageIndex, nextDays, requireMeetingLocation);
		}

		public override IDataReader GetUpcommingGroupMeetings(int userGroupID, DateTime startDate, int pageSize, int pageIndex) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "Meetings_GetGroupUpcomming", userGroupID, startDate, pageSize, pageIndex);
		}

		public override IDataReader GetAllMeetingsByDate(int portalID, DateTime startDate, DateTime endDate, string region, int pageIndex, int pageSize)
		{
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "Meetings_GetAllByDate", portalID, startDate, endDate, region, pageIndex, pageSize);
		}

		public override IDataReader GetMeeting(int meetingID) {
			return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "Meetings_Get", meetingID);
		}

		public override void DeleteMeeting(int meetingID, int userGroupID) {
			SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "Meetings_Delete", meetingID, userGroupID);
		}

		//Audit 
		public override int AddAudit(int impactedUserID, int currentUserGroupID, int reasonID, string notes, int createdByUserID, DateTime createdOnDate) {
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "Audit_Add", impactedUserID, currentUserGroupID, reasonID, notes, createdByUserID, createdOnDate));
		}

		#endregion
	}
}