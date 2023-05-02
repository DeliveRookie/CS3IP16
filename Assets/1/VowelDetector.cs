//using UnityEngine;
//using UnityEngine.Audio;
//using UnityEngine.Experimental.Audio;


//public class VowelDetector : MonoBehaviour
//{
//    public AudioMixerGroup mixerGroup; // ��������
//    public int sampleRate = 44100; // ������
//    public float minPitch = 80f; // ��С����Ƶ��
//    public float maxPitch = 400f; // ������Ƶ��
//    public float vowelThreshold = 0.1f; // Ԫ����ֵ

//    private AudioGraphOutput output; // ��Ƶ���
//    private AudioBuffer buffer; // ��Ƶ������
//    private float[] spectrum; // Ƶ������

//    void Start()
//    {
//        // ����DSPGraph
//        AudioGraph graph = AudioGraph.Create("Vowel Detector", 2, 1, sampleRate);

//        // ��������ڵ�
//        output = graph.Output(0, 2);

//        // ����������
//        buffer = new AudioBuffer(2, sampleRate / 10);

//        // ����������
//        AudioMixer mixer = new AudioMixer(2, 1, sampleRate / 10);

//        // ��������ڵ�
//        AudioGraphNode input = graph.AddNode("Input", AudioNodeType.Output, 1);
//        input.SetBuffer(0, buffer);

//        // �����������ڵ�
//        AudioGraphNode mixerNode = graph.AddNode("Mixer", AudioNodeType.Mixer, 1);
//        mixerNode.SetMixer(mixer);

//        // ���ӽڵ�
//        input.Connect(0, mixerNode, 0);

//        // ���û�������
//        mixer.SetOutput(0, mixerGroup);

//        // ��ʼDSPGraph
//        graph.Start();

//        // ��ʼ��Ƶ������
//        spectrum = new float[sampleRate / 2];
//    }

//    void Update()
//    {
//        // ��ȡ��Ƶ����
//        int samples = output.Read(buffer, buffer.samples);

//        // ����Ƶ������
//        DSPUtilities.GetSpectrumData(buffer, spectrum);

//        // �������Ƶ��
//        float pitch = DSPUtilities.FindPitch(spectrum, minPitch, maxPitch);

//        // �ж�Ԫ��
//        if (pitch > 0)
//        {
//            // ����Ԫ��ǿ��
//            float vowelIntensity = DSPUtilities.GetBandEnergy(spectrum, pitch, 50f);

//            // �ж�Ԫ��
//            if (vowelIntensity > vowelThreshold)
//            {
//                // ���ݻ���Ƶ���ж�Ԫ��
//                if (pitch >= 210 && pitch <= 260)
//                {
//                    Debug.Log("A");
//                }
//                else if (pitch >= 310 && pitch <= 370)
//                {
//                    Debug.Log("E");
//                }
//                else if (pitch >= 530 && pitch <= 600)
//                {
//                    Debug.Log("I");
//                }
//                else if (pitch >= 680 && pitch <= 730)
//                {
//                    Debug.Log("O");
//                }
//                else if (pitch >= 880 && pitch <= 1000)
//                {
//                    Debug.Log("U");
//                }
//            }
//        }
//    }

//    void OnDestroy()
//    {
//        // ֹͣDSPGraph
//        output?.Dispose();
//    }
//}

using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class VowelDetector : MonoBehaviour
{
   // public AudioClip audioClip; // Ҫ��������Ƶ����
    public AudioMixerGroup outputMixerGroup; // AudioSource �������Ƶ�������
    public float vowelThreshold = 0.1f; // Ԫ���������ֵ

    private AudioSource audioSource;
    [SerializeField]
    public List<float[]> vowelSegments = new List<float[]>(); // Ԫ����ƵƬ�ε��б�

    void Start()
    {
       // audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = audioClip;
       // audioSource.outputAudioMixerGroup = outputMixerGroup;

       // vowelSegments = new List<float[]>();
    }

    public void Update_yuanyin(float[] audioData, int numSamples,int channels)
    {
       // audioSource.clip = audioClip;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        vowelSegments.Clear(); // ���Ԫ����ƵƬ���б�

            //// ��ȡ��Ƶ����
            //int numSamples = audioClip.samples * audioClip.channels;
            //float[] audioData = new float[numSamples];
            //audioClip.GetData(audioData, 0);
        // ��ӡǰ 100 ����Ƶ��������
        for (int i = 0; i < 100; i++)
        {
            Debug.Log("Sample " + i + ": " + audioData[i]);
        }
        // ������Ƶ�����е�Ԫ������
        bool isInsideVowel = false;
            int vowelStartIndex = 0;
            for (int i = 0; i < numSamples; i += channels)
            {
                float sampleValue = audioData[i];
                if (Mathf.Abs(sampleValue) > vowelThreshold)
                {
                    if (!isInsideVowel)
                    {
                        isInsideVowel = true;
                        vowelStartIndex = i;
                    }
                }
                else
                {
                    if (isInsideVowel)
                    {
                        isInsideVowel = false;
                        int vowelEndIndex = i;
                        float[] vowelSegment = new float[vowelEndIndex - vowelStartIndex];

                        // ��ȡԪ����ƵƬ��
                        for (int j = vowelStartIndex; j < vowelEndIndex; j += channels)
                        {
                            vowelSegment[j - vowelStartIndex] = audioData[j];
                        }

                        vowelSegments.Add(vowelSegment); // ���Ԫ����ƵƬ�ε��б���
                    }
                }
            }

        // ����Ƶ�ʷ�Χ��Ԫ���������з���
        foreach (float[] segment in vowelSegments)
        {

            // �ҵ��ӽ�����С�ݴη�ֵ
            int length = 1;
            while (length < segment.Length)
            {
                length <<= 1; // �����൱�ڳ��� 2
            }


            float[] spectrum = new float[128];
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

            
            float maxFrequency = 0;
            float maxAmplitude = 0;
            for (int i = 0; i < spectrum.Length / 2; i++)
            {
                float frequency = i * AudioSettings.outputSampleRate / spectrum.Length;
                float amplitude = spectrum[i];

                if (amplitude > maxAmplitude)
                {
                    maxFrequency = frequency;
                    maxAmplitude = amplitude;
                }
            }

            // �������Ƶ�ʺͷ�ֵ���Ԫ����ĸ
            if (maxFrequency > 400 && maxFrequency < 800)
            {
                if (maxAmplitude > 0.1f)
                {
                    Debug.Log("A");
                }
                else
                {
                    Debug.Log("I");
                }
            }
            else if (maxFrequency > 800 && maxFrequency < 1200)
            {
                if (maxAmplitude > 0.1f)
                {
                    Debug.Log("E");
                }
                else
                {
                    Debug.Log("U");
                }
            }
            else if (maxFrequency > 220 && maxFrequency < 400)
            {
                Debug.Log("O");
            }
            else if (maxFrequency > 1200 && maxFrequency < 2800)
            {
                Debug.Log("I");
            }
            else
            {
                Debug.Log("Unknown vowel");
            }

            print(string.Format("segment{0},maxFrequency{1}" , segment,maxFrequency));
            foreach(float value in segment)
            {
              //  print(value);
            }

        }
    }
    }
//}

