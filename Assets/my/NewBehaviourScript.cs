//using System.Collections;
//using UnityEngine;
//using UnityEngine.Networking;

//public class TextToSpeechMy : MonoBehaviour
//{
//    public string apiKey = "<your-api-key>"; // API密钥
//    public string endpoint = "<your-endpoint>"; // API端点URL
//    public string textToConvert = "Hello, world!"; // 要转换为语音的文本
//    public AudioSource audioSource; // 音频源组件

//    private const string SSMLTemplate = "<speak version='1.0' xml:lang='en-US'><voice xml:lang='en-US' xml:gender='Female' name='en-US-AriaNeural'>{0}</voice></speak>"; // SSML模板

//    public void ConvertTextToSpeech()
//    {
//        StartCoroutine(ConvertTextToSpeechCoroutine());
//    }

//    private IEnumerator ConvertTextToSpeechCoroutine()
//    {
//        var ssml = string.Format(SSMLTemplate, textToConvert); // 格式化SSML
//        var request = CreateRequest(ssml); // 创建Web请求

//    //    using (var response = (UnityWebRequest)request.SendWebRequest()) // 发送Web请求
//        using (var response =(UnityWebRequest) request.SendWebRequest()) // 发送Web请求
//        {
//            {
//            yield return response.SendWebRequest(); // 等待响应

//            if (response.result == UnityWebRequest.Result.ConnectionError || response.result == UnityWebRequest.Result.ProtocolError)
//            {
//                Debug.LogError(response.error); // 输出错误
//                yield break;
//            }

//            var audioClip = DownloadHandlerAudioClip.GetContent(response); // 从响应中获取音频剪辑
//            audioSource.clip = audioClip; // 将音频剪辑分配给音频源
//            audioSource.Play(); // 播放音频
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
