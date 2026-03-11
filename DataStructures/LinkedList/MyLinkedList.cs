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
        if (First == null) return;


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