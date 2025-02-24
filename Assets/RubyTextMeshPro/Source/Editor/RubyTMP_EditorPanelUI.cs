﻿using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(RubyTextMeshProUGUI), true)]
    [CanEditMultipleObjects]
    public class RubyTMP_EditorPanelUI : TMP_EditorPanelUI
    {
        //Labels and Tooltips
        private static readonly GUIContent RTL_TOGGLE_LABEL = new GUIContent("Enable RTL Editor", "Reverses text direction and allows right to left editing.");
        private static readonly GUIContent STYLE_LABEL = new GUIContent("Text Style", "The style from a style sheet to be applied to the text.");

        private SerializedProperty uneditedText;
        private SerializedProperty rubyScale;
        private SerializedProperty rubyVerticalOffset;
        private SerializedProperty rubyShowType;
        private SerializedProperty rubyLineHeight;
        private SerializedProperty rubyMargin;

        protected override void OnEnable()
        {
            base.OnEnable();

            this.uneditedText = this.serializedObject.FindProperty("_uneditedText");
            this.rubyScale = this.serializedObject.FindProperty("_rubyScale");
            this.rubyVerticalOffset = this.serializedObject.FindProperty("_rubyVerticalOffset");
            this.rubyShowType = this.serializedObject.FindProperty("_rubyShowType");
            this.rubyLineHeight = this.serializedObject.FindProperty("_rubyLineHeight");
            this.rubyMargin = this.serializedObject.FindProperty("_rubyMargin");
        }

        /// <summary>
        /// Copy and change from TMP_BaseEditorPanel::OnInspectorGUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Make sure Multi selection only includes TMP Text objects.
            if (this.IsMixSelectionTypes())
            {
                return;
            }

            this.serializedObject.Update();

            this.DrawRubyTextInput(); // DrawTextInput();

            this.DrawMainSettings();

            this.DrawExtraSettings();

            EditorGUILayout.Space();

            if (this.m_HavePropertiesChanged)
            {
                this.m_HavePropertiesChanged = false;
                this.m_TextComponent.havePropertiesChanged = true;
                this.m_TextComponent.ComputeMarginSize();
                EditorUtility.SetDirty(this.target);
            }

            this.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Copy and change from TMP_Text::DrawTextInput
        /// </summary>
        protected void DrawRubyTextInput()
        {
            EditorGUILayout.Space();

            Rect rect = EditorGUILayout.GetControlRect(false, 22);
            GUI.Label(rect, new GUIContent("<b>Ruby Text Input</b>"), TMP_UIStyleManager.sectionHeader);

            EditorGUI.indentLevel = 0;

            // If the text component is linked, disable the text input box.
            if (this.m_ParentLinkedTextComponentProp.objectReferenceValue != null)
            {
                EditorGUILayout.HelpBox("The Text Input Box is disabled due to this text component being linked to another.", MessageType.Info);
            }
            else
            {
                // Display RTL Toggle
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 110f;

                this.m_IsRightToLeftProp.boolValue = EditorGUI.Toggle(new Rect(rect.width - 120, rect.y + 3, 130, 20), RubyTMP_EditorPanelUI.RTL_TOGGLE_LABEL, this.m_IsRightToLeftProp.boolValue);

                EditorGUIUtility.labelWidth = labelWidth;

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(this.uneditedText);

                if (EditorGUI.EndChangeCheck())
                {
                    this.m_HavePropertiesChanged = true;
                }

                if (this.m_IsRightToLeftProp.boolValue)
                {
                    // Copy source text to RTL string
                    this.m_RtlText = string.Empty;
                    string sourceText = this.uneditedText.stringValue;

                    // Reverse Text displayed in Text Input Box
                    for (int i = 0; i < sourceText.Length; i++)
                    {
                        this.m_RtlText += sourceText[sourceText.Length - i - 1];
                    }

                    GUILayout.Label("RTL Text Input");

                    EditorGUI.BeginChangeCheck();

                    this.m_RtlText = EditorGUILayout.TextArea(this.m_RtlText, TMP_UIStyleManager.wrappingTextArea, GUILayout.Height(EditorGUI.GetPropertyHeight(this.m_TextProp) - EditorGUIUtility.singleLineHeight),
                        GUILayout.ExpandWidth(true));

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Convert RTL input
                        sourceText = string.Empty;

                        // Reverse Text displayed in Text Input Box
                        for (int i = 0; i < this.m_RtlText.Length; i++)
                        {
                            sourceText += this.m_RtlText[this.m_RtlText.Length - i - 1];
                        }

                        this.uneditedText.stringValue = sourceText;
                    }
                }

                // TEXT STYLE
                if (this.m_StyleNames != null)
                {
                    rect = EditorGUILayout.GetControlRect(false, 17);

                    this.m_TextStyleIndexLookup.TryGetValue(this.m_TextStyleHashCodeProp.intValue, out this.m_StyleSelectionIndex);

                    EditorGUI.BeginChangeCheck();
                    this.m_StyleSelectionIndex = EditorGUI.Popup(rect, RubyTMP_EditorPanelUI.STYLE_LABEL, this.m_StyleSelectionIndex, this.m_StyleNames);

                    if (EditorGUI.EndChangeCheck())
                    {
                        this.m_TextStyleHashCodeProp.intValue = this.m_Styles[this.m_StyleSelectionIndex].hashCode;
                        this.m_TextComponent.textStyle = this.m_Styles[this.m_StyleSelectionIndex];
                        this.m_HavePropertiesChanged = true;
                    }
                }

                // show TextMeshPro text
                EditorGUILayout.LabelField("Convert Text");
                EditorGUILayout.SelectableLabel(this.m_TextProp.stringValue, EditorStyles.textArea, GUILayout.Height(EditorGUI.GetPropertyHeight(this.uneditedText) - EditorGUIUtility.singleLineHeight), GUILayout.ExpandHeight(true));

                if (this.m_IsRightToLeftProp.boolValue)
                {
                    string rtlText = string.Empty;
                    string sourceText = this.m_TextProp.stringValue;

                    EditorGUILayout.LabelField("Convert RtlText");

                    // Reverse Text displayed in Text Input Box
                    for (int i = 0; i < sourceText.Length; i++)
                    {
                        rtlText += sourceText[sourceText.Length - i - 1];
                    }

                    EditorGUILayout.SelectableLabel(rtlText, EditorStyles.textArea, GUILayout.Height(EditorGUI.GetPropertyHeight(this.uneditedText) - EditorGUIUtility.singleLineHeight), GUILayout.ExpandHeight(true));
                }
            }
        }

        protected override void DrawExtraSettings()
        {
            base.DrawExtraSettings();

            EditorGUILayout.LabelField("Ruby Param", EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;

            {
                EditorGUILayout.PropertyField(this.rubyScale);
                EditorGUILayout.PropertyField(this.rubyShowType);
                EditorGUILayout.PropertyField(this.rubyVerticalOffset);
                EditorGUILayout.PropertyField(this.rubyLineHeight);
                EditorGUILayout.PropertyField(this.rubyMargin);
            }
            --EditorGUI.indentLevel;

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}