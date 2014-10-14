//
// Copyright (c) 2010
// by Ashish Agrawal and Chris Paterra
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

using System.Linq;
using DotNetNuke.Entities.Content;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.UserGroups.Common;
using DotNetNuke.Modules.UserGroups.Components.Entities;

namespace DotNetNuke.Modules.UserGroups.Components.Common
{

	public class Content
	{

		private const string ContentTypeName = "DNN_UserGroup";

		/// <summary>
		/// This should only run after the group exists in the data store. 
		/// </summary>
		/// <returns>The newly created ContentItemID from the data store.</returns>
		public ContentItem CreateContentItem(UserGroupInfo objGroup, int tabId)
		{
			var typeController = new ContentTypeController();
			var colContentTypes = (from t in typeController.GetContentTypes() where t.ContentType == ContentTypeName select t);
			int contentTypeID;

			if (colContentTypes.Count() > 0)
			{
				var contentType = colContentTypes.Single();
				contentTypeID = contentType == null ? CreateContentType() : contentType.ContentTypeId;
			}
			else
			{
				contentTypeID = CreateContentType();
			}

			var objContent = new ContentItem
								{
									Content = objGroup.Name,
									ContentTypeId = contentTypeID,
									Indexed = false,
									ContentKey = "view=" + (int)PageScope.GroupProfile + "&id=" + objGroup.UserGroupID,
									ModuleID = objGroup.ModuleID,
									TabID = tabId
								};

			objContent.ContentItemId = DotNetNuke.Entities.Content.Common.Util.GetContentController().AddContentItem(objContent);

			//// Add Terms
			//var cntTerm = new Terms();
			//cntTerm.ManageEntryTerms(objEntry, objContent);

			return objContent;
		}

		/// <summary>
		/// This is used to update the content in the ContentItems table. Should be called when a group is updated.
		/// </summary>
		public void UpdateContentItem(UserGroupInfo objGroup, int tabId)
		{
			var objContent = DotNetNuke.Entities.Content.Common.Util.GetContentController().GetContentItem(objGroup.ContentItemId);

			if (objContent == null) return;
			objContent.Content = objGroup.Name;
			objContent.TabID = tabId;
			DotNetNuke.Entities.Content.Common.Util.GetContentController().UpdateContentItem(objContent);

			//// Update Terms
			//var cntTerm = new Terms();
			//cntTerm.ManageEntryTerms(objEntry, objContent);
		}

		/// <summary>
		/// This removes a content item associated with a group from the data store. Should run every time a group is deleted.
		/// </summary>
		/// <param name="objGroup">The Group we wish to remove from the data store.</param>
		public void DeleteContentItem(UserGroupInfo objGroup)
		{
			if (objGroup.ContentItemId <= Null.NullInteger) return;
			var objContent = DotNetNuke.Entities.Content.Common.Util.GetContentController().GetContentItem(objGroup.ContentItemId);
			if (objContent == null) return;

			//// remove any metadata/terms associated first (perhaps we should just rely on ContentItem cascade delete here?)
			//var cntTerms = new Terms();
			//cntTerms.RemoveEntryTerms(objEntry);

			DotNetNuke.Entities.Content.Common.Util.GetContentController().DeleteContentItem(objContent);
		}

		#region Private Methods

		/// <summary>
		/// Creates a Content Type (for taxonomy) in the data store.
		/// </summary>
		/// <returns>The primary key value of the new ContentType.</returns>
		private static int CreateContentType()
		{
			var typeController = new ContentTypeController();
			var objContentType = new ContentType { ContentType = ContentTypeName };

			return typeController.AddContentType(objContentType);
		}

		#endregion

	}
}