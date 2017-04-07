using System;
using System.IO;
using System.Linq;

namespace DotNetInit
{
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