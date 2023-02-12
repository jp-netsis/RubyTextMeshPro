using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RubyTextMeshProUGUI))]
public class SampleRubyTMPUGUI : MonoBehaviour
{
    public float span = 3f;

    private readonly string[] strArray =
    {
        "<ruby=いち>壱</ruby>",
        "<ruby=に>弐</ruby>",
        "<ruby=さん>参</ruby>",
        "<ruby=し>肆</ruby>",
        "<ruby=ご>伍</ruby>",
        "<ruby=ろく>禄</ruby>",
        "<ruby=しち>漆</ruby>",
        "<ruby=はち>捌</ruby>",
        "<ruby=きゅう>玖</ruby>",
        "<ruby=じゅう>拾</ruby>"
    };

    private RubyTextMeshProUGUI rubyTextMeshPro;

    private int strIndex;

    // Start is called before the first frame update
    private void Start()
    {
        this.rubyTextMeshPro = this.GetComponent<RubyTextMeshProUGUI>();
        this.StartCoroutine(this.TimeUpdate());
    }

    // Update is called once per frame
    private IEnumerator TimeUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.span);
            this.UpdateText();
        }
    }

    private void UpdateText()
    {
        this.rubyTextMeshPro.uneditedText = this.strArray[this.strIndex];
        this.strIndex++;

        if (this.strArray.Length <= this.strIndex)
        {
            this.strIndex = 0;
        }
    }
}