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

namespace Entry
{
   public partial class ContentView:UserControl
   {
       public ContentView()
        {
            InitializeComponent();
        }

       private void Items_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
       {
           if (this.Items.Visibility == System.Windows.Visibility.Collapsed)
               this.BorderThickness = new Thickness(1);
           else
               this.BorderThickness = new Thickness(0);
       }

    }
}
