using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

[Serializable]
public class TokenResponse
{
    public string access_token = null;
}

public class GetAllAudio : MonoBehaviour
{
    private static GetAllAudio _instance;
    public static GetAllAudio instance { get { return _instance; } }
    #region 参数
    //static string APIKEY = "pPVyGxjiTNIXmVhrtKy9PWzF";
    //static string screctKey = "d5rQgVkA94q3tBbIGyO7Tj9lpDxo7PhC";
    static string APIKEY = "viUGfTb6RXCiSfZZDLH4qdg7";
    static string screctKey = "1mefAAY4NnSxlDFEuUOjZ54fzG07FwNB";
    static string token_url = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + APIKEY + "&client_secret=" + screctKey;
    static string baidu_url = "https://tsn.baidu.com/text2audio"; //百度语音合成接口
    public static GetParameter getParameter = new GetParameter();//请求参数
    #endregion
    #region 音频参数
    static Dictionary<int, byte[]> audioClipArray = new Dictionary<int, byte[]>();
    static TextAsset Audiotext_current;//音频文字
    static TextAsset Audiotext_backup;//音频文字
    static Dictionary<int, string> NeedLoadAudioText = new Dictionary<int, string>();
    static string[] audiotexts;
    public static string Dir_path = Application.streamingAssetsPath + "/Audio/";//音频储存路径
    public AudioSource audioSource;
    public string str;
    #endregion

    void Awake()
    {
        _instance = this;
    }
   
    public AudioClip audioClip;

