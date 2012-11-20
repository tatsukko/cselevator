using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Program
    {
        static void Main(string[] args)
        {
            testPossion();
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
