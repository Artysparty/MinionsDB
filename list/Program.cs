using System.Collections.Generic;
using System.Collections;
using System;

namespace list
{
    public class Program
    {
        static void Main(string[] args)
        {
            var minionsList = new DoubleLinkedList<Minion>();

            minionsList.Add(new Minion(1, "Kevin", 13, 2));
            minionsList.Add(new Minion(2, "Bob", 16, 3));
            minionsList.Add(new Minion(3, "Ben", 11, 1));

            Console.WriteLine(minionsList[0].CompareTo(minionsList[1]));
            Console.WriteLine(minionsList[1].CompareTo(minionsList[0]));
            Console.WriteLine(minionsList[0].CompareTo(minionsList[0]));
        }
    }
}