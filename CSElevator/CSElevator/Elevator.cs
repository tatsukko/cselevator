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
        // 当前电梯乘客总量
        private int totalPassenger()
        {
            int count = 0;
            for (int i = 0; i < numOfFloors; i++)
            {
                count += passenger[i].Count;
            }
            return count;
        }
        public void getWork(ref Floor[] floors)
        {
            if (posCounter == Program.FLOORTIME || posCounter == 0) // 上下客
            {
                if (posCounter != 0)
                {
                    position += direction;
                    posCounter = 0;
                }

                // 下客
                if (passenger[position].Count > 0)
                {
                    if (waitCounter == Program.DROPTIME)
                    {
                        while (passenger[position].Count > 0)
                        {
                            Person p = passenger[position].Dequeue();
                            p.dropDown();
                        }
                        waitCounter = 0;
                    }
                    else
                    {
                        waitCounter++;
                    }
                }
                // 上客
                if (totalPassenger() == 0) // 乘客下完了
                {
                    int curDirection = direction;
                    direction = 0;
                    if (curDirection > 0)
                    {
                        for (int i = position; i < numOfFloors; i++) { }
                    }
                    
                }
                if (direction == 0)
                {
                    int Dif = numOfFloors + 1;
                    int target = -1;
                    for (int i = 0; i < numOfFloors; i++)
                    {
                        if ((floors[i].upQueue.Count > 0 && i >= position) 
                            || (floors[i].downQueue.Count > 0 && i <= position))
                        {
                            if (Math.Abs(i - position) < Dif)
                            {
                                target = i;
                                Dif = Math.Abs(i - position);
                            }
                        }
                    }
                    if (target == position)
                    {
                        if (floors[target].upQueue.Count > 0)
                            direction = 1;
                        if (floors[target].downQueue.Count > 0)
                            direction = -1;
                    }
                    else if (target > position)
                    {
                        direction = 1;
                    }
                    else if (target != -1) direction = -1;
                    else direction = 0;
                }

                // 上客
                if (direction == 1)
                {
                    if (floors[position].upQueue.Count > 0)
                    {
                        while (totalPassenger() < capacity)
                        {
                            if (floors[position].upQueue.Count > 0)
                            {
                                Person p = floors[position].upQueue.Dequeue();
                                passenger[p.targetFloor].Enqueue(p);
                            }
                        }
                    }
                }
                else if(direction == -1)
                {
                    if (floors[position].downQueue.Count > 0)
                    {
                        while (totalPassenger() < capacity)
                        {
                            if (floors[position].downQueue.Count > 0)
                            {
                                Person p = floors[position].downQueue.Dequeue();
                                passenger[p.targetFloor].Enqueue(p);
                            }
                        }
                    }
                }
            }
            else
            {
                if(direction != 0)
                    posCounter++;
            }
        }
    }
}
