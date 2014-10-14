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
using System.Linq;
using System.Text;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Modules.UserGroups.Components.Data;
using DotNetNuke.Modules.UserGroups.Components.Entities;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using DotNetNuke.Entities.Host;
using Config = DotNetNuke.Modules.UserGroups.Common.Config;

namespace DotNetNuke.Modules.UserGroups.Components.Controllers
{
    public class UserGroupController
    {

    	/// <summary>
    	/// Adds a group to the data store and returns the primary key that was generated for hte new Group that was just added. 
    	/// </summary>
    	/// <param name="userGroup">The User Group to add.</param>
    	/// <param name="user"></param>
    	/// <param name="tabID"></param>
    	/// <returns>The ID of the newly created User Group.</returns>
    	public UserGroupInfo AddUserGroup(UserGroupInfo userGroup, UserInfo user, int tabID) {
			userGroup.UserGroupID = DataProvider.Instance().AddUserGroup(userGroup.PortalID, userGroup.LeaderID, userGroup.Name, userGroup.Country, userGroup.Region, userGroup.City, userGroup.Url, userGroup.Logo, userGroup.Active, userGroup.Latitude, userGroup.Longitude, userGroup.TwitterUrl, userGroup.LinkedInUrl, userGroup.FacebookUrl, userGroup.About, userGroup.LanguageID, userGroup.CreatedByUserID, userGroup.CreatedOnDate, userGroup.ModuleID, userGroup.MeetingAddress, userGroup.DefaultLanguage);

		    // Send notifications
		    userGroup.ContentItemId = CompleteEntryCreation(userGroup, tabID);

		    ClearCache(userGroup.UserGroupID);
    			return userGroup;
    		}

	    /// <summary>
	    /// Updates an existing user group in the data store. 
	    /// </summary>
	    /// <param name="userGroup">The UserGroupInfo object to update</param>
	    public void UpdateUserGroup(UserGroupInfo userGroup) {
		    //DataService.UpdateUserGroup(userGroup);
		    DataProvider.Instance().UpdateUserGroup(userGroup.UserGroupID, userGroup.PortalID, userGroup.LeaderID, userGroup.Name, userGroup.Country, userGroup.Region, userGroup.City, userGroup.Url, userGroup.Logo, userGroup.Active, userGroup.Latitude, userGroup.Longitude, userGroup.TwitterUrl, userGroup.LinkedInUrl, userGroup.FacebookUrl, userGroup.About, userGroup.LanguageID, userGroup.LastModifiedByUserID, userGroup.LastModifiedOnDate, userGroup.MeetingAddress, userGroup.ContentItemId, userGroup.DefaultLanguage);

		    ClearCache(userGroup.UserGroupID);
	    }

	    /// <summary>
	    /// Deletes a User Group from the data store.
	    /// </summary>
	    /// <param name="userGroupID">The Id of the User Group to delete.</param>
	    /// <param name="portalID">The Id of the module.</param>
	    public static void DeleteUserGroup(int userGroupID, int portalID) {
		    DataProvider.Instance().DeleteUserGroup(userGroupID, portalID);
		    ClearCache(userGroupID);
	    }

	    /// <summary>
	    /// Returns a single UserGroup.
	    /// </summary>
	    /// <param name="userGroupID"></param>
	    /// <returns></returns>
	    private static UserGroupInfo GetUserGroup(int userGroupID) {
		    return (UserGroupInfo)CBO.FillObject(DataProvider.Instance().GetUserGroup(userGroupID), typeof(UserGroupInfo));
	    }

