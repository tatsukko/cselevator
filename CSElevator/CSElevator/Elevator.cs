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
        private int target;
        public  int direction;
        public  int position;
        private int posCounter; // used for check whether at a specific floor
        private int waitCounter; // used for waiting at a specific floor
        private int boardCounter; // used for boarding time
        private int stopCount;

        private Queue<Person>[] passenger;
        public Elevator(int floor)
        {
            numOfFloors = floor;
            target = -1;
            stopCount = 0;
            direction = 0;
            capacity = 0;
            position = 0;
            posCounter = 0;
            waitCounter = 0;
            passenger = new Queue<Person>[floor];
            for (int i = 0; i < floor; i++)
            {
                passenger[i] = new Queue<Person>();
                passenger[i].Clear();
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
        // 当前电梯乘客总量
        public int totalPassenger()
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
            if (posCounter == Program.FLOORTIME || posCounter == 0) // 电梯停在某一层
            {
                if (posCounter != 0)
                {
                    if (target != position)
                        position += (target > position ? 1 : -1);
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
                        return;
                    }
                }
                if (boardCounter != 0)
                {
                    if (boardCounter == Program.BOARDTIME)
                    {
                        boardCounter = 0;
                    }
                    else
                    {
                        boardCounter++;
                    }
                    return;
                }
                // 上客
                if (target > position) // 电梯继续上行
                {
                    if (floors[position].upQueue.Count > 0)
                    {
                        while (totalPassenger() < capacity)
                        {
                            if (floors[position].upQueue.Count > 0)
                            {
                                Person p = floors[position].upQueue.Dequeue();
                                target = target > p.targetFloor ? target : p.targetFloor;
                                passenger[p.targetFloor].Enqueue(p);
                                p.getOnboard();
                                boardCounter = 1;
                            }
                            else break;
                        }
                    }
                }
                else if ((target < position) && (target != -1)) // 电梯继续下行
                {
                    if (floors[position].downQueue.Count > 0)
                    {
                        while (totalPassenger() < capacity)
                        {
                            if (floors[position].downQueue.Count > 0)
                            {
                                Person p = floors[position].downQueue.Dequeue();
                                target = target < p.targetFloor ? target : p.targetFloor;
                                passenger[p.targetFloor].Enqueue(p);
                                p.getOnboard();
                                boardCounter = 1;
                            }
                            else break;
                        }
                    }
                }
                else if (target == position)
                {
                    if (direction == 1) // 电梯原来上行
                    {
                        if (floors[position].upQueue.Count > 0) // 本行有乘客需求
                        {
                            while (totalPassenger() < capacity)
                            {
                                if (floors[position].upQueue.Count > 0)
                                {
                                    Person p = floors[position].upQueue.Dequeue();
                                    target = target > p.targetFloor ? target : p.targetFloor;
                                    passenger[p.targetFloor].Enqueue(p);
                                    p.getOnboard();
                                    boardCounter = 1;
                                }
                                else break;
                            }
                        }
                        else // 本行无需求，继续找上行需求
                        {
                            for (int i = position; i < numOfFloors; i++)
                            {
                                if (floors[i].upQueue.Count > 0 || floors[i].downQueue.Count > 0)
                                {
                                    target = i;
                                    break;
                                }
                            }
                        }
                    }
                    else if (direction == -1) // 电梯原来下行
                    {
                        if (floors[position].downQueue.Count > 0) // 本行有乘客需求
                        {
                            while (totalPassenger() < capacity)
                            {
                                if (floors[position].downQueue.Count > 0)
                                {
                                    Person p = floors[position].downQueue.Dequeue();
                                    target = target < p.targetFloor ? target : p.targetFloor;
                                    passenger[p.targetFloor].Enqueue(p);
                                    p.getOnboard();
                                    boardCounter = 1;
                                }
                                else break;
                            }
                        }
                        else // 本行无需求，继续找下行需求
                        {
                            for (int i = position; i >= 0; i--)
                            {
                                if (floors[i].upQueue.Count > 0 || floors[i].downQueue.Count > 0)
                                {
                                    target = i;
                                    break;
                                }
                            }
                        }
                    }
                    if (target == position) // 没有需求
                    {
                        target = -1;
                        direction = 0;
                        stopCount++;
                    }
                }
                else if (target == -1) // 电梯原来静止
                {
                    if (stopCount % 2 == 0)
                    {
                        for (int i = position; i < numOfFloors; i++)
                        {
                            if (floors[i].upQueue.Count > 0 || floors[i].downQueue.Count > 0)
                            {
                                target = i;
                                direction = 1;
                                break;
                            }
                        }
                        if (target == -1)
                        {
                            for (int i = position; i >= 0; i--)
                            {
                                if (floors[i].upQueue.Count > 0 || floors[i].downQueue.Count > 0)
                                {
                                    target = i;
                                    direction = -1;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = position; i >= 0; i--)
                        {
                            if (floors[i].upQueue.Count > 0 || floors[i].downQueue.Count > 0)
                            {
                                target = i;
                                direction = -1;
                                break;
                            }
                        }
                        if (target == -1)
                        {
                            for (int i = position; i < numOfFloors; i++)
                            {
                                if (floors[i].upQueue.Count > 0 || floors[i].downQueue.Count > 0)
                                {
                                    target = i;
                                    direction = 1;
                                    break;
                                }
                            }
                        }
                    }
                }

            }
            if (direction != 0)
                posCounter++;
        }
        public bool outOfService()
        {
            return target == -1;
        }
    }
}
