using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Reflection;
using Library.Common.Extension;
using System.ComponentModel.Composition;

namespace Entry.Reports
{
    /// <summary>
    /// the navigation controller
    /// </summary>
    [Export]
    public class ReportsController
    {
        public ReportsController()
        {

        }

        /// <summary>
        /// get the view model by the view model name
        /// </summary>
        /// <param name="vmName"></param>
        /// <returns></returns>
        public IReportViewModel GetReportViewModel(string vmName)
        {
            var instance = IoC.GetInstance(typeof(IReportViewModel), vmName);

            if (instance == null)
                return null;

            return instance.As<IReportViewModel>();
            
        }
    }
}
