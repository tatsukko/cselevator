using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Program
    {
        public static int DROPTIME = 6; // 乘客下电梯所耗时间
        public static int FLOORTIME = 4; // 变化一层所需时间
        public static int BOARDTIME = 8; // 乘客上电梯所需时间
        public static bool realTime;
        static int capacity, numOfElevator, numOfFloors;
        public static int time;
        static double lamda;
        public static int TOTAL;
        static Elevator[] elevators;
        static Floor[] floors;
        static void Main(string[] args)
        {
            //testPossion();
            Console.WriteLine("Capacity of elevator:");
            capacity = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of elevator(s):");
            numOfElevator = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of floors:");
            numOfFloors = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of passengers per min(Lamda):");
            lamda = double.Parse(Console.ReadLine());
            Console.WriteLine("\nTOTAL Passengers per floor:");
            TOTAL = int.Parse(Console.ReadLine());
            Console.WriteLine("\nFast simulate or realtime-like? 0 for FAST 1 for REALTIME");
            int tmp = int.Parse(Console.ReadLine());
            realTime = (tmp == 1);
            elevators = new Elevator[numOfElevator];
            floors = new Floor[numOfFloors];
            initialize(ref elevators, capacity, numOfElevator, numOfFloors, lamda);
            emulate();
            printResult();
            Console.ReadLine();
        }
        private static void printResult()
        {
            Console.Clear();
            Person.analyze();
            writeStatistic();
            writeRealTime();
        }
        private static void writeStatistic()
        {
            Console.WriteLine("Time: " + time + "s");
            Console.WriteLine("Number of elevator: " + numOfElevator);
            Console.WriteLine("Total passenger: " + Person.finishedCounter);

            Console.WriteLine("Average waiting time: " + (int)Person.averageWaitingTime + "s");
            Console.WriteLine("Minimum waiting time: " + (int)Person.minimumWaitingTime + "s");
            Console.WriteLine("Maximum waiting time: " + (int)Person.maximumWaitingTime + "s");

            Console.WriteLine("Average duration: " + (int)Person.averageDuration + "s");
            Console.WriteLine("Minimum duration: " + (int)Person.minimumDuration + "s");
            Console.WriteLine("Maximum duration: " + (int)Person.maximumDuration + "s");

            Console.WriteLine("Average Throughput: " + (int)(((double)Person.finishedCounter) / time * 3600) + " pass/hour");
        }
        private static void writeRealTime()
        {
            Console.WriteLine("");
            Console.Write(string.Format("{0, -6}{1, -10}","Floor", "Passenger"));
            for (int i = 0; i < numOfElevator; i++)
            {
                Console.Write(string.Format("{0, -8}{1, -3}", "Elevator", i));
            }
            Console.WriteLine("");
            for (int i = numOfFloors - 1; i >= 0; i--)
            {
                Console.Write(string.Format("{0, -6}{1, -10}", i + 1, floors[i].downQueue.Count + floors[i].upQueue.Count));
                for (int j = 0; j < numOfElevator; j++)
                {
                    if (elevators[j].position == i)
                    {
                        char dir;
                        if (elevators[j].direction > 0) dir = '^';
                        else if (elevators[j].direction < 0) dir = 'V';
                        else dir = '-';
                        Console.Write(string.Format("{0,-5}{1, 3}   ", "[" + elevators[j].totalPassenger() + "]", dir));
                    }
                    else
                    {
                        Console.Write(string.Format("{0, -11}", " "));
                    }
                }
                Console.WriteLine("");
            }
        }
        private static void initialize(ref Elevator[] elevators,
            int capacity, int numOfElevator, int numOfFloors,
            double lamda)
        {
            for (int i = 0; i < numOfElevator; i++)
            {
                elevators[i] = new Elevator(numOfFloors);
                elevators[i].setCapacity(capacity);
                // 电梯初始位置均匀分布在各个楼层
                elevators[i].setPosition((int)(i * numOfFloors 
                    / numOfElevator));
            }
        }
        private static void emulate()
        {
            double start;
            Random randTarget = new Random(System.DateTime.Now.Millisecond);
            Queue<Person>[] ALL = new Queue<Person>[numOfFloors];
            // 初始化各楼层的乘客信息
            for (int i = 0; i < numOfFloors; i++)
            {
                floors[i] = new Floor(i);
                ALL[i] = new Queue<Person>();
                ALL[i].Clear();
                start = 0;
                for (int j = 0; j < TOTAL; j++)
                {
                    start += Possion.exprand(lamda);
                    Person p = new Person();
                    p.startFloor = i;
                    p.startTime = start;
                    ALL[i].Enqueue(p);
                }
            }

            // 每秒模拟
            for (time = 0; ; time++)
            {
                // 检查ALL矩阵, 乘客请求电梯
                for (int i = 0; i < numOfFloors; i++)
                {
                    Person p;
                    if (ALL[i].Count > 0) p = ALL[i].Peek();
                    else p = null;
                    while (p != null && p.startTime < time)
                    {
                        ALL[i].Dequeue();
                        int target = i;
                        // 随机目标楼层
                        while (target == i || target == numOfFloors) target = randTarget.Next(numOfFloors);
                        p.targetFloor = target;
                        p.requestElevator(ref floors[i]);
                        if (ALL[i].Count > 0) p = ALL[i].Peek();
                        else p = null;
                    }
                }

                // 更新楼层参数
                // 不用了，Floor自己完成

                // 触发电梯
                for (int i = 0; i < numOfElevator; i++)
                {
                    elevators[i].getWork(ref floors);
                }
                if (bOver(ref ALL)) break;
                if (realTime)
                {
                    System.Threading.Thread.Sleep(200);
                    printResult();
                }
            }
        }
        // Judge whether the whole simulation should be stopped
        public static bool bOver(ref Queue<Person>[] ALL)
        {
            for (int i = 0; i < numOfElevator; i++)
            {
                if (!elevators[i].outOfService()) return false;
            }
            for (int i = 0; i < numOfFloors; i++)
            {
                if (floors[i].downQueue.Count > 0 || floors[i].upQueue.Count > 0 || ALL[i].Count > 0)
                    return false;
            }
            return true;
        }
        private static void testPossion()
        {
            double total = 0;
            for (int i = 0; i < 10000; i++)
            {
                total += Possion.exprand(6);
            }
            // Expected output: 10
            Console.WriteLine(total / 10000);
            Console.ReadLine();
        }
    }
}
