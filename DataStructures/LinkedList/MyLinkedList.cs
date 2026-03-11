public class MyLinkedList<T> : IMyCollection<T>
{

    private int _count;
    private bool _dirty;
    Node<T>? First = null;
    public int Count { get => _count; }
    public bool Dirty { get => _dirty; }

    // private Node<T> _node;

    public void Add(T item)
    {
        Node<T> nNode = new Node<T>(item);

        if (First is null)
        {
            First = nNode;
        }
        else
        {
            Node<T> current = First;
            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = nNode;

        }
        _count++;
        _dirty = true;

    }
    public void AddFirst(T value)
    {
        First = new Node<T>(value, First);
        _count++;
        _dirty = true;
    }

    void Remove(T item)
    {
        if (First is null) return;

        if (First.Data.Equals(item))
        {
            First = First.Next;
            _count--;
            _dirty = true;
            return;
        }
        Node<T> current = First;
        while (current.Next is not null)
        {
            if (current.Next.Data.Equals(item))
            {
                current.Next = current.Next.Next;
                _count--;
                _dirty = true;
                return;
            }
            current = current.Next;
        }


    }
    T FindBy<K>(K key, Func<T, K, bool> Comparer)
    {
        Node<T>? current = First;

        while (current != null)
        {
            if (Comparer(current.Data, key))
            {
                return current.Data;
            }
            current = current.Next;
        }

        throw new InvalidOperationException("Item not found");
    }
    IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        IMyCollection<T> result = new MyCollection<T>();
        if (First is null) return result;

        Node<T>? current = First;
        while (current is not null)
        {
            if (predicate(current.Data))
            {
                result.Add(current.Data);
            }
            current = current.Next;
        }
        return result;
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