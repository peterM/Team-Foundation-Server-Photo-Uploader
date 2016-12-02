//-------------------------------------------------------------------------------------------------
// <copyright file="ActiveDirectoryPhotoProvider.cs" company="MalikP.">
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

using MalikP.TFS.PhotoProvider.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.IO;
using System.Collections;
using System.Security.Principal;

namespace MalikP.TFS.PhotoProvider
{
    public class ActiveDirectoryPhotoProvider : PhotoProviderBase<ActiveDirectoryPhotoProviderSettings>
    {
        public ActiveDirectoryPhotoProvider() : this(null) { }
        public ActiveDirectoryPhotoProvider(ActiveDirectoryPhotoProviderSettings settings) : base(settings) { }

        public override Bitmap GetBitmap(object key)
        {
            Bitmap resultBitmap = null;
            var photoBytes = GetPhotoBytes(key);

            if (photoBytes != null && photoBytes.Length > 0)
            {
                var ms = new MemoryStream(photoBytes);
                resultBitmap = new Bitmap(ms);
            }

            return resultBitmap;
        }

        public override string GetContentType(Bitmap photo)
        {
            return photo.RawFormat.GetMimeType();
        }

        public override string GetContentType(byte[] photoBytes)
        {
            string contentType = null;
            using (var ms = new MemoryStream(photoBytes))
            {
                var targetBitmap = new Bitmap(ms);
                contentType = GetContentType(targetBitmap);
            }

            return contentType;
        }

        public override byte[] GetPhotoBytes(object key)
        {
            byte[] photoBytes = null;
            using (var context = new PrincipalContext(ContextType.Domain, Settings.Domain))
            using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
            {
                var adUser = searcher.FindAll()
                                     .SingleOrDefault(m =>
                {
                    var account = (NTAccount)m.Sid.Translate(typeof(NTAccount));
                    return string.Equals(account.ToString(), key.ToString(), StringComparison.InvariantCultureIgnoreCase);
                });

                if (adUser != null)
                {
                    var directoryEntry = new DirectoryEntry(Settings.LdapDomain);
                    using (var directorySearcher = new DirectorySearcher(directoryEntry))
                    {
                        directorySearcher.Filter = $"(&(SAMAccountName={adUser.SamAccountName}))";

                        var user = directorySearcher.FindOne();
                        var propertyNames = user.Properties.PropertyNames.ToList<string>();

                        if (propertyNames.Contains(Settings.PhotoAttribute, StringComparer.InvariantCultureIgnoreCase))
                        {
                            photoBytes = user.Properties[Settings.PhotoAttribute][0] as byte[];
                        }
                    }
                }
            }

            return photoBytes;
        }
    }
}

