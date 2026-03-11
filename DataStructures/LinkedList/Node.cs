public class Node<T>
{
    public T Data;
    public Node<T>? Next;
    public Node(T data, Node<T>? next = null)
    {
        Data = data;
        Next = next;
    }

    // class LinkList()
    // {
    //     Node<T>? First = null;
    //     public void AddFirst(T value) => First = new Node<T>(value, First);



    //     public void Add(T value)
    //     {
    //         var nNode = new Node<T>(value);
    //         if (First == null)
    //         {
    //             First = nNode;
    //             return;
    //         }

    //         var curr = First;
    //         while (curr.Next != null)
    //             curr = curr.Next;
    //         curr.Next = nNode;
    //     }


    //     void DisplayList()
    //     {
    //         var curr = First;
    //         while (curr != null)
    //         {
    //             System.Console.WriteLine(curr._data);
    //         }
    //     }

    // }
    // public class TestLinkList
    // {
    //     public static void Main()
    //     {
    //         var MyIntList = new Linklist<int>();
    //         MyIntList.AddFirst(1);
    //     }
    // }
}