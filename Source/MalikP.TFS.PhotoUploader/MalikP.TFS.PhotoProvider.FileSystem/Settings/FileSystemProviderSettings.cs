//-------------------------------------------------------------------------------------------------
// <copyright file="FileSystemProviderSettings.cs" company="MalikP.">
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
using System.Configuration;
using System.IO;

namespace MalikP.TFS.PhotoProvider.Settings
{
    public class FileSystemProviderSettings : PhotoProviderSettings
    {
        public string FileExtension { get; set; }
        public string FilePrefix { get; set; }
        public string FileSuffix { get; set; }
        public string RootFolder { get; set; }

        public virtual string ConstrucFilePath(string body)
        {
            return Path.Combine(RootFolder, $"{FilePrefix}{body}{FileSuffix}{FileExtension}");
        }

        public override void UseDefaults()
        {
            FileExtension = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.FileSystemProviderSettings.{nameof(FileExtension)}"];
            FilePrefix = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.FileSystemProviderSettings.{nameof(FilePrefix)}"];
            FileSuffix = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.FileSystemProviderSettings.{nameof(FileSuffix)}"];
            RootFolder = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.FileSystemProviderSettings.{nameof(RootFolder)}"];
        }
    }
}