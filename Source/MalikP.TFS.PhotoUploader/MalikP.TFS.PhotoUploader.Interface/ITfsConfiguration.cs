using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface ITfsConfiguration
    {
        string TeamFoundationServerUrl { get; }
        Uri TeamFoundationServerUri { get; }
        Guid CollectionId { get; }
    }
}
