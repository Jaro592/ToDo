public class BstView : Serialize
{
    private IMyCollection<TaskItem> _bst;

    public BstView(IMyCollection<TaskItem> bst)
    {
        _bst = bst;
    }

    public void Display()
    {
        Console.WriteLine("Tasks in BST AVL:");

        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "Game engine opzetten" });
        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "Graphics ontwerpen" });
        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "De game daadwerkelijk coderen" });

        if(_bst is BSTAVL<TaskItem> bstAvl)
        {
            bstAvl.PrintTree();
        }
    }
}