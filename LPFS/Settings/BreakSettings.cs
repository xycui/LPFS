namespace LPFS.Settings
{
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using Properties;
    using Ssml;
    using System.Collections.Generic;


    public class BreakSettings
    {
        public static BreakSettings DefaultSettings;

        static BreakSettings()
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(new MemoryStream(Resources.CharacterBreakMapping)))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                DefaultSettings = serializer.Deserialize<BreakSettings>(jsonTextReader);
            }
        }

        public Dictionary<string, BreakLevel> BreakLvlMapping;
        public Dictionary<BreakLevel, uint> BreakLvlDetail;
        public float BreakLvlFloatRate;

        public IList<string> PuncList => BreakLvlMapping.Keys
            .OrderByDescending(x => x.Length).ToList();

        public bool IsValid => BreakLvlFloatRate >= 0 && BreakLvlFloatRate < 1/* && BreakLvlDetail != null && BreakLvlDetail.Any()*/;
    }
}
