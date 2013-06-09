using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Entry.Infrastructure;
using Library.Common.Extension;

namespace Entry
{
    [Export]
    public class ModuleListViewModel:PropertyChangedBase
    {
        private IModuleManager _moduleMgr;

        [ImportingConstructor]
        public ModuleListViewModel(IModuleManager moduleMgr)
        {
            this._moduleMgr = moduleMgr;

            InitializeModules();
        }

        private void InitializeModules()
        {
            var modules = _moduleMgr.GetAppModules();
            if (modules.Any())
                this.Modules = new ObservableCollection<ModuleViewModel>(
                    modules.Select(m => new ModuleViewModel(m) { ModuleSelectedCallback = OnModuleStartup }));
        }

        public ObservableCollection<ModuleViewModel> Modules
        {
            get;
            set;
        }

        private ModuleViewModel _currentModule;
        public ModuleViewModel CurrentModule
        {
            get
            {
                return _currentModule;
            }
            set
            {
                _currentModule = value;
                NotifyOfPropertyChange(() => this.CurrentModule);
            }
        }

        private void OnModuleStartup(ModuleViewModel currentModule)
        {
            this.CurrentModule = CurrentModule;
            CurrentModuleChangedCallback.SafeFire(new object[] { currentModule });
        }

        public Action<ModuleViewModel> CurrentModuleChangedCallback;
    }
}
