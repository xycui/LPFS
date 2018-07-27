namespace LPFS.Ssml.Microsoft
{
    using System;
    using System.Linq;

    public class MsSsmlDoc : SsmlDoc
    {
        private const string SsmlTemplate =
            "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xmlns:emo=\"http://www.w3.org/2009/10/emotionml\" xml:lang=\"{0}\"><voice name=\"{1}\">{5}<emo:emotion><emo:category name=\"{3}\" value=\"1.0\" /></emo:emotion><prosody rate=\"{2}\">{4}</prosody></voice></speak>";
        private const string EchoSettingTemplate = "<mstts:echosetting scene=\"{0}\"/>";

        public SynthesizeParams SynthesizeDesc;

        public override string ToString()
        {
            var innerSsml = string.Join(string.Empty, SsmlUnits.Select(x => x.SsmlText));
            var echoSetting = SynthesizeDesc.EchoScene == EchoScene.Normal ? string.Empty : string.Format(EchoSettingTemplate, Enum.GetName(typeof(EchoScene), SynthesizeDesc.EchoScene));

            return string.Format(SsmlTemplate, SynthesizeDesc.Language, SynthesizeDesc.VoiceFont, SynthesizeDesc.RateString, SynthesizeDesc.Emotion.ToString().ToLower(), innerSsml, echoSetting);
        }
    }
}
