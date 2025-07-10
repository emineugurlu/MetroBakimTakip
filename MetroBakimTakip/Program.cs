using System;
using System.Windows.Forms;

namespace MetroBakimTakip
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın giriş noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
