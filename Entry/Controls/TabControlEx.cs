using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Collections;
using System.Windows.Data;
using System.Collections.Specialized;
using Library.Common.Extension;

namespace Entry.Controls
{
    internal class Dimension
    {
        public double Height;
        public double MaxHeight = double.PositiveInfinity;
        public double MinHeight;
        public double Width;
        public double MaxWidth = double.PositiveInfinity;
        public double MinWidth;
    }


    [TemplatePart(Name = PART_DROPDOWN,Type=typeof(ToggleButton))]
    [TemplatePart(Name = PART_REPEAT_LEFT, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_REPEAT_RIGHT, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_SCROLLVIEWER, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_TABPANEL, Type = typeof(TabPanelEx))]
    [TemplatePart(Name = PART_MENU_LIST, Type = typeof(ListBox))]
    public class TabControlEx:TabControl
    {
        private const string PART_DROPDOWN = "PART_DropDown";
        private const string PART_REPEAT_LEFT = "PART_RepeatLeft";
        private const string PART_REPEAT_RIGHT = "PART_RepeatRight";
        private const string PART_SCROLLVIEWER = "PART_ScrollViewer";
        private const string PART_TABPANEL = "PART_TabPanel";
        private const string PART_MENU_LIST = "PART_MENU_LIST";

        private ToggleButton _dropdown;
        private RepeatButton _leftNavButton;
        private RepeatButton _rightNavButton;
        private ScrollViewer _scrollerViewer;
        private TabPanelEx _tabPanel;
        private ListBox _menuItems;

        public event EventHandler<TabItemExCancelEventArgs> TabItemClosing;
        public event EventHandler<TabItemExEventArgs> TabItemClosed;

        static TabControlEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControlEx),new FrameworkPropertyMetadata(typeof(TabControlEx)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dropdown = GetTemplateChild(PART_DROPDOWN) as ToggleButton;
            _menuItems = GetTemplateChild(PART_MENU_LIST) as ListBox;
            _scrollerViewer = GetTemplateChild(PART_SCROLLVIEWER) as ScrollViewer;
            _tabPanel = GetTemplateChild(PART_TABPANEL) as TabPanelEx;

             _leftNavButton = GetTemplateChild(PART_REPEAT_LEFT) as RepeatButton;
             if (_leftNavButton != null)
                 _leftNavButton.Click += delegate{_scrollerViewer.LineLeft();};

            _rightNavButton = GetTemplateChild(PART_REPEAT_RIGHT) as RepeatButton;
            if (_rightNavButton != null)
                _rightNavButton.Click += delegate { _scrollerViewer.LineRight(); };
                
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this._menuItems == null)
                return;

            this._menuItems.Items.Clear();

            if (!this.Items.IsEmpty)
            {
                foreach (var item in this.Items)
                {
                    this._menuItems.Items.Add(item);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                TabItem tabItem = (TabItem)this.ItemContainerGenerator.ContainerFromItem(e.NewItems[e.NewItems.Count - 1]);

                if (this._tabPanel == null)
                    return;

                this._tabPanel.MakeVisible(tabItem,Rect.Empty);

                tabItem.Focus();
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItemEx();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabItemEx;
        }
        

        /// <summary>
        /// defines the Minimum width of a Header
        /// </summary>
        [DefaultValue(20.0)]
        [Category("Layout")]
        [Description("Gets or Sets the minimum Width Constraint shared by all Items in the Control, individual child elements MinWidth property will overide this property")]
        public double TabItemMinWidth
        {
            get { return (double)GetValue(TabItemMinWidthProperty); }
            set { SetValue(TabItemMinWidthProperty, value); }
        }
        public static readonly DependencyProperty TabItemMinWidthProperty = DependencyProperty.Register("TabItemMinWidth", typeof(double), typeof(TabControlEx),
            new FrameworkPropertyMetadata(20.0, new PropertyChangedCallback(OnMinMaxChanged), CoerceMinWidth));

        private static object CoerceMinWidth(DependencyObject d, object value)
        {
            TabControlEx tc = (TabControlEx)d;
            double newValue = (double)value;

            if (newValue > tc.TabItemMaxWidth)
                return tc.TabItemMaxWidth;

            return (newValue > 0 ? newValue : 0);
        }

        /// <summary>
        /// defines the Minimum height of a Header
        /// </summary>
        [DefaultValue(20.0)]
        [Category("Layout")]
        [Description("Gets or Sets the minimum Height Constraint shared by all Items in the Control, individual child elements MinHeight property will override this value")]
        public double TabItemMinHeight
        {
            get { return (double)GetValue(TabItemMinHeightProperty); }
            set { SetValue(TabItemMinHeightProperty, value); }
        }
        public static readonly DependencyProperty TabItemMinHeightProperty = DependencyProperty.Register("TabItemMinHeight", typeof(double), typeof(TabControlEx),
            new FrameworkPropertyMetadata(20.0, new PropertyChangedCallback(OnMinMaxChanged), CoerceMinHeight));

        private static object CoerceMinHeight(DependencyObject d, object value)
        {
            TabControlEx tc = (TabControlEx)d;
            double newValue = (double)value;

            if (newValue > tc.TabItemMaxHeight)
                return tc.TabItemMaxHeight;

            return (newValue > 0 ? newValue : 0);
        }

        /// <summary>
        /// defines the Maximum width of a Header
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        [Category("Layout")]
        [Description("Gets or Sets the maximum width Constraint shared by all Items in the Control, individual child elements MaxWidth property will override this value")]
        public double TabItemMaxWidth
        {
            get { return (double)GetValue(TabItemMaxWidthProperty); }
            set { SetValue(TabItemMaxWidthProperty, value); }
        }
        public static readonly DependencyProperty TabItemMaxWidthProperty = DependencyProperty.Register("TabItemMaxWidth", typeof(double), typeof(TabControlEx),
            new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(OnMinMaxChanged), CoerceMaxWidth));

