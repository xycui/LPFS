namespace LPFS.Ssml
{
    using Analyzing;

    /// <summary>
    /// for microsoft SsmlUnit
    /// </summary>
    public abstract class SsmlUnit
    {
        public string OriginalText;
        public string SsmlText;

        public abstract SsmlUnit AppendBreak(int breakTimeInMs);
        public abstract SsmlUnit AppendBreak(BreakLevel breakLevel);
        public abstract SsmlUnit WrapTokenize(bool tokenizeStart, bool tokenizeEnd);
        public abstract SsmlUnit WrapPhoneme(string pinyinStr, Tone toneLevel);
    }
}
