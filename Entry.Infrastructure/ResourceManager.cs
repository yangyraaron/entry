using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;
using Library.Common.Extension;

namespace Entry.Infrastructure
{
    [Export(typeof(IResourceManager))]
    public class ResourceManager : IResourceManager
    {
        private const string AssemblyResourceUriFormat = "/{0};Component/{1}";

        public static Uri MakeResourceUri(string resourcePath,string projectName = null)
        {
            string path = string.Empty;
            if (!projectName.IsNullOrEmpty())
                path = string.Format(AssemblyResourceUriFormat, projectName, resourcePath);
            else
                path = string.Format(AssemblyResourceUriFormat,
                    typeof(ResourceManager).Assembly.GetName().Name,
                    resourcePath);

            return new Uri(path,UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// register a resource to the application
        /// </summary>
        /// <param name="resourcePath">resource path</param>
        /// <param name="projectName">the assembly name</param>
        public void RegisterResource(string resourcePath,string projectName=null)
        {
            ResourceDictionary rd = new ResourceDictionary { Source = ResourceManager.MakeResourceUri(resourcePath, projectName) };

            if (!Application.Current.Resources.MergedDictionaries.Contains(rd))
                Application.Current.Resources.MergedDictionaries.Add(rd);
        }
    }
}
