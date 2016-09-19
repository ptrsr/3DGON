using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextScriptBase : MonoBehaviour {

    public Color startColor = new Color(1,1,1,0);

    private delegate void ColorDelegate(Color newColor, float speed);
    private ColorDelegate colorDelegate;

    private Color desiredColor;

    private float changeSpeed;
	
	void Update () {
        if (colorDelegate != null)
            colorDelegate(desiredColor, changeSpeed);
	}

    public void ChangeColor(Color newColor, float newSpeed)
    {
        desiredColor = newColor;
        changeSpeed = newSpeed;

        Color differenceColor = desiredColor - GetTextColor();
        for (int i = 0; i <= 3; i++)
            differenceColor[i] = Mathf.Clamp(differenceColor[i], -changeSpeed, changeSpeed);
        
        SetTextColor(GetTextColor() + differenceColor);

        if (colorDelegate == null)
            colorDelegate += ChangeColor;

        if (GetTextColor() == desiredColor)
            colorDelegate -= ChangeColor;
        
    }

    protected virtual void SetTextColor(Color color) { }

    protected virtual Color GetTextColor() { return Color.white; }
}
