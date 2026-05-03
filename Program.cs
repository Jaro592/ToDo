class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        //Bstview();

        StartApp();
        // HashView();
    }

    private static void StartApp()
    {
        string taskFilePath = "tasks.json";
        string userFilePath = "user.json";
        string taskUserFilePath = "task_users.json";

        ITaskRepository taskRepository = new JsonTaskRepository(taskFilePath, new BSTAVL<TaskItem>());
        IUserRepository userRepository = new JsonUserRepository(userFilePath, new BSTAVL<User>());
        ITaskUserRepository taskUserRepository = new JsonTaskUserRepository(taskUserFilePath, new BSTAVL<TaskUser>());

        ITaskUserService taskUserService = new TaskUserService(taskUserRepository, taskRepository);
        ITaskService taskService = new TaskSerivce(taskRepository, taskUserService);
        IUserService userService = new UserService(userRepository);

        ITaskView view = new ConsoleTaskView(taskService, userService, taskUserService);
        //IMyCollection<TaskItem> tasks = new MyCollection<TaskItem>(repository.LoadTasks());


        view.Run();
    }

    public static void Bstview()
    {
        BSTAVL<TaskItem> bst = new BSTAVL<TaskItem>();
        BstView view = new BstView(bst);
        view.Fill();
        view.Display();
    }
    public static void HashView()
    {
        var table = new HashTable<string, TaskItem>();
        var view = new HashTableView(table);

        view.Fill();
        view.Display();
        view.TestBehavior();
    }
}