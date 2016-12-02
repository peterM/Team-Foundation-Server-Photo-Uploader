//-------------------------------------------------------------------------------------------------
// <copyright file="defaulttfscollectionsgetter.cs" company="MalikP.">
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

namespace MalikP.TFS.Collections
{
    public class DefaultTfsCollectionsGetter : ITfsCollectionsGetter
    {
        protected readonly object _locker = new object();
        protected TfsConfigurationServer TfsConfigurationServerInstance;
        List<KeyValuePair<string, Guid>> _teamFoundationServerCollections;

        public List<KeyValuePair<string, Guid>> TeamFoundationServerCollections
        {
            get
            {
                if (_teamFoundationServerCollections == null)
                {
                    lock (_locker)
                    {
                        if (_teamFoundationServerCollections == null)
                        {
                            var collections = GetCollections();
                            if (collections != null && collections.Count > 0)
                            {
                                _teamFoundationServerCollections = collections;
                            }
                            else
                            {
                                _teamFoundationServerCollections = new List<KeyValuePair<string, Guid>>();
                            }

                        }
                    }
                }

                return _teamFoundationServerCollections;
            }
        }
        protected ITeamProjectCollectionService TfsCollectionService { get; set; }

        public virtual void Initialize(TfsConfigurationServer tfsConfigurationServer)
        {
            _teamFoundationServerCollections = null;

            if (tfsConfigurationServer != null)
            {
                TfsConfigurationServerInstance = tfsConfigurationServer;
                TfsCollectionService = tfsConfigurationServer.GetService<ITeamProjectCollectionService>();
            }
        }

        public void Initialize(object initializationContext)
        {
            Initialize((TfsConfigurationServer)initializationContext);
        }

        public ITfsCollectionsGetter UseTfsConfigurationServer(TfsConfigurationServer tfsConfigurationServer)
        {
            Initialize(tfsConfigurationServer);
            return this;
        }

        public ITfsCollectionsGetter UseTfsConfigurationServer(object context)
        {
            return UseTfsConfigurationServer(context as TfsConfigurationServer);
        }

        protected virtual List<KeyValuePair<string, Guid>> GetCollections()
        {
            var result = new List<KeyValuePair<string, Guid>>();
            var collections = TfsCollectionService.GetCollections()
                                                  .ToList();

            if (collections != null && collections.Count > 0)
            {
                foreach (TeamProjectCollection collection in collections)
                {

                    result.Add(new KeyValuePair<string, Guid>(collection.Name, collection.Id));
                }
            }

            return result;
        }
    }
}
