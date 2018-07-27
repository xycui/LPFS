namespace LPFS.Ssml
{
    using System.Collections.Generic;

    public abstract class SsmlDoc
    {
        public IList<SsmlUnit> SsmlUnits;

        public static implicit operator string(SsmlDoc doc)
        {
            return doc.ToString();
        }
    }
}
