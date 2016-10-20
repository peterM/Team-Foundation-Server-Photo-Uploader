using MalikP.TFS.PhotoUploader.Interface;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.Identities.Management
{
    public class DefaultIdentityManagementServiceProvider : ITfsIdentityManagementServiceProvider<TfsTeamProjectCollection, TeamFoundationIdentity>
    {
        static readonly object _locker = new object();
        protected virtual IIdentityManagementService2 TfsIdentityManagementService { get; set; }

        TfsProperties _tfsProperties;
        protected virtual TfsProperties TfsProperties
        {
            get
            {
                if (_tfsProperties == null)
                {
                    lock (_locker)
                    {
                        if (_tfsProperties == null)
                        {
                            _tfsProperties = new TfsProperties();
                        }
                    }
                }
                return _tfsProperties;
            }
        }
        public void Initialize(TfsTeamProjectCollection initializationContext)
        {
            TfsIdentityManagementService = initializationContext.GetService<IIdentityManagementService2>();
        }

        public virtual void Initialize(object initializationContext)
        {
            Initialize((TfsTeamProjectCollection)initializationContext);
        }

        public TeamFoundationIdentity ReadExtendedProperties(TeamFoundationIdentity identity)
        {
            return TfsIdentityManagementService.ReadIdentity(IdentitySearchFactor.AccountName,
                                                              identity.UniqueName,
                                                              MembershipQuery.Direct,
                                                              ReadIdentityOptions.ExtendedProperties,
                                                              TfsProperties.CreateProperties(),
                                                              IdentityPropertyScope.Both);
        }

        public void UpdateExtendedProperties(TeamFoundationIdentity identity)
        {
            TfsIdentityManagementService.UpdateExtendedProperties(identity);
        }
    }
}
