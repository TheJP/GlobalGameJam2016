using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implementation of priority queue based on lists and LINQ. Not very fast.
/// </summary>
class ListPriorityQueue<T> : IPriorityQueue<T>
{
    private List<PriorityQueueNode<T>> _queue; 

    public ListPriorityQueue()
    {
        _queue = new List<PriorityQueueNode<T>>();
    }

    public int Count { get { return _queue.Count; } }

    public void Enqueue(T item, int priority)
    {
        _queue.Add(new PriorityQueueNode<T>(item, priority));
    }
    public T Dequeue()
    {
        var min = _queue.Min();
        var item = _queue.FindAll(i => i == min).Last();

        _queue.Remove(item);
        return item.Item;
    }
}

