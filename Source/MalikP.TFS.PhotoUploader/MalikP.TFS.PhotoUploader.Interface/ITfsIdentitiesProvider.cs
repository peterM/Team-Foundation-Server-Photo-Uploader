using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface ITfsIdentitiesProvider : IInicializable
    {
        List<object> GetIdentities(string filter = null);
        List<Object> LastIdentities { get; }
    }

    public interface ITfsIdentitiesProvider<TInitialization, TResult> : ITfsIdentitiesProvider, IInicializable<TInitialization>
    {
        List<TResult> GetTfsIdentities(string filter = null);
        List<TResult> LastTfsIdentities { get; }
    }
}
