using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    public class Priority : SchedulerFather
    {
        public Priority(List<Task> tasklist, bool ispreemptive)
        {
            this.compareflow = new string[] { "prior", "atime", "btime", "step" };
            this.ispreemptive = false;
            this.tasklist = tasklist;
            this.ispreemptive = ispreemptive;

            foreach (var task in this.tasklist)
            {
                task.leftatime = task.atime;
                task.leftbtime = task.btime;
            }
        }

        public override List<Task> nextStep(int time)
        {
            // update table
            if (time > workreachtime)
            {
                updateATime();
                burstingtaskindex = nonpreemptiveCompare();
                burstingtaskindex = compare(burstingtaskindex, 0, queue);

                updateBTimeNPreGanttChart();

                workreachtime++;

                queue.Union(tempqueue);
                updateQueue();
                reachFinalEndTime();
            }
            return tasklist;
        }
    }
}
