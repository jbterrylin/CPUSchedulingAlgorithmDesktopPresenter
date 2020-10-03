using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS
{
    public partial class MainPage : Form
    {
        public List<Task> tasklist = new List<Task>();
        public bool withsec = true;
        public bool isstart = false;
        public SchedulerFather method;
        public int time;
        TableLayoutPanel ganttlayoutContainer = new TableLayoutPanel();

        public MainPage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            procbbox.Items.Add("------- Delete All -------");

            typecbbox.Items.Add("Preemptive SJF");
            typecbbox.Items.Add("Non Preemptive SJF");
            typecbbox.Items.Add("Preemptive Priority");
            typecbbox.Items.Add("Non Preemptive Priority");
            typecbbox.SelectedItem = "Non Preemptive SJF";
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            if(proipt.Text != "------- Delete All -------" && proipt.Text != "")
            {
                int index = tasklist.FindIndex(element => element.process == proipt.Text);
                if (index == -1)
                {
                    // add input to tasklist
                    tasklist.Add(new Task() { process = proipt.Text, btime = btimeipt.Value, atime = atimeipt.Value, prior = prioript.Value });
                    procbbox.Items.Add(proipt.Text);
                    updateTaskListView();
                    clearIpt();
                }
                else
                {
                    if (MessageBox.Show("Your process name is already exist. Do you want to edit it?", "Redundant Process name",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        tasklist[index].process = proipt.Text;
                        tasklist[index].btime = btimeipt.Value;
                        tasklist[index].atime = atimeipt.Value;
                        tasklist[index].prior = prioript.Value;
                        clearIpt();
                        updateTaskListView();
                    }
                }
            } else if (proipt.Text == "------- Delete All -------")
            {
                MessageBox.Show("Eiiiiiiii. Change a process name lah.");
            } else if (proipt.Text == "")
            {
                MessageBox.Show("Please input process name.");
            }

        }

        private void clearIpt()
        {
            proipt.Text = "";
            btimeipt.Value = 1;
            atimeipt.Value = 0;
            prioript.Value = 1;
        }

        private void updateTaskListView()
        {
            tasklistview.Items.Clear();
            foreach (var task in tasklist)
            {
                string[] row;
                if (isstart == true)
                {
                    int temp = method.preganttchart.Take(time).Count(element => element == task.process);
                    int temp1;
                    if(time >= task.atime)
                    {
                        temp1 = 0;
                    } else
                    {
                        temp1 = (int)(task.atime - time);
                    }
                    row = new string[] { 
                        task.process,
                        task.btime.ToString() + "(" + (task.btime-temp) + ")",
                        task.atime.ToString() + "(" + temp1 + ")",
                        //task.btime.ToString() + "(" + (task.btime-temp) + ")" + task.leftbtime, 
                        //task.atime.ToString() + "(" + temp1 + ")" + task.leftatime, 
                        task.prior.ToString() 
                    };
                } else
                {
                    row = new string[] { 
                        task.process, 
                        task.btime.ToString(), 
                        task.atime.ToString(), 
                        task.prior.ToString() 
                    };
                }
                var lvi = new ListViewItem(row)
                {
                    Tag = task
                };
                tasklistview.Items.Add(lvi);
            }
        }

        private void tasklistview_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedtask = (Task)tasklistview.SelectedItems[0].Tag;
                if(selectedtask != null)
                {
                    proipt.Text = selectedtask.process;
                    btimeipt.Value = selectedtask.btime;
                    atimeipt.Value = selectedtask.atime;
                    prioript.Value = selectedtask.prior;
                }
            }
            catch (Exception)
            {
            }
        }

        private void rmvbtn_Click(object sender, EventArgs e)
        {
            if(procbbox.Text == "")
            {
                return;
            } else if (procbbox.Text == "------- Delete All -------")
            {
                procbbox.Items.Clear();
                procbbox.Items.Add("------- Delete All -------");
                tasklist = new List<Task>();
            } else
            {
                if(tasklist.Count > 0)
                {
                    tasklist.Remove(tasklist.First(element => element.process == procbbox.Text));
                    procbbox.Items.Remove(procbbox.Text);
                }
            }
            updateTaskListView();
        }

        private void typecbbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(typecbbox.Text == "Round Robin")
            {
                qlbl.Visible = true;
                qipt.Visible = true;
            } else
            {
                qlbl.Visible = false;
                qipt.Visible = false;
            }
        }

        private void withprobtn_Click(object sender, EventArgs e)
        {
            withsec = false;
            withprobtn.Enabled = false;
            withsecbtn.Enabled = true;
        }

        private void withsecbtn_Click(object sender, EventArgs e)
        {
            withsec = true;
            withprobtn.Enabled = true;
            withsecbtn.Enabled = false;
        }

        private void spbtn_Click(object sender, EventArgs e)
        {
            ganttchartpanel.Controls.Remove(ganttlayoutContainer);
            if (isstart)
            {
                // start alr, press again is stop
                spbtn.Text = "|>";
                addbtn.Enabled = true;
                procbbox.Enabled = true;
                rmvbtn.Enabled = true;
                typecbbox.Enabled = true;
                qipt.Enabled = true;

                to0btn.Enabled = false;
                backbtn.Enabled = false;
                nextbtn.Enabled = false;
                to8btn.Enabled = false;
                isstart = false;
                foreach (var task in tasklist)
                {
                    task.leftatime = task.atime;
                    task.leftbtime = task.btime;
                    task.finishtime = 0;
                }
            } else
            {
                // stop alr, press again is start
                if (tasklist.Count() != 0)
                {
                    spbtn.Text = "||";
                    addbtn.Enabled = false;
                    procbbox.Enabled = false;
                    rmvbtn.Enabled = false;
                    typecbbox.Enabled = false;
                    qipt.Enabled = false;

                    nextbtn.Enabled = true;
                    to8btn.Enabled = true;
                    isstart = true;
                } else
                {
                    MessageBox.Show("Please input some process first lah.");
                    if (MessageBox.Show("Or u want default de?", "Default?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        tasklist.Add(new Task() { process = "P1", btime = 9, atime = 0, prior = 5 });
                        procbbox.Items.Add("P1");
                        tasklist.Add(new Task() { process = "P2", btime = 4, atime = 1, prior = 2 });
                        procbbox.Items.Add("P2");
                        tasklist.Add(new Task() { process = "P3", btime = 3, atime = 2, prior = 1 });
                        procbbox.Items.Add("P3");
                        tasklist.Add(new Task() { process = "P4", btime = 1, atime = 4, prior = 3 });
                        procbbox.Items.Add("P4");
                        tasklist.Add(new Task() { process = "P5", btime = 6, atime = 5, prior = 4 });
                        procbbox.Items.Add("P5");
                    } else
                    {
                        MessageBox.Show("Bro. Like this I very hard de. Default u don't want. Buffet style u don't want. How I start.");
                    }
                }
            }
            // start action
            time = 0;
            if (typecbbox.Text == "Non Preemptive SJF")
            {
                method = new SJF(tasklist, false);
            } else if (typecbbox.Text == "Preemptive SJF")
            {
                method = new SJF(tasklist, true);
            } else if (typecbbox.Text == "Non Preemptive Priority")
            {
                method = new Priority(tasklist, false);
            } else if (typecbbox.Text == "Preemptive Priority")
            {
                method = new Priority(tasklist, true);
            }

            updateTaskListView();
        }

        private void to0btn_Click(object sender, EventArgs e)
        {
            to8btn.Enabled = true;
            nextbtn.Enabled = true;
            backbtn.Enabled = false;
            to0btn.Enabled = false;
            time = 0;
            updateTaskListView();
            updateGanttChartView();
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            int stoppoint = -1;
            do
            {
                time--;
                updateTaskListView();
                updateGanttChartView();

                to8btn.Enabled = true;
                nextbtn.Enabled = true;

                if (time == 0)
                {
                    backbtn.Enabled = false;
                    to0btn.Enabled = false;
                    return;
                }
                if(withsec == false && stoppoint == -1)
                {
                    int a = method.preganttchart.Take(time - 1).Reverse().ToList().
                    FindIndex(element => element != method.preganttchart[time]);
                    if (a != -1)
                    {
                        stoppoint = time - a - 1;
                    }
                }
            } while (withsec == false &&
                time != stoppoint &&
                method.tasklist.FindAll(task => task.atime == time).Count() == 0);
        }

        private void nextbtn_Click(object sender, EventArgs e)
        {
            do
            {
                time++;
                tasklist = method.nextStep(time);
                updateTaskListView();
                updateGanttChartView();

                to0btn.Enabled = true;
                backbtn.Enabled = true;

                if (method.finalendtime == time)
                {
                    nextbtn.Enabled = false;
                    to8btn.Enabled = false;
                    return;
                }
            } while (withsec == false && 
                method.burstingtaskindex != -1 &&
                method.tasklist.FindAll(task => task.atime == time).Count() == 0);
            
        }

        private void to8btn_Click(object sender, EventArgs e)
        {
            to0btn.Enabled = true;
            backbtn.Enabled = true;

            while (method.finalendtime != time)
            {
                time++;
                tasklist = method.nextStep(time);
                if (method.finalendtime == time)
                {
                    nextbtn.Enabled = false;
                    to8btn.Enabled = false;
                }
            }
            updateTaskListView();
            updateGanttChartView();
        }

        private void reportbtn_Click(object sender, EventArgs e)
        {
            if(time == method.finalendtime)
            {
                var resultpage = new ResultPage(method.tasklist);
                resultpage.Show();
            } else
            {
                MessageBox.Show("Haven't end leh");
            }
        }

        private TableLayoutPanel tableLayoutWithStyle(int rowCount, int colCount, AnchorStyles first, AnchorStyles second)
        {
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Anchor = (first | second);
            layout.RowCount = rowCount;
            layout.ColumnCount = colCount;
            layout.Dock = DockStyle.Fill;
            return layout;
        }

        private GanttChartForm toGanttChartForm()
        {
            GanttChartForm gcf = new GanttChartForm();
            for(int i=0; i<method.preganttchart.Count; i++)
                if (i <= time)
                    if(i == 0)
                    {
                        gcf.processname.Add(method.preganttchart[i]);
                        gcf.time.Add(i);
                        gcf.time.Add(i+1);
                    }
                    else if (method.preganttchart[i] == method.preganttchart[i - 1])
                    {
                        gcf.time.RemoveAt(gcf.time.Count - 1);
                        gcf.time.Add(i+1);
                    }
                    else if (method.preganttchart[i] != method.preganttchart[i - 1])
                    {
                        gcf.processname.Add(method.preganttchart[i]);
                        gcf.time.Add(i+1);
                    }
            return gcf;
        }

        private TableLayoutPanel labelAddToLayout(string text, TableLayoutPanel panel, int col, int row, bool withBorder)
        {
            Label label = new Label();
            label.Margin = new Padding(0, 0, 0, 0);
            label.Text = text;
            if (withBorder)
            {
                label.BorderStyle = BorderStyle.FixedSingle;
            }
            panel.Controls.Add(label, col, row);
            return panel;
        }

        private void updateGanttChartView()
        {
            GanttChartForm gcf = toGanttChartForm();
            ganttchartpanel.Controls.Remove(ganttlayoutContainer);
            ganttlayoutContainer = tableLayoutWithStyle((gcf.processname.Count / 14 * 2) + 2, 1, AnchorStyles.Top, AnchorStyles.Bottom);
            ganttlayoutContainer.AutoScroll = true;
            for (int i = 0; i < ganttlayoutContainer.RowCount; i++)
                if (i % 2 == 0)
                    ganttlayoutContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
                else
                    ganttlayoutContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            TableLayoutPanel ganttlayoutRow = tableLayoutWithStyle(1, 13, AnchorStyles.Left, AnchorStyles.Right);
            TableLayoutPanel ganttlayoutRowSecond = tableLayoutWithStyle(1, 14, AnchorStyles.Left, AnchorStyles.Right);
            int finalcolnum = 0;
            for (int j = 0; j < ganttlayoutContainer.RowCount / 2; j++)
            {
                ganttlayoutRow = tableLayoutWithStyle(1, 13, AnchorStyles.Left, AnchorStyles.Right);
                ganttlayoutRowSecond = tableLayoutWithStyle(1, 14, AnchorStyles.Left, AnchorStyles.Right);
                for (int i = 0; i < 13; i++)
                    if (j * 13 + i < gcf.processname.Count)
                    {
                        ganttlayoutRow = labelAddToLayout(gcf.processname[j * 13 + i], ganttlayoutRow, i, 0, true);
                        ganttlayoutRowSecond = labelAddToLayout(gcf.time[j * 13 + i] + "", ganttlayoutRowSecond, i, 0, false);
                        finalcolnum = i;
                    }
                TableLayoutPanel rightpush = tableLayoutWithStyle(1, 2, AnchorStyles.Left, AnchorStyles.Right);
                rightpush.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 2));
                rightpush.Controls.Add(ganttlayoutRow,2,0);
                ganttlayoutContainer.Controls.Add(rightpush, 0, j * 2);
                //ganttlayoutContainer.Controls.Add(ganttlayoutRow, 0, j * 2);
                ganttlayoutContainer.Controls.Add(ganttlayoutRowSecond, 0, j * 2 + 1);
            }
            ganttlayoutRowSecond = labelAddToLayout(gcf.time[gcf.time.Count - 1] + "", ganttlayoutRowSecond, finalcolnum + 1, 0, false);
            ganttchartpanel.Controls.Add(ganttlayoutContainer);
        }

        private void quitbtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to quit program?", "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
