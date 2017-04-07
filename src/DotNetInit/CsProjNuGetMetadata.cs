using System.Linq;
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
}