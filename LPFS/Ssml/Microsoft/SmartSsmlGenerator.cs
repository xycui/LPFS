namespace LPFS.Ssml.Microsoft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Analyzing;
    using Processing;
    using Settings;
    using Text;

    public class SmartSsmlGenerator : ISsmlGenerator<SynthesizeParams>, ISsmlOptimizer
    {
        //todo: need to add emoji spliter
        private readonly ITextSpliter _textSpliter = ChineseTextSpliter.Instance;

        private readonly IList<ITextAnalyzer> _textAnalyzers = new List<ITextAnalyzer>
        {
            //new EmojiAnalyzer(),
            //new PhonemeAnalyzer(),
            new PuncBreakAnalyzer(BreakSettings.DefaultSettings),
            //new TokenizeAnalyzer()
        };

        private readonly IDictionary<Type, ITextAnalyzer> _analyzerDict;

        private readonly IList<Type> _analyzeResultProcessOrder = new List<Type>
        {
            typeof(PhonemeEntity),
            typeof(TokenizeEntity),
            typeof(BreakEntity)
        };

        private readonly BreakSettings _breakSettings = BreakSettings.DefaultSettings;

        public SmartSsmlGenerator()
        {
            _analyzerDict = _textAnalyzers.ToDictionary(x => x.AnalyzeType, y => y);
        }

        public SmartSsmlGenerator(SsmlGeneratorSetting setting)
        {
            _textSpliter = new AggregatedTextSpliter(new List<ITextSpliter>
            {
                SplitItemListSpliter.CreatePuncSpliter(setting.BreakSetting),
                LetterBaseTextSpliter.Instance
            });

            _textAnalyzers = new List<ITextAnalyzer>
            {
                new PuncBreakAnalyzer(setting.BreakSetting)
            };
        }

        public SmartSsmlGenerator(ITextSpliter textSpliter, IList<ITextAnalyzer> textAnalyzers)
        {
            _textSpliter = textSpliter ?? _textSpliter;
            _textAnalyzers = textAnalyzers ?? _textAnalyzers;
            _analyzerDict = _textAnalyzers.ToDictionary(x => x.AnalyzeType, y => y);
        }

        public Task<IList<SsmlUnit>> GenerateAsync(string text)
        {
            var metaText = MetaText.CreateMetaText(text, _textSpliter.SplitText);
            var analyzeResults = new List<TextAnalyzeResult>();
            //todo: parallel the analyze step
            foreach (var type in _analyzeResultProcessOrder)
            {
                if (!_analyzerDict.TryGetValue(type, out var analyzer)) continue;

                try
                {
                    analyzeResults.Add(analyzer.AnalyzeText(metaText));
                }
                catch (Exception)
                {
                    //ignore for now todo: add log here
                }
            }

            return GenerateAsync(metaText, analyzeResults);
        }

        public Task<IList<SsmlUnit>> GenerateAsync(MetaText analyzedText, IList<TextAnalyzeResult> analyzeResults)
        {
            var ssmlUnitList = CreateSsmlUnit(analyzedText);
            var analyzeResultDict = analyzeResults.ToDictionary(x => x.AnalyzeType, y => y);
            foreach (var type in _analyzeResultProcessOrder)
            {
                if (!analyzeResultDict.TryGetValue(type, out var analyzeInfo)) continue;

                ssmlUnitList = Optimize(ssmlUnitList, analyzeInfo);
            }

            return Task.FromResult(ssmlUnitList);
        }

        public async Task<SsmlDoc> GenerateAsync(string text, SynthesizeParams synthesisDesc)
        {
            var ssmlUnits = await GenerateAsync(text);
            return new MsSsmlDoc { SsmlUnits = ssmlUnits, SynthesizeDesc = synthesisDesc };
        }

        public async Task<SsmlDoc> GenerateAsync(MetaText analyzedText, IList<TextAnalyzeResult> analyzeResults, SynthesizeParams synthesisDesc)
        {
            var ssmlUnits = await GenerateAsync(analyzedText, analyzeResults);
            return new MsSsmlDoc { SsmlUnits = ssmlUnits, SynthesizeDesc = synthesisDesc };
        }

        public IList<SsmlUnit> CreateSsmlUnit(MetaText metaText)
        {
            return metaText.AtomList.Select(x =>
            {
                var ssmlText = x;//_breakSettings.BreakLvlMapping.ContainsKey(x) ? string.Empty : x;
                return new MsSsmlUnit { OriginalText = x, SsmlText = SecurityElement.Escape(ssmlText) } as SsmlUnit;
            }).ToList();
        }

        public IList<SsmlUnit> Optimize(IList<SsmlUnit> ssmlUnits, TextAnalyzeResult analyzeInfo)
        {
            for (var i = 0; i < ssmlUnits.Count; i++)
            {
                if (analyzeInfo.TryGetEntity(i, out var analyzedEntity))
                {
                    analyzedEntity.Optimize(ssmlUnits[i]);
                }
            }

            return ssmlUnits;
        }

        private IList<string> SplitDel(string rawText)
        {
            var unitList = _textSpliter.SplitText(new List<TextUnit> { new TextUnit { CanSplit = true, Text = rawText } });
            return unitList.Select(x => x.Text).ToList();
        }
    }
}
