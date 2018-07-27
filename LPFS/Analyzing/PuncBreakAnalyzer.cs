namespace LPFS.Analyzing
{
    using System;
    using System.Collections.Generic;
    using Settings;
    using Text;

    public class PuncBreakAnalyzer : BaseTextAnalyzer<BreakEntity>
    {
        private readonly BreakSettings _breakSettings;

        public PuncBreakAnalyzer(BreakSettings breakSettings)
        {
            _breakSettings = breakSettings ?? BreakSettings.DefaultSettings;
            if (!_breakSettings.IsValid)
            {
                throw new ArgumentException($"{nameof(breakSettings)} is not valid!");
            }
        }

        protected override Dictionary<int, BreakEntity> AnalyzeInternal(MetaText plainText)
        {
            var dataDict = new Dictionary<int, BreakEntity>();

            for (var i = 0; i < plainText.AtomList.Count; i++)
            {
                if (!_breakSettings.BreakLvlMapping.TryGetValue(plainText.AtomList[i], out var breakLevel)) continue;
                if (_breakSettings.BreakLvlDetail == null)
                {
                    dataDict[i] = new BreakEntity { BreakLevel = breakLevel };
                }
                else
                {
                    var breakTime = _breakSettings.BreakLvlDetail[breakLevel];
                    var floatRange = new Tuple<float, float>(breakTime * (1 - _breakSettings.BreakLvlFloatRate),
                        breakTime * (1 + _breakSettings.BreakLvlFloatRate));

                    var breakTimeInMs = new Random().Next((int)floatRange.Item1, (int)floatRange.Item2);
                    dataDict[i] = new BreakEntity { BreakTimeInMs = breakTimeInMs };
                }
            }

            return dataDict;
        }
    }
}
