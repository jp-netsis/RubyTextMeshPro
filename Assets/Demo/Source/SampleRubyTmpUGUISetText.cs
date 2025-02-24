using TMPro;
using UnityEngine;

[RequireComponent(typeof(RubyTextMeshProUGUI))]
public class SampleRubyTmpUGUISetText : MonoBehaviour
{
    [SerializeField] RubyTextMeshProUGUI textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (null == this.textMeshProUGUI)
        {
            this.textMeshProUGUI = GetComponent<RubyTextMeshProUGUI>();
        }
        this.textMeshProUGUI.SetUneditedText("<r>蛞<rt>おたまじゃくし</rt></r> = {0}, <r=かえる>蛙</r> = {1:00}, <r=かわず>蛙</r> = {2:000.0}", 10.75f, 10.75f, 10.75f);
    }
}
