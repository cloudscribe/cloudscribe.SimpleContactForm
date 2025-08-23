using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.SimpleContactForm.CoreIntegration
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(CoreTenantResolver).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("fs5r3f82-5n38-4c83-93c8-cc4c303209ed"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}