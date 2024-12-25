using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discus.DistributeCalculate.From
{
    public partial class Form1 : Form
    {
        private static Process _process = null;  // 声明一个全局变量来存储进程

        public Form1()
        {
            InitializeComponent();
            IPtextBox.Text = "192.168.32.133";
            PorttextBox.Text = "8080";
            this.AcceptButton = start_button;
        }

        private void Start_Button_Click(object sender, EventArgs e)
        {
            var host = IPtextBox.Text.Trim();
            var port = PorttextBox.Text.Trim();
            WriteLog(host + ":" + port);
            DoExecutor();
        }

        private void DoExecutor()
        {
            var filePath = Directory.GetCurrentDirectory();
            string exePath = Path.Combine(filePath, "DistributeCalculate", "Discus.DistributeCalculate.exe");

            if (!File.Exists(exePath))
            {
                WriteLog($"错误：找不到可执行文件: {exePath}");
                return;
            }

            // 创建进程启动信息
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"--urls=http://{IPtextBox.Text.Trim()}:{PorttextBox.Text.Trim()}",
                UseShellExecute = false,  // 保持为 true，因为我们需要以正确的权限运行程序
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = Path.GetDirectoryName(exePath),
                RedirectStandardOutput = true,  // 重定向标准输出
                RedirectStandardError = true
            };

            WriteLog($"启动参数: {startInfo.Arguments}");
            WriteLog($"工作目录: {startInfo.WorkingDirectory}");

            try
            {
                _process = new Process { StartInfo = startInfo };

                // 移除输出和错误流的事件处理
                WriteLog("正在启动进程...");
                bool started = _process.Start();
                WriteLog($"进程启动状态: {started}, 进程ID: {_process.Id}");


                // 开始异步读取输出
                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();

                WriteLog($"程序启动成功，执行路径: {exePath}");
            }
            catch (Exception ex)
            {
                WriteLog($"启动程序时出错: {ex.Message}");
                WriteLog($"详细错误信息: {ex.ToString()}");
            }
        }

        // 程序退出时的处理方法
        public static void OnProcessExit(object sender, EventArgs e)
        {
            if (_process != null && !_process.HasExited)
            {
                try
                {
                    _process.Kill();  // 强制关闭进程
                    _process.WaitForExit(); // 等待进程完全退出
                    _process.Dispose();  // 释放资源
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"关闭进程时出错: {ex.Message}");
                }
            }
        }


        public void WriteLog(string message)
        {
            if (richTextBoxLog.InvokeRequired)
            {
                richTextBoxLog.Invoke(new Action<string>(WriteLog), message);
            }
            else
            {
                richTextBoxLog.AppendText(message + Environment.NewLine);
                richTextBoxLog.ScrollToCaret();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBoxLog.Text=string.Empty;
        }
    }
}
