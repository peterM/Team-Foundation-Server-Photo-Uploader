using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface IPhotoProvider : IInicializable
    {
        byte[] GetPhotoBytes(object key);
        Bitmap GetBitmap(object key);
        string GetContentType(Bitmap photo);
        string GetContentType(byte[] photoBytes);
    }
}
