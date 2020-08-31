using UnityEngine;
using UnityEditor;
using TMPro;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(RubyTextMeshPro), true), CanEditMultipleObjects]
    public class RubyTMP_EditorPanel : TMP_EditorPanel
    {
        //Labels and Tooltips
        private static readonly GUIContent k_RtlToggleLabel = new GUIContent("Enable RTL Editor", "Reverses text direction and allows right to left editing.");

        private SerializedProperty rubyScale;
        private SerializedProperty rubyVerticalOffset;
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

            // If the text component is linked, disable the text input box.
            if (m_ParentLinkedTextComponentProp.objectReferenceValue != null)
            {
                EditorGUILayout.HelpBox("The Text Input Box is disabled due to this text component being linked to another.", MessageType.Info);
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_UneditedText);

                if (EditorGUI.EndChangeCheck() || (m_IsRightToLeftProp.boolValue && string.IsNullOrEmpty(m_UneditedRtlText)))
                {
                    m_HavePropertiesChanged = true;

                    // Handle Left to Right or Right to Left Editor
                    if (m_IsRightToLeftProp.boolValue)
                    {
                        m_UneditedRtlText = string.Empty;
                        string sourceText = m_UneditedText.stringValue;

                        // Reverse Text displayed in Text Input Box
                        for (int i = 0; i < sourceText.Length; i++)
                        {
                            m_UneditedRtlText += sourceText[sourceText.Length - i - 1];
                        }
                    }
                }

                // Toggle showing Rich Tags
                m_IsRightToLeftProp.boolValue = EditorGUILayout.Toggle(k_RtlToggleLabel, m_IsRightToLeftProp.boolValue);

                if (m_IsRightToLeftProp.boolValue)
                {
                    EditorGUI.BeginChangeCheck();
                    m_UneditedRtlText = EditorGUILayout.TextArea(m_UneditedRtlText, TMP_UIStyleManager.wrappingTextArea, GUILayout.Height(EditorGUI.GetPropertyHeight(m_UneditedText) - EditorGUIUtility.singleLineHeight), GUILayout.ExpandWidth(true));
                    if (EditorGUI.EndChangeCheck())
                    {
                        // Convert RTL input
                        string sourceText = string.Empty;

                        // Reverse Text displayed in Text Input Box
                        for (int i = 0; i < m_UneditedRtlText.Length; i++)
                        {
                            sourceText += m_UneditedRtlText[m_UneditedRtlText.Length - i - 1];
                        }

                        m_UneditedText.stringValue = sourceText;
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
                EditorGUILayout.PropertyField(rubyVerticalOffset);
                EditorGUILayout.PropertyField(rubyShowType);
                EditorGUILayout.PropertyField(allVCompensationRuby);
                EditorGUILayout.PropertyField(allVCompensationRubyLineHeight);
            }
            --EditorGUI.indentLevel;

            serializedObject.ApplyModifiedProperties();
        }
    }
}