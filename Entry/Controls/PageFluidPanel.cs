using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using WPFSpark;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;
using Entry.Infrastructure;
using System.Windows.Data;
using Caliburn.Micro;
using Library.Client.Wpf;

namespace Entry.Controls
{
    [TemplatePart(Name = PART_PAGEBAR, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_BODY, Type = typeof(SliderPanel))]
    public class PageFluidPanel : Control
    {
        private const string PART_PAGEBAR = "PART_PAGEBAR";
        private const string PART_BODY = "PART_BODY";

        private ListBox _pageBar;
        private SliderPanel _sliderPanel;

        private int _pageSize;

        static PageFluidPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageFluidPanel),
                                                      new FrameworkPropertyMetadata(typeof(PageFluidPanel)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._pageBar = GetTemplateChild(PART_PAGEBAR) as ListBox;
            this._sliderPanel = GetTemplateChild(PART_BODY) as SliderPanel;
        }

        void _pageBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int pageIndex = (int)this._pageBar.SelectedItem;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            var maxRowsPerPage = (Int32)Math.Floor(this.ActualHeight / this.ItemHeight);
            var maxColsPerPage = (Int32)Math.Floor(this.ActualWidth / this.ItemWidth);

            this._pageSize = maxRowsPerPage * maxColsPerPage;
        }


        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth",
                                        typeof(double),
                                        typeof(PageFluidPanel),
                                        new FrameworkPropertyMetadata(0D));


        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight",
                                        typeof(double),
                                        typeof(PageFluidPanel),
                                        new FrameworkPropertyMetadata(0D));


        public IEnumerable ItemSource
        {
            get { return (IEnumerable)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource",
                                        typeof(IEnumerable),
                                        typeof(PageFluidPanel),
                                        new FrameworkPropertyMetadata(null, OnItemSourceChanged));


        static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as PageFluidPanel;

            if (d == null)
                return;

            if (e.NewValue == null && instance._pageBar != null && instance._sliderPanel != null)
            {
                instance._sliderPanel.Children.Clear();
                instance._pageBar.ItemsSource = null;
                return;
            }

            instance.CaculatePageItems();
        }


        void CaculatePageItems()
        {
            if (this.ItemSource == null)
                return;

            if (this.ItemHeight == 0D || this.ItemWidth == 0D)
                return;

            int pageCount = 0;
            IEnumerator enumerator = this.ItemSource.GetEnumerator();

            while (true)
            {
                var pageItems = GetCollectionsInPage(enumerator, this);
                if (pageItems == null)
                    break;
                else
                {
                    FluidWrapPanel pagePanel = new FluidWrapPanel
                    {
                        ItemHeight = this.ItemHeight,
                        ItemWidth = this.ItemWidth,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Orientation = Orientation.Vertical,
                        IsComposing = true,
                        ClipToBounds = false,
                        DragScale = 1.1,
                        ElementEasing = new BackEase { Amplitude = 0.35, EasingMode = EasingMode.EaseOut },
                        DragEasing = new BackEase { Amplitude = 0.65, EasingMode = EasingMode.EaseOut },
                        Background = new SolidColorBrush(Colors.Transparent)
                    };

                    pagePanel.ItemsSource = pageItems;

                    this._sliderPanel.Children.Add(pagePanel);
                    pageCount++;
                }
            }

            this._pageBar.ItemsSource = Enumerable.Range(1, pageCount);
            this._pageBar.SelectedIndex = 0;
        }



        public object ItemView
        {
            get { return (object)GetValue(ItemViewProperty); }
            set { SetValue(ItemViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemViewProperty =
            DependencyProperty.Register("ItemView", typeof(object), typeof(PageFluidPanel), new UIPropertyMetadata(0));


        ObservableCollection<UIElement> GetCollectionsInPage(IEnumerator enumerator, PageFluidPanel instance)
        {
            ObservableCollection<UIElement> collections = null;

            for (int i = 1; i <= _pageSize; i++)
            {
                if (enumerator.MoveNext())//if the current page does not have children then the collection is null.
                {
                    if (collections == null)
                        collections = new ObservableCollection<UIElement>();

                    var itemView = Activator.CreateInstance(this.ItemView.GetType()) as FrameworkElement;

                    ViewModelBinder.Bind(enumerator.Current, itemView, null);

                    //FluidMouseDragBehavior behavior = new FluidMouseDragBehavior();
                    //behavior.Attach(itemView);

                    collections.Add(itemView);
                    
                }
            }

            return collections;
        }


    }
}
