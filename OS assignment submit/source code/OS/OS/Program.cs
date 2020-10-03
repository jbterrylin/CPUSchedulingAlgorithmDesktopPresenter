using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            StartPage startpage = new StartPage();

            if (startpage.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainPage());
            }
            else
            {
                startpage.Close();
            }
        }
    }
}
