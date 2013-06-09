using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Entry.Controls
{
  public class SliderPanel : Panel
  {
    private Point MouseStart, MouseNow, MouseFirst, MouseFinal;


    public int Counter
    {
      get { return (int)GetValue(CounterProperty); }
      set { SetValue(CounterProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Counter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CounterProperty =
        DependencyProperty.Register("Counter", typeof(int), typeof(SliderPanel), new UIPropertyMetadata(0));

    

    public SliderPanel()
    {
      this.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(SliderPanel_MouseLeftButtonDown);
      this.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(SliderPanel_MouseLeftButtonUp);
    }

    void SliderPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      this.CaptureMouse();

      this.MouseStart = e.GetPosition(this);
      this.MouseNow = this.MouseStart;
      this.MouseFirst = this.MouseStart;

      this.MouseMove += new System.Windows.Input.MouseEventHandler(SliderPanel_MouseMove);
    }

    void SliderPanel_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
      this.MouseNow = e.GetPosition(this);

      for (int i = 0; i < Children.Count; i++)
      {
        TranslateTransform yu = new TranslateTransform(Children[i].RenderTransform.Value.OffsetX + (MouseNow.X - MouseStart.X), 0);
        Children[i].RenderTransform = yu;
      }

      MouseStart = MouseNow;
    }

    public int SelectedIndex
    {
      get { return (int)GetValue(SelectedIndexProperty); }
      set { SetValue(SelectedIndexProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register("SelectedIndex",
                                    typeof(int),
                                    typeof(SliderPanel),
                                    new FrameworkPropertyMetadata(-1, (d, e) => {
                                      int index = (int) e.NewValue;
                                      if (index == -1)
                                        return;

                                      var instance = d as SliderPanel;
                                      if (d == null)
                                        return;

                                      instance.AnimateBySelectedIndex(index);
                                    }));

    

    void AnimateBySelectedIndex(int index)
    {
      if (index < 0 || index > this.Children.Count - 1 || index * -1 == Counter)
        return;

      index *= -1;
      double pTo, pFrom;
      pTo = index * this.DesiredSize.Width;
      pFrom = index > Counter ? (pTo - this.DesiredSize.Width) : (pTo + this.DesiredSize.Width);

      Counter = index;

      for (int i = 0; i < Children.Count; i++)
      {
        DoubleAnimation da = new DoubleAnimation(pFrom, pTo, new Duration(TimeSpan.FromSeconds(0.3)));

        TranslateTransform yu = new TranslateTransform(Children[i].RenderTransform.Value.OffsetX, 0);
        Children[i].RenderTransform = yu;

        ((TranslateTransform)Children[i].RenderTransform).BeginAnimation(TranslateTransform.XProperty, da);
      }
    }

    void SliderPanel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      try
      {
        this.MouseMove -= new System.Windows.Input.MouseEventHandler(SliderPanel_MouseMove);

        this.MouseFinal = e.GetPosition(this);
        this.ReleaseMouseCapture();

        if ((MouseFinal.X - MouseFirst.X) > 0)
        {
          if (Math.Abs(MouseFinal.X - MouseFirst.X) > 50)
            Counter = Counter + 1;
        }
        else
        {
          if (Math.Abs(MouseFinal.X - MouseFirst.X) > 50)
            Counter = Counter - 1;
        }

        double pTo, pFrom;
        pTo = Counter * this.DesiredSize.Width;
        pFrom = (MouseFinal.X - MouseFirst.X) > 0 ? (pTo - this.DesiredSize.Width) + (MouseFinal.X - MouseFirst.X) : (pTo + this.DesiredSize.Width) + (MouseFinal.X - MouseFirst.X);

        if (Math.Abs(MouseFinal.X - MouseFirst.X) < 50)
          pFrom = pTo + (MouseFinal.X - MouseFirst.X);

        if (Counter > 0)
        {
          pTo = (Counter - 1) * this.DesiredSize.Width;
          Counter = Counter - 1;
        }
        else if (Counter <= Children.Count * -1)
        {
          pTo = (Counter + 1) * this.DesiredSize.Width;
          Counter = Counter + 1;
        }

        for (int i = 0; i < Children.Count; i++)
        {
          DoubleAnimation da = new DoubleAnimation(pFrom, pTo, new Duration(TimeSpan.FromSeconds(0.3)));

          ((TranslateTransform)Children[i].RenderTransform).BeginAnimation(TranslateTransform.XProperty, da);
        }

        SelectedIndex = Math.Abs(Counter);
      }
      catch { }
    }

    protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
    {
      Size resultSize = new Size(0, 0);

      foreach (UIElement child in this.Children)
      {
        child.Measure(availableSize);
        resultSize.Width = Math.Max(resultSize.Width, this.DesiredSize.Width);
        resultSize.Height = Math.Max(resultSize.Height, child.DesiredSize.Height);
      }

      resultSize.Width =
          double.IsPositiveInfinity(availableSize.Width) ?
          resultSize.Width : availableSize.Width;

      resultSize.Height =
          double.IsPositiveInfinity(availableSize.Height) ?
          resultSize.Height : availableSize.Height;

      return resultSize;
    }

    protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
    {
      for (int i = 0; i < Children.Count; i++)
      {
        Children[i].Arrange(new Rect((i * finalSize.Width), (double)0, finalSize.Width, finalSize.Height));
      }

      this.Clip = new RectangleGeometry(new Rect(0, 0, this.DesiredSize.Width, this.DesiredSize.Height));

      return base.ArrangeOverride(finalSize);
    }
  }
}
