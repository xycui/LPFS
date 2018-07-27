using System;
using System.Collections.Generic;

namespace LPFS.Analyzing
{
    using System.Linq;
    using Newtonsoft.Json;

    public abstract class TextAnalyzeResult
    {
        protected TextAnalyzeResult()
        {
        }

        public Type AnalyzeType;
        public abstract bool TryGetEntity(int loc, out AnalyzedEntity entity);
    }

    public class TextAnalyzeResult<T> : TextAnalyzeResult where T : AnalyzedEntity, new()
    {
        [JsonConstructor]
        public TextAnalyzeResult()
        {
        }

        public static TextAnalyzeResult<T> CreateByDict(IDictionary<int, T> analyzeDict)
        {
            return new TextAnalyzeResult<T>
            {
                AnalyzeType = typeof(T),
                DataDict = analyzeDict.ToDictionary(pair => pair.Key, pair => pair.Value)
            };
        }

        public Dictionary<int, T> DataDict;
        public override bool TryGetEntity(int loc, out AnalyzedEntity entity)
        {
            var success = DataDict.TryGetValue(loc, out var actualEntity);
            entity = actualEntity;
            return success;
        }
    }
}
