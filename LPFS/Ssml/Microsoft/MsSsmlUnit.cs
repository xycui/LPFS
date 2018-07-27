namespace LPFS.Ssml.Microsoft
{
    using System.Collections.Generic;
    using Analyzing;
    using Ssml;

    public class MsSsmlUnit : SsmlUnit
    {
        private static readonly IReadOnlyDictionary<BreakLevel, string> BreakLvlStrengthStrDict =
              new Dictionary<BreakLevel, string>
              {
                {BreakLevel.None, "none"},
                {BreakLevel.Weak, "weak"},
                {BreakLevel.XWeak, "x-weak"},
                {BreakLevel.Medium, "medium"},
                {BreakLevel.Strong, "strong"},
                {BreakLevel.XStrong, "x-strong"}
              };

        private const string BreakTemplate = "<break time=\"{0}ms\"/>";
        private const string StrengthBreakTemplate = "<break strength=\"{0}\"/>";
        private const string TokenizeStartTag = "<token>";
        private const string TokenizeEndTag = "</token>";
        private const string PhonemeTemplate = "<phoneme alphabet=\"x-microsoft-sapi\" ph=\"{1} {2}\">{0}</phoneme>";

        public MsSsmlUnit()
        {
        }

        public static MsSsmlUnit Create(string text)
        {
            return new MsSsmlUnit { SsmlText = text };
        }

        public override SsmlUnit AppendBreak(int breakTimeInMs)
        {
            SsmlText += string.Format(BreakTemplate, breakTimeInMs);
            return this;
        }

        public override SsmlUnit AppendBreak(BreakLevel breakLevel)
        {
            SsmlText += string.Format(StrengthBreakTemplate, BreakLvlStrengthStrDict[breakLevel]);
            return this;
        }

        public override SsmlUnit WrapTokenize(bool tokenizeStart, bool tokenizeEnd)
        {
            if (tokenizeStart)
            {
                SsmlText = TokenizeStartTag + SsmlText;
            }
            if (tokenizeEnd)
            {
                SsmlText = SsmlText + TokenizeEndTag;
            }

            return this;
        }

        public override SsmlUnit WrapPhoneme(string pinyinStr, Tone toneLevel)
        {
            SsmlText = string.Format(PhonemeTemplate, SsmlText, pinyinStr, (int)toneLevel);

            return this;
        }
    }
}
