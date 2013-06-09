using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Caliburn.Micro;
using Library.Common.Extension;
using Entry.Infrastructure;

namespace Entry.Reports
{
    [Export]
    public class ReportsNavigationViewModel
    {
        private MessageManager _msgMgr;
        private ReportsController _controller;
        private int _index = 0;

        [ImportingConstructor]
        public ReportsNavigationViewModel(MessageManager msgMgr, ReportsController controller)
        {
            this._msgMgr = msgMgr;
            this._controller = controller;

            this.Reports = new ObservableCollection<NavigationNodeViewModel>()
            {
                new NavigationNodeViewModel(){ DisplayName="QC Reports", 
                    ReportViewModels = new  ObservableCollection<NavigationNodeViewModel>{ 
                     new NavigationNodeViewModel{DisplayName = "Product/Lot Quality",Name="QCTest"}
                    }
                }
            };

        }

        public ObservableCollection<NavigationNodeViewModel> Reports { get; set; }

        public void Navigate(object report)
        {
            if (report.IsNull())
                return;
            NavigationNodeViewModel vm = report.As<NavigationNodeViewModel>();
            if (vm.IsNull())
                return;

            IReportViewModel viewModel = this._controller.GetReportViewModel(vm.Name);

            if (viewModel == null)
                return;

            viewModel.Initialize();

            this._msgMgr.SendSystemMessage(new SysteMessage(vm.DisplayName+_index.ToString(), "Reports", viewModel, SystemMessageType.RenderContent));
            _index++;
        }
    }


    public class NavigationNodeViewModel
    {
        public NavigationNodeViewModel()
        {
        }
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public ObservableCollection<NavigationNodeViewModel> ReportViewModels { get; set; }

    }

}
