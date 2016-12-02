//-------------------------------------------------------------------------------------------------
// <copyright file="FileSystemPhotoProvider.cs" company="MalikP.">
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoProvider
{
    public class FileSystemPhotoProvider : PhotoProviderBase<FileSystemProviderSettings>
    {
        public FileSystemPhotoProvider(FileSystemProviderSettings settings) : base(settings) { }
        public FileSystemPhotoProvider() : this(null) { }

        public override Bitmap GetBitmap(object key)
        {
            return new Bitmap(Settings.ConstrucFilePath(key.ToString()));
        }

        public override string GetContentType(byte[] photoBytes)
        {
            string result = null;

            using (var ms = new MemoryStream(photoBytes))
            using (var bitMap = new Bitmap(ms))
            {
                result = GetContentType(bitMap);
            }

            return result;
        }

        public override string GetContentType(Bitmap photo)
        {
            return photo.RawFormat.GetMimeType();
        }

        public override byte[] GetPhotoBytes(object key)
        {
            var filepath = Settings.ConstrucFilePath(key.ToString());
            byte[] result = null;

            if (File.Exists(filepath))
                result = File.ReadAllBytes(filepath);

            return result;
        }
    }
}
