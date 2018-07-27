namespace LPFS.Ssml
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class SynthesizeParamAttribute : Attribute
    {
        public SynthesizeParamAttribute(int order, object defaultValue)
        {
            Order = order;
            DefaultValue = defaultValue;
        }

        public object DefaultValue { get; }
        public int Order { get; private set; }
    }

    public class SynthesizeParams
    {
        private const float MinRate = 0.001f;
        private float _rate;

        public SynthesizeParams()
        {
            foreach (var prop in typeof(SynthesizeParams).GetTypeInfo().DeclaredProperties)
            {
                var attrs = prop.GetCustomAttributes(typeof(SynthesizeParamAttribute), false);
                var setter = prop.SetMethod;
                if (!attrs.Any() || setter == null) continue;
                if (attrs.First() is SynthesizeParamAttribute attr) setter.Invoke(this, new[] { attr.DefaultValue });
            }
        }

        public SynthesizeParams(SynthesizeParams param)
        {
            VoiceFont = param.VoiceFont;
            Rate = param.Rate;
            Emotion = param.Emotion;
            Gender = param.Gender;
            Language = param.Language;
        }

        [SynthesizeParam(0, null)]
        public string VoiceFont { get; set; }

        [SynthesizeParam(1, 1.0f)]
        public float Rate
        {
            get => _rate;
            set => _rate = value < MinRate ? MinRate : value;
        }

        public string RateString
        {
            get
            {
                var div = Rate - 1;
                return $"{(div >= 0 ? "+" : string.Empty)}{div.ToString("P").Replace(" ", "")}";
            }
        }

        [SynthesizeParam(2, Gender.Female)]
        public Gender Gender { get; set; }

        [SynthesizeParam(3, "zh-CN")]
        public string Language { get; set; }
        [SynthesizeParam(4, Emotion.Neutral)]
        public Emotion Emotion { get; set; }
        [SynthesizeParam(5, EchoScene.Normal)]
        public EchoScene EchoScene { get; set; }
    }

    public enum Emotion
    {
        Neutral,
        Sad,
        Happy,
        Angry
    }

    public enum EchoScene
    {
        Normal,
        Room,
        Hall,
        Bathroom
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
