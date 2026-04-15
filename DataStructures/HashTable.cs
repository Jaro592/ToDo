public class HashTable<K, V> : IHashTable<K, V>
{
    private IMyCollection<Entry>[] buckets;
    private int Capacity;
    private int _count;
    private bool _isResizing = false;
    public int Count { get => _count; }
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
        return Math.Abs(key!.GetHashCode()) % Capacity;
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
        if (!_isResizing && (double)_count / Capacity > 0.75)
        {
            Resize();
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
    public void Remove(K key)
    {

    }
    public bool ContainsKey(K key)
    {
        return true;
    }




    private sealed class Entry : IEquatable<Entry>
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
