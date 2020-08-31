using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TMPro
{
    public class RubyTextMeshPro : TextMeshPro
    {
        protected enum RubyShowType
        {
            RUBY_ALIGNMENT,
            BASE_ALIGNMENT
        }

        // ruby tag
        private static readonly Regex RubyRegex = new Regex(@"<r(uby)?=""?(?<ruby>[\s\S]*?)""?>(?<val>[\s\S]*?)<\/r(uby)?>");

        [Tooltip("v offset ruby. (em, px, %).")]
        [SerializeField] private string rubyVerticalOffset = "1em";

        [Tooltip("ruby scale. (1=100%)")]
        [SerializeField] private float rubyScale = 0.5f;

        [Tooltip("ruby show type.")]
        [SerializeField] private RubyShowType rubyShowType = RubyShowType.RUBY_ALIGNMENT;

        [Tooltip("all v compensation ruby.")]
        [SerializeField] private bool allVCompensationRuby = false;
        [Tooltip("all ruby v compensation. (em, px, %).")]
        [SerializeField] private string allVCompensationRubyLineHeight = "1.945em";

        [SerializeField]
        [TextArea(5, 10)]
        private string m_uneditedText;

        public string UnditedText
        {
            set { m_uneditedText = value; SetTextCustom(m_uneditedText); }
        }

        private void SetTextCustom(string value)
        {
            text = ReplaceRubyTags(value);

            // SetLayoutDirty called
            if (m_isLayoutDirty)
            {
                // changes to the text object properties need to be applied immediately.
                ForceMeshUpdate();
            }
        }

        /// <summary>
        /// replace ruby tags.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>relpaced str</returns>
        private string ReplaceRubyTags(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var pVal = GetPreferredValues("\u00A0");
            var hiddenSpaceW = GetPreferredValues("\u00A0").x * (m_isOrthographic ? 1 : 10f);
            // Replace <ruby> tags text layout.
            var matches = RubyRegex.Matches(str);
            var compensationOffset = 0f;
            foreach (Match match in matches)
            {
                if (match.Groups.Count != 5) continue;
                var fullMatch = match.Groups[0].ToString();
                var rubyText = match.Groups["ruby"].ToString();
                var baseText = match.Groups["val"].ToString();

                var rubyTextW = GetPreferredValues(rubyText).x * (m_isOrthographic ? 1 : 10f) * rubyScale;
                var baseTextW = GetPreferredValues(baseText).x * (m_isOrthographic ? 1 : 10f);
                var rubyTextH = GetPreferredValues(rubyText).y * (m_isOrthographic ? 1 : 10f) * rubyScale;
                var baseTextH = GetPreferredValues(baseText).y * (m_isOrthographic ? 1 : 10f);
                var dir = isRightToLeftText ? 1 : -1;
                var rubyTextOffset = dir * (baseTextW / 2f + rubyTextW / 2f);
                compensationOffset = -dir * ((baseTextW - rubyTextW) / 2f);
                var replace = CreateReplaceValue(baseText, rubyText, rubyTextOffset, compensationOffset, isRightToLeftText);
                str = str.Replace(fullMatch, replace);
            }
            if (allVCompensationRuby)
            {
                // warning! bad Know-how
                // line-height tag is down the next line start.
                // now line can't change, corresponding by putting a hidden ruby
                str = $"<line-height={allVCompensationRubyLineHeight}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>\u00A0</size></voffset><space={compensationOffset}>" + str;
            }

            return str;
        }

        private string CreateReplaceValue(string baseText, string rubyText, float rubyTextOffset, float compensationOffset, bool isRightToLeftText)
        {
            var replace = string.Empty;
            switch (rubyShowType)
            {
                case RubyShowType.BASE_ALIGNMENT:
                    replace = $"<nobr>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                    break;

                case RubyShowType.RUBY_ALIGNMENT:
                    if (isRightToLeftText)
                    {
                        if (compensationOffset < 0)
                        {
                            replace = $"<nobr>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                        else
                        {
                            replace = $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset></nobr>";
                        }
                    }
                    else
                    {
                        if (compensationOffset < 0)
                        {
                            replace = $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset></nobr>";
                        }
                        else
                        {
                            replace = $"<nobr>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                    }
                    break;
            }

            return replace;
        }

        /// <summary>
        /// TMP_Text ConvertToFloat
        /// if (startIndex == 0) { lastIndex = 0; return -9999; } delete version
        /// </summary>
        protected float ConvertToFloatOrigin(char[] chars, int startIndex, int lastIndex)
        {
            int endIndex = lastIndex;
            bool isIntegerValue = true;
            float decimalPointMultiplier = 0;

            // Set value multiplier checking the first character to determine if we are using '+' or '-'
            int valueSignMultiplier = 1;
            if (chars[startIndex] == '+')
            {
                valueSignMultiplier = 1;
                startIndex += 1;
            }
            else if (chars[startIndex] == '-')
            {
                valueSignMultiplier = -1;
                startIndex += 1;
            }

            float value = 0;

            for (int i = startIndex; i < endIndex; i++)
            {
                uint c = chars[i];

                if (c >= '0' && c <= '9' || c == '.')
                {
                    if (c == '.')
                    {
                        isIntegerValue = false;
                        decimalPointMultiplier = 0.1f;
                        continue;
                    }

                    //Calculate integer and floating point value
                    if (isIntegerValue)
                        value = value * 10 + (c - 48) * valueSignMultiplier;
                    else
                    {
                        value = value + (c - 48) * decimalPointMultiplier * valueSignMultiplier;
                        decimalPointMultiplier *= 0.1f;
                    }

                    continue;
                }
                else if (c == ',')
                {
                    if (i + 1 < endIndex && chars[i + 1] == ' ')
                        lastIndex = i + 1;
                    else
                        lastIndex = i;

                    return value;
                }
            }

            lastIndex = endIndex;
            return value;
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            SetTextCustom(m_uneditedText);
        }

#endif
    }
}