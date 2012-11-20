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
    }
}
