using MalikP.IoC.Core;
using MalikP.IoC.Locator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Initializers
{
    public class DefaultInitializer
    {
        protected static IIoC IoC => IocLocator.Container();

        public static void WriteHeader(List<string> items)
        {
            Console.Clear();
            foreach (var item in items)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(item);
            }

            Console.WriteLine("############################");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Configure(List<Type> configurableTypes)
        {
            var list = new List<string>();

            var path = Assembly.GetExecutingAssembly().Location;
            var files = Directory.EnumerateFiles(Path.GetDirectoryName(path), "*.dll");
            files = files.Where(file => !Path.GetFileName(file)
                                             .StartsWith("Microsoft", StringComparison.InvariantCultureIgnoreCase) &&
                                        !Path.GetFileName(file)
                                             .StartsWith("Newtonsoft", StringComparison.InvariantCultureIgnoreCase))
                         .ToList();

            foreach (var serviceType in configurableTypes)
            {
                DefaultInitializer.WriteHeader(list);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Configure type: {serviceType.GetPrettyTypeName()}");

                var searchedTypes = new List<Type>();
                foreach (var file in files)
                {
                    var assembly = Assembly.LoadFrom(file);
                    var assemblyTypes = assembly.GetTypes()
                                                .ToList();

                    var range = assemblyTypes.Where(d => !d.IsInterface &&
                                                         !d.IsAbstract &&
                                                         serviceType != d &&
                                                         serviceType.IsAssignableFrom(d))
                                             .ToList();

                    if (range != null && range.Count > 0)
                        searchedTypes.AddRange(range);
                }

                if (searchedTypes != null && searchedTypes.Count > 0)
                {
                    var i = 0;
                    foreach (var searchedType in searchedTypes)
                    {
                        i++;
                        Console.ForegroundColor = ConsoleColor.Blue;

                        Console.WriteLine($"{i}. {searchedType.Name}");

                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    var selection = int.Parse(Console.ReadLine());
                    selection--;

                    Type instanceType = null;
                    if (selection >= 0 && selection < searchedTypes.Count)
                    {
                        instanceType = searchedTypes[selection];
                        IoC.Register(serviceType, instanceType, Activator.CreateInstance(instanceType));
                    }

                    var selectedTypeName = instanceType == null ? "NOT SELECTED OR NOT FOUND" : instanceType.Name;

                    list.Add($"Configuration: {serviceType.GetPrettyTypeName()} Uses: {selectedTypeName}");
                }
            }

            DefaultInitializer.WriteHeader(list);
        }
    }
}
