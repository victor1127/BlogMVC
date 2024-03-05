using System.ComponentModel;

namespace BlogMVC.Enums
{
    public enum ModeratedReason
    {
        [Description("Rasist comment")]
        Rasist,
        [Description("Political propaganda")]
        Political,
        [Description("Inapropiate language")]
        Language,
        [Description("Threatening speech")]
        Threatening,
        [Description("Sexual content")]
        Sexual,
        [Description("Hate speech")]
        HateSpeech,
        [Description("Targeted shaming")]
        Shaming
    }
}
