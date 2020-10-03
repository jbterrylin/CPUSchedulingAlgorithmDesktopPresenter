using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS
{
    public partial class ResultPage : Form
    {
        public List<Task> tasklist;
        public ResultPage(List<Task> tasklist)
        {
            InitializeComponent();
            decimal avgtatime = 0;
            decimal avgwtime = 0;
            string[] row = new string[] { };
            foreach (var task in tasklist)
            {
                avgtatime += (task.finishtime + 1 - task.atime);
                avgwtime += (task.finishtime + 1 - task.atime - task.btime);

                row = new string[] {
                    task.process,
                    task.btime.ToString(),
                    task.atime.ToString(),
                    task.prior.ToString(),
                    (task.finishtime + 1 - task.atime).ToString(),
                    (task.finishtime + 1 - task.atime - task.btime).ToString()
                };
                var lvi = new ListViewItem(row)
                {
                    Tag = task
                };
                tasklistview.Items.Add(lvi);
            }
            row = new string[] {
                    "",
                    "",
                    "",
                    "Avg: ",
                    (avgtatime / tasklist.Count()).ToString("#.##"),
                    (avgwtime / tasklist.Count()).ToString("#.##")
                };
            var lvi1 = new ListViewItem(row)
            {
                Tag = null
            };
            tasklistview.Items.Add(lvi1);
        }

        private void quitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
