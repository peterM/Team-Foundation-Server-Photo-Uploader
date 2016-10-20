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
        protected IIoC IoC => IocLocator.Container();

        public virtual void Configure(List<Type> configurableServices)
        {
            var alreadyConfiguredServices = new List<string>();
            var applicationRootPath = GetApplicationRootPath();

            var enumerableFiles = LoadFiles();
            var files = FilterFiles(enumerableFiles).ToList();

            foreach (var serviceType in configurableServices)
            {
                WriteHeader(alreadyConfiguredServices);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Configure type: {serviceType.GetPrettyTypeName()}");

                var discoveredTypes = DiscoverServices(files, serviceType);
                if (discoveredTypes != null && discoveredTypes.Count > 0)
                {
                    var selection = 0;
                    selection = SelectFromService(discoveredTypes);

                    var selectedInstanceType = RegisterService(serviceType, discoveredTypes, selection);
                    var selectedTypeName = selectedInstanceType == null ? "NOT SELECTED OR NOT FOUND" : selectedInstanceType.Name;

                    alreadyConfiguredServices.Add($"Configuration: {serviceType.GetPrettyTypeName()} Uses: {selectedTypeName}");
                }
            }

            WriteHeader(alreadyConfiguredServices);
        }

        public virtual void WriteHeader(List<string> alreadyConfiguredServices)
        {
            Console.Clear();
            foreach (var item in alreadyConfiguredServices)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(item);
            }

            Console.WriteLine("".PadLeft(50, '#'));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected virtual List<Type> DiscoverServices(List<string> files, Type serviceType)
        {
            var discoveredTypes = new List<Type>();
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
                    discoveredTypes.AddRange(range);
            }

            return discoveredTypes;
        }

        protected virtual IEnumerable<string> FilterFiles(IEnumerable<string> files)
        {
            return files.Where(file => !Path.GetFileName(file)
                                            .StartsWith("Microsoft", StringComparison.InvariantCultureIgnoreCase) &&
                                       !Path.GetFileName(file)
                                            .StartsWith("Newtonsoft", StringComparison.InvariantCultureIgnoreCase) &&
                                       !Path.GetFileName(file)
                                            .ToUpperInvariant()
                                            .Contains("IOC") &&
                                       !Path.GetFileName(file)
                                            .ToUpperInvariant()
                                            .Contains(".VSHOST."));
        }

        protected virtual string GetApplicationRootPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        protected virtual IEnumerable<string> LoadFiles()
        {
            return Directory.EnumerateFiles(GetApplicationRootPath(), "*.*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) ||
                                        s.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase));
        }

        protected virtual Type RegisterService(Type serviceType, List<Type> discoveredTypes, int selection)
        {
            Type selectedInstanceType = null;

            if (selection >= 0 && selection < discoveredTypes.Count)
            {
                selectedInstanceType = discoveredTypes[selection];
                IoC.Register(serviceType, selectedInstanceType, Activator.CreateInstance(selectedInstanceType));
            }

            return selectedInstanceType;
        }

        protected virtual int SelectFromService(List<Type> discoveredTypes)
        {
            int selection;
            if (discoveredTypes.Count > 1)
            {
                ShowDiscoveredServices(discoveredTypes);

                selection = int.Parse(Console.ReadLine());
                selection--;
            }
            else
            {
                selection = 0;
            }

            return selection;
        }

        protected virtual void ShowDiscoveredServices(List<Type> discoveredTypes)
        {
            var iterator = 0;
            foreach (var instanceType in discoveredTypes)
            {
                iterator++;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{iterator}. {instanceType.Name}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
