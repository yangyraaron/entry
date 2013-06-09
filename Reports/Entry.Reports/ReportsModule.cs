using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entry.Infrastructure;
using System.ComponentModel.Composition;
using Library.Common.Extension;

namespace Entry.Reports
{
    [Export(typeof(IModule))]
    public partial class ReportsModule : IAppModule
    {
        [Import]
        public Lazy<ReportsNavigationViewModel> Navigation { private get; set; }
        private IResourceManager _resourceMgr;
        private string _assemlbyName;

        [ImportingConstructor]
        public ReportsModule(IResourceManager resourceManager)
        {
            this._resourceMgr = resourceManager;
            this._assemlbyName = this.GetType().Assembly.FullName;
        }

        #region IModule Members

        public string DisplayName
        {
            get { return "Reporting Service Reports"; }
        }

        public string Name
        {
            get { return "Reports"; }
        }

        public object IconContent
        {
            get { return new ReportsIcon(); }
        }

        public void Initialize()
        {
            this._resourceMgr.RegisterResource("/Themes/DataTemplates.xaml", _assemlbyName);
            this._resourceMgr.RegisterResource("/Themes/Styles.xaml", _assemlbyName);
        }

        public void Startup()
        {
            
        }

        public object ProcessRequest(IAppMessage request)
        {
            var key = request.Extensions[AppSystem.Message_Key];
            if (!key.IsNull())
            {
                if (key == AppSystem.Message_Navigation)
                    return Navigation.Value;
            }

            var report = request.Body.As<IReportViewModel>();

            if (report == null)
                return request.Body;
            else
            {
                report.Initialize();
                return report;
            }

        }

        #endregion
    }
}
