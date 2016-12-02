//-------------------------------------------------------------------------------------------------
// <copyright file="DefaultTfsProfilePhotoChecker.cs" company="MalikP.">
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
