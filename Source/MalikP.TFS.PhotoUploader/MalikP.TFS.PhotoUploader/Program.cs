using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Client;
using System.Diagnostics;
using MalikP.TFS.PhotoUploader.Interface;
using MalikP.TFS.PhotoUploader.Initializers;
using MalikP.IoC.Core;
using MalikP.IoC.Factory;
using MalikP.IoC.Locator;
using MalikP.TFS.PhotoUploader.Loggers;

namespace MalikP.TFS.PhotoUploader
{
    class Program
    {
        static IIoC _container => IocLocator.Container(new AdvancedContainerFactory());
        static ITfsConfiguration _tfsConfiguration => _container.Resolve<ITfsConfiguration>();
        static ITfsCollectionsGetter _tfsCollectionsGetter => _container.Resolve<ITfsCollectionsGetter>();
        static ITfsIdentitiesProvider<TfsTeamProjectCollection, TeamFoundationIdentity> _tfsIdentitiesProvider => _container.Resolve<ITfsIdentitiesProvider<TfsTeamProjectCollection, TeamFoundationIdentity>>();
        static ITfsIdentityManagementServiceProvider<TfsTeamProjectCollection, TeamFoundationIdentity> _tfsIdentityManagementServiceProvider => _container.Resolve<ITfsIdentityManagementServiceProvider<TfsTeamProjectCollection, TeamFoundationIdentity>>();
        static TfsProperties _tfsProperties => _container.Resolve<TfsProperties>();
        static IPhotoProvider _photoProvider => _container.Resolve<IPhotoProvider>();
        static ISettings _photoProviderSettings => _container.Resolve<ISettings>();
        static IProcessLogger Logger => _container.Resolve<IProcessLogger>();
        static ITfsProfilePhotoChecker _profilePhotoChecker => _container.Resolve<ITfsProfilePhotoChecker>();

        static List<Type> ConfigurableTypes { get; set; } = new List<Type>();
        static DefaultInitializer _initializer { get; }

        static Program()
        {
            IocLocator.Container(new AdvancedContainerFactory());
            _container.Register<TfsProperties>();
            _container.Register<IProcessLogger, ConsoleLogger>();

            _initializer = new DefaultInitializer();

            CreateServiceList();
        }

        private static void CreateServiceList()
        {
            ConfigurableTypes.Add(typeof(ITfsConfiguration));
            ConfigurableTypes.Add(typeof(ITfsCollectionsGetter));
            ConfigurableTypes.Add(typeof(ITfsIdentitiesProvider<TfsTeamProjectCollection, TeamFoundationIdentity>));
            ConfigurableTypes.Add(typeof(ITfsIdentityManagementServiceProvider<TfsTeamProjectCollection, TeamFoundationIdentity>));
            ConfigurableTypes.Add(typeof(IPhotoProvider));
            ConfigurableTypes.Add(typeof(ISettings));
            ConfigurableTypes.Add(typeof(ITfsProfilePhotoChecker));
        }

        static bool useExplicitCollectionId;

        static int Main(string[] args)
        {
            _initializer.Configure(ConfigurableTypes);

            var result = 0;

            try
            {
                Logger.LogInfo($"Connecting to TFS server: {_tfsConfiguration.TeamFoundationServerUrl}");
                var tfsServer = new TfsConfigurationServer(_tfsConfiguration.TeamFoundationServerUri);

                Logger.LogInfo("Ensuring is authenticated...");
                tfsServer.EnsureAuthenticated();

                Logger.LogInfo("Discovering TFS Project Collections");
                var tfsTeamCollections = _tfsCollectionsGetter.UseTfsConfigurationServer(tfsServer)
                                                              .TeamFoundationServerCollections;

                var i = 0;
                foreach (var teamCollection in tfsTeamCollections)
                {
                    i++;
                    Logger.LogInfo($"{i}. Project Collection discovered: {teamCollection.Key}");
                }

                i = int.Parse(Console.ReadLine());
                i--;

                TfsTeamProjectCollection tfsCollection = null;
                if (useExplicitCollectionId)
                {
                    tfsCollection = tfsServer.GetTeamProjectCollection(_tfsConfiguration.CollectionId);
                }
                else
                {
                    var collectionId = tfsTeamCollections[i].Value;
                    Logger.LogInfo($"Selected TFS Team Collection: {tfsTeamCollections[i].Key}");

                    tfsCollection = tfsServer.GetTeamProjectCollection(collectionId);
                }

                Logger.LogInfo($"Initializing TFS Identities provider");
                _tfsIdentitiesProvider.Initialize(tfsCollection);

                Logger.LogInfo($"Initializing TFS Identities Management provider");
                _tfsIdentityManagementServiceProvider.Initialize(tfsCollection);

                Logger.LogInfo($"Discovering TFS identities");
                var tfsIdentities = _tfsIdentitiesProvider.GetTfsIdentities();

                Logger.LogInfo($"Discovered TFS Identities: {tfsIdentities.Count}");

                _profilePhotoChecker.Initialize(_tfsProperties);

                foreach (TeamFoundationIdentity tfsIdentity in tfsIdentities)
                {
                    var identityKey = tfsIdentity.UniqueName;
                    Logger.LogInfo($"Processing TFS Identity: {identityKey}");
                    Logger.LogInfo($"Reading Extended Properties for Identity: {identityKey}");
                    var tfsIdentityExtended = _tfsIdentityManagementServiceProvider.ReadExtendedProperties(tfsIdentity);

                    Logger.LogInfo($"Initializing TFS Profile Photo Validator");

                    _profilePhotoChecker.Initialize(tfsIdentityExtended);
                    if (_profilePhotoChecker.HasProfilePhoto())
                    {
                        continue;
                    }

                    Logger.LogInfo($"Initializing Photo provider");
                    _photoProvider.Initialize(_photoProviderSettings);

                    Logger.LogInfo($"Getting Photo data for identity: {identityKey}");
                    var photoBytes = _photoProvider.GetPhotoBytes(identityKey);

                    if (photoBytes != null && photoBytes.Length > 0)
                    {
                        var contentType = _photoProvider.GetContentType(photoBytes);
                        Logger.LogInfo($"Discovereng ContentType of photo: {contentType}");

                        Logger.LogInfo($"Creating Personalized Extended Properties");
                        var extendedProperties = _tfsProperties.CreateExtendedProperties(photoBytes, contentType, Guid.NewGuid().ToByteArray());

                        foreach (var extendedProperty in extendedProperties)
                        {
                            Logger.LogInfo($"Configurring extended property: {extendedProperty.Key}");
                            tfsIdentity.SetProperty(extendedProperty.Key, extendedProperty.Value);
                        }

                        _tfsIdentityManagementServiceProvider.UpdateExtendedProperties(tfsIdentity);
                        Logger.LogInfo($"Photo uploaded successfully for: {identityKey}");
                    }
                    else
                    {
                        Logger.LogWarning($"Photo data for identity was not discovered: {identityKey}");
                    }

                    Logger.LogInfo($"Finnished for: {identityKey}");
                    Logger.LogDebug("".PadLeft(50, '#'));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                result = -1;
            }

            return result;
        }
    }
}
