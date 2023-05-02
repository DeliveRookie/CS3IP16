using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BaiduTextToSpeech : MonoBehaviour
{
    public AudioSource audioSource;
    public string textToConvert = "��ã����磡"; // Ҫת��Ϊ�������ı�//30556175
    private string appId = "30556175"; // ���İٶ�APIӦ��ID
    private string apiKey = "viUGfTb6RXCiSfZZDLH4qdg7"; // ���İٶ�API��Կ viUGfTb6RXCiSfZZDLH4qdg7
    private string secretKey = "1mefAAY4NnSxlDFEuUOjZ54fzG07FwNB"; // ���İٶ�API��Կ

    private const string TtsUrl = "https://tsn.baidu.com/text2audio";
    public AudioClip audioClip;
    private IEnumerator Start()
    {
        // �����������
        string lang = "zh"; // ���Դ��룬����Ϊ"zh"
        int speed = 5; // ���٣�ȡֵ��ΧΪ1-15��Ĭ��Ϊ5
        int pitch = 5; // ������ȡֵ��ΧΪ1-15��Ĭ��Ϊ5
        int volume = 5; // ������ȡֵ��ΧΪ1-15��Ĭ��Ϊ5
        string cuid = SystemInfo.deviceUniqueIdentifier; // �û�Ψһ��ʶ������ʹ���豸ID
        string token = GetAccessToken(apiKey, secretKey); // ��ȡ�ٶ�API��������

        // ��������URL
        string url = $"{TtsUrl}?tex={textToConvert}&lan={lang}&cuid={cuid}&ctp=1&tok={token}&spd={speed}&pit={pitch}&vol={volume}";

        //��������������Ƶ�ļ�
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // ������Ƶ�ļ�
                //AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                WAV wav = new WAV(request.downloadHandler.data);
                audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
                audioClip.SetData(wav.LeftChannel, 0);

                audioSource.clip = audioClip;

                print("���!");
            }
            else
            {
                Debug.LogError($"TTS failed: {request.error}");
            }
        }


        //  StartCoroutine(DownloadAndPlayAudio(url));
    }
    //IEnumerator DownloadAndPlayAudio(string url)
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(url);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError("Failed to download audio file: " + www.error);
    //        yield break;
    //    }

    //    // ͨ��������Ƶ���ݴ���һ���µ�AudioClip����
    //    audioClip = AudioClip.Create("audioClip", www.downloadHandler.audioClip.samples, www.downloadHandler.audioClip.channels, www.downloadHandler.audioClip.frequency, false);
    //    audioClip.SetData(www.downloadHandler.audioClip.GetData(), 0);

    //    // ������Ƶ
    //    AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
    //}

    //private AudioClip audioClip;

    //IEnumerator DownloadAndPlayAudio(string url)
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(url);
    //    DownloadHandlerAudioClip audioClipHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);
    //    www.downloadHandler = audioClipHandler;
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError("Failed to download audio file: " + www.error);
    //        yield break;
    //    }

    //    // ��ȡ��Ƶ���ݲ�����һ���µ�AudioClip����
    //    audioClip = audioClipHandler.audioClip;

    //    // ������Ƶ
    //    AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);

    //}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }
    }

    // ��ȡ�ٶ�API��������
    private string GetAccessToken(string apiKey, string secretKey)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={apiKey}&client_secret={secretKey}"))
        {
            request.SendWebRequest();

            while (!request.isDone)
            {
                // �ȴ��������
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                AccessTokenResponse tokenResponse = JsonUtility.FromJson<AccessTokenResponse>(response);
                return tokenResponse.access_token;
            }
            else
            {
                Debug.LogError($"Failed to get access token: {request.error}");
                return null;
            }
        }
    }

    // �ٶ�API����������Ӧ������
    [System.Serializable]
    private class AccessTokenResponse
    {
        public string access_token;
        public string expires_in;
        public string refresh_token;
        public string scope;
        public string session_key;
        public string session_secret;
    }
}

public class WAV
{
    // convert two bytes to one float in the range -1 to 1
    static float bytesToFloat(byte firstByte, byte secondByte)
    {
        // convert two bytes to one short (little endian)
        short s = (short)((secondByte << 8) | firstByte);
        // convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }

    static int bytesToInt(byte[] bytes, int offset = 0)
    {
        int value = 0;
        for (int i = 0; i < 4; i++)
        {
            value |= ((int)bytes[offset + i]) << (i * 8);
        }
        return value;
    }
    // properties
    public float[] LeftChannel { get; internal set; }
    public float[] RightChannel { get; internal set; }
    public int ChannelCount { get; internal set; }
    public int SampleCount { get; internal set; }
    public int Frequency { get; internal set; }

    public WAV(byte[] wav)
    {
        try
        {
            // Determine if mono or stereo
            ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

            // Get the frequency
            Frequency = bytesToInt(wav, 24);

            // Get past all the other sub chunks to get to the data subchunk:
            int pos = 12;   // First Subchunk ID from 12 to 16

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            pos += 8;

            // Pos is now positioned to start of actual sound data.
            SampleCount = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
            if (ChannelCount == 2) SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)

            // Allocate memory (right will be null if only mono sound)
            LeftChannel = new float[SampleCount];
            if (ChannelCount == 2) RightChannel = new float[SampleCount];
            else RightChannel = null;

            // Write to double array/s:
            int i = 0;
            while (pos < wav.Length)
            {
                LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
                if (ChannelCount == 2)
                {
                    RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                    pos += 2;
                }
                i++;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public override string ToString()
    {
        return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", LeftChannel, RightChannel, ChannelCount, SampleCount, Frequency);
    }
}

