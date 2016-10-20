using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface ITfsProfilePhotoChecker : IInicializable
    {
        bool HasProfilePhoto();
    }
}
