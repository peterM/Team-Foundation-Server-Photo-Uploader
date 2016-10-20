using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MalikP.TFS.PhotoProvider.Settings
{
    public class ActiveDirectoryPhotoProviderSettings : PhotoProviderSettings
    {
        public virtual string Domain { get; set; }
        public virtual string LdapDomain
        {
            get
            {
                return $"LDAP://{Domain}";
            }
        }
        public virtual string PhotoAttribute { get; set; } = "thumbnailphoto";

        public override void UseDefaults()
        {
            Domain = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.ActiveDirectoryPhotoProviderSettings.{nameof(Domain)}"];
            PhotoAttribute = ConfigurationManager.AppSettings[$"MalikP.TFS.PhotoProvider.Settings.ActiveDirectoryPhotoProviderSettings.{nameof(PhotoAttribute)}"];
        }
    }
}