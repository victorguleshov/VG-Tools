using System;
using System.Collections.Generic;

namespace VG.Collections
{
    public class PriorityQueue<T>
    {
        private readonly bool _isMinPriorityQueue;

        // Items array
        private readonly List<Node> queue = new();
        private int heapSize = -1;

        /// <summary> If min queue or max queue </summary>
        /// <param name="isMinPriorityQueue"></param>
        public PriorityQueue(bool isMinPriorityQueue = false) => _isMinPriorityQueue = isMinPriorityQueue;

        public int Count => queue.Count;

        public T this[int i] => queue[i].Item;

        /// <summary> Enqueue the item with priority </summary>
        /// <param name="priority"></param>
        /// <param name="item"></param>
        public void Enqueue(T item, float priority)
        {
            var node = new Node { Priority = priority, Item = item };
            queue.Add(node);
            heapSize++;

            //Maintaining heap
            if (_isMinPriorityQueue)
                BuildHeapMin(heapSize);
            else
                BuildHeapMax(heapSize);
        }

        /// <summary> Dequeue the object </summary>
        /// <returns></returns>
        public T Dequeue() => Dequeue(out _);

        /// <summary> Dequeue the object </summary>
        /// <returns></returns>
        public T Dequeue(out float priority) => RemoveAt(0, out priority);

        /// <summary> Remove the object at index </summary>
        /// <returns></returns>
        public T RemoveAt(int i) => RemoveAt(i, out _);

        /// <summary> Remove the object at index </summary>
        /// <returns></returns>
        public T RemoveAt(int i, out float priority)
        {
            if (heapSize > -1)
            {
                var returnVal = queue[i].Item;
                priority = queue[i].Priority;
                queue[i] = queue[heapSize];
                queue.RemoveAt(heapSize);
                heapSize--;

                //Maintaining lowest or highest at root based on min or max queue
                if (_isMinPriorityQueue)
                    MinHeapify(i);
                else
                    MaxHeapify(i);
                return returnVal;
            }

            throw new Exception("Queue is empty");
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            for (var i = 0; i < queue.Count; i++)
                if (queue[i].Item.Equals(item))
                {
                    RemoveAt(i);
                    return true;
                }

            return false;
        }

        public T Find(T item)
        {
            foreach (var i in queue)
                if (i.Item.Equals(item))
                    return i.Item;

            return default;
        }

        public T Find(Predicate<T> predicate)
        {
            foreach (var i in queue)
                if (predicate(i.Item))
                    return i.Item;

            return default;
        }

        public T Peek()
        {
            if (heapSize > -1)
                return queue[0].Item;
            throw new Exception("Queue is empty");
        }

        /// <summary> Updating the priority of specific item </summary>
        /// <param name="item"></param>
        /// <param name="priority"></param>
        public void UpdatePriority(T item, float priority)
        {
            var i = 0;
            for (; i <= heapSize; i++)
            {
                var node = queue[i];
                if (Equals(node.Item, item))
                {
                    node.Priority = priority;
                    if (_isMinPriorityQueue)
                    {
                        BuildHeapMin(i);
                        MinHeapify(i);
                    }
                    else
                    {
                        BuildHeapMax(i);
                        MaxHeapify(i);
                    }
                }
            }
        }

        /// <summary> Searching an item </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            foreach (var node in queue)
                if (Equals(node.Item, item))
                    return true;
            return false;
        }

        /// <summary> Maintain max heap </summary>
        /// <param name="i"></param>
        private void BuildHeapMax(int i)
        {
            while (i >= 0 && queue[(i - 1) / 2].Priority < queue[i].Priority)
            {
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        /// <summary> Maintain min heap </summary>
        /// <param name="i"></param>
        private void BuildHeapMin(int i)
        {
            while (i >= 0 && queue[(i - 1) / 2].Priority > queue[i].Priority)
            {
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        private void MaxHeapify(int i)
        {
            var left = ChildL(i);
            var right = ChildR(i);

            var heighst = i;

            if (left <= heapSize && queue[heighst].Priority < queue[left].Priority)
                heighst = left;
            if (right <= heapSize && queue[heighst].Priority < queue[right].Priority)
                heighst = right;

            if (heighst != i)
            {
                Swap(heighst, i);
                MaxHeapify(heighst);
            }
        }

        private void MinHeapify(int i)
        {
            var left = ChildL(i);
            var right = ChildR(i);

            var lowest = i;

            if (left <= heapSize && queue[lowest].Priority > queue[left].Priority)
                lowest = left;
            if (right <= heapSize && queue[lowest].Priority > queue[right].Priority)
                lowest = right;

            if (lowest != i)
            {
                Swap(lowest, i);
                MinHeapify(lowest);
            }
        }

        private void Swap(int i, int j)
        {
            (queue[i], queue[j]) = (queue[j], queue[i]);
        }

        private static int ChildL(int i) => i * 2 + 1;

        private static int ChildR(int i) => i * 2 + 2;

        private class Node
        {
            public float Priority { get; set; }
            public T Item { get; set; }
        }
    }
}