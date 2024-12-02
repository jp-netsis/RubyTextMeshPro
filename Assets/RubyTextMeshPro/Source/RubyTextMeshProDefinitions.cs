using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TMPro
{
    public static class RubyTextMeshProDefinitions
    {
        public enum RubyShowType
        {
            RUBY_ALIGNMENT,
            BASE_ALIGNMENT,
            BASE_NO_OVERRAP_RUBY_ALIGNMENT,
        }

        // ruby tag
        public static readonly Regex RUBY_REGEX = new(@"<r(uby)?=""?(?<ruby>[\s\S]*?)""?>(?<val>[\s\S]*?)<\/r(uby)?>|<ruby>(?<base>[\s\S]*?)<rt>(?<rubyText>[\s\S]*?)<\/rt><\/ruby>|<r>(?<base>[\s\S]*?)<rt>(?<rubyText>[\s\S]*?)<\/rt><\/r>");

        private static Lazy<StringBuilder> stringBuilder = new Lazy<StringBuilder>();

        public static string ReplaceRubyTags(this IRubyText targetRubyText, string str, int dir, float fontSizeScale, float hiddenSpaceW)
        {
            // Replace <ruby> tags text layout.
            MatchCollection matches = RubyTextMeshProDefinitions.RUBY_REGEX.Matches(str);
            int lastMatchIndex = 0;
            float currentTextW = 0f;
            float rubyCurrentTextW = 0f;
            StringBuilder stringBuilder = new StringBuilder();

            if (matches.Count == 0)
            {
                stringBuilder.Append(str);
            }
            else
            {
                foreach (Match match in matches)
                {
                    string baseText;
                    string rubyText;

                    if (match.Groups["base"].Success && match.Groups["rubyText"].Success)
                    {
                        baseText = match.Groups["base"].Value;
                        rubyText = match.Groups["rubyText"].Value;
                    }
                    else if (match.Groups["val"].Success && match.Groups["ruby"].Success)
                    {
                        baseText = match.Groups["val"].Value;
                        rubyText = match.Groups["ruby"].Value;
                    }
                    else
                    {
                        continue;
                    }

                    if (match.Index > lastMatchIndex)
                    {
                        string unmatchPart = str.Substring(lastMatchIndex, match.Index - lastMatchIndex);
                        float unmatchTextW = targetRubyText.GetPreferredValues(unmatchPart).x;
                        currentTextW += unmatchTextW;
                        stringBuilder.Append(unmatchPart);
                    }

                    float rubyTextW = targetRubyText.GetPreferredValues(rubyText).x * (targetRubyText.isOrthographic ? 1 : 10f) *
                                      targetRubyText.rubyScale;
                    float baseTextW = targetRubyText.GetPreferredValues(baseText).x * (targetRubyText.isOrthographic ? 1 : 10f);

                    if (targetRubyText.enableAutoSizing)
                    {
                        rubyTextW *= fontSizeScale;
                        baseTextW *= fontSizeScale;
                    }

                    string replace = targetRubyText.CreateReplaceValue(
                        dir,
                        baseText, baseTextW,
                        rubyText, rubyTextW,
                        ref currentTextW, ref rubyCurrentTextW);

                    lastMatchIndex = match.Index + match.Length;

                    stringBuilder.Append(replace);
                }

                Match lastMatch = matches[matches.Count - 1];
                stringBuilder.Append(str.Substring(lastMatch.Index + lastMatch.Length));
            }

            if (!string.IsNullOrWhiteSpace(targetRubyText.rubyLineHeight))
            {
                // warning! bad Know-how
                // line-height tag is down the next line start.
                // now line can't change, corresponding by putting a hidden ruby
                stringBuilder.Insert(0, $"<line-height={targetRubyText.rubyLineHeight}><voffset={targetRubyText.rubyVerticalOffset}><size={targetRubyText.rubyScale * 100f}%>\u00A0</size></voffset><space={hiddenSpaceW}>");
            }

            return stringBuilder.ToString();
        }

        private static string CreateReplaceValue(
            this IRubyText targetRubyText,
            int dir,
            string baseText, float baseTextW,
            string rubyText, float rubyTextW,
            ref float currentTextW, ref float rubyCurrentTextW)
        {
            float baseTextDirW = dir * baseTextW;
            float rubyTextDirW = dir * rubyTextW;
            float rubyTextOffset = -dir * (baseTextW * 0.5f + rubyTextW * 0.5f);
            float compensationOffset = dir * ((baseTextW - rubyTextW) * 0.5f);

            string replace = string.Empty;

            switch (targetRubyText.rubyShowType)
            {
                case RubyShowType.BASE_ALIGNMENT:
                    replace = targetRubyText.CreateBaseAfterRubyText(baseText, rubyText, rubyTextOffset, compensationOffset);
                    currentTextW += baseTextDirW;
                    break;

                case RubyShowType.RUBY_ALIGNMENT:
                    if (targetRubyText.isRightToLeftText)
                    {
                        if (compensationOffset < 0)
                        {
                            replace = targetRubyText.CreateBaseAfterRubyText(baseText, rubyText, rubyTextOffset, compensationOffset);
                            currentTextW += baseTextDirW;
                        }
                        else
                        {
                            replace = targetRubyText.CreateRubyAfterBaseText(baseText, rubyText, rubyTextOffset, compensationOffset);
                            currentTextW += rubyTextDirW;
                        }
                    }
                    else
                    {
                        if (compensationOffset < 0)
                        {
                            replace = targetRubyText.CreateRubyAfterBaseText(baseText, rubyText, rubyTextOffset, compensationOffset);
                            currentTextW += rubyTextDirW;
                        }
                        else
                        {
                            replace = targetRubyText.CreateBaseAfterRubyText(baseText, rubyText, rubyTextOffset, compensationOffset);
                            currentTextW += baseTextDirW;
                        }
                    }
                    break;

                case RubyShowType.BASE_NO_OVERRAP_RUBY_ALIGNMENT:
                    stringBuilder.Value.Clear();
                    float rubyCurrentTextOffsetW = currentTextW + (baseTextDirW + rubyTextOffset) - rubyCurrentTextW;

                    if (0f > rubyCurrentTextOffsetW)
                    {
                        stringBuilder.Value.Append($"<space={-rubyCurrentTextOffsetW}>");
                        rubyCurrentTextW = rubyCurrentTextW + rubyTextDirW + targetRubyText.rubyMargin;
                        currentTextW += -rubyCurrentTextOffsetW;
                    }
                    else
                    {
                        rubyCurrentTextW = currentTextW + (baseTextDirW + rubyTextOffset) + rubyTextDirW;
                    }

                    currentTextW += baseTextDirW;

                    string append = targetRubyText.CreateBaseAfterRubyText(baseText, rubyText, rubyTextOffset, compensationOffset);
                    stringBuilder.Value.Append(append);

                    replace = stringBuilder.Value.ToString();
                    break;
            }

            return replace;
        }

        public static string CreateBaseAfterRubyText(
            this IRubyText targetRubyText, string baseText, string rubyText, float rubyTextOffset, float compensationOffset)
        {
            return $"<nobr>{baseText}<space={rubyTextOffset}><voffset={targetRubyText.rubyVerticalOffset}><size={targetRubyText.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
        }

        public static string CreateRubyAfterBaseText(
            this IRubyText targetRubyText, string baseText, string rubyText, float rubyTextOffset, float compensationOffset)
        {
            return $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={targetRubyText.rubyVerticalOffset}><size={targetRubyText.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
        }
    }
}
