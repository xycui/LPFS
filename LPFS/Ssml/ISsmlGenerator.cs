namespace LPFS.Ssml
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Analyzing;
    using Text;

    public interface ISsmlGenerator<in TParam>
    {
        Task<IList<SsmlUnit>> GenerateAsync(string text);
        Task<IList<SsmlUnit>> GenerateAsync(MetaText analyzedText, IList<TextAnalyzeResult> analyzeResults);
        Task<SsmlDoc> GenerateAsync(string text, TParam synthesisDesc);
        Task<SsmlDoc> GenerateAsync(MetaText analyzedText, IList<TextAnalyzeResult> analyzeResults,
            TParam synthesisDesc);
        IList<SsmlUnit> CreateSsmlUnit(MetaText metaText);
    }
}
