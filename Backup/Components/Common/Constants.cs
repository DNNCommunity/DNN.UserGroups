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

namespace DotNetNuke.Modules.UserGroups.Common
{

    /// <summary>
    /// The Constants helper class provides a central area for Module Constants
    /// </summary>
    internal static class Constants
    {

        #region Internal Constants

		internal const string CACHE_KEY = "UserGroups_{0}";
		internal const int CACHE_TIMEOUT = 20;

		internal const string DATA_QUALIFIER = "UserGroups_";

		internal const int DEFAULT_EVENTMODULE = -1;
		internal const int DEFAULT_MINSIZE = 10;
		internal const bool DEFAULT_PUBLISHEVENTS = false;
		internal const string DEFAULT_JOINMESSAGE = "Thanks for joining a DotNetNuke user group!";
		internal const int DEFAULT_PAGESIZE = 10;
    		internal const string DEFAULT_NAMEFORMAT = "DisplayName";
		internal const string DEFAULT_MAPKEY = "ABQIAAAAseSaoksKmB4i3qluwZhcOBT2yXp_ZAY8_ufC3CFXhHIE1NvwkxS_A_LUpl6xiHmvZVQpG2wQ76mMLw";
		internal const string DEFAULT_COUNTRYSELECTMSG = "Select a country from the drop down to begin the search process.";
		internal const string DEFAULT_REGIONSELECTMSG = "Select a region from the drop down to locate groups in the area.";
		internal const string DEFAULT_LANGUAGESELECTMSG = "Select a language from the drop down to locate groups in your preferred tongue.";
		internal const string DEFAULT_JOINSELECTMSG = "Click a group name to view detailed information about the group, or click Join Group if you are ready to join.";
		internal const int DEFAULT_PROPERTYDEFINITIONID = -1;

		internal const string DEFAULT_SPOTLIGHT_MEETINGMAXDAYCOUNT = "90";
		internal const string DEFAULT_SPOTLIGHT_REQUIREMEETINGADDRESS = "True";
		internal const string DEFAULT_SPOTLIGHT_TEMPLATE = "";
		internal const string DEFAULT_SPOTLIGHT_GROUPPAGE = "-1";
		internal const string DEFAULT_SPOTLIGHT_MODULE = "-1";
    		internal const string DEFAULT_SPOTLIGHT_PAGESIZE = "5";
		internal const string DEFAULT_SPOTLIGHT_HEIGHT = "200";
		internal const string DEFAULT_SPOTLIGHT_WIDTH = "200";
		internal const string DEFAULT_SPOTLIGHT_ITEM_HEIGHT = "200";
		internal const string DEFAULT_SPOTLIGHT_ITEM_WIDTH = "200";
		internal const string DEFAULT_SPOTLIGHT_FRAME_DURATION = "2000";
		internal const string DEFAULT_SPOTLIGHT_SCROLL_DURATION = "500";

		internal const string DEFAULT_MYGROUP_JOINTEMPLATE = "<div>[JOINLINK]</div>";
		internal const string DEFAULT_MYGROUP_GROUPTEMPLATE = @"<div><a href='[GROUPLINK]'>[GROUPNAME]</a><br />Next Meeting: [MEETINGDATE]<br /><a href='[MEETINGLINK]'>[MEETINGTITLE]</a>[FLAG]</div>";
		internal const string DEFAULT_MYGROUP_GROUPPAGE = "-1";
		internal const string DEFAULT_MYGROUP_MODULE = "-1";

		internal const string LOCALIZATION_SharedResourceFile = "~/DesktopModules/UserGroups/App_LocalResources/SharedResources.resx";

		internal const string SETTING_ADMINMAIL = "UserGroups_Setting_AdminEmail";
		internal const string SETTING_EVENTMODULE = "UserGroups_Setting_EventModule";
		internal const string SETTING_MINSIZE = "UserGroups_Setting_MinSize";
		internal const string SETTING_PUBLISHEVENTS = "UserGroups_Setting_PublishEvents";
		internal const string SETTING_JOINMESSAGE = "UserGroups_Setting_JoinMessage"; 
		internal const string SETTING_PAGESIZE = "UserGroups_Setting_PageSize";
		internal const string SETTING_NAMEFORMAT = "UserGroups_Setting_NameFormat";
    		internal const string SETTING_MAPKEY = "UserGroups_Setting_MapKey";
		internal const string SETTING_COUNTRYSELECTMSG = "UserGroups_Setting_CountrySelectMsg";
		internal const string SETTING_REGIONSELECTMSG = "UserGroups_Setting_RegionSelectMsg";
		internal const string SETTING_LANGUAGESELECTMSG = "UserGroups_Setting_LanguageSelectMsg";
		internal const string SETTING_JOINSELECTMSG = "UserGroups_Setting_JoinSelectMsg";
		internal const string SETTING_PROPERTYDEFINITIONID = "UserGroups_PropertyDefinitionID";

		internal const string SKINOBJECT_JOINTEXT = "JoinUserGroup";
		internal const string SKINOBJECT_MEMBERTEXT = "MemberUserGroup";

		internal const string SPOTLIGHT_MEETINGMAXDAYCOUNT = "Spotlight_UserGroup_MeetingMaxDayCount";
		internal const string SPOTLIGHT_REQUIREMEETINGADDRESS = "Spotlight_UserGroup_RequireMeetingAddress";
		internal const string SPOTLIGHT_TEMPLATE = "Spotlight_UserGroup_Template";
    		internal const string SPOTLIGHT_GROUPPAGE = "Spotlight_UserGroup_Page";
		internal const string SPOTLIGHT_MODULE = "Spotlight_UserGroup_Module";
		internal const string SPOTLIGHT_PAGESIZE = "Spotlight_UserGroup_PageSize";
		internal const string SPOTLIGHT_HEIGHT = "Spotlight_UserGroup_Height";
		internal const string SPOTLIGHT_WIDTH = "Spotlight_UserGroup_Width";
		internal const string SPOTLIGHT_ITEM_HEIGHT = "Spotlight_UserGroup_ItemHeight";
		internal const string SPOTLIGHT_ITEM_WIDTH = "Spotlight_UserGroup_ItemWidth";
		internal const string SPOTLIGHT_FRAME_DURATION = "Spotlight_UserGroup_FrameDuration";
		internal const string SPOTLIGHT_SCROLL_DURATION = "Spotlight_UserGroup_ScrollDuration";

		internal const string MYGROUP_JOINTEMPLATE = "MyGroup_UserGroup_JoinTemplate";
		internal const string MYGROUP_GROUPTEMPLATE = "MyGroup_UserGroup_GroupTemplate";
		internal const string MYGROUP_GROUPPAGE = "MyGroup_UserGroup_Page";
		internal const string MYGROUP_MODULE = "MyGroup_UserGroup_Module";

        #endregion

    }
}