using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TMPro
{
    public class RubyTextMeshProUGUI : TextMeshProUGUI
    {
        protected enum RubyShowType
        {
            RUBY_ALIGNMENT,
            BASE_ALIGNMENT
        }

        // ruby tag
        private static readonly Regex RubyRegex = new Regex(@"<r(uby)?=(?<ruby>[\s\S]*?)>(?<val>[\s\S]*?)<\/r(uby)?>");

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
            set { 
                m_uneditedText = value; 
                SetTextCustom(m_uneditedText);
            }
        }

        private void SetTextCustom(string value)
        {
            text = ReplaceRubyTags(value);

            // m_havePropertiesChanged : text changed => true, ForceMeshUpdate in OnPreRenderCanvas => false
            if (m_havePropertiesChanged)
            {
                ForceMeshUpdate();
            }
        }
        
        public override void ForceMeshUpdate(bool ignoreActiveState = false, bool forceTextReparsing = false)
        {
            base.ForceMeshUpdate(ignoreActiveState,forceTextReparsing);
            if (m_enableAutoSizing)
            {
                // change auto size timing, update ruby tag size.
                SetTextCustom(m_uneditedText);
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
            // warning! bad Know-how
            // Can not get GetPreferredValues("\u00A0").x at width,
            // add string and calculate.
            // and Use GetPreferredValues, change this.m_maxFontSize value.
            var nonBreakSpaceW = GetPreferredValues("\u00A0a").x - GetPreferredValues("a").x;
            var fontSizeScale = 1f;
            if (this.m_enableAutoSizing)
            {
                fontSizeScale = (this.m_fontSize / this.m_maxFontSize);
            }
            var dir = isRightToLeftText ? 1 : -1;
            // Q. Why (m_isOrthographic ? 1 : 10f) => A. TMP_Text.cs L7622, L7625 
            var hiddenSpaceW = dir * nonBreakSpaceW * (m_isOrthographic ? 1 : 10f) * rubyScale * fontSizeScale;
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
                if (this.m_enableAutoSizing)
                {
                    rubyTextW *= fontSizeScale;
                    baseTextW *= fontSizeScale;
                }
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
                str = $"<line-height={allVCompensationRubyLineHeight}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>\u00A0</size></voffset><space={hiddenSpaceW}>" + str;
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
                            replace =
                                $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                        else
                        {
                            replace =
                                $"<nobr>{baseText}<space={rubyTextOffset}><voffset={rubyVerticalOffset}><size={rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                    }

                    break;
            }

            return replace;
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