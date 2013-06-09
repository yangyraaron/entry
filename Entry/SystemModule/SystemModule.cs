using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Library.Common.Extension;

namespace Entry.Infrastructure
{
    [Export(typeof(IModule))]
    internal class SystemModule:ISystemModule
    {
        private IModuleManager _moduleManager;
        private ModuleListViewModel _moduleList;
        private ContentViewModel _content;
        private IShellRegion _region;
        private NavigationHeaderViewModel _navHeader;

        [ImportingConstructor]
        internal SystemModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        private void OnCurrentModuleChanged(ModuleViewModel module)
        {
            module.Module.Initialize();
            module.Module.Startup();

            this._navHeader.Icon = module.Module.IconContent;
            this._navHeader.ModuleName = module.ModuleName;

            this._region.SetRegionContent(ShellRegions.ModuleNavigationHeaderRegion, this._navHeader);

            var message = new AppMessage(module.Module.Name, AppSystem.System_ModuleName, null);
            message.Extensions[AppSystem.Message_Key] = AppSystem.Message_Navigation;

            var navigation = module.Module.ProcessRequest(message);
            this._region.SetRegionContent(ShellRegions.ModuleNavigationRegion, navigation);
        }

        #region IModule Members

        public void Initialize(IShellRegion region)
        {
            this._moduleList = IoC.GetInstance(typeof(ModuleListViewModel), null).As<ModuleListViewModel>();
            this._moduleList.CurrentModuleChangedCallback = OnCurrentModuleChanged;
            this._content = IoC.GetInstance(typeof(ContentViewModel), null).As<ContentViewModel>(); ;
            this._navHeader = IoC.GetInstance(typeof(NavigationHeaderViewModel), null).As<NavigationHeaderViewModel>();

            this._region = region;
        }

        public void Startup()
        {
            this._region.SetRegionContent(ShellRegions.ModulesRegion, this._moduleList);
            this._region.SetRegionContent(ShellRegions.ModuleContentRegion, this._content);
        }

        public object ProcessRequest(ISystemMessage request)
        {
            var module = this._moduleManager.GetAppModule(request.SourceModule);

            if (module == null)
                return null;
            switch (request.Type)
            {
                case SystemMessageType.RenderContent:
                    this._content.ActivateItem(new ModuleContentViewModel
                    {
                        Title = request.Title,
                        ModuleName = module.Name,
                        ModuleIcon = module.IconContent,
                        ModuleContent = request.Body
                    });
                    return null;
                default:
                    return null;
            }
        }

        #endregion


        public string Name
        {
            get { return AppSystem.System_ModuleName; }
        }
    }

    public class ModuleContentViewModel
    {
        public string Title { get; set; }

        public string ModuleName { get; set; }

        public object ModuleIcon { get; set; }

        public object ModuleContent { get; set; }
    }
}
