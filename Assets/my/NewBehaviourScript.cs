//using System.Collections;
//using UnityEngine;
//using UnityEngine.Networking;

//public class TextToSpeechMy : MonoBehaviour
//{
//    public string apiKey = "<your-api-key>"; // API��Կ
//    public string endpoint = "<your-endpoint>"; // API�˵�URL
//    public string textToConvert = "Hello, world!"; // Ҫת��Ϊ�������ı�
//    public AudioSource audioSource; // ��ƵԴ���

//    private const string SSMLTemplate = "<speak version='1.0' xml:lang='en-US'><voice xml:lang='en-US' xml:gender='Female' name='en-US-AriaNeural'>{0}</voice></speak>"; // SSMLģ��

//    public void ConvertTextToSpeech()
//    {
//        StartCoroutine(ConvertTextToSpeechCoroutine());
//    }

//    private IEnumerator ConvertTextToSpeechCoroutine()
//    {
//        var ssml = string.Format(SSMLTemplate, textToConvert); // ��ʽ��SSML
//        var request = CreateRequest(ssml); // ����Web����

//    //    using (var response = (UnityWebRequest)request.SendWebRequest()) // ����Web����
//        using (var response =(UnityWebRequest) request.SendWebRequest()) // ����Web����
//        {
//            {
//            yield return response.SendWebRequest(); // �ȴ���Ӧ

//            if (response.result == UnityWebRequest.Result.ConnectionError || response.result == UnityWebRequest.Result.ProtocolError)
//            {
//                Debug.LogError(response.error); // �������
//                yield break;
//            }

//            var audioClip = DownloadHandlerAudioClip.GetContent(response); // ����Ӧ�л�ȡ��Ƶ����
//            audioSource.clip = audioClip; // ����Ƶ�����������ƵԴ
//            audioSource.Play(); // ������Ƶ
//        }
//    }

//    private UnityWebRequest CreateRequest(string ssml)
//    {
//        var url = $"{endpoint}/cognitiveservices/v1";
//        var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
//        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
//        request.SetRequestHeader("Content-Type", "application/ssml+xml");
//        var requestBodyRaw = System.Text.Encoding.UTF8.GetBytes(ssml);
//        request.uploadHandler = new UploadHandlerRaw(requestBodyRaw);
//        request.downloadHandler = new DownloadHandlerAudioClip(url, AudioType.WAV);
//        return request;
//    }
//}
