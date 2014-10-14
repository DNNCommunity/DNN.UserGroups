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
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.UserGroups.Common;
using System.Web.UI.WebControls;
using DotNetNuke.Modules.UserGroups.Components;
using Telerik.Web.UI;

namespace DotNetNuke.Modules.UserGroups 
{

	/// <summary>
	/// 
	/// </summary>
	public partial class MeetingFinder : UserGroupsModuleBase {

		private int _pageIndex = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		private int PageIndex {
			get {
				if (ViewState != null)
					if (Convert.ToString(ViewState["PageIndex"]) != null) {
						return _pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
					}
				return _pageIndex;
			}
			set {
				_pageIndex = value;
				ViewState["PageIndex"] = value;
			}
		}

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Init(object sender, EventArgs e) {
			if (Framework.AJAX.IsInstalled()) {
				Framework.AJAX.RegisterScriptManager();
			}

			if (!Page.IsPostBack) {
				SetLocalization();
				BindRegions();
				// limit dates to current day + 1 year in advance
				rdpMeetingStart.MinDate = DateTime.Now;
				rdpMeetingEnd.MinDate = DateTime.Now;
				rdpMeetingStart.MaxDate = DateTime.Now.AddYears(1);
				rdpMeetingEnd.MaxDate = DateTime.Now.AddYears(1);
				// add querystring check for date
				rdpMeetingStart.SelectedDate = DateTime.Now;
				rdpMeetingEnd.SelectedDate = DateTime.Now.AddDays(30);
			}
		}

		/// <summary>
		/// Runs when the page loads, determins if the user can join the group (if it exists and other business rules), then  adds user to new group (if applicable) and sets page up with information about what just happend.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>This class and managegroup class are only interfaces a user can be added to a group.</remarks>
		protected void Page_Load(object sender, EventArgs e) 
		{
			try 
			{
				if (!IsAuthenticated)
				{
					Response.Redirect(Util.AccessDeniedLink(), true);
				} 
				else 
				{
					if (!Page.IsPostBack) 
					{
						hlGroupSearch.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, -1, PageScope.DirectorySearch);
						// add querystring check for region
						BindGrid(true);

					}
				}
			} 
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdGo_Click(object sender, EventArgs e) {
			PageIndex = 0;

			BindGrid(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgMeetings_ItemDataBound(object sender, GridItemEventArgs e) {
			if (!(e.Item is GridDataItem)) return;
			var groupKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];
			//var keyMeetingID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MeetingID"];
			var keyMapUrl =  (string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MapUrl"];
			var dataItem = (GridDataItem)e.Item;

			var hlName = ((HyperLink)(dataItem)["Name"].Controls[0]);
			hlName.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, groupKeyID, PageScope.GroupProfile);

			var hlLocation = ((HyperLink)(dataItem)["Location"].Controls[0]);
			hlLocation.NavigateUrl = keyMapUrl;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		protected void RgMeetings_PageChange(object source, GridPageChangedEventArgs e) {
			PageIndex = e.NewPageIndex;

			BindGrid(true);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Localizes the grid header colums.
		/// </summary>
		private void SetLocalization() {
			foreach (Telerik.Web.UI.GridColumn gc in rgMeetings.MasterTableView.Columns) {
				if (gc.HeaderText != "") {
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile);
				}
			}
		}

		/// <summary>
		/// Binds a list of all meetings for management reasons.
		/// </summary>
		/// <param name="bindIt"></param>
		private void BindGrid(bool bindIt) {
			var cntMeeting = new MeetingController();
			var colMeetings = cntMeeting.GetAllMeetingsByDate(PortalId, rdpMeetingStart.SelectedDate.Value, rdpMeetingEnd.SelectedDate.Value, rcbRegion.SelectedItem.Value, PageIndex, 10 );

			rgMeetings.DataSource = colMeetings;

			rgMeetings.VirtualItemCount = colMeetings.Count > 0l ? colMeetings[0].TotalRecords : 0;
				
			if (bindIt) {
				rgMeetings.DataBind();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void BindRegions()
		{
			var cntRegion = new RegionController();
			var colRegions = cntRegion.GetCachedGroupRegions(PortalId);

			rcbRegion.DataSource = colRegions;
			rcbRegion.DataBind();

			rcbRegion.Items.Insert(0, new RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", "-1"));
		}

		#endregion

	}
}