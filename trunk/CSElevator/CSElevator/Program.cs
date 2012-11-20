using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Program
    {
        public static int DROPTIME = 4;
        public static int FLOORTIME = 4;
        static int capacity, numOfElevator, numOfFloors;
        public static int time;
        static double lamda;
        public static int TOTAL;
        static Elevator[] elevators;
        static Floor[] floors;
        static void Main(string[] args)
        {
            //testPossion();
            Person.ALL = new Queue<Person>();
            Console.WriteLine("TOTAL Passengers per floor:");
            TOTAL = int.Parse(Console.ReadLine());
            Console.WriteLine("Capacity of elevator:");
            capacity = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of elevator(s):");
            numOfElevator = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of floors:");
            numOfFloors = int.Parse(Console.ReadLine());
            Console.WriteLine("Number of passengers per min(Lamda):");
            lamda = double.Parse(Console.ReadLine());
            elevators = new Elevator[numOfElevator];
            floors = new Floor[numOfFloors];
            initialize(ref elevators, capacity, numOfElevator, numOfFloors, lamda);
            emulate();
            Person.analyze();
            //writeResult();
        }
        private static void writeResult()
        {
            Console.WriteLine("Number of elevator: " + numOfElevator);
            Console.WriteLine("Total passenger: " + TOTAL);
            Console.WriteLine("Average waiting time: " + Person.averageWaitingTime);
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
                int temp = 0;
                for (int i = 0; i < numOfElevator; i++)
                {
                    elevators[i].getWork(ref floors);
                }
                if (bOver(ref ALL)) break;
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
