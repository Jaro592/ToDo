using System;
using System.Linq.Expressions;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    private void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        Console.WriteLine("=== ToDo List ===\n");

        Console.WriteLine("---In progress---");
        tasks.Reset();
        var iterProgress = tasks.Filter(x => x.Completed == false).GetIterator();
        while (iterProgress.HasNext())
        {
            Console.WriteLine(iterProgress.Next().ToString());
        }

        Console.WriteLine("\n---Completed---");
        tasks.Reset();
        var iterCompleted = tasks.Filter(x => x.Completed == true).GetIterator();
        while (iterCompleted.HasNext())
        {
            Console.WriteLine(iterCompleted.Next().ToString());
        }
    }

    private string? Prompt(string prompt)
    {
        Console.CursorVisible = true;
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    public void Run()
    {
        MyArray<string> menu = new MyArray<string>();
        menu.Add("Add task");
        menu.Add("Remove task");
        menu.Add("Toggle task completion");
        menu.Add("Exit");
    
        while (true)
        {
            DisplayTasks(_service.GetAllTasks());

            int menuStartLine = Console.CursorTop + 1;
            int selectedIndex = NavigateMenu(menu, menuStartLine);

            switch (selectedIndex)
            {
                case 0:
                    string? description = Prompt("\nEnter task description: ");
                    _service.AddTask(description);
                    break;
                case 1:

                    while (true)
                    {
                        NavigateMenu((MyArray<TaskItem>)_service.GetAllTasks(), 0);
                    }
                    string? idToRemove = Prompt("\nEnter task id to remove: ");
                    if (int.TryParse(idToRemove, out int removeId))
                        _service.RemoveTask(removeId);
                    break;
                case 2:
                    string? idToToggle = Prompt("\nEnter task id to toggle completion: ");
                    if (int.TryParse(idToToggle, out int toggleId))
                        _service.ToggleTaskCompletion(toggleId);
                    break;
                case 3:
                    return;
            }
        }
    }

    private int NavigateMenu(MyArray<TaskItem> options, int startLine)
    {
        int selectedIndex = 0;
        ConsoleKey key;

        Console.CursorVisible = false;

        while (true)
        {

            for (int i = 0; i < options.Count; i++)
            {
                Console.SetCursorPosition(0, startLine + i);
                if (i == selectedIndex)
                    Console.Write($"> {options[i]}   ");
                else
                    Console.Write($"  {options[i]}   ");
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if (key == ConsoleKey.UpArrow)
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = options.Count - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selectedIndex++;
                if (selectedIndex >= options.Count) selectedIndex = 0;
            }
            else if (key == ConsoleKey.Enter)
            {
                Console.CursorVisible = true;
                return selectedIndex;
            }
        }
    }
}