namespace VG.Collections
{
    public interface ILinkedObject<T> where T : class, ILinkedObject<T>
    {
        LinkedObjects<T> list { get; set; }
        T next { get; set; }
        T prev { get; set; }

        void Invalidate();
    }
}