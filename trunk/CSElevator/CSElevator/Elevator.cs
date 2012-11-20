using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSElevator
{
    class Elevator
    {
        private int numOfFloors;
        private int capacity;
        public int direction;
        //private int 
        private int position;
        private int posCounter; // used for check whether at a specific floor
        private int waitCounter; // used for waiting at a specific floor
        private bool[] toStop;
        private Queue<Person>[] passenger;
        public Elevator(int floor)
        {
            numOfFloors = floor;
            capacity = 0;
            direction = 0;
            position = 0;
            posCounter = 0;
            waitCounter = 0;
            passenger = new Queue<Person>[floor];
            toStop = new bool[floor];
            for (int i = 0; i < floor; i++)
            {
                passenger[i] = new Queue<Person>();
                passenger[i].Clear();
                toStop[i] = false;
            }
        }
        public void setCapacity(int _c)
        {
            this.capacity = _c;
        }
        public void setPosition(int _p)
        {
            this.position = _p;
        }
        public void setDirection(int _d)
        {
            this.direction = _d;
        }
    }
}
