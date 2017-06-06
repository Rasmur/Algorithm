using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    public class Task
    {
        public string name;
        public bool importance;
        public int deadline;
        public int serialNumber;
        public int duration;
        

        public Task()
        {

        }

        public Task(string name, bool importance, int dl, int sN, int time)
        {
            this.name = name;
            this.importance = importance;
            deadline = dl;
            serialNumber = sN;
            duration = time;
        }

        ~Task()
        {

        }
    }
}
