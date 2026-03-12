using System.Linq.Expressions;
using Microsoft.VisualBasic;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }


    void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        Console.WriteLine("=== ToDo List ===\n");

        Console.WriteLine("---In progress---");

        tasks.Reset();
        var iterProgress = tasks.Filter(x => x.Completed == false).GetIterator();
        while (iterProgress.HasNext())
        {
            var iter = iterProgress.Next();
            Console.WriteLine(iter.ToString());
        }
        
        System.Console.WriteLine("\n");

        System.Console.WriteLine("---Completed---");
        tasks.Reset();
        var iterCompleted = tasks.Filter(x => x.Completed == true).GetIterator();
        while (iterCompleted.HasNext())
        {
            var iter = iterCompleted.Next();
            System.Console.WriteLine(iter.ToString());
        }
    }


    string? Prompt(string prompt){
        Console.Write(prompt);
        return Console.ReadLine()??  "";
    }

    public void Run()
    {
        MyArray<string> menu = new MyArray<string>();
        menu.Add("Add task");
        menu.Add("Remove task");
        menu.Add("Toggle task completion");
        menu.Add("Exit");

        int selectedIndex = 0;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
 
            Console.CursorVisible = false;
            DisplayTasks(_service.GetAllTasks());

            System.Console.WriteLine("\n");

            System.Console.WriteLine("Up and down arrey keys and enter for navigation.");  

            for (int i = 0; i < menu.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.WriteLine($"> {menu[i]}");
                    
                }
                else
                {
                    Console.WriteLine($"  {menu[i]}");
                }
            }

            ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
            key = KeyInfo.Key;

            if (key == ConsoleKey.UpArrow)
            {
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = menu.Count -1;
                }
            }
            if (key == ConsoleKey.DownArrow)
            {
                selectedIndex++;
                if (selectedIndex >= menu.Count)
                {
                    selectedIndex = 0;
                }
            }

            if (key == ConsoleKey.Enter)
            {
                switch(selectedIndex)
                {
                    case 0:
                        Console.CursorVisible = true;
                        string? description = Prompt("Enter task description: ");
                        _service.AddTask(description);
                        break;
                    case 1:
                        Console.CursorVisible = true;
                        int idToRemove = int.TryParse(Prompt("Enter task id to remove: "), out int removeId) ? removeId : 0;
                        _service.RemoveTask(idToRemove);
                        break;
                    case 2:
                        Console.CursorVisible = true;
                        string? idToToggle = Prompt("Enter task id to toggle completion: ");
                        if(int.TryParse(idToToggle, out int toggleId)){
                            _service.ToggleTaskCompletion(toggleId);
                        }
                        break;
                    case 3:
                        return;
                }
            }

        }
    }
}
