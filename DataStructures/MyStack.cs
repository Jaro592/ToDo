public class MyStack<T> where T : IEquatable<T>
{
    private readonly MyLinkedList<T> _list = new();

    public int Count => _list.Count;
    public bool IsEmpty => _list.Count == 0;

    public void Push(T item)
    {
        _list.AddFirst(item);
    }

    public T Pop()
    {
        if (IsEmpty) return default(T)!;
        var top = Peek();
        _list.Remove(top);
        return top;
    }


    public T Peek()
    {
        if (IsEmpty) return default(T)!;
        var it = _list.GetIterator();
        return it.Next();
    }

    public void Reset() => _list.Reset();

    public T Next() => _list.Next();

}