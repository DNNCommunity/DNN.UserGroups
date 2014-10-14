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

using System.Collections.Generic;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Modules.UserGroups.Components.Entities;

namespace DotNetNuke.Modules.UserGroups {

	
	/// <summary>
	/// Summary description for GroupLocator
	/// </summary>
	[WebService(Description = "A service used to return user group related information.", Namespace = "http://www.dotnetnuke.com")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[ScriptService]
	public class GroupLocatorService : WebService {

		#region Private Members

		// this needs to change but we have no way of getting ModuleID, once core properly integrates profile image handling, we don't have to use this
		// localhost = 379, dnn.com production = 3575
		private const int ModuleID = 3575;

		private static PortalSettings Ps
		{
			get
			{
				return PortalController.GetCurrentPortalSettings();
			}
		}

		private static int PortalID
		{
			get
			{						
				//var domain = DotNetNuke.Common.Globals.GetDomainName(HttpContext.Current.Request);
				// use domain to get a valid portal id. 	
				return Ps.PortalId;
			}	     
		}

		private static string SetAvatarPath(int avatarFileID, int leaderID) {
				if (avatarFileID > 0)
				{
					var homeDirectory = Ps.HomeDirectory;
					var cntMember = new UserGroupController();
					var objAvatar = cntMember.GetCachedFile(leaderID, avatarFileID);
					return "http://" + Globals.GetDomainName(HttpContext.Current.Request) + homeDirectory + objAvatar.Folder + objAvatar.FileName;
				}
				return "http://" + Globals.GetDomainName(HttpContext.Current.Request) + "/images/no_avatar.gif";
		}

		#endregion

		[WebMethod(Description = "This returns all regions with at least one user group for the country provided.")]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public List<RegionInfo> GetRegions(string country) 
		{
			var cntRegion = new RegionController();
			var colRegions = cntRegion.GetGroupRegionsByCountry(PortalID, country);

			return colRegions;
		}

		[WebMethod(Description = "This returns all user groups in the country & region combination provided.")]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public List<ServiceUserGroupInfo> SearchByRegion(string country, string region)
		{
			var cntUg = new UserGroupController();

			var colGroups =  cntUg.ServiceSearchGroups(PortalID, 0, 50, country, region, string.Empty, string.Empty, -1, Config.PropertyDefinitionID(ModuleID));

			foreach (var objGroup in colGroups)
			{
				objGroup.AvatarPath =  SetAvatarPath(objGroup.AvatarFileID, objGroup.LeaderID);
			}

			return colGroups;
		}

		[WebMethod(Description = "This returns all user groups within a certain distance (ie. radius) based on the provided latitude and longitude.")]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public List<ServiceUserGroupInfo> SearchByLocation(float lati, float longi, int distance, bool inKilometers) {
			var cntUg = new UserGroupController();

			var colGroups = cntUg.SearchGroupsByLocation(PortalID, lati, longi, distance, inKilometers, Config.PropertyDefinitionID(ModuleID));

			foreach (var objGroup in colGroups) {
				objGroup.AvatarPath = SetAvatarPath(objGroup.AvatarFileID, objGroup.LeaderID);
			}

			return colGroups;
		}
	}
}