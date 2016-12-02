﻿//-------------------------------------------------------------------------------------------------
// <copyright file="ITfsIdentitiesProvider.cs" company="MalikP.">
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
    public interface ITfsIdentitiesProvider : IInicializable
    {
        List<object> GetIdentities(string filter = null);
        List<Object> LastIdentities { get; }
    }

    public interface ITfsIdentitiesProvider<TInitialization, TResult> : ITfsIdentitiesProvider, IInicializable<TInitialization>
    {
        List<TResult> GetTfsIdentities(string filter = null);
        List<TResult> LastTfsIdentities { get; }
    }
}
