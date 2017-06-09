using System;
using System.Collections.Generic;
using System.Collections;

namespace Algorithm
{
    public sealed class Comparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if (!(x is Genome) || !(y is Genome))
                throw new ArgumentException("Not of type Genome");

            if (((Genome)x).fitness > ((Genome)y).fitness)
                return 1;
            else if (((Genome)x).fitness == ((Genome)y).fitness)
                return 0;
            else
                return -1;
        }
    }

    public sealed class CompareTrue: IComparer<Task>
    {
        public int Compare(Task x, Task y)
        {
            if (!(x is Task) || !(y is Task))
                throw new ArgumentException("Not of type Genome");

            if (((Task)x).importance == true && ((Task)y).importance == true || ((Task)x).importance == false && ((Task)y).importance == false)
            {
                return 0;
            }
            else if (((Task)x).importance == true && ((Task)y).importance == false)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}