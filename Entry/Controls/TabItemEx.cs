using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using Library.Client.Wpf;
using System.ComponentModel;

namespace Entry.Controls
{
    [TemplatePart(Name = PART_CLOSE_BUTTON, Type = typeof(ButtonBase))]
    public class TabItemEx:TabItem
    {
        private const string PART_CLOSE_BUTTON = "PART_CloseButton";
        private ButtonBase _closeButton;

        static TabItemEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItemEx),
                new FrameworkPropertyMetadata(typeof(TabItemEx)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._closeButton = GetTemplateChild(PART_CLOSE_BUTTON) as ButtonBase;

            if (this._closeButton == null)
                return;

            //attach the close handler to the close button
            this._closeButton.Click += new RoutedEventHandler(Close);
        }

        private void Close(object o,RoutedEventArgs e)
        {
            TabControlEx tabControl = WpfExtensions.FindVisualParent<TabControlEx>(this);

            if (tabControl == null)
                return;

            tabControl.RemoveItem(this);

            //after remove the item from the tabcontrol,detach the hander from the click event
            this._closeButton.Click -= new RoutedEventHandler(Close);
        }

        /// <summary>
        ///     Used by the TabPanel for sizing
        /// </summary>
        internal Dimension Dimension { get; set; }
    }

    public class TabItemExEventArgs : EventArgs
    {
        public TabItemEx TabItem { get; private set; }

        public TabItemExEventArgs(TabItemEx item)
        {
            TabItem = item;
        }
    }

    public class TabItemExCancelEventArgs : CancelEventArgs
    {
        public TabItemEx TabItem { get; private set; }

        public TabItemExCancelEventArgs(TabItemEx item)
        {
            TabItem = item;
        }
    }
}
