//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using SpeechLib;

//public class TextToSpeech : MonoBehaviour
//{
//    public TMP_InputField inputTxt;
//    public Button btn;
//    private SpVoice voice;
//    public AudioClip clicp;

//    void Awake()
//    {
//        voice = new SpVoice();
//        btn.onClick.AddListener(VoiceBtn);
//    }

//    public void VoiceBtn()
//    {
//        voice.Speak(inputTxt.text);
//       // clicp = (AudioClip)voice.AudioOutput();
//        // AudioClip audioClip = voice.GetAudioOutputs(inputTxt.text);
//    }

//    //public
//}
//using System.IO;
//using System.Speech.Synthesis;
//using UnityEngine;

//public class TextToSpeech : MonoBehaviour
//{
//    public string textToConvert = "Hello, world!"; // 要转换为语音的文本
//    public AudioSource audioSource; // 音频源组件

//    public void ConvertTextToSpeech()
//    {
//        using (var speechSynthesizer = new SpeechSynthesizer())
//        {
//            // 将文本转换为音频
//            using (var memoryStream = new MemoryStream())
//            {
//                speechSynthesizer.SetOutputToWaveStream(memoryStream);
//                speechSynthesizer.Speak(textToConvert);
//                memoryStream.Seek(0, SeekOrigin.Begin);

//                // 将音频数据加载到AudioSource中进行播放
//                var audioClip = WavUtility.ToAudioClip(memoryStream.ToArray());
//                audioSource.clip = audioClip;
//                audioSource.Play();
//            }
//        }
//    }
//}
