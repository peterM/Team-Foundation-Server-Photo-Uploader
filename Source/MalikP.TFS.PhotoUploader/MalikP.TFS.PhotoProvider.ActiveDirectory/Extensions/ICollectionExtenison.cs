using System;
using System.Linq;
using System.Collections.Generic;
using MalikP.TFS.PhotoProvider.Settings;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.IO;
using System.Collections;

namespace MalikP.TFS.PhotoProvider
{
    public static class ICollectionExtenison
    {
        public static List<T> ToList<T>(this ICollection source)
        {
            var result = new List<T>();
            foreach (var item in source)
            {
                result.Add((T)item);
            }

            return result;
        }
    }
}