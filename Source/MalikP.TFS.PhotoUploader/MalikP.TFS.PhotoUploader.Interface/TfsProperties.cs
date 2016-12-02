//-------------------------------------------------------------------------------------------------
// <copyright file="TfsProperties.cs" company="MalikP.">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public class TfsProperties
    {
        public string Microsoft_TeamFoundation_Identity_Image_Id { get; } = "Microsoft.TeamFoundation.Identity.Image.Id";
        public string Microsoft_TeamFoundation_Identity_Image_Type { get; } = "Microsoft.TeamFoundation.Identity.Image.Type";
        public string Microsoft_TeamFoundation_Identity_Image_Data { get; } = "Microsoft.TeamFoundation.Identity.Image.Data";
        public string Microsoft_TeamFoundation_Identity_CandidateImage_Data { get; } = "Microsoft.TeamFoundation.Identity.CandidateImage.Data";
        public string Microsoft_TeamFoundation_Identity_CandidateImage_UploadDate { get; } = "Microsoft.TeamFoundation.Identity.CandidateImage.UploadDate";

        public virtual string[] CreateProperties()
        {
            return new string[2]
                  {
                       Microsoft_TeamFoundation_Identity_Image_Id,
                       Microsoft_TeamFoundation_Identity_Image_Type
                  };
        }

        public virtual Dictionary<string, object> CreateExtendedProperties(byte[] photoBytes, string contentType, byte[] uidBytes, object data = null, object uploadedDate = null)
        {
            var result = new Dictionary<string, object>();

            result.Add(Microsoft_TeamFoundation_Identity_Image_Data, photoBytes);
            result.Add(Microsoft_TeamFoundation_Identity_Image_Type, contentType);
            result.Add(Microsoft_TeamFoundation_Identity_Image_Id, uidBytes);
            result.Add(Microsoft_TeamFoundation_Identity_CandidateImage_Data, data);
            result.Add(Microsoft_TeamFoundation_Identity_CandidateImage_UploadDate, uploadedDate);

            return result;
        }
    }
}
