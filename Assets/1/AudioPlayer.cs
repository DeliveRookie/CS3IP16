using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public string filePath;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private Dictionary<char, Tuple<float, float>> vowelMap = new Dictionary<char, Tuple<float, float>>()
    {
        {'a', new Tuple<float, float>(700f, 1200f)},
        {'e', new Tuple<float, float>(500f, 700f)},
        {'i', new Tuple<float, float>(300f, 500f)},
        {'o', new Tuple<float, float>(400f, 800f)},
        {'u', new Tuple<float, float>(200f, 400f)}
    };
    public AudioSource audioSource;
    public AudioClip clip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        filePath = GetComponent<test>().filePath;

        // Load the audio file
        clip = LoadAudioClip(filePath);
        audioSource.clip = clip;

        // Play audio
        audioSource.Play();
    }

    // Load the audio file
    private AudioClip LoadAudioClip(string filePath)
    {
        // Use the WWW class to load the file
        WWW www = new WWW("file://" + filePath);

        // Wait for the loading to complete
        while (!www.isDone) { }

        // Get the audio clip
        AudioClip clip = www.GetAudioClip(false, false, AudioType.WAV);

        return clip;
    }
    private int my_lock=0;
    private void Update()
    {
        my_lock++;
        if(my_lock>15)
        {
            my_lock = 0;
        }
        else
        {
            return;
        }
        // Get audio data
        float[] data = new float[1024];
        audioSource.GetOutputData(data, 0);

        // Calculate the maximum spectrum and the corresponding frequency
        float[] spectrum = new float[1024];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxFrequency = 0;
        float maxMagnitude = 0;
        for (int i = 0; i < spectrum.Length / 2; i++)
        {
            float frequency = i * audioSource.clip.frequency / spectrum.Length;
            float magnitude = spectrum[i];
            if (magnitude > maxMagnitude)
            {
                maxFrequency = frequency;
                maxMagnitude = magnitude;
            }
        }

        // Detect vowels
        foreach (KeyValuePair<char, Tuple<float, float>> pair in vowelMap)
        {
            if (maxFrequency >= pair.Value.Item1 && maxFrequency <= pair.Value.Item2)
            {
                Debug.Log("Detected vowel: " + pair.Key);
                setValue(pair.Key);
                break;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            Application.LoadLevel(0);
        }
    }

    public void setValue(char value)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(2, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(3, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(4, 0);
        switch (value)
        {
            case 'a':
                skinnedMeshRenderer.SetBlendShapeWeight(0, 100);
                break;
            case 'e':
                skinnedMeshRenderer.SetBlendShapeWeight(4, 100);
                break;
            case 'i':
                skinnedMeshRenderer.SetBlendShapeWeight(3, 100);
                break;
            case 'o':
                skinnedMeshRenderer.SetBlendShapeWeight(1, 100);
                break;
            case 'u':
                skinnedMeshRenderer.SetBlendShapeWeight(2, 100);
                break;
        }
    }
}
