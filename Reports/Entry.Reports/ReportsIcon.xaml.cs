using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Entry.Infrastructure;

namespace Entry.Reports
{
    /// <summary>
    /// Interaction logic for ReportsIcon.xaml
    /// </summary>
    public partial class ReportsIcon : UserControl
    {
        public ReportsIcon()
        {
            InitializeComponent();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            var width = sizeInfo.NewSize.Width / 3;
            var height = sizeInfo.NewSize.Height;

            this.Path.Width = width;
            this.Path.Height = height;

            this.Path_0.Width = width;
            this.Path_0.Height = height;

            this.Path_1.Width = width;
            this.Path_1.Height = height;

            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}
