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

//        // ������Ƶ�ļ����ͱ����ʽ
//        stream.Open("output.wav", SpeechStreamFileMode.SSFMCreateForWrite, true);
//        stream.Format.Type = SpeechAudioFormatType.SAFT22kHz16BitMono;

//        // ��SpeechLib���ɵ���Ƶд���ļ�
//        SpeechVoiceSpeakFlags flags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
//        voice.Speak("Hello world", flags, out object streamNumber);
//        stream.Close();
//    }

    
//}
