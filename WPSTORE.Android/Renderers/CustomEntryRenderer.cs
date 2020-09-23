using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using WPSTORE.Controls;
using WPSTORE.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly : ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace WPSTORE.Droid.Renderers
{
    public class CustomEntryRenderer: EntryRenderer
    {
        public CustomEntryRenderer(Context context): base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;
                var customEntry = (CustomEntry)Element;

                GradientDrawable border = new GradientDrawable();
                border.SetStroke(1, customEntry.BorderColor.ToAndroid());
                Drawable[] layers = { border };
                LayerDrawable layerDrawable = new LayerDrawable(layers);
                layerDrawable.SetLayerInset(0, -2, -2, -2, 1);
                nativeEditText.Background = layerDrawable;
            }
        }
    }
}
