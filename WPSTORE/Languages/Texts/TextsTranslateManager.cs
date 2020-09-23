using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Languages.Texts
{
    public static class TextsTranslateManager
    {
        const string ResourceId = "WPSTORE.Languages.Texts.AppResources";
        public static readonly Lazy<ResourceManager> LazyResourceManager =
            new Lazy<ResourceManager>(() =>
                new ResourceManager(ResourceId, typeof(TranslateExtension)
                    .GetTypeInfo().Assembly));

        public static string Translate(string text)
        {
            var ci = CrossMultilingual.Current.CurrentCultureInfo;
            var translation = LazyResourceManager.Value.GetString(text, ci);
            if (translation == null)
            {
                translation = text;
            }
            return translation;
        }
    }

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            return TextsTranslateManager.Translate(Text);
        }
    }
}
