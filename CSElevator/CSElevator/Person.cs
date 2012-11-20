using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Person
    {
        public static Queue<Person> ALL;
        public static double averageWaitingTime;
        public static double maximumWaitingTime;
        public static double minimumWaitingTime;
        public static double averageDuration;
        public static double maximumDuration;
        public static double  minimumDuration;

        public double startTime;
        public double boardTime;
        public double endTime;
        public int targetFloor;
        public int startFloor;

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
        // Passenger get into an elevator
        public void getOnboard()
        {
            boardTime = Program.time;
        }

        // 统计最终结果数据
        public static void analyze()
        {
            //Console.WriteLine(ALL.Count);
            //Console.ReadLine();
            //Person[] all = ALL.ToArray();
            //int[] result = new int[10];
            //result.Initialize();
            //for (int i = 0; i < ALL.Count; i++)
            //{
            //    result[all[i].targetFloor]++;
            //}
            //for (int i = 0; i < 10; i++) Console.WriteLine(result[i]);
            //Console.ReadLine();
            averageWaitingTime = 0;
            averageDuration = 0;
            maximumWaitingTime = 0;
            maximumDuration = 0;
            minimumWaitingTime = -1;
            minimumDuration = -1;
            Person[] all = ALL.ToArray();
            for (int i = 0; i < all.Length; i++)
            {
                double wTime = all[i].boardTime - all[i].startTime;
                double dTime = all[i].endTime - all[i].boardTime;

                averageWaitingTime += wTime;
                maximumWaitingTime = maximumWaitingTime > wTime ? maximumWaitingTime : wTime;
                if (minimumWaitingTime == -1) minimumWaitingTime = wTime;
                else minimumWaitingTime = minimumWaitingTime < wTime ? minimumWaitingTime : wTime;

                averageDuration += dTime;
                maximumDuration = maximumDuration > dTime ? maximumDuration : dTime;
                if (minimumDuration == -1) minimumDuration = dTime;
                else minimumDuration = minimumDuration < dTime ? minimumDuration : dTime;
            }
            averageWaitingTime /= all.Length;
            averageDuration /= all.Length;
        }
    }
}
