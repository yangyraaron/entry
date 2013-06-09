using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Library.Common.Extension;

namespace Entry.Infrastructure
{
    [Export(typeof(IModuleManager))]
    public class ModuleManager : IModuleManager
    {
        //private Lazy<IModule>[] _modules;

        [ImportMany(AllowRecomposition = true)]
        public Lazy<IModule>[] LazyModules
        {
            private get;
            set;
        }

        /// <summary>
        /// get all the modules
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAppModule> GetAppModules()
        {
            if (LazyModules.IsNullOrEmpty())
                return Enumerable.Empty<IAppModule>();

            return LazyModules.Select(m => m.Value).OfType<IAppModule>();
        }

        /// <summary>
        /// to indicate if there is a module which name is <para>moduleName</para> 
        /// </summary>
        /// <param name="moduleName">the module's name</param>
        /// <returns></returns>
        public bool HasModule(string moduleName)
        {
            if (this.LazyModules.IsNullOrEmpty())
                return false;
            return this.LazyModules.Any(m => m.Value.Name == moduleName);
        }

        /// <summary>
        /// get a module by its name
        /// </summary>
        /// <param name="moduleName">the module's name</param>
        /// <returns>return null if there isn't a module its name is <para>moduleName</para></returns>
        public IAppModule GetAppModule(string moduleName)
        {
            var module = this.GetModule(moduleName);
            return module == null ? null : module.As<IAppModule>();
        }

        private IModule GetModule(string moduleName)
        {
            if (this.LazyModules.IsNullOrEmpty())
                return null;
            var module = this.LazyModules.FirstOrDefault(m => m.Value.Name == moduleName);
            return module == null ? null : module.Value;
        }

        private ISystemModule _systemModule;
        public ISystemModule SystemModule
        {
            get {
                if (_systemModule.IsNull())
                {
                    var module = this.GetModule(AppSystem.System_ModuleName);
                    if (module == null)
                        throw new Exception("the system module can not be null!");

                    _systemModule = module.As<ISystemModule>();
                }
                return _systemModule;
            }
        }
    }
}
