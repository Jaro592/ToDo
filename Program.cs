class Program
{
    static void Main(string[] args)
    {
        //Bstview();
        StartApp();
    }

    private static void StartApp()
    {
                string taskFilePath = "tasks.json";
        string userFilePath = "user.json";
        string taskUserFilePath = "task_users.json";

        ITaskRepository taskrepository = new JsonTaskRepository(taskFilePath);
        IUserRepository userrepository = new JsonUserRepository(userFilePath);
        ITaskUserRepository taskUserRepository = new JsonTaskUserRepository(taskUserFilePath);

        ITaskService taskService = new TaskSerivce(taskrepository);
        IUserService userService = new UserService(userrepository);
        ITaskUserService taskUserService = new TaskUserService(taskUserRepository, taskrepository);

        ITaskView view = new ConsoleTaskView(taskService, userService, taskUserService);
        //IMyCollection<TaskItem> tasks = new MyCollection<TaskItem>(repository.LoadTasks());


        view.Run();
    }

    public static void Bstview()
    {
        IMyCollection<TaskItem> bst = new BSTAVL<TaskItem>();
        BstView view = new BstView(bst);
        view.Display();
    }
}