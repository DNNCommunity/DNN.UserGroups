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
using DotNetNuke.Modules.UserGroups.Components.Data;
using DotNetNuke.Modules.UserGroups.Components.Entities;
using DotNetNuke.Modules.Events;
using DotNetNuke.Services.Exceptions;
using Config = DotNetNuke.Modules.UserGroups.Common.Config;
using DataProvider = DotNetNuke.Modules.UserGroups.Components.Data.DataProvider;

namespace DotNetNuke.Modules.UserGroups.Components.Controllers 
{

	/// <summary>
	/// 
	/// </summary>
	public class MeetingController 
	{

		/// <summary>
		/// GetMeetingsByUserGroup fetches the meetings for a UserGroup
		/// </summary>
		/// <param name="userGroupID">The Id of the User Group to fetch</param>
		/// <param name="startDate"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <returns>A List of UserGroupMeetingInfo objects</returns>
		public List<MeetingInfo> GetMeetingsByUserGroup(int userGroupID, DateTime startDate, int pageSize, int pageIndex) 
		{
			if (pageIndex == Null.NullInteger) {
				pageIndex = 0;
			}

			if (pageSize == Null.NullInteger) {
				pageSize = Int32.MaxValue;
			}

			return CBO.FillCollection<MeetingInfo>(DataProvider.Instance().GetUpcommingGroupMeetings(userGroupID, startDate, pageSize, pageIndex));
		}

		public List<BaseMeetingInfo> GetMeetingsByUserGroupService(int userGroupID, DateTime startDate, int pageSize, int pageIndex) {
			if (pageIndex == Null.NullInteger) {
				pageIndex = 0;
			}

			if (pageSize == Null.NullInteger) {
				pageSize = Int32.MaxValue;
			}

			return CBO.FillCollection<BaseMeetingInfo>(DataProvider.Instance().GetUpcommingGroupMeetings(userGroupID, startDate, pageSize, pageIndex));
		}

