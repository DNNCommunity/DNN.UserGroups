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
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Modules.UserGroups.Components.Entities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Lists;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.UserGroups
{

	/// <summary>
	/// 
	/// </summary>
    public partial class ManageGroup : UserGroupsModuleBase 
    {

	    #region Event Handlers

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
// ReSharper disable InconsistentNaming
	    protected void Page_Init(object sender, EventArgs e) 
// ReSharper restore InconsistentNaming
	    {
		    if (Framework.AJAX.IsInstalled()) 
		    {
			    Framework.AJAX.RegisterScriptManager();
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
			    if (!IsAuthenticated) 
			    {
				    Response.Redirect(Util.AccessDeniedLink(), true);
			    }

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
	    protected void RcbCountrySelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e) 
	    {
		    BindRegions();
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void CmdSaveClick(object sender, EventArgs e) 
	    {
		    	SaveGroup();
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void CmdDeleteClick(object sender, EventArgs e) 
	    {
		    UserGroupController.DeleteUserGroup(Convert.ToInt32(txtUserGroupID.Text), ModuleId);
		    Response.Redirect(Globals.NavigateURL(), true);
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void CmdCancelClick(object sender, EventArgs e) 
	    {
		    Response.Redirect(Globals.NavigateURL(), true);
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void CmdManageUsersClick(object sender, EventArgs e) 
	    {
		    Response.Redirect(Util.ManageUsersLink(TabId, ModuleId, Convert.ToInt32(txtUserGroupID.Text)), true);
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void CmdManageMeetingsClick(object sender, EventArgs e) {
		    Response.Redirect(Util.ManageMeetingsLink(TabId, ModuleId, Convert.ToInt32(txtUserGroupID.Text)), true);
	    }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    protected void CmdAddUserClick(object sender, EventArgs e) 
	    {
		    var user = UserController.GetUserByName(PortalId, txtLeaderUsername.Text);

			if (user == null)
			{
				lblMessage.Text = Localization.GetString("valInvalidUser", LocalResourceFile);
				return;
			}

	    		var cntUgUser = new UserGroupUserController();
			var objMember = cntUgUser.GetCachedMember(user.UserID, PortalId);

			if (objMember != null) 
			{
				lblMessage.Text = Localization.GetString("valUser", LocalResourceFile);
			}
			else
			{
				var userGroupID = -1;

				if (!String.IsNullOrEmpty(Request.QueryString["UserGroupID"]))
				{
					userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
				}
				
				var cntUserGroup = new UserGroupController();
				var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

				if (objUserGroup != null)
				{
					userGroupID = cntUserGroup.JoinUserGroup(objUserGroup, UserController.GetCachedUser(PortalId, user.Username), PortalSettings, TabId);

					BindMembers(userGroupID);
				}
				else
				{
					if (txtName.Text.Trim().Length < 1) {
						lblMessage.Text = ValidatePage();
						return;
					}

					var newGroup = new UserGroupInfo
					               	{
					               		UserGroupID = -1,
					               		PortalID = PortalId,
					               		CreatedByUserID = UserId,
					               		CreatedOnDate = DateTime.Now,
					               		Name = txtName.Text.Trim(),
					               		Active = false,
					               		City = txtCity.Text.Trim(),
					               		LanguageID = Convert.ToInt32(rcbLanguageID.SelectedValue),
					               		Latitude = Convert.ToDouble(rntxtLatitude.Value),
					               		Longitude = Convert.ToDouble(rntxtLongitude.Value),
					               		Url = Util.FormatUrl(txtUrl.Text.Trim()),
					               		Logo = Util.FormatUrl(txtLogo.Text.Trim()),
					               		LinkedInUrl = Util.FormatUrl(txtLinkedInUrl.Text.Trim()),
					               		FacebookUrl = Util.FormatUrl(txtFacebookUrl.Text.Trim()),
					               		TwitterUrl = Util.FormatUrl(txtTwitterUrl.Text.Trim()),
										About = rtAbout.Text.Trim(),
					               		Country = rcbCountry.SelectedItem.Text,
					               		ModuleID = ModuleId,
										LeaderID = user.UserID,
										DefaultLanguage = txtFlag.Text.Trim()
					               	};

					if (rcbCountry.SelectedValue == "US" || rcbCountry.SelectedValue == "CA") {
						newGroup.Region = rcbRegion.SelectedItem.Text;
					} else {
						newGroup.Region = txtRegion.Text.Trim();
					}

					userGroupID = cntUserGroup.JoinUserGroup(newGroup, UserController.GetCachedUser(PortalId, user.Username), PortalSettings, TabId);
					Response.Redirect(Util.ManageGroupLink(TabId, ModuleId, userGroupID), false);
				}
			}
	    }

	    #endregion

	    #region Private Methods

		/// <summary>
		/// Sets the control up for viewing by the user's security level and prepares it for binding data.
		/// </summary>
		private void SetupPage()
		{
			rntxtLatitude.NumberFormat.DecimalDigits = 8;
			rntxtLongitude.NumberFormat.DecimalDigits = 8;

			if (IsEditable) 
			{
				divActive.Visible = true;
				divLeader.Visible = true;
				cmdDelete.Visible = true;
				cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
			} 
			else 
			{
				divActive.Visible = false;
				divLeader.Visible = false;
				cmdDelete.Visible = false;
			}

			BindCountries();
			BindLanguages();
			BindGroup();
		}

		/// <summary>
		/// Looks to see if a usergroupid is in the URL, if so it will bind the group data to the controls on the page. 
		/// </summary>
		private void BindGroup() 
		{
			if (!String.IsNullOrEmpty(Request.QueryString["UserGroupID"]))
			{
				var userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
				var cntUserGroup = new UserGroupController();
				var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

				if (objUserGroup == null)
				{
					IsNewGroup();
				}
				else
				{
					if (IsEditable || (UserId == objUserGroup.LeaderID) || CurrentUserIsOfficer) 
					{
						BindMembers(userGroupID);
						txtUserGroupID.Text = objUserGroup.UserGroupID.ToString();
						txtName.Text = objUserGroup.Name;
						txtCountry.Visible = true;
						txtCountry.Enabled = false;
						rcbCountry.Visible = false;
						txtCountry.Text = objUserGroup.Country;
						txtRegion.Visible = true;
						txtRegion.Enabled = false;
						rcbRegion.Visible = false;
						txtRegion.Text = objUserGroup.Region;
						txtCity.Text = objUserGroup.City;
						rcbLanguageID.SelectedValue = objUserGroup.LanguageID.ToString();
						rntxtLatitude.Text = objUserGroup.Latitude.ToString();
						rntxtLongitude.Text = objUserGroup.Longitude.ToString();
						txtUrl.Text = objUserGroup.Url;
						txtLogo.Text = objUserGroup.Logo;
						txtLinkedInUrl.Text = objUserGroup.LinkedInUrl;
						txtFacebookUrl.Text = objUserGroup.FacebookUrl;
						txtTwitterUrl.Text = objUserGroup.TwitterUrl;
						lblMembers.Text = objUserGroup.Members.ToString();
						rtAbout.Text = objUserGroup.About;
						chkActive.Checked = objUserGroup.Active;
						lblCreatedOnDate.Text = objUserGroup.CreatedOnDate.ToString();
						lblLastModifiedOnDate.Text = objUserGroup.LastModifiedOnDate.ToString();
						rcbLeaderID.SelectedValue = objUserGroup.LeaderID.ToString();
						txtLeaderID.Text = objUserGroup.LeaderID.ToString();
						rtMeetingAddress.Text = objUserGroup.MeetingAddress;
						txtFlag.Text = objUserGroup.DefaultLanguage;

						if (objUserGroup.LastModifiedByUserID > 0) {
							var objModUser = UserController.GetUserById(PortalId, objUserGroup.LastModifiedByUserID);
							lblLastModifiedUser.Text = Config.NameFormat(ModuleId) == "DisplayName" ? objModUser.DisplayName : objModUser.Username;
						}
					} 
					else 
					{
						Response.Redirect(Util.AccessDeniedLink(), true);
					}
				}
			}
			else
			{
				IsNewGroup();
			}
		}

		/// <summary>
		/// Sets the control up for creating a new user group.
		/// </summary>
		private void IsNewGroup()
		{

			// we need to make sure the user isn't trying to create a group and they already belong to one (unless admin)
			if (ProfileUsersGroupID > 0 & IsEditable == false)
			{
				Response.Redirect(Globals.NavigateURL(), true);
			}

			cmdDelete.Visible = false;
			txtUserGroupID.Text = @"-1";

			divMembers.Visible = false;
			cmdManageUsers.Visible = false;
			cmdManageMeetings.Visible = false;
			divCreated.Visible = false;
			divModifiedUser.Visible = false;
			divModified.Visible = false;
			txtFlag.Text = @"en-US";

			//see if we can set country/region
			if (!String.IsNullOrEmpty(Request.QueryString["country"])) 
			{
				var country = Request.QueryString["country"];

				rcbCountry.SelectedValue = country;
			} 
			else 
			{
				rcbCountry.SelectedItem.Text = ProfileUser.Profile.Country;
			}

			BindRegions();
			BindMembers(-1);
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
			rcbCountry.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", ""));
		}

		/// <summary>
		/// Binds a list of available regions to the combobox, based on the selected country. 
		/// </summary>
		private void BindRegions() 
		{
			var controller = new ListController();
			var listKey = "Country." + rcbCountry.SelectedValue;
			var regions = controller.GetListEntryInfoCollection("Region", listKey);

			if (rcbCountry.SelectedIndex > 0)
			{
				if (rcbCountry.SelectedValue == "US" || rcbCountry.SelectedValue == "CA") 
				{
					rcbRegion.Items.Clear();

					rcbRegion.DataSource = regions;
					rcbRegion.DataBind();
					rcbRegion.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", ""));

					rcbRegion.Visible = true;
					txtRegion.Visible = false;

					//rcbRegion.SelectedItem.Text = ProfileUser.Profile.Region;
				}
				else 
				{
					rcbRegion.Visible = false;
					txtRegion.Visible = true;

					txtRegion.Text = ProfileUser.Profile.Region;
				}
			}
			else
			{
				rcbRegion.Items.Clear();

				rcbRegion.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", ""));
				rcbRegion.Visible = true;
				txtRegion.Visible = false;

				//rcbRegion.SelectedItem.Text = ProfileUser.Profile.Region;
			}
		}

		/// <summary>
		/// Binds a list of languages available for the portal.
		/// </summary>
		private void BindLanguages() 
		{
			rcbLanguageID.Items.Clear();

			var cntLang = new LanguageController();
			var colLangs = cntLang.GetLanguages(PortalId);

			rcbLanguageID.DataSource = colLangs;
			rcbLanguageID.DataBind();
			rcbLanguageID.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", "-1"));
		}

		/// <summary>
		/// Binds a list of available members for selection as leader.
		/// </summary>
		/// <param name="userGroupID"></param>
		private void BindMembers(int userGroupID) 
		{
			rcbLeaderID.Items.Clear();

			if (userGroupID > 0)
			{
				var cntUgUsers = new UserGroupUserController();
				var colUsers = cntUgUsers.GetCachedUsersByUserGroup(userGroupID);

				rcbLeaderID.DataSource = colUsers;
				rcbLeaderID.DataBind();
			}
			rcbLeaderID.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("<" + GetLocalizedString("Not_Specified") + ">", "-1"));
		}

		/// <summary>
		/// First this will check to make sure all required data is valid. If so, it will save the group to the data store, then redirect the user.
		/// </summary>
		private void SaveGroup()
		{
			var strErrorMsg = ValidatePage();

			if (strErrorMsg.Length > 0)
			{
				lblMessage.Text = strErrorMsg;
				return;
			}

			var cntGroup = new UserGroupController();
			var objGroup = new UserGroupInfo
			               	{
			               		PortalID = PortalId,
			               		LeaderID = Convert.ToInt32(rcbLeaderID.SelectedValue),
			               		Name = txtName.Text.Trim(),
			               		City = txtCity.Text.Trim(),
			               		LanguageID = Convert.ToInt32(rcbLanguageID.SelectedValue),
			               		Latitude = Convert.ToDouble(rntxtLatitude.Value),
			               		Longitude = Convert.ToDouble(rntxtLongitude.Value),
			               		Url = Util.FormatUrl(txtUrl.Text.Trim()),
			               		Logo = Util.FormatUrl(txtLogo.Text.Trim()),
			               		LinkedInUrl = Util.FormatUrl(txtLinkedInUrl.Text.Trim()),
			               		FacebookUrl = Util.FormatUrl(txtFacebookUrl.Text.Trim()),
			               		TwitterUrl = Util.FormatUrl(txtTwitterUrl.Text.Trim()),
								About = rtAbout.Text.Trim(),
			               		ModuleID = ModuleId,
								MeetingAddress = rtMeetingAddress.Text,
								DefaultLanguage = txtFlag.Text.Trim()
			};

			if (txtUserGroupID.Text == @"-1") 
			{
				objGroup.CreatedByUserID = UserId;
				objGroup.CreatedOnDate = DateTime.Now;
				objGroup.Country = rcbCountry.SelectedItem.Text;

				if (rcbCountry.SelectedValue == "US" || rcbCountry.SelectedValue == "CA") 
				{
					objGroup.Region = rcbRegion.SelectedItem.Text;
				} 
				else 
				{
					objGroup.Region = txtRegion.Text.Trim();
				}

				objGroup.LeaderID = ProfileUserID;
				objGroup = cntGroup.AddUserGroup(objGroup, UserController.GetCurrentUserInfo(), TabId);

				if (objGroup.UserGroupID > 0)
				{
					cntGroup.UpdateUserGroup(objGroup);

					cntGroup.JoinUserGroup(objGroup, UserController.GetCachedUser(PortalId, ProfileUser.Username), PortalSettings, TabId);
				}
			} 
			else 
			{
				objGroup.UserGroupID = Convert.ToInt32(txtUserGroupID.Text);
				objGroup.LastModifiedOnDate = DateTime.Now;
				objGroup.LastModifiedByUserID = UserId;
				objGroup.Country = txtCountry.Text;
				objGroup.Region = txtRegion.Text;

				if (txtLeaderID.Text != rcbLeaderID.SelectedValue)
				{
					// leader changed
					var cntUgUser = new UserGroupUserController();
					cntUgUser.UpdateLeader(objGroup.UserGroupID, Convert.ToInt32(rcbLeaderID.SelectedValue), Convert.ToInt32(txtLeaderID.Text), PortalId);
				}

				cntGroup.UpdateUserGroup(objGroup);
			}

			Response.Redirect(Globals.NavigateURL(), true);
		}

	    /// <summary>
	    /// Used to determine what items are not properly set and return a message to the end user.
	    /// </summary>
	    /// <returns></returns>
	    private string ValidatePage()
	    {
			var strErrorMsg = "";

			if (rcbCountry.Visible) 
			{
				if (rcbCountry.SelectedIndex < 1)
				{
					strErrorMsg = Localization.GetString("valCountry", LocalResourceFile);
				}
			}

			if (rcbRegion.Visible)
			{
				if (rcbRegion.SelectedIndex < 1)
				{
					strErrorMsg += Localization.GetString("valRegionNA", LocalResourceFile);
				}
			}
			else
			{
	    			if (txtRegion.Text.Trim().Length < 1) 
				{
					strErrorMsg += Localization.GetString("valRegion", LocalResourceFile);
				}
			}

			if (rcbLanguageID.SelectedIndex < 1) 
			{
				strErrorMsg += Localization.GetString("valLanguage", LocalResourceFile);
			}

			if ((rcbLeaderID.SelectedIndex < 1) & (divLeader.Visible = true))
			{
				if (txtUserGroupID.Text != @"-1")
				{
					strErrorMsg += Localization.GetString("valLeader", LocalResourceFile);
				}
			}

			if (txtName.Text.Trim().Length < 1) 
			{
				strErrorMsg += Localization.GetString("valName", LocalResourceFile);
			}

			if (txtCity.Text.Trim().Length < 1) 
			{
				strErrorMsg += Localization.GetString("valCity", LocalResourceFile);
			}

	    		return strErrorMsg;
	    }

	    #endregion

    }
}