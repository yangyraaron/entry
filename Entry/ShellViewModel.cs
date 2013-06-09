using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Entry.Infrastructure;

namespace Entry
{
    [Export]
    public class ShellViewModel : Conductor<object>, IShellRegion
    {
        private ISystemModule _systemModule;
        private IModuleManager _moduleMgr;
        [ImportingConstructor]
        public ShellViewModel(IModuleManager moduleMgr)
        {
            this.DisplayName = "Entry";
            this._moduleMgr = moduleMgr;
            this._systemModule = this._moduleMgr.SystemModule;

            this._systemModule.Initialize(this);
            this._systemModule.Startup();

        }

        private object _modules;
        public object ModulesRegion
        {
            get { return _modules; }
            set
            {
                _modules = value;
                NotifyOfPropertyChange(() => this.ModulesRegion);
            }
        }


        private object _content;
        public object ContentRegion
        {
            get { return _content; }
            set
            {
                _content = value;
                NotifyOfPropertyChange(() => ContentRegion);
            }
        }

        public object _navHeader;
        public object NavigationHeaderRegion
        {
            get { return _navHeader; }
            set { _navHeader = value;
            this.NotifyOfPropertyChange(() => this.NavigationHeaderRegion);
            }
        }

        public void SetRegionContent(string regionName, object content)
        {
            switch (regionName)
            {
                case ShellRegions.ModulesRegion:
                    this.ModulesRegion = content;
                    break;
                case ShellRegions.ModuleNavigationRegion:
                    this.ActivateItem(content);
                    break;
                case ShellRegions.ModuleNavigationHeaderRegion:
                    this.NavigationHeaderRegion = content;
                    break;
                default:
                    this.ContentRegion = content;
                    break;
            }
        }
    }

    public class ShellRegions
    {
        public const string ModulesRegion = "ModulesRegion";
        public const string ModuleNavigationRegion = "ModuleNavigatonRegion";
        public const string ModuleContentRegion = "ModuleContentRegion";
        public const string ModuleNavigationHeaderRegion = "ModuleNavigationHeaderRegion";
    }

}
