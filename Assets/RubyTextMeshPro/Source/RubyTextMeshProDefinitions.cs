using System.Text.RegularExpressions;

namespace TMPro
{
    public static class RubyTextMeshProDefinitions
    {
        public enum RubyShowType
        {
            RUBY_ALIGNMENT,
            BASE_ALIGNMENT
        }

        // ruby tag
        public static readonly Regex RUBY_REGEX = new Regex(@"<r(uby)?=""?(?<ruby>[\s\S]*?)""?>(?<val>[\s\S]*?)<\/r(uby)?>");
        
        public static string ReplaceRubyTags(this IRubyText targetRubyText, string str, int dir, float fontSizeScale, float hiddenSpaceW)
        {
            // Replace <ruby> tags text layout.
            MatchCollection matches = RubyTextMeshProDefinitions.RUBY_REGEX.Matches(str);

            foreach (Match match in matches)
            {
                if (match.Groups.Count != 5)
                {
                    continue;
                }

                string fullMatch = match.Groups[0].ToString();
                string rubyText = match.Groups["ruby"].ToString();
                string baseText = match.Groups["val"].ToString();

                float rubyTextW = targetRubyText.GetPreferredValues(rubyText).x * (targetRubyText.isOrthographic ? 1 : 10f) * targetRubyText.rubyScale;
                float baseTextW = targetRubyText.GetPreferredValues(baseText).x * (targetRubyText.isOrthographic ? 1 : 10f);

                if (targetRubyText.enableAutoSizing)
                {
                    rubyTextW *= fontSizeScale;
                    baseTextW *= fontSizeScale;
                }

                float rubyTextOffset = dir * (baseTextW / 2f + rubyTextW / 2f);
                float compensationOffset = -dir * ((baseTextW - rubyTextW) / 2f);
                string replace = targetRubyText.CreateReplaceValue(
                    baseText, 
                    rubyText, rubyTextOffset, 
                    compensationOffset);
                str = str.Replace(fullMatch, replace);
            }

            if (!string.IsNullOrWhiteSpace(targetRubyText.rubyLineHeight))
                // warning! bad Know-how
                // line-height tag is down the next line start.
                // now line can't change, corresponding by putting a hidden ruby
            {
                str = $"<line-height={targetRubyText.rubyLineHeight}><voffset={targetRubyText.rubyVerticalOffset}><size={targetRubyText.rubyScale * 100f}%>\u00A0</size></voffset><space={hiddenSpaceW}>" + str;
            }

            return str;
        }

        private static string CreateReplaceValue(
            this IRubyText targetRubyText,
            string baseText, string rubyText, float rubyTextOffset, float compensationOffset)
        {
            string replace = string.Empty;

            switch (targetRubyText.rubyShowType)
            {
                case RubyShowType.BASE_ALIGNMENT:
                    replace = targetRubyText.CreateBaseAfterRubyText(baseText, rubyTextOffset, rubyText, compensationOffset);
                    break;

                case RubyShowType.RUBY_ALIGNMENT:
                    if (targetRubyText.isRightToLeftText)
                    {
                        replace = compensationOffset < 0 ?
                            targetRubyText.CreateBaseAfterRubyText(baseText, rubyTextOffset, rubyText, compensationOffset):
                            targetRubyText.CreateRubyAfterBaseText(baseText, rubyTextOffset, rubyText, compensationOffset);
                    }
                    else
                    {
                        replace = compensationOffset < 0 ?
                            targetRubyText.CreateRubyAfterBaseText(baseText, rubyTextOffset, rubyText, compensationOffset):
                            targetRubyText.CreateBaseAfterRubyText(baseText, rubyTextOffset, rubyText, compensationOffset);
                    }

                    break;
            }

            return replace;
        }

        public static string CreateBaseAfterRubyText(
            this IRubyText targetRubyText, string baseText,float rubyTextOffset, string rubyText, float compensationOffset)
        {
            return
                $"<nobr>{baseText}<space={rubyTextOffset}><voffset={targetRubyText.rubyVerticalOffset}><size={targetRubyText.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
        }

        public static string CreateRubyAfterBaseText(
            this IRubyText targetRubyText, string baseText,float rubyTextOffset, string rubyText, float compensationOffset)
        {
            return
                $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={targetRubyText.rubyVerticalOffset}><size={targetRubyText.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
        }


    }
}