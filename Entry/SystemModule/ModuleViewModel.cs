using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Entry.Infrastructure;
using Library.Common.Extension;

namespace Entry
{
    public class ModuleViewModel : PropertyChangedBase
    {
        public ModuleViewModel(IAppModule module)
        {
            this.Module = module;
        }

        public IAppModule Module { get; set; }

        public object Icon { get { return this.Module.IconContent; } }

        public string ModuleName { get { return this.Module.DisplayName; } }

        public void Selected()
        {
            ModuleSelectedCallback.SafeFire(new object[] { this });
        }

        public Action<ModuleViewModel> ModuleSelectedCallback;

    }
}
