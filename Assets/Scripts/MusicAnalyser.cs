using UnityEngine;
using System.Collections;

public class MusicAnalyser : MonoBehaviour {

    public float RmsValue;
    public bool analyse;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;

    float[] _samples;
    private float[] _spectrum;

    private AudioSource source;

    private delegate void AudioDelegate();
    AudioDelegate audioDelegate;

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];

        source = GetComponent<AudioSource>();

        analyse = false;
    }

    void Update()
    {
        if (audioDelegate != null)
            audioDelegate();
    }

    void AnalyzeSound()
    {
        source.GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
    }

    public void Analyse()
    {
        audioDelegate += AnalyzeSound;
        analyse = true;
        source.Play();
        source.pitch = 1f;
    }

    public void Stop()
    {
        audioDelegate -= AnalyzeSound;

        audioDelegate -= Stop;
        audioDelegate += Stop;

        if (source.pitch > 0f)
            source.pitch -= 0.01f;
        else 
        {
            source.Stop();
            audioDelegate -= Stop;
            analyse = false;
        }



    }
}
