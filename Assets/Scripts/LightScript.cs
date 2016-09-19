using UnityEngine;
using System.Collections;

public class LightScript : MonoBehaviour {

    public float startIntensity = 0;

    private Light thisLight;

    private delegate void LightDelegate(float newIntensity, float speed);
    private LightDelegate lightDelegate;

    private float desiredIntensity;
    private float changeSpeed;

	void Start () {

        thisLight = GetComponent<Light>();
        thisLight.intensity = startIntensity;
	}
	
	void Update () {
        if (lightDelegate != null)
            lightDelegate(desiredIntensity, changeSpeed);
	}

    public void ChangeIntensity(float newIntensity, float speed)
    {
        desiredIntensity = newIntensity;
        changeSpeed = speed;

        thisLight.intensity += Mathf.Clamp(newIntensity - thisLight.intensity, -changeSpeed, changeSpeed);

        if (lightDelegate == null)
            lightDelegate += ChangeIntensity;

        if (thisLight.intensity == desiredIntensity)
            lightDelegate -= ChangeIntensity;
    }
}
