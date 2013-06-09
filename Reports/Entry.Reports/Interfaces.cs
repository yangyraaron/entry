using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entry.Reports
{
    public interface IReportViewModel
    {
        string DisplayName { get; }
        void Initialize();
    }

    public interface IReportName
    {
        string ReportName { get; }
    }
    
}
