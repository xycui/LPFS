namespace LPFS.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Settings;
    using Text;

    public class LetterBaseTextSpliter : BaseTextSpliter
    {
        public static ITextSpliter Instance = new LetterBaseTextSpliter();

        private LetterBaseTextSpliter()
        {
        }

        protected override IList<TextUnit> SplitUnit(TextUnit textUnit)
        {
            return textUnit.Text.Select(x => new TextUnit { CanSplit = false, Text = x.ToString() }).ToList();
        }
    }

    public class SplitItemListSpliter : BaseTextSpliter
    {
        public static ITextSpliter CreatePuncSpliter(BreakSettings breakSettings = null)
        {
            return new SplitItemListSpliter((breakSettings ?? BreakSettings.DefaultSettings).PuncList);
        }

        [Obsolete("do not use before the data is prepared", true)]
        public static ITextSpliter CreateEmojiSpliter()
        {
            throw new NotImplementedException();
        }

        private readonly IReadOnlyList<string> _splitItems;

        public SplitItemListSpliter(IList<string> splitItems)
        {
            if (splitItems != null && splitItems.Any())
            {
                _splitItems = splitItems.OrderByDescending(x => x.Length).ToList();
            }
        }

        protected override IList<TextUnit> SplitUnit(TextUnit textUnit)
        {
            var foundIndex = FindLocInStr(textUnit.Text, _splitItems, out var foundItem);
            if (textUnit.CanSplit == false || foundIndex == -1)
            {
                return new List<TextUnit> { textUnit };
            }

            var retList = new List<TextUnit>();
            var firstPart = textUnit.Text.Substring(0, foundIndex);
            var lastPartStart = foundIndex + foundItem.Length;
            var lastPart = textUnit.Text.Substring(lastPartStart, textUnit.Text.Length - lastPartStart);

            if (!string.IsNullOrEmpty(firstPart))
            {
                retList.AddRange(SplitUnit(TextUnit.CreateUnit(firstPart)));
            }
            retList.Add(TextUnit.CreateUnit(foundItem, false));
            if (!string.IsNullOrEmpty(lastPart))
            {
                retList.AddRange(SplitUnit(TextUnit.CreateUnit(lastPart)));
            }

            return retList;
        }

        private static int FindLocInStr(string text, IEnumerable<string> candidates, out string foundItem)
        {
            var index = -1;
            foundItem = null;
            foreach (var item in candidates)
            {
                index = text.IndexOf(item, StringComparison.Ordinal);
                if (index == -1) continue;

                foundItem = item;
                break;
            }

            return index;
        }
    }

    public class AggregatedTextSpliter : ITextSpliter
    {
        private readonly IList<ITextSpliter> _textSpliters;

        public AggregatedTextSpliter(IList<ITextSpliter> textSpliterList)
        {
            _textSpliters = textSpliterList;
        }

        public IList<TextUnit> SplitText(IList<TextUnit> textUnitList)
        {
            return _textSpliters.Aggregate(textUnitList, (current, spliter) => spliter.SplitText(current));
        }
    }
}
