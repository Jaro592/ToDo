public class MyLinkedList<T> : IMyCollection<T>, IMyIterator<T> where T : IEquatable<T>
{

    private int _count;
    private bool _dirty;
    Node<T>? First = null;
    public int Count { get => _count; }
    public bool Dirty { get => _dirty; }

    private Node<T>? _current;

    // private Node<T> _node;

    public void Add(T item) // could add a last node to remove the while loop here
    {
        Node<T> nNode = new Node<T>(item);

        if (First is null)
        {
            First = nNode;
        }
        else
        {
            Node<T> current = First;
            while (current.Next is not null)
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

    public void Remove(T item)
    {
        if (First is null) return;

        if (First.Data.Equals(item))
        {
            First = First.Next;
            _count--;
            _dirty = true;
            return;
        }
        Node<T>? current = First;
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
    // T? FindBy<K>(K key, Func<T, K, int> Comparer); // MyCollectio
    public T FindBy<K>(K key, Func<T, K, int> Comparer)
    {
        Node<T>? current = First;

        while (current != null)
        {
            if (Comparer(current.Data, key) == 0)
            {
                return current.Data;
            }
            current = current.Next;
        }

        throw new InvalidOperationException("Item not found");
    }
    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        IMyCollection<T> result = new MyLinkedList<T>();
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

    void Swap(Node<T> a, Node<T> b)
    {
        T temp = a.Data;
        a.Data = b.Data;
        b.Data = temp;
    }
    public void Sort(Comparison<T> comparison)
    {


        if (First is null) return;

        bool swapped;
        do
        {
            Node<T>? current = First;
            swapped = false;
            while (current?.Next is not null)
            {
                if (comparison(current.Data, current.Next.Data) > 0)
                {
                    Swap(current, current.Next);
                    swapped = true;

                }
                current = current.Next;
            }
        } while (swapped);
        _dirty = true;

    }


    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        if (First is null) return result;

        Node<T>? current = First;
        while (current is not null)
        {
            result = accumulator(result, current.Data);
            current = current.Next;
        }
        return result;
    }

    // public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    // {
    //     R result = initial;
    //     for (int i = 0; i <= _index; i++)
    //     {
    //         result = accumulator(result, _data[i]);
    //     }
    //     return result;
    // }

    public IMyIterator<T> GetIterator()
    {
        Reset();
        return this;
    }
    public IMyCollection<T> GetEnumerator()
    {
        Reset();
        return this;
    }

    public bool HasNext()
    {
        return _current != null;
    }
    public T Next()
    {
        if (!HasNext()) return default!;
        T data = _current.Data;
        _current = _current.Next;
        return data;
    }
    public void Reset()
    {
        _current = First;
    }

}