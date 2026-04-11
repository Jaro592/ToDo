public class BstView : Serialize
{
private BSTAVL<TaskItem> _bst;

    public BstView(BSTAVL<TaskItem> bst)
    {
        _bst = bst;
    }


    public void Fill()
    {
        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "Task 1", Completed = false });
        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "Task 2", Completed = true });
        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "Task 3", Completed = false });
        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "Task 4", Completed = true });
        _bst.Add(new TaskItem { ID = NewSerializeString(), Description = "Task 5", Completed = false });
    }
    public void Display()
    {
        Console.WriteLine("Tasks in BST AVL:");
        IMyIterator<TaskItem> iterator = _bst.GetIterator();
        iterator.Reset();
        while (iterator.HasNext())
        {
            Console.WriteLine(iterator.Next());
        }
    }
}