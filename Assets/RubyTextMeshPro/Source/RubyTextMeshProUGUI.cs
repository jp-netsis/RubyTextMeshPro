using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMPro
{
    using static RubyTextMeshProDefinitions;

    public class RubyTextMeshProUGUI : TextMeshProUGUI
    {
        [Tooltip("v offset ruby. (em, px, %).")] [SerializeField]
        private string rubyVerticalOffset = "1em";

        [Tooltip("ruby scale. (1=100%)")] [SerializeField]
        private float rubyScale = 0.5f;

        [Tooltip("ruby show type.")] [SerializeField]
        private RubyShowType rubyShowType = RubyShowType.RUBY_ALIGNMENT;

        [Tooltip("The height of the ruby line can be specified. (em, px, %).")] [SerializeField]
        private string rubyLineHeight = "";

        [FormerlySerializedAs("m_uneditedText")] [SerializeField] [TextArea(5, 10)]
        private string _uneditedText;

        [Obsolete("This setter will be discontinued.Use uneditedText instead.")]
        public string UnditedText
        {
            set
            {
                this._uneditedText = value;
                this.SetTextCustom(this._uneditedText);
            }
        }

        public string uneditedText
        {
            set
            {
                this._uneditedText = value;
                this.SetTextCustom(this._uneditedText);
            }
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetTextCustom(this._uneditedText);
        }
#endif

        private void SetTextCustom(string value)
        {
            this.text = this.ReplaceRubyTags(value);

            // m_havePropertiesChanged : text changed => true, ForceMeshUpdate in OnPreRenderCanvas => false
            if (this.m_havePropertiesChanged)
            {
                this.ForceMeshUpdate();
            }
        }

        public override void ForceMeshUpdate(bool ignoreActiveState = false, bool forceTextReparsing = false)
        {
            base.ForceMeshUpdate(ignoreActiveState, forceTextReparsing);

            if (this.m_enableAutoSizing)
            {
                // change auto size timing, update ruby tag size.
                this.text = this.ReplaceRubyTags(this._uneditedText);
                base.ForceMeshUpdate(ignoreActiveState, forceTextReparsing);
            }
        }

        /// <summary>
        /// replace ruby tags.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>replaced str</returns>
        private string ReplaceRubyTags(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            // warning! bad Know-how
            // Can not get GetPreferredValues("\u00A0").x at width,
            // add string and calculate.
            // and Use GetPreferredValues, change this.m_maxFontSize value.
            float nonBreakSpaceW = this.GetPreferredValues("\u00A0a").x - this.GetPreferredValues("a").x;
            float fontSizeScale = 1f;

            if (this.m_enableAutoSizing)
            {
                fontSizeScale = this.m_fontSize / this.m_maxFontSize;
            }

            int dir = this.isRightToLeftText ? 1 : -1;
            // Q. Why (m_isOrthographic ? 1 : 10f) => A. TMP_Text.cs L7622, L7625 
            float hiddenSpaceW = dir * nonBreakSpaceW * (this.m_isOrthographic ? 1 : 10f) * this.rubyScale * fontSizeScale;
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

                float rubyTextW = this.GetPreferredValues(rubyText).x * (this.m_isOrthographic ? 1 : 10f) * this.rubyScale;
                float baseTextW = this.GetPreferredValues(baseText).x * (this.m_isOrthographic ? 1 : 10f);

                if (this.m_enableAutoSizing)
                {
                    rubyTextW *= fontSizeScale;
                    baseTextW *= fontSizeScale;
                }

                float rubyTextOffset = dir * (baseTextW / 2f + rubyTextW / 2f);
                float compensationOffset = -dir * ((baseTextW - rubyTextW) / 2f);
                string replace = this.CreateReplaceValue(baseText, rubyText, rubyTextOffset, compensationOffset, this.isRightToLeftText);
                str = str.Replace(fullMatch, replace);
            }

            if (!string.IsNullOrWhiteSpace(this.rubyLineHeight))
                // warning! bad Know-how
                // line-height tag is down the next line start.
                // now line can't change, corresponding by putting a hidden ruby
            {
                str = $"<line-height={this.rubyLineHeight}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>\u00A0</size></voffset><space={hiddenSpaceW}>" + str;
            }

            return str;
        }

        private string CreateReplaceValue(string baseText, string rubyText, float rubyTextOffset, float compensationOffset, bool isRightToLeftText)
        {
            string replace = string.Empty;

            switch (this.rubyShowType)
            {
                case RubyShowType.BASE_ALIGNMENT:
                    replace = $"<nobr>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                    break;

                case RubyShowType.RUBY_ALIGNMENT:
                    if (isRightToLeftText)
                    {
                        if (compensationOffset < 0)
                        {
                            replace = $"<nobr>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                        else
                        {
                            replace = $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset></nobr>";
                        }
                    }
                    else
                    {
                        if (compensationOffset < 0)
                        {
                            replace =
                                $"<nobr><space={-compensationOffset}>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                        else
                        {
                            replace =
                                $"<nobr>{baseText}<space={rubyTextOffset}><voffset={this.rubyVerticalOffset}><size={this.rubyScale * 100f}%>{rubyText}</size></voffset><space={compensationOffset}></nobr>";
                        }
                    }

                    break;
            }

            return replace;
        }
    }
}