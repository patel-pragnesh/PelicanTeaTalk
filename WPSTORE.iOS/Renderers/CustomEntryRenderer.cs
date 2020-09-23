using System;
using CoreAnimation;
using UIKit;
using WPSTORE.Controls;
using WPSTORE.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace WPSTORE.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control != null && e.NewElement != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                e.NewElement.SizeChanged += NewElement_SizeChanged;
            }    
        }

        private void NewElement_SizeChanged(object sender, EventArgs e)
        {
            var view = (sender as CustomEntry);
            if(view != null)
            {
                DrawBorder(view);
            }
        }
        private void DrawBorder(CustomEntry customEntry)
        {
            var borderLayer = new CALayer();
            borderLayer.MasksToBounds = true;
            borderLayer.Frame = new CoreGraphics.CGRect (0f, customEntry.Height - 0.8f, customEntry.Width, 1.0f);
            borderLayer.BorderColor = customEntry.BorderColor.ToCGColor ();
            borderLayer.BorderWidth = 1.0f;

            Control.Layer.AddSublayer(borderLayer);
            Control.Layer.MasksToBounds = true;
        }
    }
}
