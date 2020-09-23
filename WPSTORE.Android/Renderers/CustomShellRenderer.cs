using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using WPSTORE;
using WPSTORE.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AppShell), typeof(CustomShellRenderer))]
namespace WPSTORE.Droid.Renderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        bool _disposed;
        public CustomShellRenderer(Context context) : base(context)
        {
        }

        //  Background image for toolbar
        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new CustomToolbarAppearanceTracker();
        }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new CustomBottomNavAppearance();
        }

        protected override void OnElementSet(Shell element)
        {
            base.OnElementSet(element);
        }

        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            var renderer = base.CreateShellSectionRenderer(shellSection);
            return (IShellSectionRenderer)renderer;
        }

        protected override IShellFlyoutRenderer CreateShellFlyoutRenderer()
        {
            var flyout = base.CreateShellFlyoutRenderer();

            return flyout;
        }
        protected override IShellFlyoutContentRenderer CreateShellFlyoutContentRenderer()
        {

            var flyout = base.CreateShellFlyoutContentRenderer();

            try
            {
                GradientDrawable gradient = new GradientDrawable(
                    GradientDrawable.Orientation.LeftRight,
                    new Int32[]
                    {
                        ((Color) App.Current.Resources["FlyoutGradientStart"]).ToAndroid(),
                        ((Color) App.Current.Resources["FlyoutGradientEnd"]).ToAndroid()

                    }
                );

                var cl = ((CoordinatorLayout)flyout.AndroidView);
                cl.SetBackground(gradient);

                var g = (AppBarLayout)cl.GetChildAt(0);
                g.SetBackgroundColor(Color.Transparent.ToAndroid());
                g.OutlineProvider = null;

                var header = g.GetChildAt(0);
                header.SetBackgroundColor(Color.Transparent.ToAndroid());

            }
            catch (Exception e)
            {

            }

            return flyout;
        }
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing && Element != null)
            {
                Element.PropertyChanged -= OnElementPropertyChanged;
                Element.SizeChanged -=
                    (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, "OnElementSizeChanged"); // OnElementSizeChanged is private, so use reflection
            }

            _disposed = true;
        }
    }
    // Custom toolbar appearance
    public class CustomToolbarAppearanceTracker : IShellToolbarAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker)
        {

        }

        public void SetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            toolbar.SetBackgroundResource(Resource.Drawable.nav_background);
        }
    }

    // Custom bottom tab
    public class CustomBottomNavAppearance : IShellBottomNavViewAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(BottomNavigationView bottomView)
        {

        }
        //public void SetAppearance(BottomNavigationView bottomView, ShellAppearance appearance)
        //{
        //    // Hide title for bottom nav items
        //    // https://montemagno.com/xamarin-forms-fully-customize-bottom-tabs-on-android-turn-off-shifting/
        //    bottomView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityLabeled;


        //    IMenu myMenu = bottomView.Menu;

        //    IMenuItem myItemOne = myMenu.GetItem(0);

        //    //if (myItemOne.IsChecked)
        //    //{
        //    //    myItemOne.SetIcon(Resource.Drawable.icon_home);
        //    //}
        //    //else
        //    //{
        //    //    myItemOne.SetIcon(Resource.Drawable.icon_home);
        //    //}

        //    //The same logic if you have myItemTwo, myItemThree....

        //}

        public void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            bottomView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityLabeled;


            IMenu myMenu = bottomView.Menu;

            IMenuItem myItemOne = myMenu.GetItem(0);
        }
    }
}