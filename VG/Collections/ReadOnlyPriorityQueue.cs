namespace VG.Collections
{
    public static class ReadOnlyPriorityQueueExtensions
    {
        public static ReadOnlyPriorityQueue<T> AsReadOnly<T>(this PriorityQueue<T> queue) => new(queue);
    }

    public class ReadOnlyPriorityQueue<T>
    {
        private readonly PriorityQueue<T> queue;

        public ReadOnlyPriorityQueue(PriorityQueue<T> queue) => this.queue = queue;

        public int Count => queue.Count;
        public T this[int i] => queue[i];
    }
}