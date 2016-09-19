using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TextUiScript : TextScriptBase {

    private Text text;

    void Start()
    {

        text = GetComponent<Text>();
        SetTextColor(startColor);
    }

    protected override void SetTextColor(Color color)
    {
        text.color = color;
    }

    protected override Color GetTextColor()
    {
        return text.color;
    }
}
