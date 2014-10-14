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
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using Config = DotNetNuke.Modules.UserGroups.Common.Config;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.UserGroups
{

    /// <summary>
    /// The Settings control permits basic configuration options for the module to be edited.
    /// </summary>
	public partial class SpotlightSettings : ModuleSettingsBase
    {

		/// <summary>
		/// LoadSettings loads the settings from the Database and displays them.
		/// </summary>
		public override void LoadSettings() 
		{
			try 
			{
			if (!Page.IsPostBack) 
				{
					BindPages();
					BindModules(Config.SpotlightGroupPage(ModuleId));

					rntxtMeetingMaxDayCount.Text = Config.MeetingMaxDayCount(ModuleId).ToString();
					chkRequireMeetingAddress.Checked = Config.RequireMeetingAddress(ModuleId);
					rcbGroupPage.SelectedValue = Config.SpotlightGroupPage(ModuleId).ToString();
					txtTemplate.Text = Config.SpotlightTemplate(ModuleId);
					rcbModule.SelectedValue = Config.SpotlightGroupModule(ModuleId).ToString();
					txtPageSize.Text = Config.SpotlightPageSize(ModuleId).ToString();
					txtWidgetHeight.Text = Config.SpotlightHeight(ModuleId).ToString();
					txtWidgetWidth.Text = Config.SpotlightWidth(ModuleId).ToString();
					txtItemHeight.Text = Config.SpotlightItemHeight(ModuleId).ToString();
					txtItemWidth.Text = Config.SpotlightItemWidth(ModuleId).ToString();
					txtFrameDuration.Text = Config.SpotlightFrameDuration(ModuleId).ToString();
					txtScrollDuration.Text = Config.SpotlightScrollDuration(ModuleId).ToString();
				}
			} 
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}
    
		/// <summary>
		/// UpdateSettings is called by the ModuleSettings page to allow this control to save its settings.
		/// </summary>
		public override void UpdateSettings()
		{
		  try
		  {
			  var objModule = new ModuleController();

			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_MEETINGMAXDAYCOUNT, rntxtMeetingMaxDayCount.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_REQUIREMEETINGADDRESS, chkRequireMeetingAddress.Checked.ToString());
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_GROUPPAGE, rcbGroupPage.SelectedValue);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_TEMPLATE, txtTemplate.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_MODULE, rcbModule.SelectedValue);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_PAGESIZE, txtPageSize.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_HEIGHT, txtWidgetHeight.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_WIDTH, txtWidgetWidth.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_ITEM_HEIGHT, txtItemHeight.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_ITEM_WIDTH, txtItemWidth.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_FRAME_DURATION, txtFrameDuration.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SPOTLIGHT_SCROLL_DURATION, txtScrollDuration.Text);
		  }
		  catch (Exception exc)
		  {
			 Exceptions.ProcessModuleLoadException(this, exc);
		  }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RcbGroupPageSelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e) {
			BindModules(Convert.ToInt32(rcbGroupPage.SelectedValue));
		}

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		private void BindPages()
		{
			var arrTabs = TabController.GetPortalTabs(PortalId, Null.NullInteger, true, true, false, false);

			rcbGroupPage.DataSource = arrTabs;
			rcbGroupPage.DataBind();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabID"></param>
		private void BindModules(int tabID) {
			rcbModule.Items.Clear();

			var objModules = new ModuleController();
			var arrModules = new ArrayList();
			var arrTabModules = objModules.GetTabModules(tabID);

			ModuleInfo objModule;
			foreach (var kvp in arrTabModules) {
				objModule = kvp.Value;
				if (PortalSecurity.IsInRoles(objModule.AuthorizedEditRoles) & objModule.IsDeleted == false) {
					arrModules.Add(objModule);
				}
			}

			if (arrModules.Count <= 0) return;
			rcbModule.DataSource = arrModules;
			rcbModule.DataBind();

			rcbModule.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Localization.GetString("None", LocalResourceFile), "-1"));
		}

		#endregion

	}
}