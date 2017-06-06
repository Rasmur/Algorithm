using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class Schedule
    {
        int count;
        Genome genome;

        public Schedule()
        {
            genome = new Genome();
        }

        public void ToSchedule(Schedule schedule, Worker[] workers, Task[] tasks)
        {
            int workerNumber;
            Random random = new Random();

            for (int i = 0; i < tasks.Length; i++)
            {
                workerNumber = random.Next(1, workers.Length);
            }
        }
    }
}
