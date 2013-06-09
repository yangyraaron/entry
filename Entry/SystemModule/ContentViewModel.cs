using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace Entry
{
    [Export]
    public class ContentViewModel:Conductor<object>.Collection.OneActive
    {
        public void CloseItem(object moduleContent)
        {
            this.DeactivateItem(moduleContent, true);
            SetIsEmpty();
        }

        public override void ActivateItem(object item)
        {
            base.ActivateItem(item);
            SetIsEmpty();
        }

        void SetIsEmpty()
        {

            if (this.Items.Any())
                this.IsEmpty = true;
            else
                this.IsEmpty = false;
        }

        private bool _IsEmpty;
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            set
            {
                if (_IsEmpty == value)
                    return;
                _IsEmpty = value;
                this.NotifyOfPropertyChange(() => IsEmpty);
            }
        }    
                
    }
}
