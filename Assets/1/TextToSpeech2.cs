using UnityEngine;
using System.Diagnostics;
using System.IO;

//public class TextToSpeech2 : MonoBehaviour
//{
//    public string textToSpeak = "��ã����磡"; // Ҫת��Ϊ�������ı�

//    private Process openMaryProcess; // OpenMary����

//    private void Start()
//    {
//        // ����OpenMary����
//        openMaryProcess = new Process();
//        openMaryProcess.StartInfo.FileName = "java"; // ʹ��Java����OpenMary
//        openMaryProcess.StartInfo.Arguments = "-jar openmary-standalone.jar"; // ָ��OpenMary��JAR�ļ�
//        openMaryProcess.StartInfo.WorkingDirectory = Application.dataPath + "/OpenMary/"; // ָ��OpenMary�Ĺ���Ŀ¼
//        openMaryProcess.StartInfo.UseShellExecute = false;
//        openMaryProcess.StartInfo.RedirectStandardInput = true;
//        openMaryProcess.StartInfo.RedirectStandardOutput = true;
//        openMaryProcess.StartInfo.RedirectStandardError = true;
//        openMaryProcess.OutputDataReceived += OnOutputDataReceived; // �����׼�����
//        openMaryProcess.ErrorDataReceived += OnErrorDataReceived; // ������������
//        openMaryProcess.Start(); // ����OpenMary����
//        openMaryProcess.BeginOutputReadLine(); // ��ʼ��ȡ��׼�����
//        openMaryProcess.BeginErrorReadLine(); // ��ʼ��ȡ���������
//        Speak(textToSpeak); // ��ʼת���ı�Ϊ����
//    }

//    private void Speak(string text)
//    {
//        StreamWriter inputWriter = openMaryProcess.StandardInput; // ��ȡOpenMary���̵ı�׼������
//        inputWriter.WriteLine("INPUT_TEXT=" + text); // ����Ҫת�����ı�
//        inputWriter.WriteLine("OUTPUT_TYPE=ALLOPHONES"); // �����������
//        inputWriter.WriteLine("LOCALE=zh_CN"); // ָ������Ϊ���ģ��й���
//        inputWriter.Flush(); // ˢ�±�׼������
//        print(text);
//    }

//    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
//    {
//        if (e.Data != null && e.Data.StartsWith("ALLOPHONES"))
//        {
//            string[] allophones = e.Data.Substring(11).Split(' '); // ��ȡ����
//            foreach (string allophone in allophones)
//            {
//                if (allophone.StartsWith("v=")) // �����Ԫ��
//                {
//                    string vowel = allophone.Substring(2); // ��ȡԪ��
//                    //Debug.Log("Ԫ��: " + vowel); // ���Ԫ��
//                    print("Ԫ��: " + vowel);
//                }
//                else
//                {
//                    print(allophone);
//                }
//            }
//        }
//    }

//    private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
//    {
//        if (e.Data != null)
//        {
//            UnityEngine.Debug.LogError(e.Data); // ���������Ϣ
//        }
//    }

//    private void OnDestroy()
//    {
//        if (openMaryProcess != null && !openMaryProcess.HasExited)
//        {
//            openMaryProcess.Kill(); // �ر�OpenMary����
//            openMaryProcess = null;
//        }
//    }
//}


//using UnityEngine;
//using System.Diagnostics;
//using System.IO;
using System;

public class TextToSpeech2 : MonoBehaviour
{
    public string text = "��ã����磡"; // ��ת�����ı�
    public string openMaryPath = "Assets/OpenMary/openmary-standalone.jar"; // OpenMary��·��

    void Start()
    {
        string[] args = new string[] {
            "-jar",
            Application.dataPath + "/" + openMaryPath,
            "-loglevel",
            "ERROR",
            "-v",
            "local",
            "-o",
            "AUDIO",
            "-e",
            "UTF-8",
            "-f",
            "S16_LE",
            "-r",
            "16000",
            "-n",
            "audio",
            "zh-CN",
            text
        };

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "java";
        startInfo.Arguments = string.Join(" ", args);
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;

        Process process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
        process.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
    }

    void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        // ���������Ƶ����
        if (outLine.Data != null && outLine.Data.Length > 0)
        {
            byte[] audioData = Convert.FromBase64String(outLine.Data);
            // TODO: �����ﴦ����Ƶ���ݣ����粥������
        }
    }

    void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        // ���������Ϣ
        if (outLine.Data != null && outLine.Data.Length > 0)
        {
            UnityEngine.Debug.LogError(outLine.Data);
        }
    }
}