    public static void shanchu()
    {
        if (Directory.Exists(Dir_path))
        {
            // 获取所有的音频文件
            string[] audioFiles = Directory.GetFiles(Dir_path, "*.wav");

            // 删除所有的音频文件
            foreach (string audioFile in audioFiles)
            {
                File.Delete(audioFile);
            }
        }
    }
    public IEnumerator IGetAudioClipInBaiduAndPlay()
    {
        UnityWebRequest unityWeb = UnityWebRequest.Get(Dir_path + "1.wav");
       // IfLoadOnAuto(OpenOrSleep.sleep);
        unityWeb.timeout = 5;
        yield return unityWeb.SendWebRequest();
       // IfLoadOnAuto(OpenOrSleep.open);
        if (unityWeb.isDone && unityWeb.error == null)
        {
            try
            {
                if (unityWeb.downloadHandler.data != null)
                {
                    WAV wav = new WAV(unityWeb.downloadHandler.data);
                    audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
                    audioClip.SetData(wav.LeftChannel, 0);
                   
                    audioSource.clip = audioClip;
                    audioSource.Play();
                    unityWeb.Dispose();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("音频转换出错！" + e.Message);
            }
        }
        else
        {
            Debug.Log("获取音频出错: " + unityWeb.error);
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(IGetAudio(str, Dir_path));
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(IGetAudioClipInBaiduAndPlay());
        }
    }
    static IEnumerator IGetAudio(string audios, string _path)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("当前设备或者服务器没有连接网络");
            yield break;
        }
        using (UnityWebRequest unityWeb = UnityWebRequest.Get(token_url))
        {
            yield return unityWeb.SendWebRequest();
            if (!string.IsNullOrEmpty(unityWeb.error))
            {
                Debug.Log("百度token获取失败！请重试: " + unityWeb.error);

                yield break;
            }
            try
            {
                var result = JsonUtility.FromJson<TokenResponse>(unityWeb.downloadHandler.text);
                if (result != null)
                {
                    // token = result.access_token;
                    getParameter.tok = result.access_token;
                }
            }
            catch (System.Exception)
            {
                Debug.Log("百度接口信息：" + unityWeb.downloadHandler.text);
            }
        }
        if (string.IsNullOrEmpty(getParameter.tok))
        {
            Debug.Log("百度token获取失败！请重试");
        }
        instance.StartCoroutine(AnalysisText(audios, _path));
    }
    static IEnumerator AnalysisText(string audios, string _path)
    {
        //for (int i = 0; i < audios.Length; i++)
        //{
        //    NeedLoadAudioText.Add(i + 1, audios[i].Split('\r')[0]);
        //}
        NeedLoadAudioText.Clear();
        NeedLoadAudioText.Add(1,audios.Split('\r')[0]);
        WWWForm form = new WWWForm();
        form.AddField("tok", getParameter.tok);
        form.AddField("cuid", getParameter.cuid = SystemInfo.deviceUniqueIdentifier);
        form.AddField("ctp", getParameter.ctp);
        form.AddField("lan", getParameter.lan);
        form.AddField("spd", getParameter.spd);//语速，取值0-15
        form.AddField("pit", getParameter.pit);//音调，取值0-15
        form.AddField("vol", getParameter.vol);//音量，取值0-15
        form.AddField("per", getParameter.per);//角色
        form.AddField("aue", getParameter.aue);//格式
        foreach (var item in NeedLoadAudioText)
        {
            form.AddField("tex", item.Value);
            UnityWebRequest unityWeb = UnityWebRequest.Post(baidu_url, form);
            yield return unityWeb.SendWebRequest();
            if (!string.IsNullOrEmpty(unityWeb.error))
            {
                Debug.Log(unityWeb.error);
                yield break;
            }
            if (unityWeb.downloadHandler.data != null)
            {
                WAV wav = null;
                try
                {
                    wav = new WAV(unityWeb.downloadHandler.data);
                }
                catch (System.Exception e)
                {
                    Debug.Log("获取音频出错: 步骤音频" + item.Key + e.Message);
                }
                if (wav != null)
                {

                    if (!audioClipArray.ContainsKey(item.Key))
                    {
                        // shanchu();
                        audioClipArray.Add(item.Key, unityWeb.downloadHandler.data);
                        Debug.Log("第" + item.Key + "个音频已下载");

                        string path = string.Format("{0}/{1}.{2}", _path, item.Key, getParameter.GetFormat());
                        //创建一个文件流
                        FileStream fs = new FileStream(path, FileMode.Create);
                        fs.Write(unityWeb.downloadHandler.data, 0, unityWeb.downloadHandler.data.Length);
                        //所有流类型都要关闭流，否则会出现内存泄露问题
                        fs.Close();
                        Debug.Log("第" + item.Key + "个音频已写入本地");
                    }
                }
            }
        }
        Debug.Log("音频写入完毕");
    }

    /// <summary>
    /// 请求参数
    /// </summary>
    public class GetParameter
    {
        public string tex;//必填,合成的文本，使用UTF-8编码。不超过60个汉字或者字母数字。文本在百度服务器内转换为GBK后，长度必须小于120字节。如需合成更长文本，推荐使用长文本在线合成
        public string tok;//必填,开放平台获取到的开发者access_token（见上面的“鉴权认证机制”段落）
        public string cuid;//必填,用户唯一标识，用来计算UV值。建议填写能区分用户的机器 MAC 地址或 IMEI 码，长度为60字符以内
        public string ctp = "1";//必填,客户端类型选择，web端填写固定值1
        public string lan = "zh";//必填,固定值zh。语言选择,目前只有中英文混合模式，填写固定值zh
        public string spd = "5";//语速，取值0-15，默认为5中语速
        public string pit = "5";//音调，取值0-15，默认为5中语调
        public string vol = "5";//音量，取值0-15，默认为5中音量（取值为0时为音量最小值，并非为无声）
        public string per = "0";//度小宇=1，度小美=0，度逍遥（基础）=3，度丫丫=4
        public string aue = "6";//3为mp3格式(默认)； 4为pcm-16k；5为pcm-8k；6为wav（内容同pcm-16k）; 注意aue=4或者6是语音识别要求的格式，但是音频内容不是语音识别要求的自然人发音，所以识别效果会受影响。

        public string GetFormat()
        {
            switch (aue)
            {
                case "3":
                    return "mp3";
                case "4":
                    return "pcm";
                case "5":
                    return "pcm";
                case "6":
                    return "wav";
                default:
                    return "wav";
            }
        }
    }
}
