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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common;

namespace DotNetNuke.Modules.UserGroups
{

	/// <summary>
	/// 
	/// </summary>
    public partial class SendMessage : UserGroupsModuleBase 
    {

	    #region Event Handlers

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

				if (Page.IsPostBack == false)
				{
					if (!String.IsNullOrEmpty(Request.QueryString["UserGroupID"]))
					{
						rcbSendTo.DataTextField = Config.NameFormat(ModuleId);
						var userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
						var cntUserGroup = new UserGroupController();
						var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

						if (IsEditable || (objUserGroup.LeaderID == UserId) || CurrentUserIsOfficer)
						{
							BindMembers(userGroupID);
						}
						else
						{
							Response.Redirect(Globals.AccessDeniedURL(GetLocalizedString("ModuleAccess.Error")), true);
						}	
					}
					else
					{
						Response.Redirect(Globals.AccessDeniedURL(GetLocalizedString("ModuleAccess.Error")), true);
					}
				}
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
		protected void CmdCancelClick(object sender, EventArgs e)
		{
			Response.Redirect(Globals.NavigateURL(), true);
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
		protected void CmdSendClick(object sender, EventArgs e)
		{
			try 
			{
				if (!String.IsNullOrEmpty(Request.QueryString["UserGroupID"])) 
				{

					var strMessage = "";

					if (txtMessage.Text == "")
					{
						strMessage = "Message Required! <br />";
					}

					if (txtSubject.Text == "")
					{
						strMessage += "Subject Required! <br />";
					}

					if (strMessage.Length > 0)
					{
						lblMessage.Text = strMessage;
						lblMessage.Visible = true;
						return;
					}
					lblMessage.Visible = false;

					var userGroupID = Convert.ToInt32(Request.QueryString["UserGroupID"]);
					var cntUserGroup = new UserGroupController();
					var objUserGroup = cntUserGroup.GetCachedUserGroup(userGroupID, ModuleId);

					if (objUserGroup != null) 
					{
						//Send message
						UserGroupController.SendMessage(txtMessage.Text, txtSubject.Text, objUserGroup, PortalId, Convert.ToInt32(rcbSendTo.SelectedValue));
						Response.Redirect(Globals.NavigateURL(), true);
					}
				}
				//MessageKey = "MailSentSuccess";
			} 
			catch (Exception exc) 
			{
				Exceptions.LogException(exc);
			}
			//Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
		}

		#endregion

	    #region Private Methods

		/// <summary>
		/// Binds a list of available members, also adds "All Users". 
		/// </summary>
		/// <param name="userGroupID"></param>
		private void BindMembers(int userGroupID) {
			rcbSendTo.Items.Clear();

			if (userGroupID > 0) {
				var cntUgUsers = new UserGroupUserController();
				var colUsers = cntUgUsers.GetCachedUsersByUserGroup(userGroupID);

				rcbSendTo.DataSource = colUsers;
				rcbSendTo.DataBind();
			}
			rcbSendTo.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(GetLocalizedString("AllUsers") , "-1"));
		}

	    #endregion

    }
}