using MalikP.TFS.PhotoUploader.Interface;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Checker
{
    public class DefaultTfsProfilePhotoChecker : ITfsProfilePhotoChecker, IInicializable<TeamFoundationIdentity>, IInicializable<TfsProperties>
    {
        protected virtual TeamFoundationIdentity _identity { get; set; }
        protected TfsProperties _tfsProperties { get; set; }

        public bool HasProfilePhoto()
        {
            var result = false;
            object pictureData = null;
            if (_identity.TryGetProperty(IdentityPropertyScope.Both, _tfsProperties.Microsoft_TeamFoundation_Identity_Image_Id, out pictureData))
            {
                var photoBytesData = (byte[])pictureData;
                if (pictureData != null &&
                    photoBytesData != null &&
                    photoBytesData.Length == 16)
                {
                    result = true;
                }
            }

            return result;
        }

        public void Initialize(TfsProperties initializationContext)
        {
            _tfsProperties = initializationContext;
        }

        public void Initialize(TeamFoundationIdentity initializationContext)
        {
            _identity = initializationContext;
        }

        public void Initialize(object initializationContext)
        {
            if (initializationContext is TeamFoundationIdentity)
                Initialize((TeamFoundationIdentity)initializationContext);

            else if (initializationContext is TfsProperties)
                Initialize((TfsProperties)initializationContext);
        }
    }
}
