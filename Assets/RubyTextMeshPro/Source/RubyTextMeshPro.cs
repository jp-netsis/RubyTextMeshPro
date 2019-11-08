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
        private static readonly Regex RubyRegex = new Regex(@"<ruby=([\s\S]*?)>([\s\S]*?)<\/ruby>");

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
        public string UnditedText { 
            set { m_uneditedText = value; } 
        }

        private void SetTextCustom(string value)
        {
            text = ReplaceRubyTags(value);

            // SetLayoutDirty called
            if (m_layoutAlreadyDirty)
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
            var hiddenSpaceW = GetPreferredValues("\u00A0").x * (m_isOrthographic ? 1 : 10f);
            // Replace <ruby> tags text layout.
            var matches = RubyRegex.Matches(str);
            foreach (Match match in matches)
            {
                if (match.Groups.Count != 3) continue;
                var fullMatch = match.Groups[0].ToString();
                var rubyText = match.Groups[1].ToString();
                var baseText = match.Groups[2].ToString();

                var rubyTextW = GetPreferredValues(rubyText).x * (m_isOrthographic ? 1 : 10f) * rubyScale;
                var baseTextW = GetPreferredValues(baseText).x * (m_isOrthographic ? 1 : 10f);
                var dir = isRightToLeftText ? 1 : -1;
                var rubyTextOffset = dir * (baseTextW / 2f + rubyTextW / 2f);
                var compensationOffset = -dir * ((baseTextW - rubyTextW) / 2f);
                var replace = CreateReplaceValue(baseText, rubyText, rubyTextOffset, compensationOffset, hiddenSpaceW);
                str = str.Replace(fullMatch, replace);
            }
            // not found ruby && allVCompensationRuby
            if (matches.Count == 0 && allVCompensationRuby)
            {
                // warning! bad Know-how
                // line-height tag is down the next line start.
                // now line can't change, corresponding by putting a hidden ruby
                var dir = isRightToLeftText ? 1 : -1;
                // Get hidden ruby width
                var spaceTextWidth = hiddenSpaceW * rubyScale;
                var compensationOffset = dir * spaceTextWidth;
                str = $"<line-height={allVCompensationRubyLineHeight}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>\u00A0</size></voffset>" + str;
            }

            return str;
        }

        private string CreateReplaceValue(string baseText, string rubyText, float rubyTextOffset, float compensationOffset, float hiddenSpaceW)
        {
            var replace = string.Empty;
            switch (rubyShowType)
            {
                case RubyShowType.BASE_ALIGNMENT:
                    replace = $"<nobr>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}>";
                    break;
                case RubyShowType.RUBY_ALIGNMENT:
                    if (compensationOffset < 0)
                    {
                        replace = $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset></nobr>";
                    }
                    else
                    {
                        var n = Mathf.CeilToInt(compensationOffset / hiddenSpaceW);
                        var hiddenSpaceSize = compensationOffset / hiddenSpaceW / n;
                        string hiddenSpaces = new string(' ', n);
                        replace = $"<nobr>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset><size={hiddenSpaceSize * 100f}%>{hiddenSpaces}</size></nobr>";
                    }
                    break;
            }

            return replace;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            SetTextCustom(m_uneditedText);
        }

    }
}
