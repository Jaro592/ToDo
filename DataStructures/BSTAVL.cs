public class BSTAVL<T> : IMyCollection<T>, IMyIterator<T> where T : IComparable<T>// jaro
{
    private AVLTree<T> _tree;
        private int _count;
        private Node<T> current;
        public int Count { get => _count; }
        public bool Dirty { get => false; }

        public BSTAVL()
        {
            _tree = new AVLTree<T>();
            _count = 0;
            current = null;
        }
        public void Add(T item)
        {
            _tree.Root = _tree.Insert(_tree.Root, item);
            _count++;
        }
        public void Remove(T item)
        {
        }
        public T? FindBy<K>(K key, Func<T,K,int> Comparer) // O(log n)
        {
            Node<T>? current = _tree.Root;
            while (current != null)
            {
                int comparison = Comparer(current.Value, key);
                if (comparison == 0)
                {
                    return current.Value;
                }
                else if (comparison > 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
            return default;
        }
        public IMyCollection<T> Filter(Func<T, bool> predicate)
        {
            return null;
        }
        public void Sort(Comparison<T> comparison)
        {
        }
        public R Reduce<R>(R initial, Func<R,T,R> accumulator)
        {
            return initial;
        }
        public IMyIterator<T> GetIterator()
        {
            return this;
        }
        public IMyCollection<T> GetEnumerator()
        {
            return this;
        }
        public bool HasNext()
        {
            return current != null;
        }
        public T Next()
        {
            if (!HasNext()) return default!;
            T value = current.Value;
            current = current.Right ?? current.Left; // This is a very simplified in-order traversal, you would need to implement a proper stack-based traversal for a real implementation
            return value;
        }
        public void Reset()
        {
            current = _tree.Root;
        }

        public void PrintTree()
        {
            _tree.Display(_tree.Root);
        }
}
    public class Node<T>
    {
        public T Value;
        public int Height;
        public Node<T> Left;
        public Node<T> Right;
        public Node(T value)
        {
            Value = value;
            Height = 1;
            Left = null;
            Right = null;
        }
    }

    public class AVLTree<T> where T : IComparable<T> {
    public Node<T> Root;
    public Node<T> Insert(Node<T> node, T value)
    {
        if (node == null) {
            return new Node<T>(value);
        }
        if (value.CompareTo(node.Value) < 0) {
            node.Left = Insert(node.Left, value);
        } else if (value.CompareTo(node.Value) > 0) {
            node.Right = Insert(node.Right, value);
        } else {
            return node;  // Duplicate values not allowed
        }
        // Update height of the current node
        node.Height = 1 + (GetHeight(node.Left) > GetHeight(node.Right) ? GetHeight(node.Left) : GetHeight(node.Right));
        // Balance the tree
        int balanceFactor = GetBalanceFactor(node);
        // Left Heavy
        if (balanceFactor > 1) {
            if (value.CompareTo(node.Left.Value) < 0) {
                return RotateRight(node);
            } else {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
        }
        // Right Heavy
        if (balanceFactor < -1) {
            if (value.CompareTo(node.Right.Value) > 0) {
                return RotateLeft(node);
            } else {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }
        }
        return node;
    }
    private int GetHeight(Node<T> node) {
        if (node == null) return 0;
        return node.Height;
    }
    private int GetBalanceFactor(Node<T> node) {
        if (node == null) return 0;
        return GetHeight(node.Left) - GetHeight(node.Right);
    }
    private Node<T> RotateRight(Node<T> y) {
        Node<T> x = y.Left;
        Node<T> T3 = x.Right;
        x.Right = y;
        y.Left = T3;
        
        int ylefth = GetHeight(y.Left);
        int yrightH = GetHeight(y.Right);
        y.Height = 1 + (ylefth > yrightH ? ylefth : yrightH);

        int xlefth = GetHeight(x.Left);
        int xrightH = GetHeight(x.Right);
        x.Height = 1 + (xlefth > xrightH ? xlefth : xrightH);
        return x;
    }
    private Node<T> RotateLeft(Node<T> x) {
        Node<T> y = x.Right;
        Node<T> T2 = y.Left;
        y.Left = x;
        x.Right = T2;

        int xlefth = GetHeight(x.Left);
        int xrightH = GetHeight(x.Right);
        x.Height = 1 + (xlefth > xrightH ? xlefth : xrightH);

        int ylefth = GetHeight(y.Left);
        int yrightH = GetHeight(y.Right);
        y.Height = 1 + (ylefth > yrightH ? ylefth : yrightH);

        return y;
    }
        public void Display(Node<T> root)
        {
            if (root != null) {
                Display(root.Left);
                Console.WriteLine(root.Value);
                Display(root.Right);
            }
        }
    }

