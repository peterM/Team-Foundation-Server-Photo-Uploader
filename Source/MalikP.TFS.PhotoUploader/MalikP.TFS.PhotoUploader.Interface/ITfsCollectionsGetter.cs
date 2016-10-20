using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface ITfsCollectionsGetter : IInicializable
    {
        List<KeyValuePair<string, Guid>> TeamFoundationServerCollections { get; }
        ITfsCollectionsGetter UseTfsConfigurationServer(object context);
    }
}
