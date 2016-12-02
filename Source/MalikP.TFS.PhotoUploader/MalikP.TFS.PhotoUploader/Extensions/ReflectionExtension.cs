//-------------------------------------------------------------------------------------------------
// <copyright file="reflectionextension.cs" company="MalikP.">
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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Client;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using MalikP.TFS.PhotoUploader.Interface;
using MalikP.TFS.Configuration;
using MalikP.TFS.Collections;
using MalikP.TFS.Identities;
using MalikP.TFS.Identities.Management;
using MalikP.TFS.PhotoProvider;
using MalikP.TFS.PhotoProvider.Settings;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace MalikP.TFS.PhotoUploader
{

    public static class ReflectionExtension
    {
        public static string GetOriginalName(this Type type)
        {
            var typeName = type.FullName
                               .Replace(type.Namespace + ".", "");

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var reference = new CodeTypeReference(typeName);

            return provider.GetTypeOutput(reference);
        }

        public static string GetPrettyTypeName(this Type item)
        {
            var originalTypeName = item.GetOriginalName();

            if (originalTypeName.Contains('<') && originalTypeName.Contains('>'))
            {
                var data = originalTypeName.Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                if (data != null && data.Length == 2)
                {
                    var names = data[1].Split(',');
                    foreach (var name in names)
                    {
                        var finalNname = name.Split('.').Last();
                        originalTypeName = originalTypeName.Replace(name, finalNname);
                    }
                }
            }

            return originalTypeName;
        }
    }

}