using System.Collections.Generic;

namespace LPFS.Processing
{
    using Text;

    public interface ITextSpliter
    {
        IList<TextUnit> SplitText(IList<TextUnit> textUnitList);
    }

    public abstract class BaseTextSpliter : ITextSpliter
    {
        public IList<TextUnit> SplitText(IList<TextUnit> textUnitList)
        {
            var list = new List<TextUnit>();
            foreach (var item in textUnitList)
            {
                if (!item.CanSplit)
                {
                    list.Add(item);
                }
                else
                {
                    list.AddRange(SplitUnit(item));
                }
            }

            return list;
        }

        protected abstract IList<TextUnit> SplitUnit(TextUnit textUnit);
    }
}
