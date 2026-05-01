public class HashTable<K, V> : IHashTable<K, V>, IMyCollection<HashTable<K, V>.Entry>
{
    private IMyCollection<Entry>[] buckets;
    private int Capacity;
    private int _count;
    private bool _isDirty = false;
    private bool _isResizing = false;

    public int Count => _count;
    public bool Dirty => _isDirty;

    public HashTable(int capacity = 10)
    {
        Capacity = capacity;
        buckets = new IMyCollection<Entry>[Capacity];

        for (int i = 0; i < Capacity; i++)
        {
            buckets[i] = new MyLinkedList<Entry>();
        }
    }

    private int GetIndex(K key)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        return Math.Abs(key.GetHashCode()) % Capacity;
    }

    private void Resize()
    {
        _isResizing = true;

        var oldBuckets = buckets;

        Capacity *= 2;
        buckets = new IMyCollection<Entry>[Capacity];

        for (int i = 0; i < Capacity; i++)
        {
            buckets[i] = new MyLinkedList<Entry>();
        }

        _count = 0;

        for (int i = 0; i < oldBuckets.Length; i++)
        {
            var it = oldBuckets[i].GetIterator();
            while (it.HasNext())
            {
                var entry = it.Next();
                Add(entry.Key, entry.Value);
            }
        }

        _isResizing = false;
    }


    // HashTable 


    public void Add(K key, V value)
    {
        int index = GetIndex(key);
        var bucket = buckets[index];

        var it = bucket.GetIterator();

        while (it.HasNext())
        {
            var entry = it.Next();

            if (entry.Key.Equals(key))
            {
                entry.Value = value;
                return;
            }
        }

        bucket.Add(new Entry(key, value));
        _count++;
        _isDirty = true;

        if (!_isResizing && (double)_count / Capacity > 0.75)
        {
            Resize();
        }
    }

    public void Remove(K key)
    {
        int index = GetIndex(key);
        var bucket = buckets[index];

        var it = bucket.GetIterator();

        while (it.HasNext())
        {
            var entry = it.Next();

            if (entry.Key.Equals(key))
            {
                bucket.Remove(entry);
                _count--;
                _isDirty = true;
                return;
            }
        }
    }

    public V? Get(K key)
    {
        int index = GetIndex(key);
        var bucket = buckets[index];

        var it = bucket.GetIterator();

        while (it.HasNext())
        {
            var entry = it.Next();

            if (entry.Key.Equals(key))
            {
                return entry.Value;
            }
        }

        return default;
    }

    public bool ContainsKey(K key)
    {
        int index = GetIndex(key);
        var bucket = buckets[index];

        var it = bucket.GetIterator();

        while (it.HasNext())
        {
            if (it.Next().Key.Equals(key))
                return true;
        }

        return false;
    }


    // IMyCollection


    public void Add(Entry item)
    {
        Add(item.Key, item.Value);
    }

    public void Remove(Entry item)
    {
        Remove(item.Key);
    }

    public IMyIterator<Entry> GetIterator()
    {
        var all = new MyLinkedList<Entry>();

        for (int i = 0; i < Capacity; i++)
        {
            var it = buckets[i].GetIterator();
            while (it.HasNext())
            {
                all.Add(it.Next());
            }
        }

        return all.GetIterator();
    }

    public IMyCollection<Entry> GetEnumerator()
    {
        return this;
    }

    public Entry? FindBy<K2>(K2 key, Func<Entry, K2, int> comparer)
    {
        var it = GetIterator();

        while (it.HasNext())
        {
            var e = it.Next();
            if (comparer(e, key) == 0)
                return e;
        }

        return default;
    }

    public IMyCollection<Entry> Filter(Func<Entry, bool> predicate)
    {
        var result = new MyLinkedList<Entry>();
        var it = GetIterator();

        while (it.HasNext())
        {
            var e = it.Next();
            if (predicate(e))
                result.Add(e);
        }

        return result;
    }

    public R Reduce<R>(R initial, Func<R, Entry, R> accumulator)
    {
        var result = initial;
        var it = GetIterator();

        while (it.HasNext())
        {
            result = accumulator(result, it.Next());
        }

        return result;
    }

    public void Sort(Comparison<Entry> comparison)
    {
    }


    public sealed class Entry : IEquatable<Entry>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public Entry(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public bool Equals(Entry? other)
        {
            if (other == null) return false;
            return Key!.Equals(other.Key);
        }
    }
}