    	/// <summary>
    	/// This is the method used by DirectorySearch.
    	/// </summary>
    	/// <param name="portalId"></param>
    	/// <param name="pageIndex"></param>
    	/// <param name="pageSize"></param>
    	/// <param name="country"></param>
    	/// <param name="region"></param>
    	/// <param name="city"></param>
    	/// <param name="name"></param>
    	/// <param name="languageID"></param>
    	/// <param name="propertyDefinitionID"></param>
    	/// <returns></returns>
    	public List<UserGroupInfo> SearchGroups(int portalId, int pageIndex, int pageSize, string country, string region, string city, string name, int languageID, int propertyDefinitionID) 
	    {
		    if (pageIndex == Null.NullInteger) 
		    {
			    pageIndex = 0;
		    }

		    if (pageSize == Null.NullInteger)
		    {
		    	pageSize = Int32.MaxValue;
		    }

		    //country, region, city, name, languageid
		    if (country == "<" + "Not Specified" + ">") 
		    {
		    	country = string.Empty;
		    }

		    if (region == "<" + "Not Specified" + ">") 
		    {
			    region = string.Empty;
		    }

		    return CBO.FillCollection<UserGroupInfo>(DataProvider.Instance().SearchGroups(portalId, pageIndex, pageSize, country, region, city, name, languageID, propertyDefinitionID));
	    }

	public List<ServiceUserGroupInfo> ServiceSearchGroups(int portalId, int pageIndex, int pageSize, string country, string region, string city, string name, int languageID, int propertyDefinitionID) {
		    if (pageIndex == Null.NullInteger) {
			    pageIndex = 0;
		    }

		    if (pageSize == Null.NullInteger) {
			    pageSize = Int32.MaxValue;
		    }

		    //country, region, city, name, languageid
		    if (country == "<" + "Not Specified" + ">") {
			    country = string.Empty;
		    }

		    if (region == "<" + "Not Specified" + ">") {
			    region = string.Empty;
		    }

		    return CBO.FillCollection<ServiceUserGroupInfo>(DataProvider.Instance().SearchGroups(portalId, pageIndex, pageSize, country, region, city, name, languageID, propertyDefinitionID));
	    }

    		/// <summary>
    		/// This is used by the LocatorService only at this time.
    		/// </summary>
    		/// <param name="portalId"></param>
    		/// <param name="latitude"></param>
    		/// <param name="longitude"></param>
    		/// <param name="distance"></param>
    		/// <param name="inKilometers"></param>
    		/// <param name="propertyDefinitionID"></param>
    		/// <returns></returns>
    		public List<ServiceUserGroupInfo> SearchGroupsByLocation(int portalId, float latitude, float longitude, int distance, bool inKilometers, int propertyDefinitionID) {
		    return CBO.FillCollection<ServiceUserGroupInfo>(DataProvider.Instance().SearchGroupsByLocation(portalId, latitude, longitude, distance, inKilometers, propertyDefinitionID));
	    }

	    public List<UserGroupInfo> GetSitemapUrLs(int portalID) {
		    return CBO.FillCollection<UserGroupInfo>(DataProvider.Instance().GetSitemapUrLs(portalID));
	    }

	    /// <summary>
	    /// GetUserGroup fetches a single UserGroup
	    /// </summary>
	    /// <param name="userGroupID">The Id of the User Group to fetch</param>
	    /// <param name="portalID"></param>
	    /// <returns>A UserGroupInfo object</returns>
	    public UserGroupInfo GetCachedUserGroup(int userGroupID, int portalID) {
		    var strCacheKey = Constants.CACHE_KEY + "-SingleGroup-" + userGroupID;
		    var objGroup = (UserGroupInfo)DataCache.GetCache(strCacheKey);

		    if (objGroup == null) {
			    // caching settings
			    var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

			    objGroup = GetUserGroup(userGroupID);

			    //Cache List if timeout > 0 and collection is not null
			    if (timeOut > 0 & objGroup != null) {
				    DataCache.SetCache(strCacheKey, objGroup, TimeSpan.FromMinutes(timeOut));
			    }
		    }
		    return objGroup;
	    }

	    public AvatarInfo GetCachedFile(int userID, int fileID) {
		    var strCacheKey = "UserGroup-FileObject-Cache-" + userID + "-" + fileID;
		    var objFile = (AvatarInfo)DataCache.GetCache(strCacheKey);

		    if (objFile == null) {
			    // caching settings
			    var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

			    objFile = GetFile(userID, fileID);

			    //Cache List if timeout > 0 and collection is not null
			    if (timeOut > 0 & objFile != null) {
				    DataCache.SetCache(strCacheKey, objFile, TimeSpan.FromMinutes(timeOut));
			    }
		    }
		    return objFile;
	    }

