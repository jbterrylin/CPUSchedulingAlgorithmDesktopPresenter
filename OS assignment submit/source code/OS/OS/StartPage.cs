using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OS
{
    public partial class StartPage : Form
    {
        public StartPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void StartPage_Load(object sender, EventArgs e)
        {
        }   

        private void sptitle_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I'm JB terry lin. I like cat. Meow Meow Meow de. You know? Skr Skr");
        }

        private void startbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void quitbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
