using Microsoft.VisualBasic;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    private int? filterStatus = null;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        Console.WriteLine("=== ToDo List ===");
        tasks.Reset();
        while (tasks.HasNext())
        {
            var task = tasks.Next();
            Console.WriteLine(task.ToString());
        }
    }


    string? Prompt(string prompt){
        Console.Write(prompt);
        return Console.ReadLine()??  "";
    }

    public void Run()
    {
        while (true)
        {
            var tmp = _service.GetAllTasks();

            switch(filterStatus)
            {
                case 1:
                    tmp = tmp.Filter(x => x.Completed == true);
                    break;
                case 2:
                    tmp = tmp.Filter(x => x.Completed == false);
                    break;
                case 3:
                    tmp = _service.GetAllTasks();
                    break;
                default:
                    tmp = _service.GetAllTasks();
                    break;

            }
            DisplayTasks(tmp);

            System.Console.WriteLine("\n Options");
            System.Console.WriteLine("1. Add task");
            System.Console.WriteLine("2. Remove task");
            System.Console.WriteLine("3. Toggle task completion");
            System.Console.WriteLine("4. Filter by completion");
            System.Console.WriteLine("5. Exit");

            string? option = Prompt("select an option: ");
            switch (option)
            {
                case "1":
                    string? description = Prompt("Enter task description: ");
                    _service.AddTask(description);
                    break;
                case "2":
                    int idToRemove = int.TryParse(Prompt("Enter task id to remove: "), out int removeId) ? removeId : 0; // veiliger parsen
                    _service.RemoveTask(idToRemove);
                    break;
                case "3":
                    string? idToToggle = Prompt("Enter task id to toggle completion: ");
                    if(int.TryParse(idToToggle, out int toggleId)){
                        _service.ToggleTaskCompletion(toggleId);
                    }
                    break;
                case "4":

                    string? optionFilter = Prompt("'completed' (1) or 'in progress' (2) remove filter (3)\n> ");
                    switch(optionFilter)
                    {
                        case "1":
                            filterStatus = 1;
                            break;
                        case "2":
                            filterStatus = 2;
                            break;
                        case "3":
                            filterStatus = 3;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
