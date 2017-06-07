using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class Worker
    {
        public int costPerHour;
        public int[] schedule;
        public int serialNumber;

        public List<int> lastWork = new List<int>();

        public Worker()
        {
            lastWork.Add(0);
        }

        public Worker(int cost, int[]schedule, int serialNumber)
        {
            costPerHour = cost;
            this.schedule = schedule;
            this.serialNumber = serialNumber;
        }
        
        ~Worker()
        {

        }

    }
}