	    public AvatarInfo GetFile(int userID, int fileID) {
		    return (AvatarInfo)CBO.FillObject(DataProvider.Instance().GetFile(userID, fileID), typeof(AvatarInfo));
	    }

        /// <summary>
        /// Activeates/Deactivates a User Group
        /// </summary>
        /// <param name="userGroupID">The Id of the User Group to (de)activate</param>
        /// <param name="activate">Switch that indicates whether we are activating (true) or deactivating (false)</param>
        private static void ActivateUserGroup(int userGroupID, bool activate)
        {
			//Get the User Group
			var userGroup = GetUserGroup(userGroupID);

			//Update the status
			userGroup.Active = activate;

			//Update the UserGroup
			var cntUg = new UserGroupController();
			cntUg.UpdateUserGroup(userGroup);
        }

        /// <summary>
        /// AddLogProperty adds a log property to a LogInfo object
        /// </summary>
        /// <param name="log">LogInfo object to add the property to</param>
        /// <param name="name">Name of the Property</param>
        /// <param name="value">Value of the Property</param>
        private static void AddLogProperty(LogInfo log, string name, string value)
        {
            log.LogProperties.Add(new LogDetailInfo(name, value));
        }

		/// <summary>
		/// Clears the UserGroup Cache
		/// </summary>
		/// <param name="userGroupID"></param>
    		public static void ClearCache(int userGroupID)
		{
			var strCacheKey = Constants.CACHE_KEY + "-SingleGroup-" + userGroupID;
			DataCache.RemoveCache(strCacheKey);
		}

        #region Public Static Methods

        /// <summary>
        /// ActivateUserGroup activates a User Group
        /// </summary>
        /// <param name="userGroupID">The Id of the User Group to activate</param>
        public static void ActivateUserGroup(int userGroupID)
        {
            ActivateUserGroup(userGroupID, true);
        }

        /// <summary>
        /// DeactivateUserGroup deactivates a User Group
        /// </summary>
        /// <param name="userGroupID">The Id of the User Group to deactivate</param>
        public static void DeactivateUserGroup(int userGroupID)
        {
            ActivateUserGroup(userGroupID, false);
        }

        /// <summary>
        /// IsValid tests if the User Group already exists and can be created (uses country, region, city and language). 
        /// </summary>
        /// <param name="newUserGroup">The new User Group to validate</param>
        public static bool IsValid(UserGroupInfo newUserGroup)
        {
            var isValid = true;

        	var cntUg = new UserGroupController();
		foreach (var userGroup in cntUg.SearchGroups(newUserGroup.PortalID, 0, 1000, newUserGroup.Country, newUserGroup.Region, newUserGroup.City, "", -1,  Config.PropertyDefinitionID(newUserGroup.ModuleID)))
            {
                if (userGroup.Region == newUserGroup.Region && userGroup.City == newUserGroup.City && userGroup.LanguageID == newUserGroup.LanguageID)
                    isValid = false;
            }

            return isValid;
        }

    	/// <summary>
    	/// JoinUserGroup adds the user  to the User Group
    	/// </summary>
    	/// <param name="userGroup">The user Group</param>
    	/// <param name="user">The user</param>
    	/// <param name="settings"></param>
    	/// <param name="tabID"></param>
    	public int JoinUserGroup(UserGroupInfo userGroup, UserInfo user, PortalSettings settings, int tabID)
		{
			var userGroupID = -1;
			if (userGroup.UserGroupID == Null.NullInteger)
			{
				//Need to first add the User Group
				userGroup = AddUserGroup(userGroup, user, tabID);
				userGroup.UserGroupID = userGroupID;
				UpdateUserGroup(userGroup);
			}
			else
			{
				userGroupID = userGroup.UserGroupID;
			}

			var cntUgUser = new UserGroupUserController();
			cntUgUser.JoinGroup(userGroupID, user.UserID, user.PortalID);
			var profileLink = Util.ViewControlLink(tabID, userGroup.ModuleID, userGroupID, PageScope.GroupProfile);

			//SendNotification of New Member
			SendNotifications(user, userGroup, settings, EmailType.NewMember, "", profileLink);

			//Send Notification if membership is at threshold
			if (userGroup.Members + 1 == Config.MinSize(userGroup.ModuleID))
			{
				ActivateUserGroup(userGroup.UserGroupID);
				SendNotifications(user, userGroup, settings, EmailType.MinimumMembership, "", profileLink);
			}

			ClearCache(userGroup.PortalID);
			return userGroupID;
		}

