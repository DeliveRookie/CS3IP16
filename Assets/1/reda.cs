//using UnityEngine;
//using System.IO;
//using NAudio.Wave;

//public class PlaySavedSpeech : MonoBehaviour
//{
//    [SerializeField] private string filePath = "speech.wav"; // 保存音频的文件路径
//    private AudioSource audioSource;

//    private void Start()
//    {
//        // 创建AudioSource对象
//        audioSource = gameObject.AddComponent<AudioSource>();

//        // 从文件读取音频数据
//        byte[] audioData = File.ReadAllBytes(filePath);
//        int sampleRate = 16000; // 采样率
//        int channels = 1; // 声道数
//        WaveFormat waveFormat = new WaveFormat(sampleRate, channels);
//        // 创建AudioClip对象
//        AudioClip audioClip = WavUtility.ToAudioClip(audioData);

//        // 将AudioClip设置为AudioSource的clip属性值
//        audioSource.clip = audioClip;

//        // 播放音频
//        audioSource.Play();

//        // 输出元音
//        string text = "hello, world!";
//        foreach (char c in text.ToLower())
//        {
//            if ("aeiou".Contains(c))
//            {
//                Debug.Log("Vowel: " + c);
//            }
//        }
//    }
//}
