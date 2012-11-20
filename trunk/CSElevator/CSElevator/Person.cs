using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Person
    {
        public static Queue<Person> ALL;
        public static int finishedCounter;
        public static double totalWaitingTime;
        public static double averageWaitingTime;
        public static double maximumWaitingTime;
        public static double minimumWaitingTime;

        public static double totalDuration;
        public static double averageDuration;
        public static double maximumDuration;
        public static double minimumDuration;

        public double startTime;
        public double boardTime;
        public double endTime;
        public int targetFloor;
        public int startFloor;

        static Person()
        {
            ALL = new Queue<Person>();
            ALL.Clear();

            finishedCounter = 0;

            totalWaitingTime = 0;
            averageWaitingTime = 0;
            maximumWaitingTime = 0;
            minimumWaitingTime = -1;

            totalDuration = 0;
            averageDuration = 0;
            maximumDuration = 0;
            minimumDuration = -1;
        }
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
            if (ALL.Count > 0)
            {
                finishedCounter += ALL.Count;
                Person[] all = ALL.ToArray();
                ALL.Clear();
                for (int i = 0; i < all.Length; i++)
                {
                    double wTime = all[i].boardTime - all[i].startTime;
                    double dTime = all[i].endTime - all[i].boardTime;

                    totalWaitingTime += wTime;
                    maximumWaitingTime = maximumWaitingTime > wTime ? maximumWaitingTime : wTime;
                    if (minimumWaitingTime == -1) minimumWaitingTime = wTime;
                    else minimumWaitingTime = minimumWaitingTime < wTime ? minimumWaitingTime : wTime;

                    totalDuration += dTime;
                    maximumDuration = maximumDuration > dTime ? maximumDuration : dTime;
                    if (minimumDuration == -1) minimumDuration = dTime;
                    else minimumDuration = minimumDuration < dTime ? minimumDuration : dTime;
                }
                averageWaitingTime = totalWaitingTime / finishedCounter;
                averageDuration = totalDuration / finishedCounter;
            }
        }
    }
}
