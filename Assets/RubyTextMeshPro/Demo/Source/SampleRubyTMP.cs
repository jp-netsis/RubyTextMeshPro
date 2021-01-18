using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RubyTextMeshPro))]
public class SampleRubyTMP : MonoBehaviour
{
    public float span = 3f;

    RubyTextMeshPro rubyTextMeshPro;

    string[] strArray = new string[]
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
        "<ruby=じゅう>拾</ruby>",
    };

    int strIndex;

    // Start is called before the first frame update
    void Start()
    {
        rubyTextMeshPro = GetComponent<RubyTextMeshPro>();
        StartCoroutine(TimeUpdate());
    }

    // Update is called once per frame
    IEnumerator TimeUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(span);
            UpdateText();
        }
    }

    void UpdateText()
    {
        rubyTextMeshPro.UnditedText = strArray[strIndex];
        strIndex++;
        if (this.strArray.Length <= this.strIndex)
        {
            this.strIndex = 0;
        }
    }

}
