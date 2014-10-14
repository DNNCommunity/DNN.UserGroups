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
using System.Linq;
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Services.Exceptions;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Lists;
using Telerik.Web.UI;
using Config = DotNetNuke.Modules.UserGroups.Common.Config;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.UserGroups.Common;

namespace DotNetNuke.Modules.UserGroups 
{

	/// <summary>
	/// The directory search control is loaded as the initial view control for all unauthenticated users. It is also loaded, by default, when an authenticated user is not a member of a user group and can be accessed as a general group search page as well. 
	/// This control guides the user through searching the directory of user groups. 
	/// </summary>
	public partial class DirectorySearch : UserGroupsModuleBase 
	{

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
// ReSharper disable InconsistentNaming
		protected void Page_Init(object sender, EventArgs e) {
// ReSharper restore InconsistentNaming
			if (Framework.AJAX.IsInstalled()) {
				Framework.AJAX.RegisterScriptManager();
			}

			if (!Page.IsPostBack) {
				SetLocalization();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
// ReSharper disable InconsistentNaming
		protected void Page_Load(object sender, EventArgs e) 
// ReSharper restore InconsistentNaming
		{
			try 
			{
				if (!Page.IsPostBack) 
				{
					SetupPage();
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
		protected void RcbCountrySelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e) 
		{
			BindRegions();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RcbRegionSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e) {
			if (rcbRegion.SelectedIndex == 0)
			{
				rgUserGroups.Visible = false;
				SetInstructions(1);
			}
			else
			{
				BindGrid(true);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RcbLanguageSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e) {
			if (rcbLanguage.SelectedIndex == 0)
			{
				rgUserGroups.Visible = false;
				SetInstructions(2);
			}
			else
			{
				BindGrid(true);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdSearchClick(object sender, EventArgs e) 
		{
			BindGrid(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdMyGroupClick(object sender, EventArgs e) {
			Response.Redirect(Util.ViewControlLink(TabId, ModuleId, ProfileUsersGroupID, PageScope.GroupProfile), true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdAddGroupClick(object sender, EventArgs e) {
			Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(TabId, "Manage", "mid=" + ModuleId, "country=" + rcbCountry.SelectedValue), true);
		}

		//protected void cmdMeetFind_Click(object sender, EventArgs e) {
		//     Response.Redirect(Util.ViewControlLink(TabId, ModuleId, -1, PageScope.MeetingFinder));
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgUserGroupsItemDataBound(object sender, GridItemEventArgs e) 
		{
			if (!(e.Item is GridDataItem)) return;
			var groupKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];
			var userKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LeaderID"];
			var dataItem = (GridDataItem)e.Item;

			var hlName = ((HyperLink) (dataItem)["Name"].Controls[0]);
			hlName.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, groupKeyID, PageScope.GroupProfile);

			var hlLeader = ((HyperLink)(dataItem)["Leader"].Controls[0]);
			string name;

			if (Config.NameFormat(ModuleId) == "DisplayName")
			{
				name = (string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LeaderDisplayName"];
			}
			else
			{
				name = (string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LeaderUsername"];
			}
			
			hlLeader.Text = name;
			hlLeader.NavigateUrl = DotNetNuke.Common.Globals.UserProfileURL(userKeyID);

			var joinColumn = ((HyperLink)(dataItem)["Join"].Controls[0]);

			if (ProfileUsersGroupID > 0)
			{
				if (groupKeyID == ProfileUsersGroupID)
				{
					joinColumn.Text = Localization.GetString("LeaveGroup", Constants.LOCALIZATION_SharedResourceFile);
					joinColumn.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, -1, PageScope.RemoveUser);
				}
				else
				{
					joinColumn.Text = Localization.GetString("SwitchGroup", Constants.LOCALIZATION_SharedResourceFile);
					joinColumn.NavigateUrl = Util.SwitchGroupsLink(TabId, ModuleId, groupKeyID);
				}
			}
			else
			{
				joinColumn.Text = IsAuthenticated ? Localization.GetString("JoinGroup", Constants.LOCALIZATION_SharedResourceFile) : Localization.GetString("LoginToJoin", Constants.LOCALIZATION_SharedResourceFile);
				joinColumn.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, groupKeyID, PageScope.WelcomeUser);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgUserGroupsItemCommand(object sender, GridCommandEventArgs e) 
		{
			if (
				(!(e.Item.ItemType == GridItemType.Item |
				   e.Item.ItemType == GridItemType.AlternatingItem))) return;
			switch (e.CommandName) {
				case "EditItem":
					var keyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];
					Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(TabId, "Manage", "mid=" + ModuleId, "UserGroupID=" + keyID), true);
					break;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		private void SetupPage()
		{
			divLanguage.Visible = false;
			divRegion.Visible = false;

			if (IsEditable)
			{
				divAdmin.Visible = true;
				rgUserGroups.MasterTableView.GetColumn("EditItem").Display = true;
				rgUserGroups.MasterTableView.GetColumn("Active").Display = true;
			}
			else
			{
				divAdmin.Visible = false;
				rgUserGroups.MasterTableView.GetColumn("EditItem").Display = false;
				rgUserGroups.MasterTableView.GetColumn("Active").Display = false;
			}

			cmdMyGroup.Visible = ProfileUsersGroupID >= 1;

               BindCountries();
		}

		/// <summary>
		/// Binds a list of countries to the combobox. 
		/// </summary>
		private void BindCountries() 
		{
			var listController = new ListController();
			var entryCollection = listController.GetListEntryInfoCollection("Country");
			rcbCountry.DataSource = entryCollection;
			rcbCountry.DataBind();
			rcbCountry.Items.Insert(0, new RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", ""));

			//if (ProfileUserID > 0)
			//{
			//     rcbCountry.SelectedItem.Text = ProfileUser.Profile.Country;
			//}

			if (rcbCountry.SelectedIndex > 0)
			{
				BindRegions();
			}
			else
			{
				rgUserGroups.Visible = false;
				SetInstructions(0);
			}
          }

		/// <summary>
		/// Binds a list of available regions to the combobox, based on the selected country. 
		/// </summary>
		private void BindRegions() 
		{
			BindLanguages();

			var controller = new ListController();
			var listKey = "Country." + rcbCountry.SelectedValue;
			var regions = controller.GetListEntryInfoCollection("Region", listKey);

			if (rcbCountry.SelectedValue == "US" || rcbCountry.SelectedValue == "CA") 
			{
				divLanguage.Visible = false;
				rcbRegion.Items.Clear();

				if (regions.Count > 0) 
				{
					divRegion.Visible = true;
					rgUserGroups.Visible = false;

					rcbRegion.DataSource = regions;
					rcbRegion.DataBind();
					rcbRegion.Items.Insert(0, new RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", ""));

					//var region = string.Empty;
					//if (!Page.IsPostBack && !String.IsNullOrEmpty(Request.QueryString["region"])) {
					//     region = Request.QueryString["region"];
					//}
					//if (String.IsNullOrEmpty(region)) {
					//     region = UserInfo.Profile.Region;
					//}

					//if (!String.IsNullOrEmpty(UserInfo.Profile.Region)) {
					//     Telerik.Web.UI.RadComboBoxItem item = rcbRegion.FindItemByText(region);
					//     if (item != null) {
					//          rcbRegion.ClearSelection();
					//          item.Selected = true;
					//     }
					//}

				} 
				else 
				{
					divRegion.Visible = false;

					if (rcbCountry.SelectedIndex != 0) 
					{
						BindGrid(true);
					}
					else
					{
						rgUserGroups.Visible = false;
					}
				}
				SetInstructions(1);
			}
			else
			{
				divLanguage.Visible = true;
				divRegion.Visible = false;
				rgUserGroups.Visible = false;
				SetInstructions(2);
			}
		}

		/// <summary>
		/// Binds a list of available languages to the combobox.
		/// </summary>
		private void BindLanguages()
		{
			rcbLanguage.Items.Clear();
			var cntLang = new LanguageController();
			var colLangs = cntLang.GetLanguages(PortalId);

			rcbLanguage.DataSource = colLangs;
			rcbLanguage.DataBind();
			rcbLanguage.Items.Insert(0, new RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", "-1"));
		}

		/// <summary>
		/// Binds data to the grid.
		/// </summary>
		private void BindGrid(bool bindIt)
		{
			var cntUserGroup = new UserGroupController();
			var colUserGroups = cntUserGroup.SearchGroups(PortalId, 0, 10, rcbCountry.Text, rcbRegion.Text, string.Empty, txtGroupName.Text, Convert.ToInt32(rcbLanguage.SelectedValue), Config.PropertyDefinitionID(ModuleId));

			rgUserGroups.DataSource = colUserGroups;
			rgUserGroups.Visible = true;

			if (ProfileUsersGroupID < 1)
			{
				cmdAddGroup.Visible = true;
			}

			if (!bindIt) return;
			rgUserGroups.DataBind();
			SetInstructions(3);
		}

		/// <summary>
		/// This provides the end user with basic instructions based on which step they are on in the user group directory search. 
		/// </summary>
		/// <param name="step"></param>
		private void SetInstructions(int step)
		{
			switch (step)
			{
				case 1:
					litInstructions.Text = Config.RegionSelectMsg(ModuleId);
					break;
				case 2:
					litInstructions.Text = Config.LanguageSelectMsg(ModuleId);
					break;
				case 3:
					litInstructions.Text = Config.JoinSelectMsg(ModuleId);
					break;
				default:
					litInstructions.Text = Config.CountrySelectMsg(ModuleId);
					break;
			}
		}

		/// <summary>
		/// Localizes the headers in the Telerik datagrid.
		/// </summary>
		private void SetLocalization()
		{
			foreach (var gc in rgUserGroups.MasterTableView.Columns.Cast<GridColumn>().Where(gc => gc.HeaderText != ""))
			{
				gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile);
			}
		}

		#endregion 

	}
}