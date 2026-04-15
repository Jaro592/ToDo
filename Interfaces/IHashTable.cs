public interface IHashTable<K, V>
{
    void Add(K key, V value);
    V? Get(K key);
    void Remove(K key);
    bool ContainsKey(K key);

}