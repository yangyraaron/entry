using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entry.Reports.Model;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using System.ComponentModel;
using System.Windows.Data;

namespace Entry.Reports
{
    [Export("QCTest",typeof(IReportViewModel))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public class QCTestViewModel:PropertyChangedBase, IReportViewModel
    {
        private WarehouseModel _model;

        public QCTestViewModel()
        {
            _model = LazyInitializer.EnsureInitialized<WarehouseModel>(ref _model, () => new WarehouseModel());
        }

        #region IReportViewModel Members

        public string DisplayName
        {
            get
            {
                return "Product/Lot Quality";
            }
        }

        public void Initialize()
        {
            this.Warehouses = new ObservableCollection<Warehouse>(_model.GetWarehouses());

            var list = _model.GetWarehouses().ToList();
            list.Insert(0,new Warehouse { Division = null, Name = "(All)" });

            var view = CollectionViewSource.GetDefaultView(list);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Division"));

            this.View = view;
        }

        #endregion

        private ObservableCollection<Warehouse> _warehouses;
        public ObservableCollection<Warehouse> Warehouses
        {
            get
            {
                return _warehouses;
            }
            set { _warehouses = value; }
        }

        private Warehouse _selectedWarehouse;
        public Warehouse SelectedWarehouse
        {
            get { return _selectedWarehouse; }
            set
            {
                _selectedWarehouse = value;
            this.NotifyOfPropertyChange(() => SelectedWarehouse);
            }
        }

        private string _product;
        public string Product
        {
            get { return _product; }
            set { _product = value;
            this.NotifyOfPropertyChange(() => Product);
            }
        }
        private ICollectionView _view;
        public ICollectionView View
        {
            get { return _view; }
            set { _view = value;
            this.NotifyOfPropertyChange(() => View);
            }
        }

        public Warehouse _selectedGroup;
        public Warehouse SelectedGroup
        {
            get { return _selectedGroup; }
            set { 
                _selectedGroup = value;
            this.NotifyOfPropertyChange(() => this.SelectedGroup);
            }
        }
    }
}
