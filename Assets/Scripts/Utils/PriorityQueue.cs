using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

public class PriorityQueue<T>
{
    private SortedSet<QueuedItem> _sortedSet = new SortedSet<QueuedItem>(new QueuedItemComparer());

    public void Enqueue(T item, int priority)
    {
        QueuedItem queuedItem = new QueuedItem { Priority = priority, Value = item };
        if (_sortedSet.Contains(queuedItem)) {
            //UnityEngine.Debug.Log(string.Format("%d is already in.", priority));
            _sortedSet.Remove(queuedItem); 
        }
        _sortedSet.Add(queuedItem);
    }

    public bool Dequeue(out T value)
    {
        if( _sortedSet.Count > 0 )
        {
            QueuedItem item = _sortedSet.Min;
            _sortedSet.Remove(item);
            value = item.Value; 
            return true;
        }
        value = default(T);
        return false;
    }

    public void Clear()
    {
        _sortedSet.Clear();
    }
    public bool isEmpty()
    {
        return _sortedSet.Count == 0;
    }

    private class QueuedItem
    {
        public int Priority { get; set; }
        public T Value { get; set; }

        public static bool operator ==(QueuedItem p1, QueuedItem p2)
        {
            return p1.Priority == p2.Priority;
        }

        public static bool operator !=(QueuedItem p1, QueuedItem p2)
        {
            return p1.Priority != p2.Priority;
        }

        public override int GetHashCode()
        {
            return Priority.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (QueuedItem)obj == this;
        }
    }



    private class QueuedItemComparer : IComparer<QueuedItem>
    {
        public int Compare(QueuedItem x, QueuedItem y)
        {
            return x.Priority.CompareTo(y.Priority);
        }
    }
}
