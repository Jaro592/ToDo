// class BSTAVL<T> : IMyCollection<T> where T : IComparable<T>  // jaro
// {
//     public class Node 
//     {
//         public int Value;
//         public int Height;
//         public Node Left;
//         public Node Right;
//         public Node(int value) 
//         {
//             Value = value;
//             Height = 1;
//             Left = null;
//             Right = null;
//         }
//     }

//     public class AVLTree {
//     public Node Root;
//     public Node Insert(Node node, int value) 
//     {
//         if (node == null) {
//             return new Node(value);
//         }
//         if (value < node.Value) {
//             node.Left = Insert(node.Left, value);
//         } else if (value > node.Value) {
//             node.Right = Insert(node.Right, value);
//         } else {
//             return node;  // Duplicate values not allowed
//         }
//         // Update height of the current node
//         node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
//         // Balance the tree
//         int balanceFactor = GetBalanceFactor(node);
//         // Left Heavy
//         if (balanceFactor > 1) {
//             if (value < node.Left.Value) {
//                 return RotateRight(node);
//             } else {
//                 node.Left = RotateLeft(node.Left);
//                 return RotateRight(node);
//             }
//         }
//         // Right Heavy
//         if (balanceFactor < -1) {
//             if (value > node.Right.Value) {
//                 return RotateLeft(node);
//             } else {
//                 node.Right = RotateRight(node.Right);
//                 return RotateLeft(node);
//             }
//         }
//         return node;
//     }
//     private int GetHeight(Node node) {
//         if (node == null) return 0;
//         return node.Height;
//     }
//     private int GetBalanceFactor(Node node) {
//         if (node == null) return 0;
//         return GetHeight(node.Left) - GetHeight(node.Right);
//     }
//     private Node RotateRight(Node y) {
//         Node x = y.Left;
//         Node T3 = x.Right;
//         x.Right = y;
//         y.Left = T3;
//         y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
//         x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
//         return x;
//     }
//     private Node RotateLeft(Node x) {
//         Node y = x.Right;
//         Node T2 = y.Left;
//         y.Left = x;
//         x.Right = T2;
//         x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
//         y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
//         return y;
//     }
//         public void Display(Node root) 
//         {
//             if (root != null) {
//                 Display(root.Left);
//                 Console.WriteLine(root.Value);
//                 Display(root.Right);
//             }
//         }
//     }
// }