using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Conventions;
using System.Configuration;

namespace psmonSearch.extconf
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            string rootPath = ConfigurationManager.AppSettings.Get(@"WebPublicDir");            
            return rootPath;
        }
    }

    public class CustomBoostrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);
            var rootPath = RootPathProvider.GetRootPath();
            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("", rootPath + @"\public")
            );
        }
    }

}
