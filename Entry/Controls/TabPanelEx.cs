using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace Entry.Controls
{
    public class TabPanelEx:Panel,IScrollInfo
    {
        private double _maxItemHeight;
        private double _maxItemWidth;
        private int _maxVisibleItems;

        private Size _extent = new Size(0, 0);
        private Size _oldExtent = new Size(0, 0);
        private Size _viewPort=new Size(0,0);
        private Size _lastSize = new Size(0, 0);
        private Point _offset = new Point(0, 0);

        private TranslateTransform _translatorTrasform = new TranslateTransform();
        private readonly List<Rect> _childRects;

        private TabControlEx _tabControl;
        private ScrollViewer _scrollViewer;

        public TabPanelEx()
        {
            base.RenderTransform = this._translatorTrasform;
            this._childRects = new List<Rect>();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _tabControl = base.TemplatedParent as TabControlEx;
        }

        #region CLR Properties

        private Dock TabStripPlacement
        {
            get {
                if (_tabControl == null)
                    return Dock.Top;

                return _tabControl.TabStripPlacement;
            }
        }

        private double MinimumChildWidth
        {
            get {
                if (_tabControl == null)
                    return 0d;
                return _tabControl.TabItemMinWidth;
            }
        }

        private double MaximumChildWith
        { get{
            if (_tabControl == null)
                return 0d;
            return _tabControl.TabItemMaxWidth;
        } }

        private double MinimumChildHeight { get {
            if (_tabControl == null)
                return 0d;
            return _tabControl.TabItemMinHeight;
        }}

        private double MaximimChildHeight { get {
            if (_tabControl == null)
                return 0d;
            return _tabControl.TabItemMaxHeight;
        } }

        private int _firstVisibleIndex;

        public int FirstVisibleIndex
        {
            get { return _firstVisibleIndex; }
            set
            {
                if (_firstVisibleIndex == value)
                    return;

                if (value < 0)
                {
                    _firstVisibleIndex = 0;
                    return;
                }

                _firstVisibleIndex = value;
            }
        }

        private int LastVisibleIndex
        { get { return _firstVisibleIndex + _maxVisibleItems - 1; } }

        #endregion

        private void InvalidateScrollInfo()
        {
            if (_scrollViewer != null)
                _scrollViewer.InvalidateScrollInfo();
        }

        #region Dependancy Properties

        /// <summary>
        /// CanScrollLeftOrUp Dependancy Property
        /// </summary>
        [Browsable(false)]
        internal bool CanScrollLeftOrUp
        {
            get { return (bool)GetValue(CanScrollLeftOrUpProperty); }
            set { SetValue(CanScrollLeftOrUpProperty, value); }
        }
        internal static readonly DependencyProperty CanScrollLeftOrUpProperty = DependencyProperty.Register(
                                                "CanScrollLeftOrUp", 
                                                typeof(bool), 
                                                typeof(TabPanelEx), 
                                                new UIPropertyMetadata(false));

        /// <summary>
        /// CanScrollRightOrDown Dependancy Property
        /// </summary>
        [Browsable(false)]
        internal bool CanScrollRightOrDown
        {
            get { return (bool)GetValue(CanScrollRightOrDownProperty); }
            set { SetValue(CanScrollRightOrDownProperty, value); }
        }
        internal static readonly DependencyProperty CanScrollRightOrDownProperty = DependencyProperty.Register(
            "CanScrollRightOrDown", 
            typeof(bool), 
            typeof(TabPanelEx), 
            new UIPropertyMetadata(false));

        #endregion


        protected override Size MeasureOverride(Size availableSize)
        {
            this._viewPort = availableSize;

            switch (this.TabStripPlacement)
            {
                case Dock.Top:
                case Dock.Bottom:
                    return MeasureHorizontal(availableSize);
                case Dock.Left:
                case Dock.Right:
                    return MeasureVertical(availableSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //monitor changes to the scrollviewer extent value
            if (_oldExtent != _extent)
            {
                _oldExtent = _extent;
                InvalidateScrollInfo();
            }

            // monitors changes to the parent container size, (ie window resizes)
            if (finalSize != _lastSize)
            {
                _lastSize = finalSize;
                InvalidateScrollInfo();
            }

            bool isInvalidateMeasure = false;

            if (this._extent.Width <= this._viewPort.Width && _offset.X > 0)
            {
                _offset.X = 0;
                _translatorTrasform.X = 0;

                isInvalidateMeasure = true;
                InvalidateScrollInfo();
            }

            if (this._extent.Height <= this._viewPort.Height && _offset.Y > 0)
            {
                _offset.Y = 0;
                _translatorTrasform.Y = 0;

                isInvalidateMeasure = true;
                InvalidateScrollInfo();
            }

            if (isInvalidateMeasure)
                InvalidateMeasure();

            // arrange the children
            for (var i = 0; i < InternalChildren.Count; i++)
            {
                InternalChildren[i].Arrange(_childRects[i]);
            }

            if (this.InternalChildren.Count > 0)
            {
                _offset = _childRects[FirstVisibleIndex].TopLeft;
                _translatorTrasform.X = -_offset.X;
                _translatorTrasform.Y = -_offset.Y;
            }

            return finalSize;
        }

        #region Measure Horizontal

        private Size MeasureHorizontal(Size availableSize)
        {
            int childCount = base.InternalChildren.Count;
            
            double extentWidth = 0d;

            double[] widths = new double[childCount];

            for (int i = 0; i < childCount; i++)
            {
                TabItemEx tabItem = base.InternalChildren[i] as TabItemEx;

                if (tabItem == null)
                    return new Size();

                SetDimensions(tabItem);
                tabItem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                ClearDimensions(tabItem);

                //caculate the max item height recursively
                _maxItemHeight = Math.Max(_maxItemHeight, Math.Ceiling(tabItem.DesiredSize.Height));

                //caculate this tabitem's wdith while respecting the max and min width constraints
                var itemWidth = Math.Max(this.MinimumChildWidth, Math.Min(this.MaximumChildWith, Math.Ceiling(tabItem.DesiredSize.Width)));

                widths[i] = itemWidth;
                //cacualte the total horitantal width
                extentWidth += itemWidth;
            }

            //caculate the max child height finally while respecting the max and min widtt constraints
            _maxItemHeight = Math.Max(this.MinimumChildHeight, Math.Min(this.MaximimChildHeight, _maxItemHeight));
            this._extent = new Size(extentWidth, _maxItemHeight);

            if(extentWidth<=availableSize.Width)
                MeasureHorizontalFitted(childCount,widths);
            else
            {
                if (!MeasureHoriontalUniform(availableSize, childCount, widths))
                    MeasureHoriontalPageing(availableSize, childCount, widths);
            }

            return new Size(double.IsInfinity(availableSize.Width) ? this._extent.Width : availableSize.Width, _maxItemHeight);
        }

        /// <summary>
        /// measure every child items when all the items in the tabpanel can be displayed in the available space
        /// </summary>
        /// <param name="childCount"></param>
        /// <param name="widths"></param>
        private void MeasureHorizontalFitted(int childCount,double[] widths)
        {
            this._maxVisibleItems = childCount;
            this.FirstVisibleIndex = 0;
            var rects = new List<Rect>();

            double left = 0d;
            for (int i = 0; i < childCount; i++)
            {

                var width = widths[i];
                rects.Add(new Rect(left,0,width,_maxItemHeight));

                left+=width;

                FrameworkElement child = base.InternalChildren[i] as FrameworkElement;

                if(child != null)
                    child.Measure(new Size(width,_maxItemHeight));
            }
            this._childRects.Clear();
            this._childRects.AddRange(rects);

            this.CanScrollLeftOrUp = false;
            this.CanScrollRightOrDown = false;
        }

        /// <summary>
        /// measure all items when these items' size were changed to uniform size can be fitted into
        /// the avialable space.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <param name="childCount"></param>
        /// <param name="heights"></param>
        private bool MeasureHoriontalUniform(Size availableSize,int childCount,double[] widths)
        {
            //make sure the item with is not follow the max&min width constraints
            double targetWidth = Math.Min(this.MaximumChildWith, availableSize.Width / childCount);

            if (targetWidth >= this.MinimumChildWidth)
            {
                this._maxVisibleItems = childCount;
                this.FirstVisibleIndex = 0;

                CaculateItemsWidth(childCount, widths, targetWidth);

                this.CanScrollLeftOrUp = false;
                this.CanScrollRightOrDown = false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// measure all items when all items can not be fitted into the viewport
        /// </summary>
        /// <param name="availableSize"></param>
        /// <param name="childCount"></param>
        /// <param name="widths"></param>
        private void MeasureHoriontalPageing(Size availableSize, int childCount, double[] widths)
        {
            this._maxVisibleItems = (int)Math.Floor(this._viewPort.Width / this.MinimumChildWidth);
            if (this._maxVisibleItems == 0)
                this._maxVisibleItems = 1;

            double targetWidth = availableSize.Width / _maxVisibleItems;

            CaculateItemsWidth(childCount, widths, targetWidth);

            this.CanScrollLeftOrUp = LastVisibleIndex < childCount - 1;
            this.CanScrollRightOrDown = this.FirstVisibleIndex > 0;
        }

        private void CaculateItemsWidth(int childCount, double[] itemWidths,double? uniformWidth=null)
        {
            var rects = new List<Rect>();
            var extentWidth = 0d;
            var left = 0d;

            for (int i = 0; i < childCount; i++)
            {
                var width = 0d;
                if (uniformWidth.HasValue)
                {
                    width = uniformWidth.Value;
                    itemWidths[i] = uniformWidth.Value;
                }
                else
                    width = itemWidths[i];
                
                extentWidth += width;
                
                var rect = new Rect(left, 0, width, _maxItemHeight);

                left += width;

                FrameworkElement child = base.InternalChildren[i] as FrameworkElement;
                if (child != null)
                    child.Measure(new Size(width, _maxItemHeight));

                rects.Add(rect);
            }
            this._childRects.Clear();
            this._childRects.AddRange(rects);
            this._extent = new Size(extentWidth, _maxItemHeight);
        }


        #endregion

        #region Measure Vertical

        public Size MeasureVertical(Size availableSize)
        {
            int childCount = base.InternalChildren.Count;

            double extentHeight = 0d;

            double[] heights = new double[childCount];

            for (int i = 0; i < childCount; i++)
            {
                TabItemEx tabItem = base.InternalChildren[i] as TabItemEx;

                if (tabItem == null)
                    return new Size();

                SetDimensions(tabItem);
                tabItem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                ClearDimensions(tabItem);

                //caculate the max item width recursively
                _maxItemWidth = Math.Max(_maxItemWidth, Math.Ceiling(tabItem.DesiredSize.Width));

                //caculate this tabitem's wdith while respecting the max and min width constraints
                var itemHeight = Math.Max(this.MinimumChildHeight, Math.Min(this.MaximimChildHeight, Math.Ceiling(tabItem.DesiredSize.Height)));

                heights[i] = itemHeight;
                //cacualte the total horitantal width
                extentHeight += itemHeight;
            }

            //caculate the max child height finally while respecting the max and min widtt constraints
            _maxItemWidth = Math.Max(this.MinimumChildWidth, Math.Min(this.MaximumChildWith, _maxItemWidth));
            this._extent = new Size(extentHeight, _maxItemWidth);

            if (extentHeight <= availableSize.Height)
                MeasureVerticalFitted(childCount, heights);
            else
            {
                if (!MeasureVerticalUniform(availableSize, childCount, heights))
                    MeasureVerticalPageing(availableSize, childCount, heights);
            }

            return new Size(_maxItemWidth,double.IsInfinity(availableSize.Height)?_extent.Height:availableSize.Height);
        }

        /// <summary>
        /// measure every child items when all the items in the tabpanel can be displayed in the available space
        /// </summary>
        /// <param name="childCount"></param>
        /// <param name="heights"></param>
        private void MeasureVerticalFitted(int childCount, double[] heights)
        {
            this._maxVisibleItems = childCount;
            this.FirstVisibleIndex = 0;
            var rects = new List<Rect>();

            double top = 0d;
            for (int i = 0; i < childCount; i++)
            {

                var height = heights[i];
                rects.Add(new Rect(0, top, _maxItemWidth, height));

                top += height;

                FrameworkElement child = base.InternalChildren[i] as FrameworkElement;

                if (child != null)
                    child.Measure(new Size(_maxItemWidth, height));
            }
            this._childRects.Clear();
            this._childRects.AddRange(rects);

            this.CanScrollLeftOrUp = false;
            this.CanScrollRightOrDown = false;
        }

        /// <summary>
        /// measure all items when these items' size were changed to uniform size can be fitted into
        /// the avialable space.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <param name="childCount"></param>
        /// <param name="widths"></param>
        private bool MeasureVerticalUniform(Size availableSize, int childCount, double[] heights)
        {
            //make sure the item with is not follow the max&min width constraints
            double targetHeight = Math.Min(this.MaximimChildHeight, availableSize.Height / childCount);

            if (targetHeight >= this.MinimumChildHeight)
            {
                this._maxVisibleItems = childCount;
                this.FirstVisibleIndex = 0;

                CaculateItemsHeight(childCount, heights, targetHeight);

                this.CanScrollLeftOrUp = false;
                this.CanScrollRightOrDown = false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// measure all items when all items can not be fitted into the viewport
        /// </summary>
        /// <param name="availableSize"></param>
        /// <param name="childCount"></param>
        /// <param name="heights"></param>
        private void MeasureVerticalPageing(Size availableSize, int childCount, double[] heights)
        {
            this._maxVisibleItems = (int)Math.Floor(this._viewPort.Height / this.MinimumChildHeight);
            if (this._maxVisibleItems == 0)
                this._maxVisibleItems = 1;

            double targetHeight = availableSize.Height / _maxVisibleItems;

            CaculateItemsHeight(childCount, heights, targetHeight);

            this.CanScrollLeftOrUp = LastVisibleIndex < childCount - 1;
            this.CanScrollRightOrDown = this.FirstVisibleIndex > 0;
        }

        private void CaculateItemsHeight(int childCount, double[] itemHeights, double? uniformHeight = null)
        {
            var rects = new List<Rect>();
            var extentHeight = 0d;
            var top = 0d;

            for (int i = 0; i < childCount; i++)
            {
                var height = 0d;
                if (uniformHeight.HasValue)
                {
                    height = uniformHeight.Value;
                    itemHeights[i] = uniformHeight.Value;
                }
                else
                    height = itemHeights[i];

                extentHeight += height;

                var rect = new Rect(0, top, _maxItemWidth, height);

                top += height;

                FrameworkElement child = base.InternalChildren[i] as FrameworkElement;
                if (child != null)
                    child.Measure(new Size(_maxItemWidth, height));

                rects.Add(rect);
            }
            this._childRects.Clear();
            this._childRects.AddRange(rects);
            this._extent = new Size(_maxItemWidth, extentHeight);
        }

        #endregion

        private static void SetDimensions(TabItemEx tabItem)
        {
            if (tabItem.Dimension == null)
            {
                tabItem.Dimension = new Dimension
                {
                    Height = tabItem.Height,
                    Width = tabItem.Width,
                    MinHeight = tabItem.MinHeight,
                    MaxHeight = tabItem.MaxHeight,
                    MinWidth = tabItem.MinWidth,
                    MaxWidth = tabItem.MaxWidth
                };
            }
            else
            {
                tabItem.BeginInit();
                tabItem.Height = tabItem.Dimension.Height;
                tabItem.MinHeight = tabItem.Dimension.MinHeight;
                tabItem.MaxHeight = tabItem.Dimension.MaxHeight;
                tabItem.Width = tabItem.Dimension.Width;
                tabItem.MinWidth = tabItem.Dimension.MinWidth;
                tabItem.MaxWidth = tabItem.Dimension.MaxWidth;
                tabItem.EndInit();

            }
        }

        private static void ClearDimensions(FrameworkElement tabItem)
        {
            // remove any size restrictions from the Header,
            // this is because the TabControl's size restriction properties takes precedence over
            // the individual tab items
            // eg, if the TabControl sets the TabItemMaxWidth property to 300, but the Header
            // has a minWidth of 400, the TabControls value of 300 should be used
            tabItem.BeginInit();
            tabItem.Height = double.NaN;
            tabItem.Width = double.NaN;
            tabItem.MaxHeight = double.PositiveInfinity;
            tabItem.MaxWidth = double.PositiveInfinity;
            tabItem.MinHeight = 0;
            tabItem.MinWidth = 0;
            tabItem.EndInit();
        }


        #region IScrollInfo Members

        public bool CanHorizontallyScroll
        {
            get;
            set;
        }

        public bool CanVerticallyScroll
        {
            get;
            set;
        }

        public double ExtentHeight
        {
            get { return _extent.Height; }
        }

        public double ExtentWidth
        {
            get { return _extent.Width; }
        }

        public double HorizontalOffset
        {
            get { return _offset.X; }
        }

        public void LineDown()
        {
            LineRight();
        }

        public void LineLeft()
        {
            FirstVisibleIndex++;
            switch (this.TabStripPlacement)
            {
                case Dock.Left:
                case Dock.Right:
                    SetVerticalOffset(VerticalOffset + _childRects[0].Height);
                    break;
                case Dock.Top:
                case Dock.Bottom:
                    SetHorizontalOffset(HorizontalOffset + _childRects[0].Width);
                    break;
            }
        }

        public void LineRight()
        {
            FirstVisibleIndex--;
            switch (this.TabStripPlacement)
            {
                case Dock.Left:
                case Dock.Right:
                    SetVerticalOffset(VerticalOffset - _childRects[0].Height);
                    break;
                case Dock.Top:
                case Dock.Bottom:
                    SetHorizontalOffset(HorizontalOffset - _childRects[0].Width);
                    break;
            }
            
        }

        public void LineUp()
        {
            LineLeft();
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            InvalidateMeasure();
            UpdateLayout();

            if (this._tabControl == null)
                return Rect.Empty;

            int index = -1;
            int tabCounts = _tabControl.GetTabsCount();

            for (int i = 0; i < tabCounts; i++)
            {
                if (Visual.Equals(visual, _tabControl.GetTabItem(i)))
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                if (index < FirstVisibleIndex)
                    FirstVisibleIndex = index;
                else if (index > LastVisibleIndex)
                {
                    while (index > LastVisibleIndex)
                        FirstVisibleIndex++;
                }
                InvalidateArrange();
            }

            return Rect.Empty;
        }

        public void MouseWheelDown()
        {
            LineDown();
        }

        public void MouseWheelLeft()
        {
            LineLeft();
        }

        public void MouseWheelRight()
        {
            LineRight();
        }

        public void MouseWheelUp()
        {
            LineUp();
        }

        public void PageDown()
        {
            throw new NotImplementedException();
        }

        public void PageLeft()
        {
            throw new NotImplementedException();
        }

        public void PageRight()
        {
            throw new NotImplementedException();
        }

        public void PageUp()
        {
            throw new NotImplementedException();
        }

        public ScrollViewer ScrollOwner
        {
            get
            {
                return _scrollViewer;
            }
            set
            {
                _scrollViewer = value;
            }
        }

        public void SetHorizontalOffset(double offset)
        {
            if (offset < 0 || this._viewPort.Width >= this._extent.Width)
                offset = 0;
            else if (this._extent.Width - this._viewPort.Width < offset)
                offset = this._extent.Width - this._viewPort.Width;

            this._offset.X = offset;

            InvalidateScrollInfo();

            this._translatorTrasform.X = -offset;

            this.InvalidateMeasure();
        }

        public void SetVerticalOffset(double offset)
        {
            if (offset < 0 || this._viewPort.Height >= this._extent.Height)
                offset = 0;
            else if (this._extent.Height - this._viewPort.Height < offset)
                offset = this._extent.Height - this._viewPort.Height;

            this._offset.Y= offset;

            InvalidateScrollInfo();

            this._translatorTrasform.Y = -offset;

            this.InvalidateMeasure();
        }

        public double VerticalOffset
        {
            get { return _offset.Y; }
        }

        public double ViewportHeight
        {
            get { return _viewPort.Height; }
        }

        public double ViewportWidth
        {
            get { return _viewPort.Width; }
        }

        #endregion

    }
}
