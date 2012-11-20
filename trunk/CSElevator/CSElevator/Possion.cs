using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace CSElevator
{
    class Possion
    {
        /*
         * lamda 表示单位时间(60s)内的流量
         * 输出间隔单位为1s
         * 所以一进来需要处理lamda: lamda /= 60
         */
        static Random random = new Random(System.DateTime.Now.Millisecond);
        public static double exprand(double lamda)
        {
            lamda /= 60;
            int RAND_MAX = 100000000;
            int rnd;
            double r, x;
            while (true)
            {
                rnd = random.Next(RAND_MAX);
                if (rnd != 0 && rnd != RAND_MAX) break;
            }
            r = (double)rnd / RAND_MAX;
            x = (-1 / lamda) * Math.Log(r);
            return x;
        }
    }
}