    	/// <summary>
    	/// 
    	/// </summary>
    	/// <param name="userGroup"></param>
    	/// <param name="user"></param>
    	/// <param name="settings"></param>
    	/// <param name="tabID"></param>
    	/// <returns></returns>
    	public void LeaveUserGroup(UserGroupInfo userGroup, UserInfo user, PortalSettings settings, int tabID) {
			var cntUgUser = new UserGroupUserController();
			cntUgUser.LeaveGroup(userGroup.UserGroupID, user.UserID, user.PortalID);
			var profileLink = Util.ViewControlLink(tabID, userGroup.ModuleID, userGroup.UserGroupID, PageScope.GroupProfile);

			SendNotifications(user, userGroup, settings, EmailType.LeaveGroup, "", profileLink);

			//Send Notification if membership is below threshold
			if (userGroup.Members - 1 == Config.MinSize(userGroup.ModuleID)) 
			{
				DeactivateUserGroup(userGroup.UserGroupID);
				SendNotifications(user, userGroup, settings, EmailType.MinimumMembership, "", profileLink);
			}

			ClearCache(userGroup.PortalID);
		}

		/// <summary>
		/// This completes the things necessary for creating a content item in the data store. 
		/// </summary>
		/// <param name="objGroup">The User Group  we just created in the data store.</param>
		/// <param name="tabId">The page we will associate with our content item.</param>
		/// <returns>The ContentItemId primary key created in the Core ContentItems table.</returns>
		public static int CompleteEntryCreation(UserGroupInfo objGroup, int tabId) {
			var cntTaxonomy = new Content();
			var objContentItem = cntTaxonomy.CreateContentItem(objGroup, tabId);

			return objContentItem.ContentItemId;
		}

		private static void CompleteEntryUpdate(UserGroupInfo objGroup, int tabId) {
			var cntTaxonomy = new Content();
			cntTaxonomy.UpdateContentItem(objGroup, tabId);

			// clear active entry cache and single item cache.
		}

	    /// <summary>
	    /// This is currently used for the Send Message control only, as a bulk email to group members.
	    /// </summary>
	    /// <param name="message"></param>
	    /// <param name="subject"></param>
	    /// <param name="userGroup"></param>
	    /// <param name="portalId"></param>
	    /// <param name="recipientID"></param>
		public static void SendMessage(string message, string subject, UserGroupInfo userGroup, int portalId, int recipientID)
		{
			var leaderEmail = Null.NullString;
			var emailTo = Null.NullString;
			var emailBcc = Null.NullString;

			if (userGroup == null) return;
			var cntUser = new UserController();
			var leader = cntUser.GetUser(portalId, userGroup.LeaderID);
			if (leader != null)
				leaderEmail = leader.Email;

			if (recipientID == -1)
			{
				emailTo = leaderEmail;

				//Send to all
				var cntUgUsers = new UserGroupUserController();
				var colUsers = cntUgUsers.GetCachedUsersByUserGroup(userGroup.UserGroupID);

				emailBcc = colUsers.Aggregate(emailBcc, (current, user) => current + (user.Email + "; "));

				//remove trailing ";"
				if (emailBcc.Length > 2)
	    				emailBcc = emailBcc.Substring(0, emailBcc.Length - 2);
			}
			else
			{
				//Send to one
				var user = cntUser.GetUser(portalId, recipientID);

				if (user != null)
	    				emailTo = user.Email;
			}

			//Send the email
			Mail.SendMail(leaderEmail, emailTo, "", emailBcc, MailPriority.Normal, subject, MailFormat.Text, Encoding.UTF7, message, "", "", "", "", "");
		}

