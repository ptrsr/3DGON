using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextMeshScript : TextScriptBase {

    private TextMesh text;

	void Start () {

        text = GetComponent<TextMesh>();
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
