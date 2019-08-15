// 版权所有：
// 文 件  名：Common.cs
// 功能描述：
// 创建标识：Seven Song(m.sh.lin0328@163.com) 2014/1/19 11:28:48
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using TimedTask.Utility;

namespace TimedTask
{
    public class Helper
    {
        private static Helper _instance;
        private static readonly object _lock = new Object();
        private System.Windows.Media.MediaPlayer _player = new System.Windows.Media.MediaPlayer();

        #region 单一实例
        /// <summary>
        /// 
        /// </summary>
        private Helper()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        ~Helper()
        {
            Dispose();
        }
        /// <summary>
        /// 返回唯一实例
        /// </summary>
        public static Helper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        //if (_instance == null)
                        //{
                        _instance = new Helper();
                        //}
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 进程

        /// <summary>
        /// 杀死进程
        /// </summary>
        /// <param name="proccessName">进程名</param>
        public void EndApp(string proccessName)
        {
            if ((proccessName + "").Length == 0)
            {
                return;
            }
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == proccessName)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch (Exception e)
                    {
                        Log.SaveLog("Common KillProccess", e.ToString());
                    }
                }
            }
        }
        #endregion

        #region CMD

        /// <summary>
        /// 运行cmd命令
        /// </summary>
        /// <param name="command">cmd命令文本</param>
        public void Run(string command)
        {
            if (String.IsNullOrEmpty(command))
                return;
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                //p.StandardInput.WriteLine("shutdown -s -t 30 -c \"如果此时不想关机，就点击“取消关机”按钮！\"");
                //p.StandardInput.WriteLine("shutdown -s -t 30 -c \"如果此时不想关机，就点击“取消关机”按钮！\"");
                p.StandardInput.WriteLine(command);
                p.StandardInput.WriteLine("exit");
                string strRst = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                Log.SaveLog("Common Run", ex.ToString());
            }
        }
        #endregion

        #region 声音

        /// <summary>
        /// 文字转语音
        /// </summary>
        /// <param name="message">文字信息</param>
        /// <param name="speekType">声音类别 0：单词男声Sam,1:单词男声Mike,2:单词女声Mary,3:中文发音，如果是英文，就依单词字母一个一个发音</param>
        public void Speek(string message)
        {
            try
            {
                //引入COM组件:Microsoft speech object Library
                //SpeechLib.SpVoice voice = new SpeechLib.SpVoice();
                //voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
                //voice.Speak(message, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 停止播放声音
        /// </summary>
        public void StopAudio()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    if (_player != null)
                        _player.Stop();

                    _player = null;
                }
                catch (Exception ex)
                {
                    Log.SaveLog("Core Common_StopAudio", ex.ToString());
                }
            }));
        }
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="soundName">声音名</param>
        public void PalyAudio(string soundName, double volume)
        {
            if (String.IsNullOrEmpty(soundName) || soundName.Contains("无"))
                return;

            string path = TimedTask.Model.PM.AudioHt[soundName].ToString();
            if (!System.IO.File.Exists(path))
                return;
            /*
              SoundPlayer类特点
              1）仅支持.wav音频文件；
            2）不支持同时播放多个音频（任何新播放的操作将终止当前正在播放的）；
            3）无法控制声音的音量；
              if (path.EndsWith(".wav"))
              {
                  using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(path))
                  {
                      player.Play();//播放波形文件
                  }
              }
            */
            if (_player != null)
            {
                StopAudio();
            }
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (path.EndsWith(".mp3") || path.EndsWith(".wma") || path.EndsWith(".wav"))
                {
                    try
                    {
                        _player = new System.Windows.Media.MediaPlayer();
                        _player.Open(new Uri(path));
                        _player.Volume = volume / 100;//大小为0~1.0
                        _player.Play();
                        //_player.MediaOpened+=
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("Core Common_PalyAudio", ex.ToString());
                    }
                }
            }));
        }

        #endregion

        #region 获得程序版本

        /// <summary>
        /// 获得程序版本
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region 显示器

        private const uint WM_SYSCOMMAND = 0x0112;
        private const uint SC_MONITORPOWER = 0xF170;
        private readonly IntPtr HWND_HANDLE = new IntPtr(0xffff);
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, uint wParam, int lParam);

        /// <summary>
        /// 打开显示器
        /// </summary>
        public void OpenMonitor()
        {
            // 2 为关闭显示器， －1则打开显示器
            SendMessage(HWND_HANDLE, WM_SYSCOMMAND, SC_MONITORPOWER, -1);
        }
        /// <summary>
        /// 关闭显示器
        /// </summary>
        public void CloseMonitor()
        {
            // 2 为关闭显示器， －1则打开显示器
            SendMessage(HWND_HANDLE, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }
        #endregion

        #region IO

        /// <summary>
        /// 创建日志文件夹
        /// </summary>
        /// <param name="path"></param>
        public void CreateFolder(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (System.IO.IOException)
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    Log.SaveLog("Common CreateMatchFolder", e.ToString());
                }
            }
        }
        /// <summary>
        /// 递归删除文件夹及其下的所有文件
        /// </summary>
        /// <param name="path"></param>
        public void DeleteFolder(string path)
        {
            if (System.IO.Directory.Exists(path)) //如果存在这个文件夹删除之 
            {
                foreach (string d in System.IO.Directory.GetFileSystemEntries(path))
                {
                    if (System.IO.File.Exists(d))
                        System.IO.File.Delete(d); //直接删除其中的文件                        
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                System.IO.Directory.Delete(path, true); //删除已空文件夹                 
            }
        }
        #endregion

        #region 删除x天前的文件

        /// <summary>
        /// 删除x天前创建的文件  2013-03-26 mashanlin 添加<br/>
        /// </summary>
        /// <param name="suffix">文件名中包含</param>
        /// <param name="suffixArray">文件后缀名 如：.rar  .doc</param>
        /// <param name="days">x天前的</param>
        public void DropFiles(string filepath, string suffix, string[] suffixArray, int days)
        {
            if (filepath == "")
                return;
            string path = "";
            foreach (string p in GetAllDir(filepath, suffixArray))
            {
                path = filepath + p;
                if (!String.IsNullOrEmpty(suffix) && !p.Contains(suffix))
                    continue;

                System.IO.FileInfo f = new System.IO.FileInfo(path);
                if (f.Exists && f.CreationTime < DateTime.Now.AddDays(-(days)))
                {
                    try
                    {
                        f.Delete();
                    }
                    catch
                    {
                        //Response.Write("<script>alert('删除出错！');                }
                    }
                }
            }
        }

        /// <summary> 
        /// 获取指定目录和扩展名的所有文件  2013-03-26 mashanlin 添加
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="extensionArray">文件后缀数组</param>
        /// <returns></returns>
        public List<String> GetAllDir(string path, string[] extensionArray)
        {
            List<String> list = new List<string>();
            if (System.IO.Directory.Exists(path))
            {
                string[] fileList = System.IO.Directory.GetFileSystemEntries(path);
                list.Clear();
                foreach (string f in fileList)
                {
                    string filename = System.IO.Path.GetFileName(f);
                    string strExtension = System.IO.Path.GetExtension(System.IO.Path.GetFullPath(f)).ToLower();
                    if (extensionArray == null || extensionArray.Length == 0)
                    {
                        list.Add(filename);
                        continue;
                    }
                    foreach (string ss in extensionArray)
                    {
                        if (string.Equals(strExtension, ss, StringComparison.CurrentCultureIgnoreCase) || string.Equals(strExtension, ss.StartsWith(".") ? ss : "." + ss, StringComparison.CurrentCultureIgnoreCase))
                        {
                            list.Add(filename);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region 启动外部程序

        /// <summary>
        /// 启动外部Windows应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <returns></returns>
        public bool StartApp(string appName)
        {
            return StartApp(appName, null, System.Diagnostics.ProcessWindowStyle.Normal);
        }

        /// <summary>
        /// 启动外部应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="arguments">启动参数</param>
        /// <param name="style">进程窗口模式</param>
        /// <returns></returns>
        public bool StartApp(string appName, string arguments, System.Diagnostics.ProcessWindowStyle style)
        {
            bool result = false;
            using (System.Diagnostics.Process p = new System.Diagnostics.Process())
            {
                p.StartInfo.FileName = appName;//exe,bat and so on
                p.StartInfo.WindowStyle = style;
                p.StartInfo.Arguments = arguments;
                try
                {
                    p.Start();
                    p.WaitForExit();
                    p.Close();
                    result = true;
                }
                catch
                {
                }
            }
            return result;
        }
        #endregion

        #region 读文件
        /// <summary>
        /// 读取本地文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public string ReadFile(string path)
        {
            return ReadFile(path, null);
        }
        /// <summary>
        /// 读取本地文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="pageEncode">编码：GB2312\utf-8 可空默认GB2312</param>
        /// <returns></returns>
        public string ReadFile(string path, string pageEncode)
        {
            string result = "";
            if (String.IsNullOrEmpty(pageEncode))
            {
                pageEncode = "GB2312";
            }
            try
            {
                if (System.IO.File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(pageEncode)))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }

            return result;
        }

        #endregion

        #region 写文件
        /// <summary>
        /// 写文件（当文件不存时，则创建文件，并追加文件）
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public void WriteFile(string path, string content)
        {
            if (!System.IO.File.Exists(path))
            {
                System.IO.FileStream f = System.IO.File.Create(path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(path, true, System.Text.Encoding.UTF8);
            f2.WriteLine(content);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        #region 传回阳历y年m月的总天数

        /// <summary>
        /// 传回阳历y年m月的总天数
        /// </summary>
        public int GetDaysByMonth(int y, int m)
        {
            int[] days = new int[] { 31, DateTime.IsLeapYear(y) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            return days[m - 1];
        }
        #endregion

        #region 通用数据截取

        /// <summary>
        /// 通用数据截取 开始标签支持正则
        /// </summary>
        /// <param name="input">要截取的字符串</param>
        /// <param name="startTag">开始代码</param>
        /// <param name="endTag">结束代码</param>
        /// <param name="enableRegular">是否启用正则</param>
        /// <returns></returns>
        public string CutString(string input, string startTag, string endTag, bool enableRegular)
        {
            int startPos = 0;
            //使用正则
            if (!input.Contains(startTag) && enableRegular)
            {
                MatchCollection mc = Regex.Matches(input, startTag, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                foreach (Match m in mc)
                {
                    if (!String.IsNullOrEmpty(m.Value))//如果起始标签不唯一的话会使用第一个找到的起始标签
                    {
                        startTag = m.Value;
                        break;
                    }
                }
            }
            startPos = input.IndexOf(startTag);//开始位置
            string returnValue = "";
            int start = startPos + startTag.Length;
            if (startPos == -1) return "";
            if (startTag == "0") start = 0;
            try
            {
                int end = input.IndexOf(endTag, start) - start;
                if (endTag == "0") end = input.Length;
                returnValue = input.Substring(start, end).Trim();
            }
            catch (Exception)
            {

            }
            return returnValue;
        }

        #endregion

        #region 多个匹配内容

        /// <summary>
        /// 多个匹配内容
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="pattern">表达式字符串</param>
        /// <param name="groupName">分组名, ""代表不分组</param>
        public List<string> GetList(string input, string pattern, string groupName)
        {
            List<string> list = new List<string>();
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);//| RegexOptions.IgnorePatternWhitespace
            MatchCollection mcs = re.Matches(input);
            foreach (Match mc in mcs)
            {
                if (groupName != "")
                {
                    list.Add(mc.Groups[groupName].Value);
                }
                else
                {
                    list.Add(mc.Value);
                }
            }
            return list;
        }

        /// <summary>
        /// 多个匹配内容
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="pattern">表达式字符串</param>
        /// <param name="groupIndex">第几个分组, 从1开始, 0代表不分组</param>
        public List<string> GetList(string input, string pattern, int groupIndex)
        {
            List<string> list = new List<string>();
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            MatchCollection mcs = re.Matches(input);
            foreach (Match mc in mcs)
            {
                if (groupIndex > 0)
                {
                    list.Add(mc.Groups[groupIndex].Value);
                }
                else
                {
                    list.Add(mc.Value);
                }
            }
            return list;
        }
        #endregion

        #region 文件删除

        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="FileFullName">物理路径文件名</param>
        /// <returns></returns>
        public bool DeleteFile(string fileFullName)
        {
            FileInfo fi = new FileInfo(fileFullName);
            if (fi.Exists)
            {
                try
                {
                    fi.Delete();
                    return true;

                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region JSON转对象

        ///<summary>
        /// JSON序列化
        ///</summary>
        public string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        ///<summary>
        /// JSON反序列化
        ///</summary>
        public T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T obj = (T)ser.ReadObject(ms);
                return obj;
            }
        }
        #endregion

        /// <summary>
        /// 获取配置项值
        /// </summary>
        /// <param name="NodeTag">节点名</param>
        /// <returns></returns>
        public string GetValue(XmlHelper xml, string NodeTag)
        {
            string obj = xml.SelectNodeText("Configuration/" + NodeTag);
            if (obj == null)
                obj = String.Empty;

            return obj.Trim();
        }
        /// <summary>
        /// 设置配置项值
        /// </summary>
        /// <param name="NodeTag">节点名</param>
        /// <param name="nodeValue"></param>
        /// <returns></returns>
        public void SetValue(XmlHelper xml, string NodeTag, string nodeValue)
        {
            try
            {
                xml.SetXmlNodeValue("Configuration/" + NodeTag, nodeValue);
            }
            catch (Exception)
            {

            }
        }
    }
}