    	/// <summary>
    	/// SendNotifications sends emails to the appropriate people when certain events happen
    	/// </summary>
    	/// <param name="user">The user triggering the event</param>
    	/// <param name="userGroup">The User group</param>
    	/// <param name="settings">The portal settings</param>
    	/// <param name="type">the type of email></param>
    	/// <param name="messageBody">This will be the body of the email, only necessary for Contact Officers type.</param>
    	/// <param name="profileLink">A full link to the group's profile page.</param>
    	public static void SendNotifications(UserInfo user, UserGroupInfo userGroup, PortalSettings settings, EmailType type, string messageBody, string profileLink)
        {
            var leaderEmail = Null.NullString;
		  var adminEmail = Config.AdminMail(userGroup.ModuleID);
		  var emailCc = Null.NullString;
		  var emailTo = Null.NullString;
        	  var emailBcc = Null.NullString;
		  var subject = Null.NullString;
		  var body = Null.NullString;
    		  var emailFrom = Null.NullString;


            //If Admin Email not specified use Portal Admin Email)
            if (String.IsNullOrEmpty(adminEmail))
                adminEmail = settings.Email;

            //First Log the Notification
		  var controller = new LogController();
		  var log = new LogInfo
		            	{
		            		LogUserID = user.UserID,
		            		LogPortalID = settings.PortalId,
		            		LogTypeKey = "DEBUG",
		            		LogPortalName = settings.PortalName
		            	};

		  var cntUser = new UserController();
		  var leader = cntUser.GetUser(settings.PortalId, userGroup.LeaderID);
            if (leader != null)
                leaderEmail = leader.Email;

            switch (type)
            {
			  case EmailType.NewMember: // May need to switch to/cc
                    AddLogProperty(log, "USERGROUP_NewMember", "New Member.");
                    AddLogProperty(log, "UserGroupId", userGroup.UserGroupID.ToString());
                    AddLogProperty(log, "UserGroup Name", userGroup.Name);
                    AddLogProperty(log, "UserId", user.UserID.ToString());
                    AddLogProperty(log, "UserName", user.Username);

                    //Send an email to the UGL that there is a new User
                    subject = String.Format(Localization.GetString("EMAIL_NewMember_Subject", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name);
                    body = String.Format(Localization.GetString("EMAIL_NewMember_Body", Constants.LOCALIZATION_SharedResourceFile), leader.DisplayName, userGroup.Name, user.Username, profileLink);
                    emailTo = leaderEmail;
                    emailCc = user.Email;
            		emailFrom = adminEmail;
                    break;
                case EmailType.MinimumMembership:
                    AddLogProperty(log, "USERGROUP_MinimumMembership", "Minimum Membership Reached.");
                    AddLogProperty(log, "UserGroupId", userGroup.UserGroupID.ToString());
                    AddLogProperty(log, "UserGroup Name", userGroup.Name);

                    //Send an email to the Admin (cc'd to the UGL) that the UserGroup has reached minimum membership
                    subject = String.Format(Localization.GetString("EMAIL_MinimumMembership_Subject", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name);
                    body = String.Format(Localization.GetString("EMAIL_MinimumMembership_Body", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name, profileLink);
                    emailCc = leaderEmail;
                    emailTo = adminEmail;
				emailFrom = adminEmail;
                    break;
                case EmailType.Activate:
                    AddLogProperty(log, "USERGROUP_Activated", "UserGroup Activated.");
                    AddLogProperty(log, "UserGroupId", userGroup.UserGroupID.ToString());
                    AddLogProperty(log, "UserGroup Name", userGroup.Name);

                    //Send an email to the User Group Leader (cc'd to the Admin) that the UserGroup has ben Activated
                    subject = String.Format(Localization.GetString("EMAIL_Activitated_Subject", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name);
                    body = String.Format(Localization.GetString("EMAIL_Activitated_Body", Constants.LOCALIZATION_SharedResourceFile), user.DisplayName, userGroup.Name, profileLink);
                    emailCc = adminEmail;
                    emailTo = leaderEmail;
				emailFrom = adminEmail;
                    break;
			 case EmailType.DeActivate:
                    AddLogProperty(log, "USERGROUP_DeActivated", "UserGroup De-Activated.");
                    AddLogProperty(log, "UserGroupId", userGroup.UserGroupID.ToString());
                    AddLogProperty(log, "UserGroup Name", userGroup.Name);

                    //Send an email to the User Group Leader (cc'd to the Admin) that the UserGroup has ben De-Activated
                    subject = String.Format(Localization.GetString("EMAIL_DeActivitated_Subject", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name);
                    body = String.Format(Localization.GetString("EMAIL_DeActivitated_Body", Constants.LOCALIZATION_SharedResourceFile), user.DisplayName, userGroup.Name, profileLink);
                    emailCc = adminEmail;
                    emailTo = leaderEmail;
				emailFrom = adminEmail;
                    break;
			 case EmailType.LeaveGroup: // Seems Good
				AddLogProperty(log, "USERGROUP_LeaveGroup", "Member Leaving Group.");
				AddLogProperty(log, "UserGroupId", userGroup.UserGroupID.ToString());
				AddLogProperty(log, "UserGroup Name", userGroup.Name);
				AddLogProperty(log, "UserId", user.UserID.ToString());
				AddLogProperty(log, "UserName", user.Username);

				//Send an email to the UGL that there is a user leaving (cc the user leaving, Bcc the admin)
				subject = String.Format(Localization.GetString("EMAIL_Leaving_Subject", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name);
				body = String.Format(Localization.GetString("EMAIL_Leaving_Body", Constants.LOCALIZATION_SharedResourceFile), leader.DisplayName, userGroup.Name, user.Username, profileLink);
				emailTo = leaderEmail;
				emailCc = user.Email;
            		emailBcc = adminEmail;
				emailFrom = adminEmail;
				break;
			 case EmailType.AddMeeting: // Not Active Yet
				AddLogProperty(log, "USERGROUP_AddMeeting", "Group Meeting Added.");
				AddLogProperty(log, "UserGroupId", userGroup.UserGroupID.ToString());
				AddLogProperty(log, "UserGroup Name", userGroup.Name);
				//AddLogProperty(log, "UserId", user.UserID.ToString());
				//AddLogProperty(log, "UserName", user.Username);

				////Send an email to the UGL that there is a user leaving (cc the user leaving, Bcc the admin)
				//subject = String.Format(Localization.GetString("EMAIL_NewMeeting_Subject", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name);
				//body = String.Format(Localization.GetString("EMAIL_NewMeeting_Body", Constants.LOCALIZATION_SharedResourceFile), leader.DisplayName, userGroup.Name, user.Username);
				//emailTo = leaderEmail;
				  // loop through all members, create list for bcc
				//emailBcc = Null.NullString;
				break;
			  case EmailType.ContactOfficers: // New
				AddLogProperty(log, "USERGROUP_ContactOfficers", "Contacting Group Officers.");
				AddLogProperty(log, "UserGroupId", userGroup.UserGroupID.ToString());
				AddLogProperty(log, "UserGroup Name", userGroup.Name);

				subject = String.Format(Localization.GetString("EMAIL_ContactOfficers_Subject", Constants.LOCALIZATION_SharedResourceFile), userGroup.Name, profileLink);
                    body = messageBody;

            		var cntUgu = new UserGroupUserController();
            		var colUsers = cntUgu.GetGroupOfficers(userGroup.UserGroupID);

            		emailTo = colUsers.Aggregate(emailTo, (current, objUser) => current + objUser.Email + ";");
				//emailCc = Null.NullString;
				emailFrom = user.Email;
				//emailBcc = Null.NullString; //this shouldn't be necessary, but just in case.
            		break;
            }

            //Log the Notification
            controller.AddLog(log);

            //Send the email
		  Mail.SendMail(emailFrom,  emailTo, emailCc, emailBcc, MailPriority.Normal, subject, MailFormat.Text, Encoding.UTF7, body, "", "", "", "", "");
        }

        #endregion

    }
}