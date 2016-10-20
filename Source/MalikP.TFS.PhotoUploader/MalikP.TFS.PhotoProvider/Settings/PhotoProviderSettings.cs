using System;
using System.Linq;
using System.Collections.Generic;
using MalikP.TFS.PhotoUploader.Interface;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoProvider.Settings
{
    public abstract class PhotoProviderSettings : ISettings
    {
        protected PhotoProviderSettings()
        {
            UseDefaults();
        }

        public virtual ISettings Instance
        {
            get
            {
                return this;
            }
        }

        public abstract void UseDefaults();
    }
}