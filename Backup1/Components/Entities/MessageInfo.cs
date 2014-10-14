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

using System.ComponentModel;
using DotNetNuke.Common.Utilities;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Security;

namespace DotNetNuke.Modules.UserGroups.Components.Entities
{
    public class UserGroupMessageInfo
    {

        #region Private Members

        private int userGroupID = Null.NullInteger;
        private int recipientID = Null.NullInteger;
        private string subject = Null.NullString;
        private string message = Null.NullString;

          #endregion

        #region Constructors

        public UserGroupMessageInfo()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The Id of the User Group
        /// </summary>
        [Browsable(false)]
        public int UserGroupID
        {
            get { return userGroupID; }
            set { userGroupID = value; }
        }

        /// <summary>
        /// The ID of the User to receive the Message
        /// </summary>
        [SortOrder(0),
        Editor("DotNetNuke.Modules.UserGroups.UI.Editors.SelectUserEditControl, DotNetNuke.Modules.UserGroups", "DotNetNuke.UI.WebControls.EditControl")]
        public int RecipientID
        {
            get { return recipientID; }
            set { recipientID = value; }
        }

        /// <summary>
        /// The Subject of the Message
        /// </summary>
        [SortOrder(1)] 
        [Required(true)]
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// The Description of the meeting
        /// </summary>
        [SortOrder(2)]
        [Editor("DotNetNuke.UI.WebControls.MultiLineTextEditControl, DotNetNuke", "DotNetNuke.UI.WebControls.EditControl")]
        [ControlStyle("NormalTextBox", "300px", "150px")]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        #endregion

        #region Public Methods

        public void Clean(PortalSecurity.FilterFlag filter)
        {
		   var security = new PortalSecurity();
            Subject = security.InputFilter(Subject, filter);
            Message = security.InputFilter(Message, filter);
        }

        #endregion

    }
}
