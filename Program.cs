class Program
{
    static void Main(string[] args)
    {
        string filePath = "tasks.json";
        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskSerivce(repository);
        ITaskView view = new ConsoleTaskView(service);
        //IMyCollection<TaskItem> tasks = new MyCollection<TaskItem>(repository.LoadTasks());


        view.Run();
    }
}