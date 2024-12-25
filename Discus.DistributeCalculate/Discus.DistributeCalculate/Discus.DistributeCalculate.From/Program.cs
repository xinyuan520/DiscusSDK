using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discus.DistributeCalculate.From
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            // 注册应用程序退出事件
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(Form1.OnProcessExit);
        }
    }
}
