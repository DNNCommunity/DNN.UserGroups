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
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules.Actions;

namespace DotNetNuke.Modules.UserGroups
{
    public partial class Dispatch : UserGroupsModuleBase, IActionable {

	    #region Private Members

	    const string CtlDirectorySearch = "/DirectorySearch.ascx";
	    const string CtlGroupProfile = "/GroupProfile.ascx";
	    const string CtlWelcomeUser = "/WelcomeUser.ascx";
	    const string CtlLeaveGroup = "/LeaveGroup.ascx";
	    const string CtlMeetingFinder = "/MeetingFinder.ascx";
    	    const string CtlMeetingDetail = "/MeetingDetail.ascx";
    	    const string CtlContactOfficers = "/ContactOfficers.ascx";

	    string _ctlToLoad;

	    #endregion

	    #region Event Handlers

	    /// <summary>
	    /// Initialize the control by first registering any scripts for Ajax. 
	    /// </summary>
	    /// <param name="sender"></param>
	    /// <param name="e"></param>
	    /// <remarks>This requires the control having 'SupportsPartialRendering' set to true in the manifest file.</remarks>
	    protected void Page_Init(Object sender, EventArgs e)
	    {
		    if (Framework.AJAX.IsInstalled()) 
		    {
			    Framework.AJAX.RegisterScriptManager();
		    }
	    }

	    /// <summary>
	    /// Page_Load runs when the control is loaded
	    /// </summary>
	    protected void Page_Load(object sender, EventArgs e) 
	    {
		    try 
		    {
			    if (!Page.IsPostBack)
			    {

				    if (Request.QueryString["view"] != null)
				    {
					    var strTempControl = Convert.ToInt32(Request.QueryString["view"]);
					    ControlToLoad = CtlDirectorySearch;

					    switch (strTempControl)
					    {
						    case (int)PageScope.DirectorySearch:
							    ControlToLoad = CtlDirectorySearch;
					    			break;
						    case (int)PageScope.GroupProfile:
								ControlToLoad = CtlGroupProfile;
					    			break;
						    case (int)PageScope.WelcomeUser:
								ControlToLoad = CtlWelcomeUser;
					    			break;
							case (int)PageScope.RemoveUser:
					    			ControlToLoad = CtlLeaveGroup;
					    			break;
							case (int)PageScope.MeetingFinder:
								ControlToLoad = CtlMeetingFinder;
								break;
						    case (int)PageScope.MeetingDetail:
					    			ControlToLoad = CtlMeetingDetail;
					    			break;
						    case (int)PageScope.ContactOfficers:
					    			ControlToLoad = CtlContactOfficers;
					    			break;
                              }
				    }
				    else
				    {
					    ControlToLoad = CtlDirectorySearch;
					    // this requires logic based on authenticated/non-authenticated and member status
					    if (IsAuthenticated) {
						    if (ProfileUsersGroupID > 0) {
							    ControlToLoad = CtlGroupProfile;
						    }
					    }
				    }
			    }

				//Load the appropriate control
				_ctlToLoad = ControlToLoad;
				LoadGroupControl(_ctlToLoad);
		    } 
		    catch (Exception exc) //Module failed to load
		    {
			    Exceptions.ProcessModuleLoadException(this, exc);
		    }
	    }

	    #endregion

	    #region Private Methods

	    /// <summary>
	    /// This method actually loads the appropriate control into the placeholder, sets the viewstate parameter to ensure it persists across any postbacks. 
	    /// </summary>
	    /// <param name="control"></param>
	    private void LoadGroupControl(string control) {
			var ctlDirectory = TemplateSourceDirectory;

		    var objControl = LoadControl(ctlDirectory + control) as PortalModuleBase;
	    		if (objControl == null) return;
	    		phUserControl.Controls.Clear();
	    		objControl.ModuleConfiguration = ModuleConfiguration;
	    		objControl.ID = System.IO.Path.GetFileNameWithoutExtension(ctlDirectory + control);
	    		phUserControl.Controls.Add(objControl);

	    		if ((string)ViewState["CtlToLoad"] != control) {
	    			//Utilities.AjaxLoader.IPageLoad ctlLoaded;
	    			//ctlLoaded = (Utilities.AjaxLoader.IPageLoad)phUserControl.FindControl(objControl.ID);
	    			//ctlLoaded.LoadInitialView();
	    			ViewState["CtlToLoad"] = control;
	    		}
	    }

	    #endregion

	    #region IActionable Members

	    /// <summary>
	    /// 
	    /// </summary>
	    public ModuleActionCollection ModuleActions
        {
            get
            {
                var actionCollection = new ModuleActionCollection {};
                return actionCollection;
            }
        }

        #endregion
  
    }
}