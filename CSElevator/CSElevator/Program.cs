using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Program
    {
        static int capacity, numOfElevator, numOfFloors;
        public static int time;
        static double lamda;
        public static int TOTAL;
        static Elevator[] elevators;
        static Floor[] floors;
        static void Main(string[] args)
        {
            //testPossion();
            TOTAL = 50;
            Person.ALL = new Queue<Person>();
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
            emulate(TOTAL);
        }
        private static void initialize(ref Elevator[] elevators,
            int capacity, int numOfElevator, int numOfFloors,
            double lamda)
        {
            for (int i = 0; i < numOfElevator; i++)
            {
                elevators[i] = new Elevator(numOfFloors);
                elevators[i].setCapacity(capacity);
                elevators[i].setDirection(0);
                // 电梯初始位置均匀分布在各个楼层
                elevators[i].setPosition((int)(i * numOfFloors 
                    / numOfElevator));
            }
        }
        private static void emulate(int TOTAL)
        {
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
