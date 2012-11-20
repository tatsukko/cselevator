using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Floor
    {
        public int position;
        public Queue<Person> upQueue;
        public Queue<Person> downQueue;
        public Floor(int pos)
        {
            position = pos;
            upQueue = new Queue<Person>();
            upQueue.Clear();
            downQueue = new Queue<Person>();
            downQueue.Clear();
        }
    }
}
