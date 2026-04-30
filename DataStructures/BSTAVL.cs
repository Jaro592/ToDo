public class BSTAVL<T> : IMyCollection<T>, IMyIterator<T> where T : IComparable<T> // Jaro
{
    private Node? _root;
    private int _count;
    private MyStack<Node>? _stack;
    public int Count => _count;
    public bool Dirty { get; set; }

    public BSTAVL()
    {
        _stack = new MyStack<Node>();
    }


    public void Add(T item)
    {
        int before = _count;
        bool added = false;
        _root = Insert(_root, item, ref added);
        if (added) 
        { 
            _count++; 
            Dirty = true; 
        }
    }

    public void Remove(T item)
    {
        bool found = false;
        _root = Remove(_root, item, ref found);
        if (found) 
        { 
            _count--; 
            Dirty = true; 
        }
    }

    public T? FindBy<K>(K key, Func<T, K, int> comparer)   // O(log n)
    {
        Node? cur = _root;
        while (cur != null)
        {
            int cmp = comparer(cur.Value, key);
            if (cmp == 0) return cur.Value;
            cur = cmp > 0 ? cur.Left : cur.Right;
        }
        return default;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate) 
    { 
        var res = new BSTAVL<T>();
        if(_root is null) 
        {
            return re;
        }

        var it = GetIterator();
        while (it.HasNext())
        {
            T value = it.Next();
            if(predicate(value))
            {
                res.Add(value);
            }
        }
        return result;

    }
    public void Sort(Comparison<T> comparison)
    {
        // bst is already sorted based on id
    }
    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    { 
        R res = initial; 

        var it = GetIterator();
        while(it.HasNext())
        {
            result = accumulator(res, it.Next());
        }

        return result;

    }

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


    public void Reset()
    {
        _stack = new MyStack<Node>();
        PushLeftSpine(_root);
    }

    public bool HasNext() => !_stack.IsEmpty;

    public T? Next()
    {
        if (!HasNext()) return default!;

        Node? node = _stack?.Pop();
        T value = node.Value;

        PushLeftSpine(node.Right);

        return value;
    }


    private void PushLeftSpine(Node? node)
    {
        while (node != null)
        {
            _stack.Push(node);
            node = node.Left;
        }
    }

    public void PrintTree() => Display(_root);


    private Node Insert(Node? node, T value, ref bool added)
    {
        if (node == null) { added = true; return new Node(value); }

        int cmp = value.CompareTo(node.Value);
        if(cmp < 0) 
        {
            node.Left  = Insert(node.Left,  value, ref added);
        }
        else if(cmp > 0)
        { 
            node.Right = Insert(node.Right, value, ref added);
        }
        else
        {
             return node; // duplicates not allowed
        }

        return Rebalance(node);
    }
    private Node? Remove(Node? node, T value, ref bool found)
    {
        if (node == null) return null;

        int cmp = value.CompareTo(node.Value);
        if      (cmp < 0) node.Left  = Remove(node.Left,  value, ref found);
        else if (cmp > 0) node.Right = Remove(node.Right, value, ref found);
        else
        {
            found = true;
            if (node.Left == null)  return node.Right;
            if (node.Right == null) return node.Left;

            Node successor = node.Right;
            while (successor.Left != null) successor = successor.Left;
            node.Value = successor.Value;
            node.Right = Remove(node.Right, successor.Value, ref found);
        }

        return Rebalance(node);
    }

    private Node? Rebalance(Node? node)
    {
        if (node == null) return null;
        node.Height = 1 + Math.Max(GetHeight(node?.Left), GetHeight(node?.Right));
        int bf = GetBalanceFactor(node);

        if (bf > 1)
        {
            if (GetBalanceFactor(node.Left) >= 0)  return RotateRight(node);
            node.Left = RotateLeft(node.Left);      return RotateRight(node);
        }
        if (bf < -1)
        {
            if (GetBalanceFactor(node.Right) <= 0) return RotateLeft(node);
            node.Right = RotateRight(node.Right);   return RotateLeft(node);
        }
        return node;
    }

    private int GetHeight(Node? n)
    {
        return n == null ? 0 : n.Height;
    }
    private int GetBalanceFactor(Node? n)
    {
        return n == null ? 0 : GetHeight(n.Left) - GetHeight(n.Right); 
    }

    private Node? RotateRight(Node y)
    {
        Node? x = y.Left!, t = x.Right;
        x.Right = y; y.Left = t;
        y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
        x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
        return x;
    }

    private Node? RotateLeft(Node x)
    {
        Node? y = x.Right!, t = y?.Left;
        y?.Left = x; x?.Right = t;
        x?.Height = 1 + Math.Max(GetHeight(x?.Left), GetHeight(x?.Right));
        y?.Height = 1 + Math.Max(GetHeight(y?.Left), GetHeight(y?.Right));
        return y;
    }

    private void Display(Node? root)
    {
        if (root == null) return;
        Display(root.Left);
        Console.WriteLine(root.Value);
        Display(root.Right);
    }

    private class Node : IEquatable<Node>
    {
        public T Value;
        public int Height;
        public Node? Left, Right;
        public Node(T value) 
        { 
            Value = value; Height = 1; 
        }

        public bool Equals(Node? other) 
        {
            return ReferenceEquals(this, other);
        }
    }
    }