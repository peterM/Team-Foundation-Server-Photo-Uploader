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
