namespace LPFS.Text
{
    using Analyzing;

    public class TextUnit
    {
        public static TextUnit CreateUnit(string text, bool canSplit = true)
        {
            return new TextUnit { CanSplit = canSplit, Text = text };
        }

        public string Text;
        public bool CanSplit;

        public static implicit operator string(TextUnit unit)
        {
            return unit.Text;
        }
    }

    public class RichTextUnit : TextUnit
    {
        public PhonemeEntity PhonemeEntity;
        public TokenizeEntity TokenizeEntity;
        public BreakEntity BreakEntity;
    }
}
