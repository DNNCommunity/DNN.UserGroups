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
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using Telerik.Web.UI;

namespace DotNetNuke.Modules.UserGroups
{
    public partial class GroupProfile : UserGroupsModuleBase {

	    #region Event Handlers

	    /// <summary>
		/// Initialize the control by first registering any scripts for Ajax. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>This requires the control having 'SupportsPartialRendering' set to true in the manifest file.</remarks>
// ReSharper disable InconsistentNaming
		protected void Page_Init(Object sender, EventArgs e)
// ReSharper restore InconsistentNaming
		{
			Framework.jQuery.RequestRegistration();

			var strMapScript = @"<script src=""http://maps.google.com/maps?file=api&amp;v=2&amp;key=" + Config.MapKey(ModuleId) + @"&sensor=false"" type=""text/javascript""></script>";

			Page.ClientScript.RegisterClientScriptBlock(GetType(), "mapsRegister", strMapScript);
			Page.ClientScript.RegisterClientScriptInclude("maps", TemplateSourceDirectory + "/scripts/jquery.googlemaps1.01.js"); //jquery.googlemaps.pack.1.01.js
	             
			if (Framework.AJAX.IsInstalled())
			{
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
		protected void Page_Load(object sender, EventArgs e) {
// ReSharper restore InconsistentNaming
			try 
			{
					if (!Page.IsPostBack) {
						var groupID = -1;
						if (Request.QueryString["id"] != null) 
						{
							groupID = Convert.ToInt32(Request.QueryString["id"]);
						}
						else
						{
							if (IsAuthenticated) {
								if (ProfileUsersGroupID > 0) {
									groupID = ProfileUsersGroupID;
								}
							}
						}

						var invalidGroup = true;

						if (groupID != -1) {
							var cntGroup = new UserGroupController();
							var objGroup = cntGroup.GetCachedUserGroup(groupID, ModuleId);

							if (objGroup != null) 
							{
								BindGroup(objGroup);
								invalidGroup = false;
							}
						}

						if (invalidGroup) 
						{
							lblGroupName.Text = @"N/A";
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
		protected void RgMembersItemDataBound(object sender, GridItemEventArgs e) 
		{
			if (!(e.Item is GridDataItem)) return;
			var userKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"];
			var officer = (bool)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Officer"];
			var leaderID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["LeaderID"];
			var dataItem = (GridDataItem)e.Item;

			var hlName = ((HyperLink)(dataItem)["Name"].Controls[0]);
			string name;

			if (Config.NameFormat(ModuleId) == "DisplayName") {
				name = (string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DisplayName"];
			} else {
				name = (string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Username"];
			}

			hlName.Text = name;
			hlName.NavigateUrl = DotNetNuke.Common.Globals.UserProfileURL(userKeyID);

			var imgOfficer = ((Image)(dataItem)["Officer"].Controls[0]);

			if (officer) 
			{
				imgOfficer.ImageUrl = TemplateSourceDirectory + "/images/check.png";
				imgOfficer.AlternateText = Localization.GetString("IsOfficer", LocalResourceFile);
				imgOfficer.ToolTip = Localization.GetString("IsOfficer", LocalResourceFile);
			} 
			else 
			{
				if (userKeyID == leaderID) 
				{
					imgOfficer.ImageUrl = TemplateSourceDirectory + "/images/check.png";
					imgOfficer.AlternateText = Localization.GetString("IsOfficer", LocalResourceFile);
					imgOfficer.ToolTip = Localization.GetString("IsOfficer", LocalResourceFile);
				} 
				else 
				{
					imgOfficer.ImageUrl = TemplateSourceDirectory + "/images/unchecked.png";
					imgOfficer.AlternateText = Localization.GetString("NotOfficer", LocalResourceFile);
					imgOfficer.ToolTip = Localization.GetString("NotOfficer", LocalResourceFile);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgMeetingsItemDataBound(object sender, GridItemEventArgs e) {
			if (!(e.Item is GridDataItem)) return;
			//var groupKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];
			var keyMeetingID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MeetingID"];
			var dataItem = (GridDataItem)e.Item;

			var imgEdit = ((ImageButton)(dataItem)["EditItem"].Controls[0]);
			imgEdit.AlternateText = Localization.GetString("Edit", LocalResourceFile);
			imgEdit.ToolTip = Localization.GetString("Edit", LocalResourceFile);

			var imgDelete = ((ImageButton)(dataItem)["DeleteItem"].Controls[0]);
			//imgDelete.CommandArgument = keyMeetingID.ToString();
			imgDelete.AlternateText = Localization.GetString("Delete", LocalResourceFile);
			imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile);
			imgDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

			var hlMeetingDetail = ((HyperLink)(dataItem)["Title"].Controls[0]);
			hlMeetingDetail.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, keyMeetingID, PageScope.MeetingDetail);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RgMeetingsItemCommand(object sender, GridCommandEventArgs e) {
			if (
				(!(e.Item.ItemType == GridItemType.Item |
				   e.Item.ItemType == GridItemType.AlternatingItem))) return;

			var keyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MeetingID"];
			var userGroupID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupID"];

			switch (e.CommandName) {
				case "EditItem":
					Response.Redirect(Util.EditMeeting(TabId, ModuleId, userGroupID, keyID), true);
					break;
				case "DeleteItem":
					var cntMeeting = new MeetingController();
					cntMeeting.DeleteUserGroupMeeting(keyID, userGroupID, ModuleId);
					Response.Redirect(Util.ViewControlLink(TabId, ModuleId, userGroupID, PageScope.GroupProfile));
					break;
			}
		}

	    #endregion

		#region Private Methods

		/// <summary>
		/// Localizes the grid header colums.
		/// </summary>
		private void SetLocalization() 
		{
			foreach (GridColumn gc in rgMembers.MasterTableView.Columns) 
			{
				if (gc.HeaderText != "") 
				{
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile);
				}
			}

			foreach (GridColumn gc in rgMeetings.MasterTableView.Columns) 
			{
				if (gc.HeaderText != "") 
				{
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objGroup"></param>
    		private void BindGroup(UserGroupInfo objGroup)
		{
			if (objGroup.ContentItemId < 1)
			{
				objGroup.ContentItemId = UserGroupController.CompleteEntryCreation(objGroup, TabId);
				var cntUg = new UserGroupController();
				cntUg.UpdateUserGroup(objGroup);
			}

			lblGroupName.Text = objGroup.Name;
			hlLeader.Text = Config.NameFormat(ModuleId) == "DisplayName" ? objGroup.LeaderDisplayName : objGroup.LeaderUsername;
			hlLeader.NavigateUrl = DotNetNuke.Common.Globals.UserProfileURL(objGroup.LeaderID);
			lblCountry.Text = objGroup.Country;
			lblLocation.Text = objGroup.City + @", " + objGroup.Region;
			lblLanguageValue.Text = objGroup.LanguageName;
			lblAbout.Text = objGroup.About;
			hlGroupProfile.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, objGroup.UserGroupID, PageScope.GroupProfile);
			imgGroupProfile.ImageUrl = TemplateSourceDirectory + "/images/dnnlogo.png";

			if (objGroup.Url.Trim() != "")
			{
				hlUrl.Text = objGroup.Url;
				hlUrl.NavigateUrl = objGroup.Url;
			}
			else
			{
				hlUrl.Text = @"N/A";
			}

			// we send second param blank for now.
			litMapScript.Text = Util.SetMap(objGroup, "");

			if (objGroup.FacebookUrl.Trim() == "") {
				hlFacebook.Visible = false;
				imgFacebook.Visible = false;
			} else {
				hlFacebook.NavigateUrl = objGroup.FacebookUrl;
				imgFacebook.ImageUrl = TemplateSourceDirectory + "/images/facebook.png";
				hlFacebook.Visible = true;
			}

			if (objGroup.TwitterUrl.Trim() == "") {
				hlTwitter.Visible = false;
				imgTwitter.Visible = false;
			} else {
				hlTwitter.NavigateUrl = objGroup.TwitterUrl;
				imgTwitter.ImageUrl = TemplateSourceDirectory + "/images/twitter.png";
				hlTwitter.Visible = true;
			}

			if (objGroup.LinkedInUrl.Trim() == "") {
				hlLinkedIn.Visible = false;
				imgLinkedIn.Visible = false;
			} else {
				hlLinkedIn.NavigateUrl = objGroup.LinkedInUrl;
				imgLinkedIn.ImageUrl = TemplateSourceDirectory + "/images/linkedin.png";
				hlLinkedIn.Visible = true;
			}

			hlContact.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, objGroup.UserGroupID, PageScope.ContactOfficers);

			if (ProfileUsersGroupID > 0) {
				if (objGroup.UserGroupID == ProfileUsersGroupID) {
					hlJoin.Text = Localization.GetString("LeaveGroup", LocalResourceFile);
					hlJoin.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, objGroup.UserGroupID, PageScope.RemoveUser);
					hlContact.Visible = true;
					//hlContact.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, objGroup.UserGroupID, PageScope.ContactOfficers);
				} else {
					hlJoin.Text = Localization.GetString("SwitchGroup", LocalResourceFile);
					//Add logic to join new group (ie. new group id)
					hlJoin.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, objGroup.UserGroupID, PageScope.RemoveUser);
					hlContact.Visible = false;
				}
			} else {
				hlJoin.Text = IsAuthenticated ? Localization.GetString("JoinGroup", Constants.LOCALIZATION_SharedResourceFile) : Localization.GetString("LoginToJoin", LocalResourceFile);
				hlJoin.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, objGroup.UserGroupID, PageScope.WelcomeUser);
				hlContact.Visible = false;
			}

			BindMembers(objGroup.UserGroupID, true);
			BindEvents(objGroup.UserGroupID);

			hlGroupSearch.NavigateUrl = Util.ViewControlLink(TabId, ModuleId, -1, PageScope.DirectorySearch);

			if (IsEditable || (objGroup.LeaderID == UserId) || CurrentUserIsOfficer) {
				hlAddEvent.Visible = true;
				hlEditGroup.Visible = true;
				hlSendNews.Visible = true;
				hlMgUsers.Visible = true;
				rgMeetings.MasterTableView.GetColumn("EditItem").Display = true;
				rgMeetings.MasterTableView.GetColumn("DeleteItem").Display = true;

				hlAddEvent.NavigateUrl = Util.EditMeeting(TabId, ModuleId, objGroup.UserGroupID, -1);
				hlEditGroup.NavigateUrl = Util.ManageGroupLink(TabId, ModuleId, objGroup.UserGroupID);
				hlSendNews.NavigateUrl = Util.SendMessageLink(TabId, ModuleId, objGroup.UserGroupID);
				hlMgUsers.NavigateUrl = Util.ManageUsersLink(TabId, ModuleId, objGroup.UserGroupID);
			} 
			else 
			{
				hlAddEvent.Visible = false;
				hlEditGroup.Visible = false;
				hlSendNews.Visible = false;
				hlMgUsers.Visible = false;
				rgMeetings.MasterTableView.GetColumn("EditItem").Display = false;
				rgMeetings.MasterTableView.GetColumn("DeleteItem").Display = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userGroupID"></param>
		/// <param name="bindIt"></param>
		private void BindMembers(int userGroupID, bool bindIt)
		{
			var cntUgUsers = new UserGroupUserController();
			var colUsers = cntUgUsers.GetCachedUsersByUserGroup(userGroupID);

			rgMembers.DataSource = colUsers;
			rgMembers.Visible = true;

			if (bindIt) {
				rgMembers.DataBind();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userGroupID"></param>
		private void BindEvents(int userGroupID) 
		{
			var cntMeeting = new MeetingController();
			var colMeetings = cntMeeting.GetMeetingsByUserGroup(Convert.ToInt32(userGroupID), DateTime.Now, 5, 0);

			if (colMeetings != null)
			{
				rgMeetings.DataSource = colMeetings;
				rgMeetings.DataBind();

			divEvents.Visible = true;
			}
			else
			{
			divEvents.Visible = false;
			}
		}

	    #endregion

    }
}