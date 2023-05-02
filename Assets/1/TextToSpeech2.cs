using UnityEngine;
using System.Diagnostics;
using System.IO;

//public class TextToSpeech2 : MonoBehaviour
//{
//    public string textToSpeak = "你好，世界！"; // 要转换为语音的文本

//    private Process openMaryProcess; // OpenMary进程

//    private void Start()
//    {
//        // 创建OpenMary进程
//        openMaryProcess = new Process();
//        openMaryProcess.StartInfo.FileName = "java"; // 使用Java运行OpenMary
//        openMaryProcess.StartInfo.Arguments = "-jar openmary-standalone.jar"; // 指定OpenMary的JAR文件
//        openMaryProcess.StartInfo.WorkingDirectory = Application.dataPath + "/OpenMary/"; // 指定OpenMary的工作目录
//        openMaryProcess.StartInfo.UseShellExecute = false;
//        openMaryProcess.StartInfo.RedirectStandardInput = true;
//        openMaryProcess.StartInfo.RedirectStandardOutput = true;
//        openMaryProcess.StartInfo.RedirectStandardError = true;
//        openMaryProcess.OutputDataReceived += OnOutputDataReceived; // 处理标准输出流
//        openMaryProcess.ErrorDataReceived += OnErrorDataReceived; // 处理错误输出流
//        openMaryProcess.Start(); // 启动OpenMary进程
//        openMaryProcess.BeginOutputReadLine(); // 开始读取标准输出流
//        openMaryProcess.BeginErrorReadLine(); // 开始读取错误输出流
//        Speak(textToSpeak); // 开始转换文本为语音
//    }

//    private void Speak(string text)
//    {
//        StreamWriter inputWriter = openMaryProcess.StandardInput; // 获取OpenMary进程的标准输入流
//        inputWriter.WriteLine("INPUT_TEXT=" + text); // 发送要转换的文本
//        inputWriter.WriteLine("OUTPUT_TYPE=ALLOPHONES"); // 请求输出音素
//        inputWriter.WriteLine("LOCALE=zh_CN"); // 指定语言为中文（中国）
//        inputWriter.Flush(); // 刷新标准输入流
//        print(text);
//    }

//    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
//    {
//        if (e.Data != null && e.Data.StartsWith("ALLOPHONES"))
//        {
//            string[] allophones = e.Data.Substring(11).Split(' '); // 提取音素
//            foreach (string allophone in allophones)
//            {
//                if (allophone.StartsWith("v=")) // 如果是元音
//                {
//                    string vowel = allophone.Substring(2); // 提取元音
//                    //Debug.Log("元音: " + vowel); // 输出元音
//                    print("元音: " + vowel);
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
//            UnityEngine.Debug.LogError(e.Data); // 输出错误信息
//        }
//    }

//    private void OnDestroy()
//    {
//        if (openMaryProcess != null && !openMaryProcess.HasExited)
//        {
//            openMaryProcess.Kill(); // 关闭OpenMary进程
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
    public string text = "你好，世界！"; // 待转换的文本
    public string openMaryPath = "Assets/OpenMary/openmary-standalone.jar"; // OpenMary的路径

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
        // 输出语音音频数据
        if (outLine.Data != null && outLine.Data.Length > 0)
        {
            byte[] audioData = Convert.FromBase64String(outLine.Data);
            // TODO: 在这里处理音频数据，例如播放语音
        }
    }

    void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        // 输出错误信息
        if (outLine.Data != null && outLine.Data.Length > 0)
        {
            UnityEngine.Debug.LogError(outLine.Data);
        }
    }
}
