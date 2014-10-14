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
using DotNetNuke.Entities.Host;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Common.Utilities;
using System.Collections.Generic;
using DotNetNuke.Modules.UserGroups.Components.Data;

namespace DotNetNuke.Modules.UserGroups.Components.Controllers
{

	/// <summary>
	/// 
	/// </summary>
    public class UserGroupUserController
    {

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="userGroupID"></param>
	    /// <returns></returns>
	    public List<UserGroupUserInfo> GetCachedUsersByUserGroup(int userGroupID) {
		    var strCacheKey = Constants.CACHE_KEY + "-GroupMembers-" + userGroupID;
		    var colMembers = (List<UserGroupUserInfo>)DataCache.GetCache(strCacheKey);

		    if (colMembers == null) {
			    // caching settings
			    var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

			    colMembers = GetUsersByUserGroup(userGroupID);

			    //Cache List if timeout > 0 and collection is not null
			    if (timeOut > 0 & colMembers != null) {
				    DataCache.SetCache(strCacheKey, colMembers, TimeSpan.FromMinutes(timeOut));
			    }
		    }
		    return colMembers;
	    }

	    /// <summary>
	    /// Retrieves a list of users tied to a specific user group from the data store. 
	    /// </summary>
	    /// <param name="userGroupID">The user group we want to retrieve users for.</param>
	    /// <returns>A collection of users that belong to a specific user group.</returns>
	    private static List<UserGroupUserInfo> GetUsersByUserGroup(int userGroupID) {
		    return CBO.FillCollection<UserGroupUserInfo>(DataProvider.Instance().GetUsersByUserGroup(userGroupID));
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="userID"></param>
	    /// <param name="portalID"></param>
	    /// <returns></returns>
	    private static UserGroupUserInfo GetUserGroupUser(int userID, int portalID) {
		    return (UserGroupUserInfo)CBO.FillObject(DataProvider.Instance().GetUserGroupUser(userID, portalID), typeof(UserGroupUserInfo));
	    }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="portalID"></param>
		/// <returns></returns>
		public UserGroupUserInfo GetCachedMember(int userID, int portalID)
		{
			var strCacheKey = Constants.CACHE_KEY + "-Member-" + userID + "-" + portalID;
		    var objMember = (UserGroupUserInfo)DataCache.GetCache(strCacheKey);

		    if (objMember == null) {
			    // caching settings
			    var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

			    objMember = GetUserGroupUser(userID, portalID);

			    //Cache List if timeout > 0 and collection is not null
			    if (timeOut > 0 & objMember != null) {
				    DataCache.SetCache(strCacheKey, objMember, TimeSpan.FromMinutes(timeOut));
			    }
		    }
		    return objMember;
	    }

	    /// <summary>
	    /// Retrieves a list of officers associated with a specific group.
	    /// </summary>
	    /// <param name="userGroupID">The user group we want to retrieve users for.</param>
	    /// <returns>A collection of users that are officers of the specified user group.</returns>
	    public List<UserGroupUserInfo> GetGroupOfficers(int userGroupID) {
		    return CBO.FillCollection<UserGroupUserInfo>(DataProvider.Instance().GetGroupOfficers(userGroupID));
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="userGroupID"></param>
	    /// <param name="userID"></param>
	    /// <param name="portalID"></param>
	    public void JoinGroup(int userGroupID, int userID, int portalID) {
			DataProvider.Instance().JoinUserGroup(userGroupID, userID);
	    		ClearUserGroupUsersCache(userGroupID);
			ClearCachedMember(userID, portalID, userGroupID);
	    }

	    /// <summary>
	    /// Removes a user to group association in the data store. 
	    /// </summary>
	    /// <param name="userGroupID"></param>
	    /// <param name="userID"></param>
	    /// <param name="portalID"></param>
	    public void LeaveGroup(int userGroupID, int userID, int portalID) {
		    DataProvider.Instance().LeaveUserGroup(userGroupID, userID);
		    ClearUserGroupUsersCache(userGroupID);
		    ClearCachedMember(userID, portalID, userGroupID);
	    }

	    /// <summary>
	    /// Changes officer status of current group member. 
	    /// </summary>
	    /// <param name="userGroupID"></param>
	    /// <param name="userID"></param>
	    /// <param name="isOfficer"></param>
	    /// <param name="portalID"></param>
	    /// <remarks>Users are already group members when this is called.</remarks>
	    public void UpdateOfficer(int userGroupID, int userID, bool isOfficer, int portalID) {
			DataProvider.Instance().UpdateOfficer(userGroupID, userID, isOfficer);
			// Clear group users cache
			ClearUserGroupUsersCache(userGroupID);
			ClearCachedMember(userID, portalID, userGroupID);
	    }

	    /// <summary>
	    /// Changes the group leader in the data store, marks old leader as normal member.
	    /// </summary>
	    /// <param name="userGroupID"></param>
	    /// <param name="newLeaderID"></param>
	    /// <param name="oldLeaderID"></param>
	    /// <param name="portalID"></param>
	    /// <remarks>Users (new leader/old leader) are always members at this point.</remarks>
	    public void UpdateLeader(int userGroupID, int newLeaderID, int oldLeaderID, int portalID) {
		    DataProvider.Instance().UpdateLeader(userGroupID, newLeaderID, oldLeaderID);
		    // Clear group users cache
		    ClearUserGroupUsersCache(userGroupID);
		    ClearCachedMember(newLeaderID, portalID, userGroupID);
		    ClearCachedMember(oldLeaderID, portalID, userGroupID);
	    }

	    /// <summary>
	    /// Clears the cache of group members.
	    /// </summary>
	    /// <param name="userGroupID"></param>
	    private static void ClearUserGroupUsersCache(int userGroupID) {
		    var strCacheKey = Constants.CACHE_KEY + "-GroupMembers-" + userGroupID;
		    DataCache.RemoveCache(strCacheKey);
	    }

		/// <summary>
		/// Clears the cache for an individual user.
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="portalID"></param>
		/// <param name="userGroupID"></param>
		private static void ClearCachedMember(int userID, int portalID, int userGroupID)
		{
			var strCacheKey = Constants.CACHE_KEY + "-Member-" + userID + "-" + portalID;
		    DataCache.RemoveCache(strCacheKey);
	    }

    }
}