using System;
using System.Linq;
using System.Collections.Generic;
using MalikP.TFS.PhotoUploader.Interface;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using MalikP.TFS.PhotoProvider.Settings;

namespace MalikP.TFS.PhotoProvider
{
    public abstract class PhotoProviderBase<TSettings> : IPhotoProvider, IInicializable<TSettings> where TSettings : PhotoProviderSettings
    {
        protected virtual TSettings Settings { get; set; }

        protected PhotoProviderBase() : this(null) { }
        protected PhotoProviderBase(TSettings settings)
        {
            Settings = settings;
        }

        public abstract Bitmap GetBitmap(object key);
        public abstract string GetContentType(byte[] photoBytes);
        public abstract string GetContentType(Bitmap photo);
        public abstract byte[] GetPhotoBytes(object key);

        public virtual void Initialize(object initializationContext)
        {
            Initialize((TSettings)initializationContext);
        }

        public void Initialize(TSettings initializationContext)
        {
            Settings = initializationContext;
        }
    }
}