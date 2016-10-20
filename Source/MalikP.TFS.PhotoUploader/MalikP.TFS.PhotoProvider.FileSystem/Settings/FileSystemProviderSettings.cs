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