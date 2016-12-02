//-------------------------------------------------------------------------------------------------
// <copyright file="DefaultTfsIdentitiesProvider.cs" company="MalikP.">
//   Copyright (c) 2016-2017, Peter Malik.
//   Authors: Peter Malik (MalikP.) (peter.malik@outlook.com)
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------------------------

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
