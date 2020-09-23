using Newtonsoft.Json;

namespace WPSTORE.Models.WooCommerce
{
    public class PaymentMethodModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Order { get; set; }
        public bool Enabled { get; set; }

        [JsonProperty("method_title")]
        public string MethodTitle { get; set; }

        [JsonProperty("method_description")]
        public string MethodDescription { get; set; }

        [JsonProperty("method_supports")]
        public string[] MethodSupports { get; set; }

        public PaymentMethodSettings Settings { get; set; }

        public class PaymentMethodSettings
        {
            public PaymentSettingTitle Title { get; set; }
            public Instructions Instructions { get; set; }
        }
        public class BasePaymentMethodSetting
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public string Default { get; set; }
            public string Tip { get; set; }
            public string Placeholder { get; set; }
        }
        public class PaymentSettingTitle : BasePaymentMethodSetting
        {
        }

        public class Instructions : BasePaymentMethodSetting
        {
        }
    }
}
