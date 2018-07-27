namespace LPFS.Processing
{
    using System.Collections.Generic;

    public class ChineseTextSpliter : AggregatedTextSpliter
    {
        private static readonly IList<ITextSpliter> TextSpliter = new List<ITextSpliter>
        {
            SplitItemListSpliter.CreatePuncSpliter(),
            LetterBaseTextSpliter.Instance
        };

        public static ITextSpliter Instance = new ChineseTextSpliter();

        private ChineseTextSpliter() : base(TextSpliter)
        {
        }

        internal ChineseTextSpliter(IList<ITextSpliter> textSpliterList) : base(textSpliterList)
        {
        }
    }
}
