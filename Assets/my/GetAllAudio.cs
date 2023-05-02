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
    #region ����
    //static string APIKEY = "pPVyGxjiTNIXmVhrtKy9PWzF";
    //static string screctKey = "d5rQgVkA94q3tBbIGyO7Tj9lpDxo7PhC";
    static string APIKEY = "viUGfTb6RXCiSfZZDLH4qdg7";
    static string screctKey = "1mefAAY4NnSxlDFEuUOjZ54fzG07FwNB";
    static string token_url = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=" + APIKEY + "&client_secret=" + screctKey;
    static string baidu_url = "https://tsn.baidu.com/text2audio"; //�ٶ������ϳɽӿ�
    public static GetParameter getParameter = new GetParameter();//�������
    #endregion
    #region ��Ƶ����
    static Dictionary<int, byte[]> audioClipArray = new Dictionary<int, byte[]>();
    static TextAsset Audiotext_current;//��Ƶ����
    static TextAsset Audiotext_backup;//��Ƶ����
    static Dictionary<int, string> NeedLoadAudioText = new Dictionary<int, string>();
    static string[] audiotexts;
    public static string Dir_path = Application.streamingAssetsPath + "/Audio/";//��Ƶ����·��
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
            // ��ȡ���е���Ƶ�ļ�
            string[] audioFiles = Directory.GetFiles(Dir_path, "*.wav");

            // ɾ�����е���Ƶ�ļ�
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
                Debug.Log("��Ƶת������" + e.Message);
            }
        }
        else
        {
            Debug.Log("��ȡ��Ƶ����: " + unityWeb.error);
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
            Debug.Log("��ǰ�豸���߷�����û����������");
            yield break;
        }
        using (UnityWebRequest unityWeb = UnityWebRequest.Get(token_url))
        {
            yield return unityWeb.SendWebRequest();
            if (!string.IsNullOrEmpty(unityWeb.error))
            {
                Debug.Log("�ٶ�token��ȡʧ�ܣ�������: " + unityWeb.error);

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
                Debug.Log("�ٶȽӿ���Ϣ��" + unityWeb.downloadHandler.text);
            }
        }
        if (string.IsNullOrEmpty(getParameter.tok))
        {
            Debug.Log("�ٶ�token��ȡʧ�ܣ�������");
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
        form.AddField("spd", getParameter.spd);//���٣�ȡֵ0-15
        form.AddField("pit", getParameter.pit);//������ȡֵ0-15
        form.AddField("vol", getParameter.vol);//������ȡֵ0-15
        form.AddField("per", getParameter.per);//��ɫ
        form.AddField("aue", getParameter.aue);//��ʽ
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
                    Debug.Log("��ȡ��Ƶ����: ������Ƶ" + item.Key + e.Message);
                }
                if (wav != null)
                {

                    if (!audioClipArray.ContainsKey(item.Key))
                    {
                        // shanchu();
                        audioClipArray.Add(item.Key, unityWeb.downloadHandler.data);
                        Debug.Log("��" + item.Key + "����Ƶ������");

                        string path = string.Format("{0}/{1}.{2}", _path, item.Key, getParameter.GetFormat());
                        //����һ���ļ���
                        FileStream fs = new FileStream(path, FileMode.Create);
                        fs.Write(unityWeb.downloadHandler.data, 0, unityWeb.downloadHandler.data.Length);
                        //���������Ͷ�Ҫ�ر��������������ڴ�й¶����
                        fs.Close();
                        Debug.Log("��" + item.Key + "����Ƶ��д�뱾��");
                    }
                }
            }
        }
        Debug.Log("��Ƶд�����");
    }

    /// <summary>
    /// �������
    /// </summary>
    public class GetParameter
    {
        public string tex;//����,�ϳɵ��ı���ʹ��UTF-8���롣������60�����ֻ�����ĸ���֡��ı��ڰٶȷ�������ת��ΪGBK�󣬳��ȱ���С��120�ֽڡ�����ϳɸ����ı����Ƽ�ʹ�ó��ı����ߺϳ�
        public string tok;//����,����ƽ̨��ȡ���Ŀ�����access_token��������ġ���Ȩ��֤���ơ����䣩
        public string cuid;//����,�û�Ψһ��ʶ����������UVֵ��������д�������û��Ļ��� MAC ��ַ�� IMEI �룬����Ϊ60�ַ�����
        public string ctp = "1";//����,�ͻ�������ѡ��web����д�̶�ֵ1
        public string lan = "zh";//����,�̶�ֵzh������ѡ��,Ŀǰֻ����Ӣ�Ļ��ģʽ����д�̶�ֵzh
        public string spd = "5";//���٣�ȡֵ0-15��Ĭ��Ϊ5������
        public string pit = "5";//������ȡֵ0-15��Ĭ��Ϊ5�����
        public string vol = "5";//������ȡֵ0-15��Ĭ��Ϊ5��������ȡֵΪ0ʱΪ������Сֵ������Ϊ������
        public string per = "0";//��С��=1����С��=0������ң��������=3����ѾѾ=4
        public string aue = "6";//3Ϊmp3��ʽ(Ĭ��)�� 4Ϊpcm-16k��5Ϊpcm-8k��6Ϊwav������ͬpcm-16k��; ע��aue=4����6������ʶ��Ҫ��ĸ�ʽ��������Ƶ���ݲ�������ʶ��Ҫ�����Ȼ�˷���������ʶ��Ч������Ӱ�졣

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
