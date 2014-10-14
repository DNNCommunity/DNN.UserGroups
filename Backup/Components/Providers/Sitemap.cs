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

using DotNetNuke.Entities.Portals;
using System.Collections.Generic;
using DotNetNuke.Modules.UserGroups.Components.Controllers;
using DotNetNuke.Modules.UserGroups.Components.Entities;
using DotNetNuke.Services.Sitemap;

namespace DotNetNuke.Modules.UserGroups.Components.Providers
{

	public class Sitemap : SitemapProvider
	{

		#region Public Methods

		/// <summary>
		/// This is used to populate a list of active user groups to include in the SEO sitemap. 
		/// </summary>
		/// <param name="portalID"></param>
		/// <param name="ps"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		public override List<SitemapUrl> GetUrls(int portalID, PortalSettings ps, string version) {
			var cntGroup = new UserGroupController();
			var colEntries = cntGroup.GetSitemapUrLs(portalID);
			var urls = new List<SitemapUrl>();

			foreach (var objGroup in colEntries)
			{
				urls.Add(GetProfileUrl(objGroup));
			}

			return urls;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Sets a single sitemap item.
		/// </summary>
		/// <param name="objGroup"></param>
		/// <returns></returns>
		private static SitemapUrl GetProfileUrl(UserGroupInfo objGroup) {
			var pageUrl = new SitemapUrl
			              	{
			              		Url = objGroup.GroupProfileUrl,
			              		Priority = (float) 0.5,
							LastModified = objGroup.LastModifiedOnDate,
			              		ChangeFrequency = SitemapChangeFrequency.Monthly
			              	};

			return pageUrl;
		}

		#endregion

	}
}