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
using System.Collections.Generic;
using DotNetNuke.Entities.Host;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.UserGroups.Components.Data;

namespace DotNetNuke.Modules.UserGroups.Components.Controllers
{

	/// <summary>
	/// 
	/// </summary>
	public class RegionController
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		private static List<RegionInfo> GetGroupRegions(int portalId) {
			return CBO.FillCollection<RegionInfo>(DataProvider.Instance().GetGroupRegions(portalId));
		}

		public List<RegionInfo> GetGroupRegionsByCountry(int portalId, string country) {
			return CBO.FillCollection<RegionInfo>(DataProvider.Instance().GetGroupRegionsByCountry(portalId, country));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="portalID"></param>
		/// <returns></returns>
		public List<RegionInfo> GetCachedGroupRegions(int portalID) {
			var strCacheKey = Constants.CACHE_KEY + "-Regions-" + portalID;
			var colRegions = (List<RegionInfo>)DataCache.GetCache(strCacheKey);

			if (colRegions == null) {
				// caching settings
				var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

				colRegions = GetGroupRegions(portalID);

				//Cache List if timeout > 0 and collection is not null
				if (timeOut > 0 & colRegions != null) {
					DataCache.SetCache(strCacheKey, colRegions, TimeSpan.FromMinutes(timeOut));
				}
			}
			return colRegions;
		}

     }
}