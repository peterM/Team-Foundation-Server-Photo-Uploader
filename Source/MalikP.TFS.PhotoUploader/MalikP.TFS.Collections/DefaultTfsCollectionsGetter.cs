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
