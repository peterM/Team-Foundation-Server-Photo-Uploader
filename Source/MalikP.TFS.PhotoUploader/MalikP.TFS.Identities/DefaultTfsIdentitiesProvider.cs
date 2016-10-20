using MalikP.TFS.PhotoUploader.Interface;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.Identities
{
    public class DefaultTfsIdentitiesProvider : ITfsIdentitiesProvider<TfsTeamProjectCollection, TeamFoundationIdentity>
    {
        static readonly object _locker = new object();
        List<TeamFoundationIdentity> _lastIdentities;
        public virtual List<object> LastIdentities
        {
            get
            {
                return LastTfsIdentities.ToList<object>();
            }
        }

        public List<TeamFoundationIdentity> LastTfsIdentities
        {
            get
            {
                if (_lastIdentities == null)
                {
                    lock (_locker)
                    {
                        if (_lastIdentities == null)
                        {
                            _lastIdentities = new List<TeamFoundationIdentity>();
                        }
                    }
                }

                return _lastIdentities;
            }

            protected set
            {
                _lastIdentities = value;
            }
        }

        protected virtual FilteredIdentityService IdentityService { get; set; }

        public virtual List<TeamFoundationIdentity> GetTfsIdentities(string filter = null)
        {
            LastTfsIdentities = IdentityService.SearchForUsers(filter == null ? string.Empty : filter)
                                               .ToList();
            return _lastIdentities;
        }

        public virtual void Initialize(object initializationContext)
        {
            Initialize((TfsTeamProjectCollection)initializationContext);
        }

        public virtual void Initialize(TfsTeamProjectCollection initializationContext)
        {
            IdentityService = initializationContext.GetService<FilteredIdentityService>();
        }

        List<object> ITfsIdentitiesProvider.GetIdentities(string filter)
        {
            return GetTfsIdentities(filter).ToList<object>();
        }
    }
}
