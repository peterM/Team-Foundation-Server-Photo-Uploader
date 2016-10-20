﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Interface
{
    public interface ITfsIdentityManagementServiceProvider<TInitialization, TInTOut> : IInicializable<TInitialization>
    {
        TInTOut ReadExtendedProperties(TInTOut identity);
        void UpdateExtendedProperties(TInTOut identity);
    }
}
