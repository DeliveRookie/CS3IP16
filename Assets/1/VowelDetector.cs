//using UnityEngine;
//using UnityEngine.Audio;
//using UnityEngine.Experimental.Audio;


//public class VowelDetector : MonoBehaviour
//{
//    public AudioMixerGroup mixerGroup; // 混音器组
//    public int sampleRate = 44100; // 采样率
//    public float minPitch = 80f; // 最小基本频率
//    public float maxPitch = 400f; // 最大基本频率
//    public float vowelThreshold = 0.1f; // 元音阈值

//    private AudioGraphOutput output; // 音频输出
//    private AudioBuffer buffer; // 音频缓冲区
//    private float[] spectrum; // 频谱数据

//    void Start()
//    {
//        // 创建DSPGraph
//        AudioGraph graph = AudioGraph.Create("Vowel Detector", 2, 1, sampleRate);

//        // 创建输出节点
//        output = graph.Output(0, 2);

//        // 创建缓冲区
//        buffer = new AudioBuffer(2, sampleRate / 10);

//        // 创建混音器
//        AudioMixer mixer = new AudioMixer(2, 1, sampleRate / 10);

//        // 创建输入节点
//        AudioGraphNode input = graph.AddNode("Input", AudioNodeType.Output, 1);
//        input.SetBuffer(0, buffer);

//        // 创建混音器节点
//        AudioGraphNode mixerNode = graph.AddNode("Mixer", AudioNodeType.Mixer, 1);
//        mixerNode.SetMixer(mixer);

//        // 连接节点
//        input.Connect(0, mixerNode, 0);

//        // 设置混音器组
//        mixer.SetOutput(0, mixerGroup);

//        // 开始DSPGraph
//        graph.Start();

//        // 初始化频谱数据
//        spectrum = new float[sampleRate / 2];
//    }

//    void Update()
//    {
//        // 获取音频数据
//        int samples = output.Read(buffer, buffer.samples);

//        // 计算频谱数据
//        DSPUtilities.GetSpectrumData(buffer, spectrum);

//        // 计算基本频率
//        float pitch = DSPUtilities.FindPitch(spectrum, minPitch, maxPitch);

//        // 判断元音
//        if (pitch > 0)
//        {
//            // 计算元音强度
//            float vowelIntensity = DSPUtilities.GetBandEnergy(spectrum, pitch, 50f);

//            // 判断元音
//            if (vowelIntensity > vowelThreshold)
//            {
//                // 根据基本频率判断元音
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
//        // 停止DSPGraph
//        output?.Dispose();
//    }
//}

using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class VowelDetector : MonoBehaviour
{
   // public AudioClip audioClip; // 要分析的音频剪辑
    public AudioMixerGroup outputMixerGroup; // AudioSource 的输出音频混合器组
    public float vowelThreshold = 0.1f; // 元音的振幅阈值

    private AudioSource audioSource;
    [SerializeField]
    public List<float[]> vowelSegments = new List<float[]>(); // 元音音频片段的列表

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
        vowelSegments.Clear(); // 清空元音音频片段列表

            //// 获取音频数据
            //int numSamples = audioClip.samples * audioClip.channels;
            //float[] audioData = new float[numSamples];
            //audioClip.GetData(audioData, 0);
        // 打印前 100 个音频采样数据
        for (int i = 0; i < 100; i++)
        {
            Debug.Log("Sample " + i + ": " + audioData[i]);
        }
        // 分析音频数据中的元音声音
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

                        // 提取元音音频片段
                        for (int j = vowelStartIndex; j < vowelEndIndex; j += channels)
                        {
                            vowelSegment[j - vowelStartIndex] = audioData[j];
                        }

                        vowelSegments.Add(vowelSegment); // 添加元音音频片段到列表中
                    }
                }
            }

        // 根据频率范围对元音声音进行分类
        foreach (float[] segment in vowelSegments)
        {

            // 找到接近的最小幂次方值
            int length = 1;
            while (length < segment.Length)
            {
                length <<= 1; // 左移相当于乘以 2
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

            // 根据最大频率和幅值输出元音字母
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

