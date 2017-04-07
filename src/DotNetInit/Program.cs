using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DotNetInit
{
    public class CsProjNuGetMetadata
    {
        private readonly XDocument _xmlDocument;

        public CsProjNuGetMetadata(XDocument xmlDocument)
        {
            _xmlDocument = xmlDocument;
        }

        public string PackageId
        {
            get
            {
                var element = FindElement(nameof(PackageId));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(PackageId));
                    element.Value = value;
                }
            }
        }

        public string PackageVersion
        {
            get
            {
                var element = FindElement(nameof(PackageVersion));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(PackageVersion));
                    element.Value = value;
                }
            }
        }

        public string Authors
        {
            get
            {
                var element = FindElement(nameof(Authors));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(Authors));
                    element.Value = value;
                }
            }
        }

        public string Summary
        {
            get
            {
                var element = FindElement(nameof(Summary));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(Summary));
                    element.Value = value;
                }
            }
        }

        public string Description
        {
            get
            {
                var element = FindElement(nameof(Description));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(Description));
                    element.Value = value;
                }
            }
        }

        public string Copyright
        {
            get
            {
                var element = FindElement(nameof(Copyright));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(Copyright));
                    element.Value = value;
                }
            }
        }

        public string PackageTags
        {
            get
            {
                var element = FindElement(nameof(PackageTags));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(PackageTags));
                    element.Value = value;
                }
            }
        }

        public string PackageProjectUrl
        {
            get
            {
                var element = FindElement(nameof(PackageProjectUrl));
                return element?.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var element = FindOrCreateElement(nameof(PackageProjectUrl));
                    element.Value = value;
                }
            }
        }

        public bool GeneratePackageOnBuild
        {
            get
            {
                var element = FindElement(nameof(GeneratePackageOnBuild));
                if (!string.IsNullOrEmpty(element?.Value))
                {
                    return bool.Parse(element?.Value);
                }
                return false;
            }
            set
            {
                var element = FindOrCreateElement(nameof(GeneratePackageOnBuild));
                element.Value = value.ToString();
            }
        }

        private XElement FindElement(string elementName)
        {
            return _xmlDocument.Descendants(elementName).FirstOrDefault();
        }

        private XElement FindOrCreateElement(string elementName)
        {
            var element = FindElement(elementName);
            if (element == null)
            {
                var propertyGroup = _xmlDocument.Descendants("PropertyGroup").FirstOrDefault();
                if (propertyGroup == null)
                {
                    propertyGroup = new XElement("PropertyGroup");
                    _xmlDocument.Root.Add(propertyGroup);
                }

                element = new XElement(elementName);
                propertyGroup.Add(element);
            }
            return element;;
        }
    }

    public class CsProjFile
        : IDisposable
    {
        private readonly Stream _stream;
        private readonly bool _leaveOpen;
        private readonly XDocument _xmlDocument;

        public CsProjNuGetMetadata NuGetMetadata { get; }

        public CsProjFile(Stream stream, bool leaveOpen = false)
        {
            _stream = stream;
            _leaveOpen = leaveOpen;

            _xmlDocument = XDocument.Load(stream);

            var projectElement = _xmlDocument.Descendants("Project").FirstOrDefault();
            if (projectElement == null || projectElement.Attribute("Sdk")?.Value != "Microsoft.NET.Sdk")
            {
                throw new ArgumentException("Project file is not of the new .csproj type.");
            }

            NuGetMetadata = new CsProjNuGetMetadata(_xmlDocument);
        }

        public void Save()
        {
            _stream.SetLength(0);
            _stream.Position = 0;

            _xmlDocument.Save(_stream);
        }

        public void Dispose()
        {
            if (!_leaveOpen)
            {
                _stream?.Dispose();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This utility will walk you through initializing NuGet metadata in the current project.");
            Console.WriteLine("It only covers the most common items, and tries to guess sensible defaults.");
            Console.WriteLine();

            var currentDirectory = Directory.GetCurrentDirectory();
            var projectFile = Directory.EnumerateFiles(currentDirectory, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();

            Console.WriteLine($"Project file: {projectFile}");
            Console.WriteLine();

            Console.WriteLine($"Press ^C at any time to quit.");
            Console.WriteLine();

            using (var stream = File.Open(projectFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var csproj = new CsProjFile(stream, leaveOpen: true))
            {
                // Set values
                csproj.NuGetMetadata.PackageId = ReadFromConsole(
                    "Id", csproj.NuGetMetadata.PackageId ?? Path.GetFileNameWithoutExtension(projectFile), "MyPackage", false);

                csproj.NuGetMetadata.PackageVersion = ReadFromConsole(
                    "Version", csproj.NuGetMetadata.PackageVersion, "1.0.0", false);

                csproj.NuGetMetadata.Authors = ReadFromConsole(
                    "Authors", csproj.NuGetMetadata.Authors, null, false);

                csproj.NuGetMetadata.Summary = ReadFromConsole(
                    "Summary", csproj.NuGetMetadata.Summary, null, false);

                csproj.NuGetMetadata.Description = ReadFromConsole(
                    "Description", csproj.NuGetMetadata.Description, null, true);

                csproj.NuGetMetadata.Copyright = ReadFromConsole(
                    "Copyright", csproj.NuGetMetadata.Copyright, null, true);

                csproj.NuGetMetadata.PackageTags = ReadFromConsole(
                    "PackageTags", csproj.NuGetMetadata.PackageTags, null, true);

                csproj.NuGetMetadata.PackageProjectUrl = ReadFromConsole(
                    "PackageProjectUrl", csproj.NuGetMetadata.PackageProjectUrl, null, true);

                csproj.NuGetMetadata.GeneratePackageOnBuild = true;

                // Ask confirmation
                Console.WriteLine();
                Console.WriteLine("The following package metadata will be written:");
                Console.WriteLine($"  Id: {csproj.NuGetMetadata.PackageId}");
                Console.WriteLine($"  Version: {csproj.NuGetMetadata.PackageVersion}");
                Console.WriteLine($"  Authors: {csproj.NuGetMetadata.Authors}");
                Console.WriteLine($"  Summary: {csproj.NuGetMetadata.Summary}");
                Console.WriteLine($"  Description: {csproj.NuGetMetadata.Description}");
                Console.WriteLine($"  Copyright: {csproj.NuGetMetadata.Copyright}");
                Console.WriteLine($"  PackageTags: {csproj.NuGetMetadata.PackageTags}");
                Console.WriteLine($"  PackageProjectUrl: {csproj.NuGetMetadata.PackageProjectUrl}");
                Console.WriteLine($"  GeneratePackageOnBuild: True");

                char ok = 'x';
                do
                {
                    Console.WriteLine();
                    Console.Write("Is that OK? (y/n) ");
                    ok = (char)Console.Read();
                }
                while (ok != 'y' && ok != 'n');

                if (ok == 'y')
                {
                    csproj.Save();
                    Console.WriteLine($"Metadata was written to {projectFile}.");
                }
            }
        }

        static string ReadFromConsole(string propertyName, string currentValue, string defaultValue, bool canBeNull)
        {
            string data = null;
            do
            {
                Console.Write($"{propertyName}: ({currentValue ?? defaultValue ?? (canBeNull ? "null" : "")}) ");

                data = Console.ReadLine().Trim();

                if (!canBeNull && string.IsNullOrEmpty(data))
                {
                    data = currentValue ?? defaultValue;
                }
            }
            while (!canBeNull && string.IsNullOrEmpty(data));

            return data;
        }
    }
}