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
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;

namespace DotNetNuke.Modules.UserGroups.Components.Entities
{

	/// <summary>
	/// This is our Info class that represents columns in our data store that are associated with the UserGroups_UserGroups table. This only adds the additional fields for the webservice beyond the base classes properties (to keep size down). 
	/// </summary>
	public class ServiceUserGroupInfo 
    {

		private string _groupProfileUrl;

		public int UserGroupID { get; set; }

		public int TabID { get; set; }

		public int ModuleID { get; set; }

		public int LeaderID { get; set; }

		public string Name { get; set; }

		public string Country { get; set; }

		public string Region { get; set; }

		public string City { get; set; }

		public string Url { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int Members { get; set; }

		public string LeaderDisplayName { get; set; }

		public string TwitterUrl { get; set; }

		public string LinkedInUrl { get; set; }

		public string FacebookUrl { get; set; }

		public string LanguageName { get; set; }

		public string GroupProfileUrl {
			get 
			{
				// we use 1074 because content items didn't originally exist (where tabid is stored) and this is the tab we want on dnn.com
				_groupProfileUrl = Common.Util.ViewControlLink(TabID < 1 ? 1074 : TabID, ModuleID, UserGroupID, PageScope.GroupProfile);
				return _groupProfileUrl;
			}
			set { _groupProfileUrl = value; }
		}

		public List<BaseMeetingInfo> UpcomingMeetings
		{
			get
			{
				var cntMeeting = new MeetingController();
				var colMeetings = cntMeeting.GetMeetingsByUserGroupService(Convert.ToInt32(UserGroupID), DateTime.Now, 5, 0);

				return colMeetings;
			}
		}

		public int Distance { get; set; }

		public int AvatarFileID { get; set; }

		public string AvatarPath { get; set; }
    }
}