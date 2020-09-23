﻿using System;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WPSTORE.Controls
{
    [Preserve(AllMembers = true)]
    public class ParallaxListView : ListView
    {
        public ParallaxListView() : base(ListViewCachingStrategy.RetainElement)
        {
            if (Device.RuntimePlatform != Device.iOS)
            {
                this.ItemSelected += this.ParallaxListView_ItemSelected;
            }
        }

        public event EventHandler<SelectedItemChangedEventArgs> SelectionChanged;

        public event EventHandler<ScrollChangedEventArgs> ScrollChanged;

        public double WidthInPixel { get; set; }

        public static void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ((ParallaxListView)sender).ScrollChanged?.Invoke((ParallaxListView)sender, e);
        }

        public static void OnSelectionChanged(object sender, SelectedItemChangedEventArgs e)
        {
            ((ParallaxListView)sender).SelectionChanged(sender, e);
            ((ParallaxListView)sender).SelectedItem = e.SelectedItem;
            ((ParallaxListView)sender).SelectedItem = null;
        }

        public static void OnSelectionChanged(object sender, int index)
        {
            var listView = sender as ParallaxListView;
            OnSelectionChanged(sender, new SelectedItemChangedEventArgs((listView.ItemsSource as IList)[index], index));
        }

        private void ParallaxListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            OnSelectionChanged(this, e);
        }
    }

    public class ScrollChangedEventArgs : EventArgs
    {
        public ScrollChangedEventArgs(int position)
        {
            this.Position = position;
        }

        public int Position { get; set; }
    }
}