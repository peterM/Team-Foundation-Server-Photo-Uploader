//-------------------------------------------------------------------------------------------------
// <copyright file="DefaultTfsConfiguration.cs" company="MalikP.">
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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.Configuration
{
    public class DefaultTfsConfiguration : ITfsConfiguration
    {
        public DefaultTfsConfiguration()
        {
            InitializeDefaults();
        }

        public virtual void InitializeDefaults()
        {
            // TODO: check automatic way to initialize from config
            WithTeamFoundationServerUrl(ConfigurationManager.AppSettings["TFS-Server-URL"]);
            WithCollectionId(ConfigurationManager.AppSettings["TFS-Collection-ID"]);
        }

        public DefaultTfsConfiguration WithCollectionId(Guid collectionId)
        {
            CollectionId = collectionId;
            return this;
        }

        public DefaultTfsConfiguration WithCollectionId(string guid)
        {
            Guid uid;
            if (Guid.TryParse(guid, out uid))
            {
                WithCollectionId(uid);
            }

            return this;
        }

        public DefaultTfsConfiguration WithTeamFoundationServerUrl(string url)
        {
            TeamFoundationServerUrl = url;
            return this;
        }

        Guid _collectionId;
        public virtual Guid CollectionId
        {
            get
            {
                return _collectionId;
            }

            protected set
            {
                if (_collectionId != value)
                {
                    _collectionId = value;
                }
            }
        }

        string _teamFoundationServerUrl;
        public virtual string TeamFoundationServerUrl
        {
            get
            {
                return _teamFoundationServerUrl;
            }

            protected set
            {
                if (_teamFoundationServerUrl != value)
                {
                    _teamFoundationServerUrl = value;
                }
            }
        }
        public virtual Uri TeamFoundationServerUri
        {
            get
            {
                return new Uri(TeamFoundationServerUrl);
            }
        }
    }
}
