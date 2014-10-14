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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Services.Localization;
using Config = DotNetNuke.Modules.UserGroups.Common.Config;

namespace DotNetNuke.Modules.UserGroups.Components.Common
{
    public abstract class UserGroupsModuleBase: PortalModuleBase
    {

	   #region Private Members

		const string CtlDirectorySearch = "/UserGroupWizard.ascx";
		private string _defaultControlToLoad = CtlDirectorySearch;
    		private int _profileUserID = -1;

	   #endregion

		#region Protected Properties

		/// <summary>
		/// IsAuthenticated determines whether the current user is authenticated
		/// </summary>
		protected bool IsAuthenticated {
		   get { return (UserId > Null.NullInteger) ? true : false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string ControlToLoad {
		   get {
			   if ((string)ViewState["CtlToLoad"] != string.Empty) {
				   _defaultControlToLoad = (string)ViewState["CtlToLoad"];
			   }
			   return _defaultControlToLoad;
		   }
		   set {
			   _defaultControlToLoad = value;
			   ViewState["CtlToLoad"] = value;
		   }
		}
         
		/// <summary>
		/// The profile userid is the user currently logged in or the user who is being viewed (by admin/leaders/officers). This only changes from current user if "u" is in URL. 
		/// </summary>
		public int ProfileUserID 
		{
		   get 
		   {
			   if (Request.QueryString["u"] != null) 
			   {
				   _profileUserID = Convert.ToInt32(Request.QueryString["u"]);
			   }
			   else 
			   {
				   if (UserId > 0) 
				   {
					   _profileUserID = UserId;
				   } 
			   }
			   return _profileUserID;
		   }
		   set
		   {
   			_profileUserID = value;
		   }
		}

		/// <summary>
	   /// The profile user is the user currently logged in or the user who is being viewed (by admin/leaders/officers). This only changes from current user if "u" is in URL. 
	    /// </summary>
		public UserInfo ProfileUser
    		{
    			get
    			{
				//return Entities.Users.UserController.GetCachedUser(PortalId, ProfileUser.Username);
				   return UserController.GetUserById(PortalId, ProfileUserID);
    			}
    		}

		/// <summary>
		/// 
		/// </summary>
    		public string ProfileUserAlias
    		{
				get
				{
					var userAlias = Config.NameFormat(ModuleId) == "DisplayName" ? ProfileUser.DisplayName : ProfileUser.Username;
					return userAlias;
				}
    		}

		/// <summary>
		/// 
		/// </summary>
		public int ProfileUsersGroupID {
			get {
				var groupID = -1;
				var cntUgUser = new UserGroupUserController();
				var objMember = cntUgUser.GetCachedMember(ProfileUserID, PortalId);

				if (objMember != null) {
					groupID = objMember.UserGroupID;
				}
				return groupID;
			}
		}

		public int CurrentUsersGroupID {
			get {
				var groupID = -1;
				var cntUgUser = new UserGroupUserController();
				var objMember = cntUgUser.GetCachedMember(UserId, PortalId);

				if (objMember != null) {
					groupID = objMember.UserGroupID;
				}
				return groupID;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool ProfileUserIsOfficer {
			get {
				var cntUserGroup = new UserGroupUserController();
				var objMember = cntUserGroup.GetCachedMember(ProfileUserID, PortalId);

				if (objMember != null) {
					var cntGroup = new UserGroupController();
					var objGroup = cntGroup.GetCachedUserGroup(ProfileUserID, PortalId);

					if (objGroup != null) {
						return objGroup.LeaderID == ProfileUserID || objMember.Officer;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool CurrentUserIsOfficer {
			get {
				var cntUserGroup = new UserGroupUserController();
				var objMember = cntUserGroup.GetCachedMember(UserId, PortalId);

				if (objMember != null) {
					var cntGroup = new UserGroupController();
					var objGroup = cntGroup.GetCachedUserGroup(UserId, PortalId);

					if (objGroup != null) {
						return objGroup.LeaderID == ProfileUserID || objMember.Officer;
					}
				}
				return false;
			}
		}

		#endregion

		/// <summary>
		/// GetLocalizedString is a helper method that localizes a string
		/// </summary>
		/// <param name="key">The localization key that represents the message</param>
		/// <returns>The localized string</returns>
		protected string GetLocalizedString(string key) {
			return Localization.GetString(key, LocalResourceFile);
		}

    }
}
