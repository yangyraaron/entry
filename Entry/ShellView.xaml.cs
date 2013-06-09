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
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using Library.Client.Wpf.Controls;

namespace Entry
{
  /// <summary>
  /// Interaction logic for ShellView.xaml
  /// </summary>
  [Export(typeof(ShellView))]
  public partial class ShellView : Window
  {
    private bool _isDraging;
    private WindowResizingAdorner _resizingAdorner;

    public ShellView()
    {
      InitializeComponent();

      MinimumButton.Click += new RoutedEventHandler(MinimumButton_Click);
      MaximumButton.Click += new RoutedEventHandler(MaximumButton_Click);
      NormalButton.Click += new RoutedEventHandler(MaximumButton_Click);
      CloseButton.Click += new RoutedEventHandler(CloseButton_Click);

      HeaderContent.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(HeaderContent_PreviewMouseLeftButtonDown);
      HeaderContent.PreviewMouseMove += new MouseEventHandler(HeaderContent_PreviewMouseMove);
      HeaderContent.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(HeaderContent_PreviewMouseLeftButtonUp);

    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        InitializeResizingAdorner();
    }

    private void InitializeResizingAdorner()
    {
        _resizingAdorner = new WindowResizingAdorner((UIElement)this.Content, this);

        var adorneLayer = AdornerLayer.GetAdornerLayer((UIElement)this.Content);
        if (adorneLayer == null)
            return;
        AdornerLayer.GetAdornerLayer((UIElement)this.Content).Add(_resizingAdorner);
    }

    void HeaderContent_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this._isDraging = false;
    }

    void HeaderContent_PreviewMouseMove(object sender, MouseEventArgs e)
    {
      if (_isDraging)
        this.DragMove();

      _isDraging = false;
    }

    void HeaderContent_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      _isDraging = true;
    }


    void CloseButton_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    void MaximumButton_Click(object sender, RoutedEventArgs e)
    {
      this.WindowState = this.WindowState == WindowState.Normal ?
        WindowState.Maximized : WindowState.Normal;

      this._resizingAdorner.Visibility = this.WindowState == WindowState.Normal ?
            Visibility.Visible:Visibility.Collapsed;
    }

    void MinimumButton_Click(object sender, RoutedEventArgs e)
    {
      this.WindowState = WindowState.Minimized;
    }
  }
}
