public class MyLinkedList<T> : IMyCollection<T>
{
    private int _count;
    int Count { get => _count; }
    bool Dirty { get; }

    private Node _node;

    void Add(T item)
    {
        Node<T> newNode = new Node<T>();

        if (_node is null)
        {
            _count++;
            Dirty = true;
            return;
        }
        while (_node.Next != null)
        {

        }
    }
    void Remove(T item)
    {

    }
    T FindBy<K>(K key, Func<T, K, bool> Comparer)
    {

    }
    IMyCollection<T> Filter(Func<T, bool> predicate)
    {

    }
    void Sort(Comparison<T> comparison)
    {

    }


    R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {

    }

    IMyIterator<T> GetIterator()
    {

    }
    IEnumerator<T> GetEnumerator()
    {

    }

    bool Hasnext()
    {

    }
    T Next()
    {

    }
    void Reset()
    {

    }

}