using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Counter : MonoBehaviour {

    public Text secondsText;
    public Text miliSecondsText;

    private int seconds;
    private int miliSeconds;

    private delegate void CounterDelegate();
    private CounterDelegate counterDelegate;

    private float timer;

    void Start()
    {
        secondsText.text = "00";
        miliSecondsText.text = "00";
    }

	void Update () {
        if (counterDelegate != null)
            counterDelegate();
	}

    public void StartCounter()
    {
        counterDelegate -= Count;
        counterDelegate += Count;
    }

    public void StopCounter()
    {
        counterDelegate -= Count;
    }

    public void ResetCounter()
    {
        timer = 0;
    }

    private void Count()
    {
        timer += (1f / 60f);

        seconds = Mathf.FloorToInt(timer);
        miliSeconds = Mathf.FloorToInt((timer - seconds) * 60);

        secondsText.text = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
        miliSecondsText.text = miliSeconds < 10 ? "0" +  miliSeconds.ToString() : miliSeconds.ToString();
    }
}
