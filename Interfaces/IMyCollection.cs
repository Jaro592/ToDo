public interface IMyCollection<T>
{
    void Add(T item);
    void Remove(T item);
    T? FindBy<K>(K key, Func<T, K, int> Comparer);
    IMyCollection<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; }
    bool Dirty { get; }

    R Reduce<R>(R initial, Func<R, T, R> accumulator);

    IMyIterator<T> GetIterator();
    IMyCollection<T> GetEnumerator();
}
