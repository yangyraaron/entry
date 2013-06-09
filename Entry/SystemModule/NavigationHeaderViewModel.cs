using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Entry
{
    [Export]
   public class NavigationHeaderViewModel:PropertyChangedBase
    {
        private object _icon;
        public object Icon {
            get { return _icon; }
            set {
                _icon = value;
                this.NotifyOfPropertyChange(() => this.Icon);
            }
        }

        private string _moduleName;
        public string ModuleName
        {
            get { return _moduleName; }
            set{
                _moduleName = value;
                this.NotifyOfPropertyChange(() => this.ModuleName);
            }
        }
    }
}
