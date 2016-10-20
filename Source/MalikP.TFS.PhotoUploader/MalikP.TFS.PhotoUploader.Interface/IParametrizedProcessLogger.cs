using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface IParametrizedProcessLogger<TParameter> : IInicializable
    {
        void Log(string message, TParameter parameter);
    }
}