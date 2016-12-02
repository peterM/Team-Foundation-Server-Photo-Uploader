//-------------------------------------------------------------------------------------------------
// <copyright file="DefaultIdentityManagementServiceProvider.cs" company="MalikP.">
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