        private static object CoerceMaxWidth(DependencyObject d, object value)
        {
            TabControlEx tc = (TabControlEx)d;
            double newValue = (double)value;

            if (newValue < tc.TabItemMinWidth)
                return tc.TabItemMinWidth;

            return newValue;
        }

        /// <summary>
        /// defines the Maximum width of a Header
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        [Category("Layout")]
        [Description("Gets or Sets the maximum height Constraint shared by all Items in the Control, individual child elements MaxHeight property will override this value")]
        public double TabItemMaxHeight
        {
            get { return (double)GetValue(TabItemMaxHeightProperty); }
            set { SetValue(TabItemMaxHeightProperty, value); }
        }
        public static readonly DependencyProperty TabItemMaxHeightProperty = DependencyProperty.Register("TabItemMaxHeight", typeof(double), typeof(TabControlEx),
            new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(OnMinMaxChanged), CoerceMaxHeight));

        private static object CoerceMaxHeight(DependencyObject d, object value)
        {
            TabControlEx tc = (TabControlEx)d;
            double newValue = (double)value;

            if (newValue < tc.TabItemMinHeight)
                return tc.TabItemMinHeight;

            return newValue;
        }

        /// <summary>
        /// OnMinMaxChanged callback responds to any of the Min/Max dependancy properties changing
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TabControlEx tc = (TabControlEx)d;
            if (tc.Template == null) return;

            foreach (TabItemEx child in tc.InternalChildren())
            {
                if (child != null)
                    child.Dimension = null;
            }

            if (tc._tabPanel != null)
                tc._tabPanel.InvalidateMeasure();
        }

        /// <summary>
        /// get all the internal children
        /// </summary>
        /// <returns></returns>
        internal IEnumerable InternalChildren()
        {
            IEnumerator enumerator = ((IEnumerable)this.Items).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is TabItemEx)
                    yield return enumerator.Current;
                else
                    yield return this.ItemContainerGenerator.ContainerFromItem(enumerator.Current) as TabItemEx;
            }
        }

        internal int GetTabsCount()
        {
            if (BindingOperations.IsDataBound(this, ItemsSourceProperty))
            {
                IList list = ItemsSource as IList;
                if (list != null)
                    return list.Count;

                // ItemsSource is only an IEnumerable
                int i = 0;
                IEnumerator enumerator = ItemsSource.GetEnumerator();
                while (enumerator.MoveNext())
                    i++;
                return i;
            }

            if (Items != null)
                return Items.Count;

            return 0;
        }

        /// <summary>
        /// get the tabitem by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal TabItem GetTabItem(int index)
        {
            //if the itemssource is databinding
            if (BindingOperations.IsDataBound(this, ItemsSourceProperty))
            {
                //if the itemssource is IList
                IList list = ItemsSource as IList;
                if (list != null)
                    return this.ItemContainerGenerator.ContainerFromItem(list[index]) as TabItemEx;

                // ItemsSource is at least an IEnumerable
                int i = 0;
                IEnumerator enumerator = ItemsSource.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (i == index)
                        return this.ItemContainerGenerator.ContainerFromItem(enumerator.Current) as TabItemEx;
                    i++;
                }
                return null;
            }
            return Items[index] as TabItemEx;
        }

        /// <summary>
        /// remove a tabItem
        /// </summary>
        /// <param name="tabItem"></param>
        public void RemoveItem(TabItemEx tabItem)
        {
            var c = new TabItemExCancelEventArgs(tabItem);

            TabItemClosing.SafeFire(new object[] { c });

            //if the itemssource is using
            if (this.ItemsSource != null)
            {
                IList list = this.ItemsSource as IList;

                if (list == null)
                    return;

                //get the data item
                var item = ItemContainerGenerator.ItemFromContainer(tabItem);
                if (item == null)
                    return;

                list.Remove(item);
            }
            else
                this.Items.Remove(tabItem);

            TabItemClosed.SafeFire(new object[] { c });
        }
    }
}
