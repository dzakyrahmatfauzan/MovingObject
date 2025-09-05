using System;
using System.Windows.Forms;

namespace MovingObjectServer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());   // <- akan ketemu kalau namespace cocok
        }
    }
}
