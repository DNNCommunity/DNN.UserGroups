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
using DotNetNuke.Modules.UserGroups.Common;

namespace DotNetNuke.Modules.UserGroups.Components.Entities
{

	/// <summary>
	/// 
	/// </summary>
    public class BaseMeetingInfo 
    {

		private string _meetingUrl;

		#region Public Properties

		public int MeetingID { get; set; }

		public int TabID { get; set; }

		public int ModuleID { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Location { get; set; }

		public DateTime MeetingDate { get; set; }

		public string MeetingDetailUrl {
			get
			{
				// we use 1074 because content items didn't originally exist (where tabid is stored) and this is the tab we want on dnn.com
			_meetingUrl = Common.Util.ViewControlLink(TabID < 1 ? 1074 : TabID, ModuleID, MeetingID, PageScope.MeetingDetail);
			return _meetingUrl;
			}
			set { _meetingUrl = value; }
		}

		#endregion

    }
}
