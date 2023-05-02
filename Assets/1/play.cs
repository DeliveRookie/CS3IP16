//using UnityEngine;
//using System.IO;
//using SpeechLib;

//public class play : MonoBehaviour
//{
//    public AudioSource audioSource;
//    public string text = "Hello world";
//    public string filePath = "Assets/Audio/speech.wav";

//    private SpVoice voice;

//    void Start()
//    {
//        SpFileStream stream = new SpFileStream();

//        // 设置音频文件名和保存格式
//        stream.Open("output.wav", SpeechStreamFileMode.SSFMCreateForWrite, true);
//        stream.Format.Type = SpeechAudioFormatType.SAFT22kHz16BitMono;

//        // 将SpeechLib生成的音频写入文件
//        SpeechVoiceSpeakFlags flags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
//        voice.Speak("Hello world", flags, out object streamNumber);
//        stream.Close();
//    }

    
//}
