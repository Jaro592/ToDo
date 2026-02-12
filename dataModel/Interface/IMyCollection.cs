interface IMyCollection<T>
{
    void Add(T item);
    void Remove(T item);
    T FindBy<K>(K key, Func<T,K,bool> Comparer);
    IMyCollection<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; }
    bool Dirty { get; }

    R Reduce<R>( R initial, Func<R,T,R> accumulator);

    IMyIterator<T> GetIterator();
    IEnumerator<T> GetEnumerator();

}
