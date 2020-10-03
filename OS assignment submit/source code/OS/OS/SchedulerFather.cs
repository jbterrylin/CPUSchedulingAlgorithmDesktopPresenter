using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    public class SchedulerFather
    {
        public List<Task> tasklist = new List<Task>();
        public string[] compareflow;

        public bool ispreemptive;
        public int burstingtaskindex = -1;
        public List<Task> queue = new List<Task>();
        public List<Task> tempqueue = new List<Task>();
        public List<string> preganttchart = new List<string>();

        internal int workreachtime;
        internal int finalendtime = -1;

        public virtual List<Task> nextStep(int time)
        {
            return new List<Task>();
        }

        public int nonpreemptiveCompare()
        {
            // nonpreemptive and having current working task
            if (ispreemptive == false && burstingtaskindex != -1)
            {
                return burstingtaskindex;
            }
            return -1;
        }

        public int compare(int result, int compareflowindex, List<Task> leftselection)
        {
            if (result == -1)
            {
                if (leftselection.Count != 0)
                {
                    string varname = compareflow[compareflowindex];
                    if (varname == "btime" || varname == "atime" || varname == "prior")
                    {
                        decimal smallestvalue = (decimal)leftselection.Min(task => task.GetType().GetField(varname).GetValue(task));
                        List<Task> highesttask = leftselection.FindAll(task => (decimal)task.GetType().GetField(varname).GetValue(task) == smallestvalue);
                        if (highesttask.Count == 1)
                        {
                            return tasklist.FindIndex(task => task == highesttask[0]);
                        }
                        else
                        {
                            result = compare(result, compareflowindex + 1, highesttask);
                        }
                    } else if (varname == "step")
                    {
                        return tasklist.FindIndex(task => task == leftselection[0]);
                    }
                } else
                {
                    // treat -1 as error
                    return -1;
                }
            }
            return result;
        }

        public void updateQueue()
        {
            for(int i=0; i<queue.Count; i++)
            {
                Task temp = tasklist.Where(task => task.process == queue[i].process).ToList()[0];
                queue[i] = temp;
            }
        }

        public bool reachFinalEndTime()
        {
            foreach (var task in tasklist)
            {
                if (task.leftbtime != 0)
                {
                    return false;
                }
            }
            if(finalendtime == -1)
            {
                finalendtime = workreachtime;
                return true;
            }
            return true;
        }

        public void updateATime()
        {
            foreach (var task in tasklist)
            {
                if (task.leftbtime != 0)
                {
                    if (task.leftatime == 0)
                    {
                        queue.Add(task);
                        if (ispreemptive == true)
                        {
                            burstingtaskindex = -1;
                        }
                    }
                    else if (task.leftatime != 0)
                    {
                        task.leftatime -= 1;
                        if (task.leftatime == 0)
                        {
                            tempqueue.Add(task);
                        }
                    }
                }
            }
        }

        public void updateBTimeNPreGanttChart()
        {
            if (burstingtaskindex != -1)
            {
                tasklist[burstingtaskindex].leftbtime--;
                preganttchart.Add(tasklist[burstingtaskindex].process);
                if (tasklist[burstingtaskindex].leftbtime == 0)
                {
                    tasklist[burstingtaskindex].finishtime = workreachtime;
                    queue.RemoveAll(task => task.process == tasklist[burstingtaskindex].process);
                    burstingtaskindex = -1;
                }
            }
            else
            {
                preganttchart.Add("");
            }
        }
    }
}
