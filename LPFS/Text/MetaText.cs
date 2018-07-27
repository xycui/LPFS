using System.Collections.Generic;

namespace LPFS.Text
{
    using System.Linq;
    using Newtonsoft.Json;
    using Processing;

    public delegate ITextSpliter GetSpliterDel();

    public delegate IList<TextUnit> SplitDel(IList<TextUnit> textUnitList);

    public class MetaText
    {
        private readonly SplitDel _splitDel;
        [JsonProperty]
        private List<TextUnit> _atomList;

        [JsonConstructor]
        protected MetaText(string rawText, SplitDel splitDel)
        {
            _splitDel = splitDel;
            RawText = rawText;
            _atomList = splitDel(new List<TextUnit> { TextUnit.CreateUnit(rawText) }).ToList();
        }

        public static MetaText CreateMetaText(string rawText, GetSpliterDel splitDel)
        {
            var metaText = new MetaText(rawText, splitDel().SplitText);

            return metaText;
        }

        public static MetaText CreateMetaText(string rawText, SplitDel splitDel)
        {
            var metaText = new MetaText(rawText, splitDel);

            return metaText;
        }

        [JsonProperty]
        public string RawText { get; private set; }

        [JsonIgnore]
        public IReadOnlyList<TextUnit> AtomList
        {
            get => _atomList;
            set
            {
                var list = new List<TextUnit>(value);
                _atomList = _splitDel?.Invoke(list).ToList() ?? list;
                RawText = string.Join("", _atomList.Select(x => x.Text));
            }
        }

        public static implicit operator string(MetaText text)
        {
            return text?.RawText;
        }
    }

    //public class RichMetaText : MetaText
    //{
    //    [JsonConstructor]
    //    public RichMetaText(string rawText, SplitDel splitDel) : base(rawText, splitDel)
    //    {
    //    }

    //    public IReadOnlyList<RichTextUnit> AtomList
    //    {
    //        get => _atomList;
    //        set
    //        {
    //            var list = new List<TextUnit>(value);
    //            _atomList = _splitDel?.Invoke(list).ToList() ?? list;
    //            RawText = string.Join("", _atomList.Select(x => x.Text));
    //        }
    //    }
    //}
}
