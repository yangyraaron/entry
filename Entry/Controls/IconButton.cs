using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace Entry.Controls
{
  public class IconButton:Button
  {

    static IconButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton),
        new FrameworkPropertyMetadata(typeof(IconButton)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
    }

    public CornerRadius Corner
    {
      get { return (CornerRadius)GetValue(CornerProperty); }
      set { SetValue(CornerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Corner.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CornerProperty =
        DependencyProperty.Register("Corner", 
        typeof(CornerRadius), 
        typeof(IconButton),
        new FrameworkPropertyMetadata(new CornerRadius(0), OnCornerChanged,CoerceCorner));


    private static void OnCornerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {

    }

    private static object CoerceCorner(DependencyObject d, object value)
    {
      CornerRadius corner = (CornerRadius)value;

      if (corner.BottomLeft < 0)
      {
        corner = new CornerRadius(0);
      }
      else if (corner.BottomLeft > 50)
      {
        corner = new CornerRadius(50);
      }

      return corner;
    }
    
  }
}
