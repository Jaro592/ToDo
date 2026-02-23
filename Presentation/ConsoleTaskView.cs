public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    void DisplayTasks(IEnumerable<TaskItem> tasks)
    {
        Console.Clear();
        Console.WriteLine("=== ToDo List ===");
        foreach (var task in tasks)
        {
            System.Console.WriteLine($"{task}");
        }
    }

    string? Prompt(string prompt){
        Console.Write(prompt);
        return Console.ReadLine();  
    }

    public void Run()
    {
        while (true)
        {
            DisplayTasks(_service.GetAllTasks());
            System.Console.WriteLine("\n Options");
            System.Console.WriteLine("1. Add Task");
            System.Console.WriteLine("2. Remove Task");
            System.Console.WriteLine("3. Toggle Task Completion");
            System.Console.WriteLine("4. Exit");

            string option = Prompt("select an option: ");
            switch (option)
            {
                case "1":
                    string description = Prompt("Enter task description: ");
                    _service.AddTask(description);
                    break;
                case "2":
                    int idToRemove = int.Parse(Prompt("Enter task id to remove: "));
                    _service.RemoveTask(idToRemove);
                    break;
                case "3":
                    string idToToggle = Prompt("Enter task id to toggle completion: ");
                    if(int.TryParse(idToToggle, out int toggleId)){
                        _service.ToggleTaskCompletion(toggleId);
                    }
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}