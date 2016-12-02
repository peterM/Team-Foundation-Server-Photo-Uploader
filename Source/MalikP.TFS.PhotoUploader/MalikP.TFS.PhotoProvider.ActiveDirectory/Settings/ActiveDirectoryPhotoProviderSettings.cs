//-------------------------------------------------------------------------------------------------
// <copyright file="ActiveDirectoryPhotoProviderSettings.cs" company="MalikP.">
//   Copyright (c) 2016-2017, Peter Malik.
//   Authors: Peter Malik (MalikP.) (peter.malik@outlook.com)
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MalikP.TFS.PhotoProvider.Settings
{
    public class ActiveDirectoryPhotoProviderSettings : PhotoProviderSettings
    {
        public virtual string Domain { get; set; }
        public virtual string LdapDomain
        {
            get
            {
                return $"LDAP://{Domain}";
            }
        }
        public virtual string PhotoAttribute { get; set; } = "thumbnailphoto";

        public override void UseDefaults()
        {
            Domain = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.ActiveDirectoryPhotoProviderSettings.{nameof(Domain)}"];
            PhotoAttribute = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.ActiveDirectoryPhotoProviderSettings.{nameof(PhotoAttribute)}"];
        }
    }
}