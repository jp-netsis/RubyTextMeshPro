using UnityEngine;
using UnityEditor;
using TMPro;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(RubyTextMeshProUGUI), true), CanEditMultipleObjects]
    public class RubyTMP_EditorPanelUI : TMP_EditorPanelUI
    {
        //Labels and Tooltips
        static readonly GUIContent k_RtlToggleLabel = new GUIContent("Enable RTL Editor", "Reverses text direction and allows right to left editing.");
        static readonly GUIContent k_StyleLabel = new GUIContent("Text Style", "The style from a style sheet to be applied to the text.");

        private SerializedProperty rubyVerticalOffset;
        private SerializedProperty rubyScale;
        private SerializedProperty rubyShowType;
        private SerializedProperty allVCompensationRuby;
        private SerializedProperty allVCompensationRubyLineHeight;
        private SerializedProperty m_UneditedText;
        private string m_UneditedRtlText;

        protected override void OnEnable()
        {
            base.OnEnable();

            rubyScale = serializedObject.FindProperty("rubyScale");
            rubyVerticalOffset = serializedObject.FindProperty("rubyVerticalOffset");
            rubyShowType = serializedObject.FindProperty("rubyShowType");
            allVCompensationRuby = serializedObject.FindProperty("allVCompensationRuby");
            allVCompensationRubyLineHeight = serializedObject.FindProperty("allVCompensationRubyLineHeight");
            m_UneditedText = serializedObject.FindProperty("m_uneditedText");
        }

        /// <summary>
        /// Copy and change from TMP_BaseEditorPanel::OnInspectorGUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Make sure Multi selection only includes TMP Text objects.
            if (IsMixSelectionTypes()) return;

            serializedObject.Update();

            DrawRubyTextInput(); // DrawTextInput();

            DrawMainSettings();

            DrawExtraSettings();

            EditorGUILayout.Space();

            if (m_HavePropertiesChanged)
            {
                m_HavePropertiesChanged = false;
                m_TextComponent.havePropertiesChanged = true;
                m_TextComponent.ComputeMarginSize();
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
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
            if (m_ParentLinkedTextComponentProp.objectReferenceValue != null)
            {
                EditorGUILayout.HelpBox("The Text Input Box is disabled due to this text component being linked to another.", MessageType.Info);
            }
            else
            {
                // Display RTL Toggle
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 110f;

                m_IsRightToLeftProp.boolValue = EditorGUI.Toggle(new Rect(rect.width - 120, rect.y + 3, 130, 20), k_RtlToggleLabel, m_IsRightToLeftProp.boolValue);

                EditorGUIUtility.labelWidth = labelWidth;
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_UneditedText);

                if (EditorGUI.EndChangeCheck())
                {
                    m_HavePropertiesChanged = true;
                }

                if (m_IsRightToLeftProp.boolValue)
                {
                    // Copy source text to RTL string
                    m_RtlText = string.Empty;
                    string sourceText = m_UneditedText.stringValue;

                    // Reverse Text displayed in Text Input Box
                    for (int i = 0; i < sourceText.Length; i++)
                        m_RtlText += sourceText[sourceText.Length - i - 1];

                    GUILayout.Label("RTL Text Input");

                    EditorGUI.BeginChangeCheck();
                    m_RtlText = EditorGUILayout.TextArea(m_RtlText, TMP_UIStyleManager.wrappingTextArea, GUILayout.Height(EditorGUI.GetPropertyHeight(m_TextProp) - EditorGUIUtility.singleLineHeight), GUILayout.ExpandWidth(true));

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Convert RTL input
                        sourceText = string.Empty;

                        // Reverse Text displayed in Text Input Box
                        for (int i = 0; i < m_RtlText.Length; i++)
                            sourceText += m_RtlText[m_RtlText.Length - i - 1];

                        m_UneditedText.stringValue = sourceText;
                    }
                }

                // TEXT STYLE
                if (m_StyleNames != null)
                {
                    rect = EditorGUILayout.GetControlRect(false, 17);

                    m_TextStyleIndexLookup.TryGetValue(m_TextStyleHashCodeProp.intValue, out m_StyleSelectionIndex);

                    EditorGUI.BeginChangeCheck();
                    m_StyleSelectionIndex = EditorGUI.Popup(rect, k_StyleLabel, m_StyleSelectionIndex, m_StyleNames);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_TextStyleHashCodeProp.intValue = m_Styles[m_StyleSelectionIndex].hashCode;
                        m_TextComponent.textStyle = m_Styles[m_StyleSelectionIndex];
                        m_HavePropertiesChanged = true;
                    }
                }

                // show TextMeshPro text
                EditorGUILayout.LabelField("Convert Text");
                EditorGUILayout.SelectableLabel(m_TextProp.stringValue, EditorStyles.textArea, GUILayout.Height(EditorGUI.GetPropertyHeight(m_UneditedText) - EditorGUIUtility.singleLineHeight), GUILayout.ExpandHeight(true));
                if (m_IsRightToLeftProp.boolValue)
                {
                    var RtlText = string.Empty;
                    string sourceText = m_TextProp.stringValue;

                    EditorGUILayout.LabelField("Convert RtlText");

                    // Reverse Text displayed in Text Input Box
                    for (int i = 0; i < sourceText.Length; i++)
                    {
                        RtlText += sourceText[sourceText.Length - i - 1];
                    }
                    EditorGUILayout.SelectableLabel(RtlText, EditorStyles.textArea, GUILayout.Height(EditorGUI.GetPropertyHeight(m_UneditedText) - EditorGUIUtility.singleLineHeight), GUILayout.ExpandHeight(true));
                }
            }
        }

        protected override void DrawExtraSettings()
        {
            base.DrawExtraSettings();

            EditorGUILayout.LabelField("Ruby Param", EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;
            {
                EditorGUILayout.PropertyField(rubyScale);
                EditorGUILayout.PropertyField(rubyShowType);
                EditorGUILayout.PropertyField(rubyVerticalOffset);
                EditorGUILayout.PropertyField(allVCompensationRuby);
                EditorGUILayout.PropertyField(allVCompensationRubyLineHeight);
            }
            --EditorGUI.indentLevel;

            serializedObject.ApplyModifiedProperties();
        }
    }
}