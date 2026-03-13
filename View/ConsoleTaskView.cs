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

        tasks.Sort((a, b) => a.ID.CompareTo(b.ID));
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
            var tmp = _service.GetAllTasks();
            DisplayTasks(tmp);

            int menuStartLine = Console.CursorTop + 1;
            int selectedIndex = NavigateMenu(menu, menuStartLine, false);

            switch (selectedIndex)
            {
                case 0:
                    string? description = Prompt("\nEnter task description: ");
                    _service.AddTask(description);
                    break;
                case 1:
                    MyArray<TaskItem> tasks = (MyArray<TaskItem>)_service.GetAllTasks();
                    if (tasks.Count == 0)
                    {
                        System.Console.WriteLine("\nThere are no tasks to delete, press enter to continue");
                        System.Console.ReadKey();
                        break;
                    }
                    int idxTasks = NavigateMenu(tasks, 0);
                    TaskItem selectedTask = tasks[idxTasks];
                    _service.RemoveTask(selectedTask.ID);
                    break;
                case 2:
                    MyArray<TaskItem> tasksToggle = (MyArray<TaskItem>)_service.GetAllTasks();
                    if (tasksToggle.Count == 0)
                    {
                        System.Console.WriteLine("\nThere are no tasks to toggle, press enter to continue");
                        System.Console.ReadKey();
                        break;
                    }
                    int idxTasksToggle = NavigateMenu(tasksToggle, 0);
                    TaskItem selectedTaskToggle = tasksToggle[idxTasksToggle];
                    _service.ToggleTaskCompletion(selectedTaskToggle.ID);
                    break;
                case 3:
                    return;
            }
        }
    }

    private int NavigateMenu<T>(MyArray<T> options, int startLine, bool clear = true) where T : IEquatable<T>
    {

        
        int selectedIndex = 0;
        ConsoleKey key;

        if (clear)
        {
            Console.Clear();
        }

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
                if (selectedIndex < 0)
                    selectedIndex = options.Count - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selectedIndex++;
                if (selectedIndex >= options.Count)
                    selectedIndex = 0;
            }
            else if (key == ConsoleKey.Enter)
            {
                Console.CursorVisible = true;
                return selectedIndex;
            }
        }
    }
}