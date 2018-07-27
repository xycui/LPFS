using System;
using System.Collections.Generic;

namespace LPFS.Analyzing
{
    using Text;

    public interface ITextAnalyzer
    {
        Type AnalyzeType { get; }
        TextAnalyzeResult AnalyzeText(MetaText plainText);
    }

    public abstract class BaseTextAnalyzer<T> : ITextAnalyzer where T : AnalyzedEntity, new()
    {
        public Type AnalyzeType => typeof(T);
        public TextAnalyzeResult AnalyzeText(MetaText plainText)
        {
            var dataDict = AnalyzeInternal(plainText);
            return TextAnalyzeResult<T>.CreateByDict(dataDict);
        }

        protected abstract Dictionary<int, T> AnalyzeInternal(MetaText plainText);
    }
}
