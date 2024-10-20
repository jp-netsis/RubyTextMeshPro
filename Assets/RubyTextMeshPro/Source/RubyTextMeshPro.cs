using jp.netsis.RubyText;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMPro
{
    using static RubyTextConstants;

    public class RubyTextMeshPro : TextMeshPro, IRubyText
    {
        [Tooltip("v offset ruby. (em, px, %).")]
        [SerializeField]
        [FormerlySerializedAs("rubyVerticalOffset")]
        private string _rubyVerticalOffset = "1em";

        public string rubyVerticalOffset => this._rubyVerticalOffset;

        [Tooltip("ruby scale. (1=100%)")]
        [SerializeField]
        [FormerlySerializedAs("rubyScale")]
        private float _rubyScale = 0.5f;

        public float rubyScale => this._rubyScale;

        [Tooltip("The height of the ruby line can be specified. (em, px, %).")]
        [SerializeField]
        [FormerlySerializedAs("rubyLineHeight")]
        private string _rubyLineHeight = "";

        public string rubyLineHeight => this._rubyLineHeight;

        [TextArea(5, 10)]
        [SerializeField]
        [FormerlySerializedAs("m_uneditedText")]
        private string _uneditedText;

        public string uneditedText
        {
            get => this._uneditedText;
            set
            {
                this._uneditedText = value;
                this.SetTextCustom(this._uneditedText);
            }
        }

        [Tooltip("ruby show type.")]
        [SerializeField]
        [FormerlySerializedAs("rubyShowType")]
        private RubyShowType _rubyShowType = RubyShowType.RUBY_ALIGNMENT;

        public RubyShowType rubyShowType => this._rubyShowType;

        [Tooltip("Affects only BASE_NO_OVERRAP_RUBY_ALIGNMENT ruby margin.")]
        [SerializeField]
        private float _rubyMargin = 10;

        public float rubyMargin => this._rubyMargin;

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
                // changes to the text object properties need to be applied immediately.
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
        /// <returns>relpaced str</returns>
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

            int dir = this.isRightToLeftText ? -1 : 1;
            // Q. Why (m_isOrthographic ? 1 : 10f) => A. TMP_Text.cs L7622, L7625 
            float hiddenSpaceW = dir * nonBreakSpaceW * (this.m_isOrthographic ? 1 : 10f) * this.rubyScale * fontSizeScale;
            str = this.ReplaceRubyTags(str, dir, fontSizeScale, hiddenSpaceW);

            return str;
        }
    }
}