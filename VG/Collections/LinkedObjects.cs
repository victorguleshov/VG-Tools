using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace VG.Collections
{
    /// <summary>
    /// Это как <see cref="T:System.Collections.Generic.LinkedList`1" /> только через интерфейс
    /// </summary>
    public class LinkedObjects<T> : ICollection<T>, ICollection where T : class, ILinkedObject<T>
    {
        private int version;
        private object _syncRoot;

        public LinkedObjects () { }

        public LinkedObjects (params T[] collection)
        {
            if (collection == null)
                throw new ArgumentNullException (nameof (collection));
            foreach (T obj in collection)
                this.AddLast (obj);
        }

        public LinkedObjects (IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException (nameof (collection));
            foreach (T obj in collection)
                this.AddLast (obj);
        }

        public int Count { get; private set; }
        public T First { get; private set; }
        public T Last => First?.prev;

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.Add (T value)
        {
            this.AddLast (value);
        }

        public void AddAfter (T node, T newNode)
        {
            this.ValidateNode (node);
            this.ValidateNewNode (newNode);
            this.InternalInsertNodeBefore (node.next, newNode);
            newNode.list = this;
        }

        public void AddAfter (T node, IEnumerable<T> collection)
        {
            foreach (var newNode in collection)
            {
                AddAfter (node, newNode);
                node = newNode;
            }
        }

        public void AddBefore (T node, T newNode)
        {
            this.ValidateNode (node);
            this.ValidateNewNode (newNode);
            this.InternalInsertNodeBefore (node, newNode);
            newNode.list = this;
            if (node != this.First)
                return;
            this.First = newNode;
        }

        public void AddFirst (T node)
        {
            this.ValidateNewNode (node);
            if (this.First == null)
            {
                this.InternalInsertNodeToEmptyList (node);
            }
            else
            {
                this.InternalInsertNodeBefore (this.First, node);
                this.First = node;
            }
            node.list = this;
        }

        public void AddLast (T node)
        {
            this.ValidateNewNode (node);
            if (this.First == null)
                this.InternalInsertNodeToEmptyList (node);
            else
                this.InternalInsertNodeBefore (this.First, node);
            node.list = this;
        }

        public void Clear ()
        {
            T linkedListNode1 = this.First;
            while (linkedListNode1 != null)
            {
                T linkedListNode2 = linkedListNode1;
                linkedListNode1 = linkedListNode1.next != null && linkedListNode1.next != this.First ? linkedListNode1.next : null;
                linkedListNode2.Invalidate ();
            }
            this.First = null;
            this.Count = 0;
            ++this.version;
        }

        public bool Contains (T value)
        {
            return this.Find (value) != null;
        }

        public void CopyTo (T[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException (nameof (array));
            if (index < 0)
                throw new ArgumentOutOfRangeException (nameof (index), index, "Non-negative number required.");
            if (index > array.Length)
                throw new ArgumentOutOfRangeException (nameof (index), index, "Must be less than or equal to the size of the collection.");
            if (array.Length - index < this.Count)
                throw new ArgumentException ("Insufficient space in the target location to copy the information.");
            T linkedListNode = this.First;
            if (linkedListNode == null)
                return;
            do
            {
                array[index++] = linkedListNode;
                linkedListNode = linkedListNode.next;
            }
            while (linkedListNode != this.First);
        }

        public T Find (T value)
        {
            T linkedListNode = this.First;
            EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
            if (linkedListNode != null)
            {
                if (value != null)
                {
                    while (!equalityComparer.Equals (linkedListNode, value))
                    {
                        linkedListNode = linkedListNode.next;
                        if (linkedListNode == this.First)
                            goto label_8;
                    }
                    return linkedListNode;
                }
                while (linkedListNode != null)
                {
                    linkedListNode = linkedListNode.next;
                    if (linkedListNode == this.First)
                        goto label_8;
                }
                return linkedListNode;
            }
            label_8:
            return null;
        }

        public T Find (Predicate<T> match)
        {
            foreach (var item in this)
            {
                if (match (item))
                {
                    return item;
                }
            }

            return default;
        }

        public T FindLast (T value)
        {
            if (this.First == null)
                return null;
            T prev = this.First.prev;
            T linkedListNode = prev;
            EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
            if (linkedListNode != null)
            {
                if (value != null)
                {
                    while (!equalityComparer.Equals (linkedListNode, value))
                    {
                        linkedListNode = linkedListNode.prev;
                        if (linkedListNode == prev)
                            goto label_10;
                    }
                    return linkedListNode;
                }
                while (linkedListNode != null)
                {
                    linkedListNode = linkedListNode.prev;
                    if (linkedListNode == prev)
                        goto label_10;
                }
                return linkedListNode;
            }
            label_10:
            return null;
        }
        public bool Remove (T node)
        {
            if (this.Find (node) == null)
                return false;
            this.InternalRemoveNode (node);
            return true;
        }

        public void RemoveFirst ()
        {
            if (this.First == null)
                throw new InvalidOperationException ("The LinkedList is empty.");
            this.InternalRemoveNode (this.First);
        }

        public void RemoveLast ()
        {
            if (this.First == null)
                throw new InvalidOperationException ("The LinkedList is empty.");
            this.InternalRemoveNode (this.First.prev);
        }

        private void InternalInsertNodeBefore (T node, T newNode)
        {
            newNode.next = node;
            newNode.prev = node.prev;
            node.prev.next = newNode;
            node.prev = newNode;
            ++this.version;
            ++this.Count;
        }

        private void InternalInsertNodeToEmptyList (T newNode)
        {
            newNode.next = newNode;
            newNode.prev = newNode;
            this.First = newNode;
            ++this.version;
            ++this.Count;
        }

        internal void InternalRemoveNode (T node)
        {
            if (node.next == node)
            {
                this.First = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                if (this.First == node)
                    this.First = node.next;
            }
            node.Invalidate ();
            --this.Count;
            ++this.version;
        }

        internal void ValidateNewNode (T node)
        {
            if (node == null)
                throw new ArgumentNullException (nameof (node));

            // if (node.LinkedObjects != null)
            //     throw new InvalidOperationException ("The LinkedList node already belongs to a LinkedList.");
        }

        internal void ValidateNode (T node)
        {
            if (node == null)
                throw new ArgumentNullException (nameof (node));
            if (node.list != this)
                throw new InvalidOperationException ("The LinkedList node does not belong to current LinkedList.");
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                    Interlocked.CompareExchange<object> (ref this._syncRoot, new object (), null);
                return this._syncRoot;
            }
        }

        void ICollection.CopyTo (Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException (nameof (array));
            if (array.Rank != 1)
                throw new ArgumentException ("Only single dimensional arrays are supported for the requested action.", nameof (array));
            if (array.GetLowerBound (0) != 0)
                throw new ArgumentException ("The lower bound of target array must be zero.", nameof (array));
            if (index < 0)
                throw new ArgumentOutOfRangeException (nameof (index), index, "Non-negative number required.");
            if (array.Length - index < this.Count)
                throw new ArgumentException ("Insufficient space in the target location to copy the information.");
            switch (array)
            {
                case T[] array1:
                    this.CopyTo (array1, index);
                    break;
                case object[] objArray:
                    T linkedListNode = this.First;
                    try
                    {
                        if (linkedListNode == null)
                            break;
                        do
                        {
                            var index1 = index++;

                            // ISSUE: variable of a boxed type
                            var local = (object) linkedListNode;
                            objArray[index1] = local;
                            linkedListNode = linkedListNode.next;
                        }
                        while (linkedListNode != this.First);
                        break;
                    }
                    catch (ArrayTypeMismatchException ex)
                    {
                        throw new ArgumentException ("Target array type is not compatible with the type of items in the collection.", nameof (array));
                    }
                default:
                    throw new ArgumentException ("Target array type is not compatible with the type of items in the collection.", nameof (array));
            }
        }

        public Enumerator GetEnumerator () => new Enumerator (this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator () => this.GetEnumerator ();
        IEnumerator IEnumerable.GetEnumerator () => this.GetEnumerator ();

        public struct Enumerator : IEnumerator<T>
        {
            private LinkedObjects<T> _list;
            private T _node;
            private int _version;
            private T _current;
            private int _index;

            internal Enumerator (LinkedObjects<T> list)
            {
                this._list = list;
                this._version = list.version;
                this._node = list.First;
                this._current = default;
                this._index = 0;
            }

            public T Current => this._current;

            object IEnumerator.Current
            {
                get
                {
                    if (this._index == 0 || this._index == this._list.Count + 1)
                        throw new InvalidOperationException ("Enumeration has either not started or has already finished.");
                    return this._current;
                }
            }

            public bool MoveNext ()
            {
                if (this._version != this._list.version)
                    throw new InvalidOperationException ("Collection was modified; enumeration operation may not execute.");
                if (this._node == null)
                {
                    this._index = this._list.Count + 1;
                    return false;
                }

                ++this._index;
                this._current = this._node;
                this._node = this._node.next;
                if (this._node == this._list.First)
                    this._node = null;

                return true;
            }

            void IEnumerator.Reset ()
            {
                if (this._version != this._list.version)
                    throw new InvalidOperationException ("Collection was modified; enumeration operation may not execute.");
                this._current = default;
                this._node = this._list.First;
                this._index = 0;
            }

            public void Dispose () { }
        }

        public LinkedObjects<T> SplitFrom (T fromNode)
        {
            var newList = new LinkedObjects<T> ();
            for (var node = fromNode; fromNode != fromNode.list.Last;)
            {
                var nextNode = node.next;
                node.list.Remove (nextNode);
                newList.AddLast (nextNode);
            }
            return newList;
        }
    }
}