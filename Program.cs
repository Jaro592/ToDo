class Program
{
    static void Main(string[] args)
    {
        string filePath = "tasks.json";
        ITaskRepository repository = new FileTaskRepository(filePath);
        ITaskService service = new TaskService(repository);
        ITaskView view = new ConsoleTaskView(service);
        //IMyCollection<TaskItem> tasks = new MyCollection<TaskItem>(repository.LoadTasks());


        view.Run();
    }
}