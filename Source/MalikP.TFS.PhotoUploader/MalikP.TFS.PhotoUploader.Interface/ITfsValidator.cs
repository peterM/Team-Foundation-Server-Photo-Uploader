using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface ITfsValidator : IInicializable
    {
        void OnSuccess(ITfsProcessor processor);
        void OnFailed(ITfsProcessor processor);
        ITfsProcessor Validate();
    }
}
