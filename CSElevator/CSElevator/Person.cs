using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Person
    {
        public static Queue<Person> ALL;
        public static int averageWaitingTime;
        public static int maximumWaitingTime;
        public static int minimumWaitingTime;
        public static int averageDuration;
        public static int maximumDuration;
        public static int minimumDuration;

        public double startTime;
        public double endTime;
        public int targetFloor;

        public void requestElevator(ref Floor floor)
        {
            if (targetFloor > floor.position)
            {
                floor.upQueue.Enqueue(this);
            }
            else
            {
                floor.downQueue.Enqueue(this);
            }
        }
        // Passenger out of elevator
        public void dropDown()
        {
            endTime = Program.time;
            ALL.Enqueue(this);
        }
        public static void analyze()
        {
            Console.WriteLine(ALL.Count);
            Console.ReadLine();
            Person[] all = ALL.ToArray();
            int[] result = new int[10];
            result.Initialize();
            for (int i = 0; i < ALL.Count; i++)
            {
                result[all[i].targetFloor]++;
            }
            for (int i = 0; i < 10; i++) Console.WriteLine(result[i]);
            Console.ReadLine();
            averageWaitingTime = 0;
            averageDuration = 0;
            maximumWaitingTime = 0;
            maximumDuration = 0;
            minimumWaitingTime = -1;
            minimumDuration = -1;
            //for (int i = 0; i < Program.TOTAL; i++)
            //{
            //    averageWaitingTime += 
            //}
        }
    }
}
