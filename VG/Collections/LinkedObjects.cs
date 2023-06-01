using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace VG.Collections
{
    /// <summary>
    ///     Это как <see cref="T:System.Collections.Generic.LinkedList`1" /> только через интерфейс
    /// </summary>
    public class LinkedObjects<T> : ICollection<T>, ICollection where T : class, ILinkedObject<T>
    {
        private object _syncRoot;
        private int version;

        public LinkedObjects() { }

        public LinkedObjects(params T[] collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            foreach (var obj in collection)
                AddLast(obj);
        }

        public LinkedObjects(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            foreach (var obj in collection)
                AddLast(obj);
        }

        public T First { get; private set; }
        public T Last => First?.prev;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1)
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.",
                    nameof(array));
            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException("The lower bound of target array must be zero.", nameof(array));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Non-negative number required.");
            if (array.Length - index < Count)
                throw new ArgumentException("Insufficient space in the target location to copy the information.");
            switch (array)
            {
                case T[] array1:
                    CopyTo(array1, index);
                    break;
                case object[] objArray:
                    var linkedListNode = First;
                    try
                    {
                        if (linkedListNode == null)
                            break;
                        do
                        {
                            var index1 = index++;

                            // ISSUE: variable of a boxed type
                            var local = (object)linkedListNode;
                            objArray[index1] = local;
                            linkedListNode = linkedListNode.next;
                        } while (linkedListNode != First);

                        break;
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException(
                            "Target array type is not compatible with the type of items in the collection.",
                            nameof(array));
                    }
                default:
                    throw new ArgumentException(
                        "Target array type is not compatible with the type of items in the collection.", nameof(array));
            }
        }

        public int Count { get; private set; }

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.Add(T value)
        {
            AddLast(value);
        }

        public void Clear()
        {
            var linkedListNode1 = First;
            while (linkedListNode1 != null)
            {
                var linkedListNode2 = linkedListNode1;
                linkedListNode1 = linkedListNode1.next != null && linkedListNode1.next != First
                    ? linkedListNode1.next
                    : null;
                linkedListNode2.Invalidate();
            }

            First = null;
            Count = 0;
            ++version;
        }

        public bool Contains(T value) => Find(value) != null;

        public void CopyTo(T[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Non-negative number required.");
            if (index > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index), index,
                    "Must be less than or equal to the size of the collection.");
            if (array.Length - index < Count)
                throw new ArgumentException("Insufficient space in the target location to copy the information.");
            var linkedListNode = First;
            if (linkedListNode == null)
                return;
            do
            {
                array[index++] = linkedListNode;
                linkedListNode = linkedListNode.next;
            } while (linkedListNode != First);
        }

        public bool Remove(T node)
        {
            if (Find(node) == null)
                return false;
            InternalRemoveNode(node);
            return true;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddAfter(T node, T newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node.next, newNode);
            newNode.list = this;
        }

        public void AddAfter(T node, IEnumerable<T> collection)
        {
            foreach (var newNode in collection)
            {
                AddAfter(node, newNode);
                node = newNode;
            }
        }

        public void AddBefore(T node, T newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node, newNode);
            newNode.list = this;
            if (node != First)
                return;
            First = newNode;
        }

        public void AddFirst(T node)
        {
            ValidateNewNode(node);
            if (First == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(First, node);
                First = node;
            }

            node.list = this;
        }

        public void AddLast(T node)
        {
            ValidateNewNode(node);
            if (First == null)
                InternalInsertNodeToEmptyList(node);
            else
                InternalInsertNodeBefore(First, node);
            node.list = this;
        }

        public T Find(T value)
        {
            var linkedListNode = First;
            var equalityComparer = EqualityComparer<T>.Default;
            if (linkedListNode != null)
            {
                if (value != null)
                {
                    while (!equalityComparer.Equals(linkedListNode, value))
                    {
                        linkedListNode = linkedListNode.next;
                        if (linkedListNode == First)
                            goto label_8;
                    }

                    return linkedListNode;
                }

                while (linkedListNode != null)
                {
                    linkedListNode = linkedListNode.next;
                    if (linkedListNode == First)
                        goto label_8;
                }

                return linkedListNode;
            }

            label_8:
            return null;
        }

        public T Find(Predicate<T> match)
        {
            foreach (var item in this)
                if (match(item))
                    return item;

            return default;
        }

        public T FindLast(T value)
        {
            if (First == null)
                return null;
            var prev = First.prev;
            var linkedListNode = prev;
            var equalityComparer = EqualityComparer<T>.Default;
            if (linkedListNode != null)
            {
                if (value != null)
                {
                    while (!equalityComparer.Equals(linkedListNode, value))
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

        public void RemoveFirst()
        {
            if (First == null)
                throw new InvalidOperationException("The LinkedList is empty.");
            InternalRemoveNode(First);
        }

        public void RemoveLast()
        {
            if (First == null)
                throw new InvalidOperationException("The LinkedList is empty.");
            InternalRemoveNode(First.prev);
        }

        private void InternalInsertNodeBefore(T node, T newNode)
        {
            newNode.next = node;
            newNode.prev = node.prev;
            node.prev.next = newNode;
            node.prev = newNode;
            ++version;
            ++Count;
        }

        private void InternalInsertNodeToEmptyList(T newNode)
        {
            newNode.next = newNode;
            newNode.prev = newNode;
            First = newNode;
            ++version;
            ++Count;
        }

        internal void InternalRemoveNode(T node)
        {
            if (node.next == node)
            {
                First = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                if (First == node)
                    First = node.next;
            }

            node.Invalidate();
            --Count;
            ++version;
        }

        internal void ValidateNewNode(T node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // if (node.LinkedObjects != null)
            //     throw new InvalidOperationException ("The LinkedList node already belongs to a LinkedList.");
        }

        internal void ValidateNode(T node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node.list != this)
                throw new InvalidOperationException("The LinkedList node does not belong to current LinkedList.");
        }

        public Enumerator GetEnumerator() => new(this);

        public LinkedObjects<T> SplitFrom(T fromNode)
        {
            var newList = new LinkedObjects<T>();
            for (var node = fromNode; fromNode != fromNode.list.Last;)
            {
                var nextNode = node.next;
                node.list.Remove(nextNode);
                newList.AddLast(nextNode);
            }

            return newList;
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly LinkedObjects<T> _list;
            private T _node;
            private readonly int _version;
            private int _index;

            internal Enumerator(LinkedObjects<T> list)
            {
                _list = list;
                _version = list.version;
                _node = list.First;
                Current = default;
                _index = 0;
            }

            public T Current { get; private set; }

            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || _index == _list.Count + 1)
                        throw new InvalidOperationException(
                            "Enumeration has either not started or has already finished.");
                    return Current;
                }
            }

            public bool MoveNext()
            {
                if (_version != _list.version)
                    throw new InvalidOperationException(
                        "Collection was modified; enumeration operation may not execute.");
                if (_node == null)
                {
                    _index = _list.Count + 1;
                    return false;
                }

                ++_index;
                Current = _node;
                _node = _node.next;
                if (_node == _list.First)
                    _node = null;

                return true;
            }

            void IEnumerator.Reset()
            {
                if (_version != _list.version)
                    throw new InvalidOperationException(
                        "Collection was modified; enumeration operation may not execute.");
                Current = default;
                _node = _list.First;
                _index = 0;
            }

            public void Dispose() { }
        }
    }
}