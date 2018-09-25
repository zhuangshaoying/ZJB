using System;
using System.Collections.Generic;

namespace ZJB.Core.Collections
{
    public class RollingQueue<T> : IEnumerable<T>
    {
        private int front;
        private int rear;
        private int size;
        private int actureSize;
        public T[] data;

        public RollingQueue(int size)
        {
            this.size = size;
            actureSize = size + 1;
            front = 0;
            rear = 0;
            data = new T[actureSize];
        }

        public void Enqueue(T item)
        {
            data[rear] = item;
            rear = (rear + 1) % actureSize;
            if (rear == front)
                front = (front + 1) % actureSize;
        }

        public T Dequeue()
        {
            if (front == rear)
                throw new Exception("Queue Empty!");

            T item = data[front];

            front = (front + 1) % actureSize;

            return item;
        }

        public int Count
        {
            get
            {
                if (front < rear)
                    return rear - front;
                if (front > rear)
                    return actureSize - front + rear;
                return 0;

            }
        }

        public int Size
        {
            get { return size; }
            set
            {
                T[] newData = new T[value + 1];

                for (int i = 0; i < ((value > data.Length) ? data.Length : value); i++)
                {
                    newData[i] = this[i];
                }

                size = value;
                actureSize = value + 1;
                data = newData;
            }
        }

        public T this[int index]
        {
            get { return data[(rear + size - index) % actureSize]; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }
    }
}
