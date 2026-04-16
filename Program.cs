class Program
{
    static void Main(string[] args)
    {
        //Bstview();
        StartApp();
        // var ht = new HashTable<string, int>();

        // ht.Add("A", 1);
        // ht.Add("B", 2);
        // ht.Add("A", 5);

        // Console.WriteLine(ht.Get("A"));
        // Console.WriteLine(ht.Get("B"));
        // Console.WriteLine(ht.Get("C"));
    }

    private static void StartApp()
    {
        string taskFilePath = "tasks.json";
        string userFilePath = "user.json";
        string taskUserFilePath = "task_users.json";

        ITaskRepository taskRepository = new JsonTaskRepository(taskFilePath, new MyArray<TaskItem>());
        IUserRepository userRepository = new JsonUserRepository(userFilePath, new MyLinkedList<User>());
        ITaskUserRepository taskUserRepository = new JsonTaskUserRepository(taskUserFilePath, new MyLinkedList<TaskUser>());

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
}