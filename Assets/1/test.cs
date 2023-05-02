using UnityEngine;
using SpeechLib;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class test : MonoBehaviour
{
    [SerializeField] private string text = "Hello, world!你在哪里啊啊   啊啊    监控链接发坷拉激发了就"; // The text to be converted
                                                                                      // [SerializeField] private string filePath = "speech.wav"; // The file path where the audio is saved
    [SerializeField] public string filePath = "c:/aa/01.mp3"; // The file path where the audio is saved
    private SpVoice voice;
    public TMP_InputField text_input;
    public string result;
   // public InputField inputField;

    public void OnStart(string value)
    {
        voice = new SpVoice();

        // Create a SpFileStream object to hold the audio file
        SpFileStream stream = new SpFileStream();
        stream.Open(filePath, SpeechStreamFileMode.SSFMCreateForWrite, false);
        voice.AudioOutputStream = stream;

        // Send text to the SpeechLib API to write speech to SpFileStream
        //print(text);
        //print(value);
        voice.Speak(text_input.text.ToString());
        getyuanyin(text_input.text.ToString());
        // Close SpFileStream
        stream.Close();

        Debug.Log("Speech saved to " + filePath);
    }

    private void OnDestroy()
    {
        if (voice != null)
        {
            voice.Speak(string.Empty, SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            voice = null;
        }
    }

    //public string path = "c:/omar/song.mp3";
    //public AudioSource aud = null;

    //private void Play()
    //{
    //    print("play");
    //   // aud = this.GetComponent<AudioSource>();
    //    StartCoroutine(LoadSongCoroutine());
    //}
    //private IEnumerator LoadSongCoroutine()
    //{
    //    string url = string.Format("file://{0}", filePath);
    //    WWW www = new WWW(url);
    //    yield return www;
    //    print(www.text);
    //    aud.clip = NAudioPlayer.FromMp3Data(www.bytes);
    //    aud.Play();
    //}

    //public void Update()
    //{
    //    //if(Input.GetKeyUp(KeyCode.Space))
    //    //{
    //    //    Play();
    //    //}
    //}

    public void getyuanyin(string inputString)
    {
        // The string entered
        // string inputString = "我爱你的我的妈妈";

        // Contains all vowel letters
        string vowels = "aeiouAEIOU";

        // Stores extracted vowel letters
        result = "";

        // Iterate through each character of the input string
        for (int i = 0; i < inputString.Length; i++)
        {
            // If the current character is a vowel letter
            if (vowels.Contains(inputString[i].ToString()))
            {
                //  Adds the current character to the result string
                result += inputString[i];
            }
        }

        // Output result string
      print(result);

    }

}
