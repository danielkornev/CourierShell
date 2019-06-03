using System.Collections.Generic;
namespace ZU.Collections.Generic
{
    /// <summary>
    /// This class supports Zet Universe and is not intended to be used directly from your code.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPriorityQuadTree<T> : IEnumerable<T>
    {
        void Clear();
        System.Windows.Rect Extent { get; set; }
        new IEnumerator<T> GetEnumerator();
        IEnumerable<T> GetItemsInside(System.Windows.Rect bounds);
        IEnumerable<T> GetItemsIntersecting(System.Windows.Rect bounds);
        bool HasItemsInside(System.Windows.Rect bounds);
        bool HasItemsIntersecting(System.Windows.Rect bounds);
        void Insert(T item, System.Windows.Rect bounds, double priority);
        bool Remove(T item);
        bool Remove(T item, System.Windows.Rect bounds);
    } // interface
} // namespace
