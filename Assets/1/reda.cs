//using UnityEngine;
//using System.IO;
//using NAudio.Wave;

//public class PlaySavedSpeech : MonoBehaviour
//{
//    [SerializeField] private string filePath = "speech.wav"; // ������Ƶ���ļ�·��
//    private AudioSource audioSource;

//    private void Start()
//    {
//        // ����AudioSource����
//        audioSource = gameObject.AddComponent<AudioSource>();

//        // ���ļ���ȡ��Ƶ����
//        byte[] audioData = File.ReadAllBytes(filePath);
//        int sampleRate = 16000; // ������
//        int channels = 1; // ������
//        WaveFormat waveFormat = new WaveFormat(sampleRate, channels);
//        // ����AudioClip����
//        AudioClip audioClip = WavUtility.ToAudioClip(audioData);

//        // ��AudioClip����ΪAudioSource��clip����ֵ
//        audioSource.clip = audioClip;

//        // ������Ƶ
//        audioSource.Play();

//        // ���Ԫ��
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
