namespace LPFS.Ssml
{
    using System.Collections.Generic;
    using Analyzing;

    public interface ISsmlOptimizer
    {
        IList<SsmlUnit> Optimize(IList<SsmlUnit> ssmlUnits, TextAnalyzeResult analyzeInfo);
    }
}
