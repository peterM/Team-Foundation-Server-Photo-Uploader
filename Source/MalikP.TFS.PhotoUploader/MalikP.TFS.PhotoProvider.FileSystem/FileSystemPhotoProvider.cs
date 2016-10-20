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
