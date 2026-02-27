public class MyArray<T> : IMyCollection<T>, IMyIterator<T> where T : IEquatable<T>
{
    private T[] _data;
    private int _index;
    private int _iteratorIndex = -1;
    private bool _isDirty = false;

    public MyArray(int size = 10)
    {
        _data = new T[size];
        _index = -1;
    }

    public MyArray(T[] data, int lastIndex)
    {
        _data = data;
        _index = lastIndex;
    }

    public int Count => _index + 1;
    public bool Dirty => _isDirty;
    public T[] data { get => _data; set { _data = value; _isDirty = true; } }


    public void Add(T item)
    {
        if (_index + 1 >= _data.Length)
        {
            T[] newData = new T[_data.Length * 2];
            for (int i = 0; i <= _index; i++) newData[i] = _data[i];
            _data = newData;
        }
        
        _index++;
        _data[_index] = item;
        _isDirty = true;
    }

    public void Remove(T item)
    {
        int foundIndex = Find(item);
        if (foundIndex != -1)
        {
            Shift(foundIndex, false);
            _isDirty = true;
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        for(int i = 0; i <= _index; i++)
        {
            if(comparer(_data[i], key)) return _data[i];
        }
        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        MyArray<T> result = new MyArray<T>(_data.Length);
        for (int i = 0; i <= _index; i++)
        {
            if (predicate(_data[i]))
            {
                result.Add(_data[i]);
            }
        }
        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        for (int i = 0; i <= _index; i++)
        {
            for (int j = 0; j < _index - i; j++)
            {
                if (comparison(_data[j], _data[j + 1]) > 0)
                {
                    Swap(j, j + 1);
                }
            }
        }
        _isDirty = true;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        for (int i = 0; i <= _index; i++)
        {
            result = accumulator(result, _data[i]);
        }
        return result;
    }

    public bool HasNext() => _iteratorIndex + 1 <= _index;

    public T Next()
    {
        if (!HasNext()) return default!;
        return _data[++_iteratorIndex];
    }

    public void Reset() => _iteratorIndex = -1;

    public IMyIterator<T> GetIterator() => (IMyIterator<T>)this; // ook ai

    public IEnumerator<T> GetEnumerator() // dit is pure ai geen idee hoe dit werkt
    {
        Reset();
        while (HasNext())
        {
            yield return Next();
        }
    }

    public int Find(T Item, int startIndex = 0)
    {
        if (startIndex < 0 || startIndex > _index) return -1;
        for (int i = startIndex; i <= _index; i++)
        {
            if (_data[i] != null && _data[i].Equals(Item)) return i;
        }
        return -1;
    }

    public void Swap(int i, int j)
    {
        if (i < 0 || i > _index || j < 0 || j > _index) return;
        T temp = _data[i];
        _data[i] = _data[j];
        _data[j] = temp;
    }

    public void Shift(int i, bool right = true)
    {
        if (right)
        {
            if (_index + 1 >= _data.Length) return;
            for (int j = _index; j >= i; j--) _data[j + 1] = _data[j];
            _index++;
        }
        else
        {
            for (int j = i; j < _index; j++) _data[j] = _data[j + 1];
            _data[_index] = default!;
            _index--;
        }
        _isDirty = true;
    }
}