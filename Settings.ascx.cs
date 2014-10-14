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
using DotNetNuke.Entities.Profile;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Services.Exceptions;
using Config = DotNetNuke.Modules.UserGroups.Common.Config;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.UserGroups
{

    /// <summary>
    /// The Settings control permits basic configuration options for the module to be edited.
    /// </summary>
    public partial class Settings : ModuleSettingsBase
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
					BindNameFormats();
					BindProfileProperties();

					txtAdminEmail.Text = Config.AdminMail(ModuleId);
					txtEventModuleId.Text = Config.EventModule(ModuleId).ToString();
					rntxtMinGroupSize.Value = Config.MinSize(ModuleId);
					chkPublishMeetings.Checked = Config.PublishEvents(ModuleId);
					txtMapKey.Text = Config.MapKey(ModuleId);
					txtJoinMsg.Text = Config.JoinMessage(ModuleId);
					rntxtPageSize.Value = Config.PageSize(ModuleId);
					rcbNameFormat.SelectedValue = Config.NameFormat(ModuleId);
					txtCountrySelect.Text = Config.CountrySelectMsg(ModuleId);
					txtRegionSelect.Text = Config.RegionSelectMsg(ModuleId);
					txtLanguageSelect.Text = Config.LanguageSelectMsg(ModuleId);
					txtJoinSelect.Text = Config.JoinSelectMsg(ModuleId);
					rcbProfileProperty.SelectedValue = Settings[Constants.SETTING_PROPERTYDEFINITIONID] != null ? Settings[Constants.SETTING_PROPERTYDEFINITIONID].ToString() : "-1";
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

			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_ADMINMAIL, txtAdminEmail.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_EVENTMODULE, txtEventModuleId.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_MINSIZE, rntxtMinGroupSize.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_PUBLISHEVENTS, chkPublishMeetings.Checked.ToString());
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_MAPKEY, txtMapKey.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_JOINMESSAGE, txtJoinMsg.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_PAGESIZE, rntxtPageSize.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_NAMEFORMAT, rcbNameFormat.SelectedValue);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_COUNTRYSELECTMSG, txtCountrySelect.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_REGIONSELECTMSG, txtRegionSelect.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_LANGUAGESELECTMSG, txtLanguageSelect.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_JOINSELECTMSG, txtJoinSelect.Text);
			  objModule.UpdateModuleSetting(ModuleId, Constants.SETTING_PROPERTYDEFINITIONID, rcbProfileProperty.SelectedValue);
		  }
		  catch (Exception exc)
		  {
			 Exceptions.ProcessModuleLoadException(this, exc);
		  }
		}

		/// <summary>
		/// Uses localization to bind list of options for user name display (either username or display name, both located in the Users table).
		/// </summary>
		private void BindNameFormats()
		{
			rcbNameFormat.Items.Clear();
			rcbNameFormat.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(Localization.GetString("DisplayName", LocalResourceFile), "DisplayName"));
			rcbNameFormat.Items.Insert(1, new Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Username", LocalResourceFile), "Username"));
		}

		/// <summary>
		/// 
		/// </summary>
		private void BindProfileProperties() {
			rcbProfileProperty.Items.Clear();

			var colProfileProps = ProfileController.GetPropertyDefinitionsByPortal(PortalId);

			if (colProfileProps == null) return;
			rcbProfileProperty.DataSource = colProfileProps;
			rcbProfileProperty.DataBind();
			rcbProfileProperty.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("<" + Localization.GetString("Not_Specified", LocalResourceFile) + ">", ""));
		}

    }
}