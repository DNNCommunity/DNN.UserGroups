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
using System.Collections;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.UserGroups.Common
{
    /// <summary>
    /// The Config class provides a central area for management of Module Configuration Settings.
    /// </summary>
    internal static class Config
    {

		#region Internal Static Methods

		#region Main User Group Settings

		/// <summary>
		/// AddDefaultSettings adds default settings for any missing Settings
		/// </summary>
		/// <param name="settings">The settings</param>
		/// <param name="defaultSettings">The default settings.</param>
		/// <returns></returns>
		internal static void AddDefaultSettings(Hashtable settings, Hashtable defaultSettings)
		{
			foreach (DictionaryEntry setting in defaultSettings)
			{
				//Update other unset settings to the default value
				if (settings[setting.Key] == null)
				settings[setting.Key] = setting.Value;
			}
		}

		/// <summary>
		/// Gets the default settings for the Module.
		/// </summary>
		/// <returns>A Hashtable ofSettings</returns>
		internal static Hashtable GetDefaultModuleSettings()
		{
			//Create new Hashtable
			var settings = new Hashtable();

			//Add Default Settings
			settings[Constants.SETTING_ADMINMAIL] = Null.NullString;
			settings[Constants.SETTING_EVENTMODULE] = Constants.DEFAULT_EVENTMODULE;
			settings[Constants.SETTING_MINSIZE] = Constants.DEFAULT_MINSIZE;
			settings[Constants.SETTING_PUBLISHEVENTS] = Constants.DEFAULT_PUBLISHEVENTS;
			settings[Constants.SETTING_MAPKEY] = Constants.DEFAULT_MAPKEY;
			settings[Constants.SETTING_JOINMESSAGE] = Constants.DEFAULT_JOINMESSAGE;
			settings[Constants.SETTING_PAGESIZE] = Constants.DEFAULT_PAGESIZE;
			settings[Constants.SETTING_NAMEFORMAT] = Constants.DEFAULT_NAMEFORMAT;
			settings[Constants.SETTING_COUNTRYSELECTMSG] = Constants.DEFAULT_COUNTRYSELECTMSG;
			settings[Constants.SETTING_REGIONSELECTMSG] = Constants.DEFAULT_REGIONSELECTMSG;
			settings[Constants.SETTING_LANGUAGESELECTMSG] = Constants.DEFAULT_LANGUAGESELECTMSG;
			settings[Constants.SETTING_JOINSELECTMSG] = Constants.DEFAULT_JOINSELECTMSG;
			settings[Constants.SETTING_PROPERTYDEFINITIONID] = Constants.DEFAULT_PROPERTYDEFINITIONID;
			//Return Default Settings
			return settings;
		}

		/// <summary>
		/// GetModuleSetting gets a specific Module wide Setting
		/// </summary>
		/// <param name="moduleID">The iId of the module</param>
		/// <param name="setting">The setting to retrieve</param>
		/// <returns>The setting (as a string)</returns>
		internal static string GetModuleSetting(int moduleID, string setting)
		{
			var settings = GetModuleSettings(moduleID);

			return Convert.ToString(settings[setting]);
		}

		/// <summary>
		/// GetModuleSettings fetches the Module wide settings
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns>A Hashtable of Module Settings</returns>
		internal static Hashtable GetModuleSettings(int moduleID)
		{
			var controller = new ModuleController();
			var settings = controller.GetModuleSettings(moduleID);

			//Add any missing settings
			AddDefaultSettings(settings, GetDefaultModuleSettings());

			return settings;
		}
 
		/// <summary>
		/// AdminMail returns the value of the AdminMail Setting for the module
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns>A string value (true/false)</returns>
		internal static string AdminMail(int moduleID)
		{
			return GetModuleSetting(moduleID, Constants.SETTING_ADMINMAIL);
		}

		/// <summary>
		/// The Id of the EventsModule used for publishing Events.
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns>An integer representing the Module ID of the Events Module</returns>
		internal static int EventModule(int moduleID)
		{
			return Int32.Parse(GetModuleSetting(moduleID, Constants.SETTING_EVENTMODULE));
		}

		/// <summary>
		/// The value of the MinSize threshold for user groups to become active.
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns>An integer representing the Minimum User Group Size</returns>
		internal static int MinSize(int moduleID)
		{
			return Int32.Parse(GetModuleSetting(moduleID, Constants.SETTING_MINSIZE));
		}

		/// <summary>
		/// PublishEvents determines whether the module will publish its meeting to an Events Module.
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns>True if events are published to the events module, false otherwise.</returns>
		internal static bool PublishEvents(int moduleID)
		{
			return Boolean.Parse(GetModuleSetting(moduleID, Constants.SETTING_PUBLISHEVENTS));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static string MapKey(int moduleID) {
			return GetModuleSetting(moduleID, Constants.SETTING_MAPKEY);
		}

		/// <summary>
		/// The welcome message displayed to users when they join a user group. 
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns></returns>
		internal static string JoinMessage(int moduleID) {
			return GetModuleSetting(moduleID, Constants.SETTING_JOINMESSAGE);
		}

		/// <summary>
		/// The page size used for directory search results. NOTE: Directory search is the only place that utilizes page size, all other grids do not support paging!
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns></returns>
		internal static int PageSize(int moduleID) {
			return Int32.Parse(GetModuleSetting(moduleID, Constants.SETTING_PAGESIZE));
		}

		/// <summary>
		/// Determines if username or display name should be used throughout the module.
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns></returns>
		internal static string NameFormat(int moduleID) {
			return GetModuleSetting(moduleID, Constants.SETTING_NAMEFORMAT);
		}

		/// <summary>
		/// Message displayed in directory search page when users are at the first step of directory search process (ie. where they select a country from the drop down list). 
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns></returns>
		internal static string CountrySelectMsg(int moduleID) {
			return GetModuleSetting(moduleID, Constants.SETTING_COUNTRYSELECTMSG);
		}

		/// <summary>
		/// Message displayed in directory search page when users are at the second step of directory search process (ie. where they select a region; NA only!). 
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns></returns>
		internal static string RegionSelectMsg(int moduleID) {
			return GetModuleSetting(moduleID, Constants.SETTING_REGIONSELECTMSG);
		}

		/// <summary>
		/// Message displayed in directory search page when users are at the second step of directory search process (ie. where they select a language; Non-NA only!). 
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns></returns>
		internal static string LanguageSelectMsg(int moduleID) {
			return GetModuleSetting(moduleID, Constants.SETTING_LANGUAGESELECTMSG);
		}

		/// <summary>
		/// Message displayed in directory search page when users are at the final step of directory search process (ie. where they can join a group). 
		/// </summary>
		/// <param name="moduleID">The Id of the Module we are retrieving settings for.</param>
		/// <returns></returns>
		internal static string JoinSelectMsg(int moduleID) {
			return GetModuleSetting(moduleID, Constants.SETTING_JOINSELECTMSG);
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="moduleID"></param>
	    /// <returns></returns>
	    /// <remarks>If this module setting is not set, it appears to throw an error in directory search (after selecting a language or a region).</remarks>
		internal static int PropertyDefinitionID(int moduleID) {
			return Int32.Parse(GetModuleSetting(moduleID, Constants.SETTING_PROPERTYDEFINITIONID));
		}

		#endregion

		#region Spotlight

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <param name="setting"></param>
		/// <returns></returns>
		internal static string GetSpotlightModuleSetting(int moduleID, string setting) {
			var settings = GetSpotlightModuleSettings(moduleID);

			return Convert.ToString(settings[setting]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static Hashtable GetSpotlightModuleSettings(int moduleID) {
			var controller = new ModuleController();
			var settings = controller.GetModuleSettings(moduleID);

			//Add any missing settings
			AddDefaultSettings(settings, GetDefaultSpotlightModuleSettings());

			return settings;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal static Hashtable GetDefaultSpotlightModuleSettings() {
			//Create new Hashtable
			var settings = new Hashtable();

			//Add Default Settings
			settings[Constants.SPOTLIGHT_GROUPPAGE] = Constants.DEFAULT_SPOTLIGHT_GROUPPAGE;
			settings[Constants.SPOTLIGHT_MODULE] = Constants.DEFAULT_SPOTLIGHT_MODULE;
			settings[Constants.SPOTLIGHT_MEETINGMAXDAYCOUNT] = Constants.DEFAULT_SPOTLIGHT_MEETINGMAXDAYCOUNT;
			settings[Constants.SPOTLIGHT_REQUIREMEETINGADDRESS] = Constants.DEFAULT_SPOTLIGHT_REQUIREMEETINGADDRESS;
			settings[Constants.SPOTLIGHT_TEMPLATE] = Constants.DEFAULT_SPOTLIGHT_TEMPLATE;
			settings[Constants.SPOTLIGHT_PAGESIZE] = Constants.DEFAULT_SPOTLIGHT_PAGESIZE;
			settings[Constants.SPOTLIGHT_HEIGHT] = Constants.DEFAULT_SPOTLIGHT_HEIGHT;
			settings[Constants.SPOTLIGHT_WIDTH] = Constants.DEFAULT_SPOTLIGHT_WIDTH;
			settings[Constants.SPOTLIGHT_ITEM_HEIGHT] = Constants.DEFAULT_SPOTLIGHT_ITEM_HEIGHT;
			settings[Constants.SPOTLIGHT_ITEM_WIDTH] = Constants.DEFAULT_SPOTLIGHT_ITEM_WIDTH;
			settings[Constants.SPOTLIGHT_FRAME_DURATION] = Constants.DEFAULT_SPOTLIGHT_FRAME_DURATION;
			settings[Constants.SPOTLIGHT_SCROLL_DURATION] = Constants.DEFAULT_SPOTLIGHT_SCROLL_DURATION;

			//Return Default Settings
			return settings;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static int MeetingMaxDayCount(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_MEETINGMAXDAYCOUNT));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static bool RequireMeetingAddress(int moduleID)
		{
			return Boolean.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_REQUIREMEETINGADDRESS));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static int SpotlightGroupPage(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_GROUPPAGE));
		}

	    /// <summary>
	    /// The number of items to display in the rotator.
	    /// </summary>
	    /// <param name="moduleID"></param>
	    /// <returns></returns>
		internal static int SpotlightPageSize(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_PAGESIZE));
		}

		internal static int SpotlightHeight(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_HEIGHT));
		}

		internal static int SpotlightWidth(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_WIDTH));
		}

		internal static int SpotlightItemHeight(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_ITEM_HEIGHT));
		}

		internal static int SpotlightItemWidth(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_ITEM_WIDTH));
		}

		internal static int SpotlightFrameDuration(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_FRAME_DURATION));
		}

		internal static int SpotlightScrollDuration(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_SCROLL_DURATION));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static int SpotlightGroupModule(int moduleID) {
			return Int32.Parse(GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_MODULE));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static string SpotlightTemplate(int moduleID) {
			return GetSpotlightModuleSetting(moduleID, Constants.SPOTLIGHT_TEMPLATE);
		}

		#endregion

		#region My Group

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <param name="setting"></param>
		/// <returns></returns>
		internal static string GetMyGroupModuleSetting(int moduleID, string setting) {
			var settings = GetMyGroupModuleSettings(moduleID);

			return Convert.ToString(settings[setting]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static Hashtable GetMyGroupModuleSettings(int moduleID) {
			var controller = new ModuleController();
			var settings = controller.GetModuleSettings(moduleID);

			//Add any missing settings
			AddDefaultSettings(settings, GetDefaultMyGroupModuleSettings());

			return settings;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal static Hashtable GetDefaultMyGroupModuleSettings() {
			//Create new Hashtable
			var settings = new Hashtable();

			//Add Default Settings
			settings[Constants.MYGROUP_GROUPPAGE] = Constants.DEFAULT_MYGROUP_GROUPPAGE;
			settings[Constants.MYGROUP_MODULE] = Constants.DEFAULT_MYGROUP_MODULE;
			settings[Constants.MYGROUP_JOINTEMPLATE] = Constants.DEFAULT_MYGROUP_JOINTEMPLATE;
			settings[Constants.MYGROUP_GROUPTEMPLATE] = Constants.DEFAULT_MYGROUP_GROUPTEMPLATE;
			//Return Default Settings
			return settings;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static int MyGroupPage(int moduleID) {
			return Int32.Parse(GetMyGroupModuleSetting(moduleID, Constants.MYGROUP_GROUPPAGE));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static int MyGroupModule(int moduleID) {
			return Int32.Parse(GetMyGroupModuleSetting(moduleID, Constants.MYGROUP_MODULE));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static string JoinTemplate(int moduleID) {
			return GetMyGroupModuleSetting(moduleID, Constants.MYGROUP_JOINTEMPLATE);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		internal static string GroupTemplate(int moduleID) {
			return GetMyGroupModuleSetting(moduleID, Constants.MYGROUP_GROUPTEMPLATE);
		}

		#endregion

		#endregion

    }
}