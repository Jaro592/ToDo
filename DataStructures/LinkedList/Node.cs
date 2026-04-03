public sealed class Node<T> // Basel
{
    public T Data;
    public Node<T>? Next;
    public Node(T data, Node<T>? next = null)
    {
        Data = data;
        Next = next;
    }

}