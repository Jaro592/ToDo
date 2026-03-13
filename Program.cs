class Program
{
    static void Main(string[] args)
    {
        var list = new MyLinkedList<int>();

        list.Add(1);
        list.Add(2);
        list.Add(3);

        list.Remove(2);

        Console.WriteLine("Count: " + list.Count);

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        var found = list.FindBy(3, (value, key) => value.CompareTo(key));
        Console.WriteLine("Found: " + found);
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();


        int sum = list.Reduce(0, (acc, x) => acc + x);
        Console.WriteLine("Sum: " + sum);
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        string filePath = "tasks.json";
        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskSerivce(repository);
        ITaskView view = new ConsoleTaskView(service);
        //IMyCollection<TaskItem> tasks = new MyCollection<TaskItem>(repository.LoadTasks());


        view.Run();
    }
}