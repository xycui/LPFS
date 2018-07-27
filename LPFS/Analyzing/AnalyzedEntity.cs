namespace LPFS.Analyzing
{
    using Ssml;

    public abstract class AnalyzedEntity
    {
        public int Pos;

        public virtual SsmlUnit Optimize(SsmlUnit ssmlUnit)
        {
            return ssmlUnit;
        }
    }

    public class PhonemeEntity : AnalyzedEntity
    {
        public string PinyinStr;
        public Tone ToneLevel;

        public override SsmlUnit Optimize(SsmlUnit ssmlUnit)
        {
            return ssmlUnit.WrapPhoneme(PinyinStr, ToneLevel);
        }

        public override string ToString()
        {
            return PinyinStr + (int)ToneLevel;
        }
    }

    public enum Tone
    {
        First = 1,
        Second,
        Third,
        Fourth,
        Neutral
    }

    public class TokenizeEntity : AnalyzedEntity
    {
        public bool IsTokenizeStart;
        public bool IsTokenizeEnd;

        public override SsmlUnit Optimize(SsmlUnit ssmlUnit)
        {
            return ssmlUnit.WrapTokenize(IsTokenizeStart, IsTokenizeEnd);
        }

        public override string ToString()
        {
            string str = "";
            if (IsTokenizeStart)
            {
                str += "Start";
            }
            if (IsTokenizeEnd)
            {
                str += "End";
            }
            return str;
        }
    }

    public class BreakEntity : AnalyzedEntity
    {
        public int BreakTimeInMs;
        public BreakLevel? BreakLevel;

        public override SsmlUnit Optimize(SsmlUnit ssmlUnit)
        {
            return BreakLevel.HasValue ? ssmlUnit.AppendBreak(BreakLevel.Value) : ssmlUnit.AppendBreak(BreakTimeInMs);
        }
    }
}
