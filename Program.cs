class Program
{
    static void Main(string[] args)
    {
        Bstview();
        //StartApp();
    }

    private static void StartApp()
    {
        string taskFilePath = "tasks.json";
        string userFilePath = "user.json";
        string taskUserFilePath = "task_users.json";

        ITaskRepository taskrepository = new JsonTaskRepository(taskFilePath);
        IUserRepository userrepository = new JsonUserRepository(userFilePath);
        ITaskUserRepository taskUserRepository = new JsonTaskUserRepository(taskUserFilePath);

        ITaskUserService taskUserService = new TaskUserService(taskUserRepository, taskrepository);
        ITaskService taskService = new TaskSerivce(taskrepository, taskUserService);
        IUserService userService = new UserService(userrepository);

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