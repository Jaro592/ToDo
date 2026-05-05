public class HashCollection<T> : IMyCollection<T> where T : IEquatable<T>
{
    private readonly HashTable<string, T> _table;

    public int Count => _table.Count;
    public bool Dirty => _table.Dirty;

    public HashCollection()
    {
        _table = new HashTable<string, T>();
    }
    private string GetKey(T item)
    {
        return item switch
        {
            TaskItem t => t.ID,
            User u => u.UserID,
            TaskUser tu => $"{tu.TaskID}_{tu.UserID}",
            _ => throw new InvalidOperationException(
                    $"No key for type {typeof(T).Name}")
        };

    }
    public void Add(T item)
    {
        _table.Add(GetKey(item), item);
    }

    public void Remove(T item)
    {
        _table.Remove(GetKey(item));
    }

    public T? FindBy<K>(K key, Func<T, K, int> comparer)
    {
        var it = GetIterator();
        while (it.HasNext())
        {
            var item = it.Next();
            if (comparer(item, key) == 0)
                return item;
        }
        return default;
    }

    public IMyIterator<T> GetIterator()
    {
        var all = new MyLinkedList<T>();

        var it = _table.GetIterator();
        while (it.HasNext())
        {
            all.Add(it.Next().Value);
        }

        return all.GetIterator();
    }

    public IMyCollection<T> GetEnumerator() => this;

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyLinkedList<T>();
        var it = GetIterator();

        while (it.HasNext())
        {
            var item = it.Next();
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        var result = initial;
        var it = GetIterator();

        while (it.HasNext())
            result = accumulator(result, it.Next());

        return result;
    }

    public void Sort(Comparison<T> comparison) { }
}