		public List<MeetingInfo> SpotlightSearch(int portalID, int pageSize, int pageIndex, int nextDays, bool requireMeetingLocation) {
			if (pageIndex == Null.NullInteger) {
				pageIndex = 0;
			}

			if (pageSize == Null.NullInteger) {
				pageSize = Int32.MaxValue;
			}

			return CBO.FillCollection<MeetingInfo>(DataProvider.Instance().SpotlightSearch(portalID, pageSize, pageIndex, nextDays, requireMeetingLocation));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="portalID"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="region"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public List<MeetingInfo> GetAllMeetingsByDate(int portalID, DateTime startDate, DateTime endDate, string region, int pageIndex, int pageSize) {
			if (pageIndex == Null.NullInteger) {
				pageIndex = 0;
			}

			if (pageSize == Null.NullInteger) {
				pageSize = Int32.MaxValue;
			}

			if (region == Null.NullString)
			{
				region = "-1";
			}
			return CBO.FillCollection<MeetingInfo>(DataProvider.Instance().GetAllMeetingsByDate(portalID, startDate, endDate, region, pageIndex, pageSize));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="meetingID"></param>
		/// <returns></returns>
		public MeetingInfo GetMeeting(int meetingID) 
		{
			return (MeetingInfo)CBO.FillObject(DataProvider.Instance().GetMeeting(meetingID), typeof(MeetingInfo));
		}

		public int AddUserGroupMeeting(UserGroupInfo userGroup, string title, string description, string location, DateTime meetingDate, int moduleId, int portalId, string profileLink) {
			var eventID = -1;
			var appendHtml = "<br /><a href='" + profileLink + "'>Group Profile</a>";

			if (Config.PublishEvents(moduleId)) {
			//if (Config.PublishEvents(moduleId) && userGroup.Active == true) {
				var evntController = new EventController();
				var evntInfo = new EventInfo
				               	{
				               		ModuleID = Config.EventModule(moduleId),
				               		EventID = Null.NullInteger,
				               		EventName = userGroup.Name,
				               		EventDesc = title + " - " + description + appendHtml,
				               		EventDateBegin = meetingDate,
				               		EventDateEnd = meetingDate,
				               		EventTimeBegin = meetingDate,
				               		EnrollType = "FREE",
				               		Approved = true,
				               		PortalID = portalId,
				               		Every = 0,
				               		RepeatType = "N",
				               		Duration = 5,
				               		Location = -1,
				               		LastUpdatedAt = DateTime.Now,
				               		OriginalDateBegin = DateTime.Now,
				               		LocationName = location,
				               		Category = 1,
				               		SendReminder = false,
				               		MaxEnrollment = 0,
				               		ReminderTime = 8,
				               		ReminderTimeMeasurement = "h",
									ReminderFrom = "",
				               		SearchSubmitted = false
				};

				//10-19-09 the following code is sloppy, I don't claim it's nice, but it appears to work with Events 5.0.2! -CJH

				var objCtlEventRecurMaster = new EventRecurMasterController();
				var objEventRecurMaster = new EventRecurMasterInfo();

				if (evntInfo.RecurMasterID > 0)
					objEventRecurMaster = objCtlEventRecurMaster.EventsRecurMasterGet(evntInfo.RecurMasterID,
																		 Config.EventModule(moduleId));
				//we're setting a LOT of crap here just because we couldn't get the add to work without it. Probably not all necessary but some of it is.
				objEventRecurMaster.DTSTART = evntInfo.EventDateBegin;
				objEventRecurMaster.AllDayEvent = false;
				objEventRecurMaster.Approved = true;
				objEventRecurMaster.CreatedDate = evntInfo.CreatedDate;
				objEventRecurMaster.UpdatedDate = evntInfo.LastUpdatedAt;
				objEventRecurMaster.Duration = "5M";
				objEventRecurMaster.Until = evntInfo.EventDateEnd;
				objEventRecurMaster.EventName = evntInfo.EventName;
				objEventRecurMaster.EnrollType = evntInfo.EnrollType;
				objEventRecurMaster.EventDesc = evntInfo.EventDesc;
				objEventRecurMaster.Location = -1;
				objEventRecurMaster.RecurMasterID = -1;
				objEventRecurMaster.Notify = string.Empty;
				objEventRecurMaster.Approved = true;
				objEventRecurMaster.AllDayEvent = false;
				objEventRecurMaster.ModuleID = Config.EventModule(moduleId);
				objEventRecurMaster.PortalID = portalId;
				objEventRecurMaster.RRULE = string.Empty;
				objEventRecurMaster.Category = 1;
				//objEventRecurMaster.Category = 2;
				objEventRecurMaster.CreatedByID = -1;
				objEventRecurMaster.DisplayEndDate = false;
				objEventRecurMaster.Importance = EventRecurMasterInfo.Priority.Medium;
				objEventRecurMaster.ImageDisplay = false;
				objEventRecurMaster.Reminder = string.Empty;
				objEventRecurMaster.CultureName = "en-US";
				objEventRecurMaster.EnrollType = " ";
				objEventRecurMaster.ReminderTimeMeasurement = " ";
				objEventRecurMaster.ReminderFrom = " ";

				objCtlEventRecurMaster.CreateEventRecurrences(objEventRecurMaster, evntInfo.Duration, "12");
				objEventRecurMaster = objCtlEventRecurMaster.EventsRecurMasterSave(objEventRecurMaster);
				evntInfo.RecurMasterID = objEventRecurMaster.RecurMasterID;

				try {
					evntInfo = evntController.EventsSave(evntInfo, false);
					eventID = evntInfo.EventID;
				} catch (Exception exc) {
					Exceptions.LogException(exc);
				}
			}

			return DataProvider.Instance().AddUserGroupMeeting(userGroup.UserGroupID, eventID, title, description, location, meetingDate);
		}

		public void UpdateUserGroupMeeting(int meetingID, UserGroupInfo userGroup, string title, string description, string location,  DateTime meetingDate, int moduleId, int portalId, int eventId, string profileLink) {
			if (Config.PublishEvents(moduleId) && userGroup.Active) {
				var evntController = new EventController();
				EventInfo evntInfo;
				var appendHtml = "<br /><a href='" + profileLink + "'>Group Profile</a>";

				evntInfo = eventId > Null.NullInteger ? evntController.EventsGet(eventId, Config.EventModule(moduleId)) : new EventInfo { ModuleID = Config.EventModule(moduleId), EventID = Null.NullInteger };

				evntInfo.EventName = userGroup.Name;
				evntInfo.EventDesc = title + " - " + description + appendHtml;
				evntInfo.EventDateBegin = meetingDate;
				evntInfo.EventDateEnd = meetingDate;
				evntInfo.EventTimeBegin = meetingDate;
				evntInfo.EnrollType = "FREE";
				evntInfo.Approved = true;
				evntInfo.PortalID = portalId;
				evntInfo.Every = 0;
				evntInfo.RepeatType = "N";
				evntInfo.Duration = 5;
				evntInfo.Location = -1;
				evntInfo.LastUpdatedAt = DateTime.Now;
				evntInfo.OriginalDateBegin = DateTime.Now;
				evntInfo.LocationName = location;
				evntInfo.Category = 1;
				evntInfo.SendReminder = false;
				evntInfo.MaxEnrollment = 0;
				evntInfo.ReminderTime = 8;
				evntInfo.ReminderTimeMeasurement = "h";
				evntInfo.ReminderFrom = "";
				evntInfo.SearchSubmitted = false;

				//10-19-09 the following code is sloppy, I don't claim it's nice, but it appears to work with Events 5.0.2! -CJH

				var objCtlEventRecurMaster = new EventRecurMasterController();
				var objEventRecurMaster = new EventRecurMasterInfo();
				if (evntInfo.RecurMasterID > 0)
					objEventRecurMaster = objCtlEventRecurMaster.EventsRecurMasterGet(evntInfo.RecurMasterID,
																		 Config.EventModule(moduleId));
				//we're setting a LOT of crap here just because we couldn't get the add to work without it. Probably not all necessary but some of it is.
				objEventRecurMaster.DTSTART = evntInfo.EventDateBegin;
				objEventRecurMaster.AllDayEvent = false;
				objEventRecurMaster.Approved = true;
				objEventRecurMaster.CreatedDate = evntInfo.CreatedDate;
				objEventRecurMaster.UpdatedDate = evntInfo.LastUpdatedAt;
				objEventRecurMaster.Duration = "5M";
				objEventRecurMaster.Until = evntInfo.EventDateEnd;
				objEventRecurMaster.EventName = evntInfo.EventName;
				objEventRecurMaster.EnrollType = evntInfo.EnrollType;
				objEventRecurMaster.EventDesc = evntInfo.EventDesc;
				objEventRecurMaster.Location = -1;
				objEventRecurMaster.RecurMasterID = -1;
				objEventRecurMaster.Notify = string.Empty;
				objEventRecurMaster.Approved = true;
				objEventRecurMaster.AllDayEvent = false;
				objEventRecurMaster.ModuleID = Config.EventModule(moduleId);
				objEventRecurMaster.PortalID = portalId;
				objEventRecurMaster.RRULE = string.Empty;
				objEventRecurMaster.Category = 1;
				objEventRecurMaster.CreatedByID = -1;
				objEventRecurMaster.DisplayEndDate = false;
				objEventRecurMaster.Importance = EventRecurMasterInfo.Priority.Medium;
				objEventRecurMaster.ImageDisplay = false;
				objEventRecurMaster.Reminder = string.Empty;
				objEventRecurMaster.CultureName = "en-US";

				objCtlEventRecurMaster.CreateEventRecurrences(objEventRecurMaster, evntInfo.Duration, "12");
				objEventRecurMaster = objCtlEventRecurMaster.EventsRecurMasterSave(objEventRecurMaster);
				evntInfo.RecurMasterID = objEventRecurMaster.RecurMasterID;

				try {
					evntInfo = evntController.EventsSave(evntInfo, false);
					eventId = evntInfo.EventID;
				} catch (Exception exc) {
					Exceptions.LogException(exc);
				}
			}

			DataProvider.Instance().UpdateUserGroupMeeting(meetingID, userGroup.UserGroupID, eventId, title, description, location, meetingDate);
			//DataService.UpdateUserGroupMeeting(meeting);
		}

		/// <summary>
		/// DeleteUserGroupMeeting deletes a User Group Meeting
		/// </summary>
		/// <param name="meetingID">The Id of the User Group Meeting to delete</param>
		/// <param name="userGroupID"></param>
		/// <param name="moduleId">The Id of the module</param>
		public void DeleteUserGroupMeeting(int meetingID, int userGroupID, int moduleId) {
			if (!Config.PublishEvents(moduleId)) return;
			var cntUgMeeting = new MeetingController();
			var meeting = cntUgMeeting.GetMeeting(meetingID);

			if (meeting != null && meeting.EventID > Null.NullInteger) {
				var evntController = new EventController();
				try {
					evntController.EventsDelete(meeting.EventID, Config.EventModule(moduleId));
				} catch (Exception exc) {
					Exceptions.LogException(exc);
				}
			}

			DataProvider.Instance().DeleteMeeting(meetingID, userGroupID);
		}

	}
}
