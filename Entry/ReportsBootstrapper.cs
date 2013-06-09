using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.IO;
using Library.Common.Extension;
using System.Security.Policy;
using Entry.Infrastructure;

namespace Entry
{
  public class ReportsBootstrapper:Bootstrapper<ShellViewModel>
  {
      CompositionContainer _container;

      protected override void Configure()
      {
          ConfigureModules();
          ConfigureEntry();

          _container = new CompositionContainer(new AggregateCatalog(
            AssemblySource.Instance.Select(x => new AssemblyCatalog(x))
                          .OfType<ComposablePartCatalog>()));

          var batch = new CompositionBatch();

          batch.AddExportedValue<IWindowManager>(new WindowManager());
          batch.AddExportedValue<IEventAggregator>(new EventAggregator());
          batch.AddExportedValue<IResourceManager>(new ResourceManager());
          batch.AddExportedValue(_container);

          _container.Compose(batch);

          RegisterResource();

      }

      protected override object GetInstance(Type service, string key)
      {
          string contact = string.IsNullOrEmpty(key) ?
            AttributedModelServices.GetContractName(service) : key;

          var exports = _container.GetExportedValues<object>(contact);

          if (exports.Any())
              return exports.FirstOrDefault();

          throw new Exception(string.Format("Could not locate any instance of contract {0}", contact));
      }

      protected override IEnumerable<object> GetAllInstances(Type service)
      {
          return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
      }

      protected override void BuildUp(object instance)
      {
          _container.SatisfyImportsOnce(instance);
      }

      private void ConfigureModules()
      {
          string baseUri = Directory.GetCurrentDirectory();

          string baseModulesUri = string.Format("{0}/{1}", baseUri, "Modules");

          if (!Directory.Exists(baseModulesUri))
              return ;

          var directories = Directory.GetDirectories(baseModulesUri);

          if (directories.IsNullOrEmpty())
              return ;

          var assemblies = new List<Assembly>();

          foreach (var directory in directories)
          {
              var dlls = Directory.GetFiles(directory, "*.dll");

              if (dlls.IsNullOrEmpty())
                  continue;

              assemblies.AddRange(dlls.Select(s=>{return Assembly.LoadFile(s);}));
          }

          AssemblySource.Instance.AddRange(assemblies);
          
      }

      private void ConfigureEntry()
      {
          AssemblySource.Instance.Add(typeof(ModuleManager).Assembly);
      }

      private void RegisterResource()
      {
          var resourceManager = this.GetInstance(typeof(IResourceManager), null).As<IResourceManager>();
          if (resourceManager.IsNull())
              return;

          resourceManager.RegisterResource("Themes/Styles.xaml", "Entry");
          resourceManager.RegisterResource("Themes/Styles.xaml");

      }
  }
}
