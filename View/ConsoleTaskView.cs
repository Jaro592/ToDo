using System;
using System.Linq.Expressions;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    private readonly IUserService _userService;
    private readonly ITaskUserService _taskUserService;


    public ConsoleTaskView(ITaskService service, IUserService userService, ITaskUserService taskUserService)
    {
        _service = service;
        _userService = userService;
        _taskUserService = taskUserService;
    }

    private void DisplayTasks(IMyCollection<TaskItem> tasks) //akif
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

    private string? Prompt(string prompt) //akif
    {
        Console.CursorVisible = true;
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    public void Run() //akif
    {
        IMyCollection<string> menu = new MyArray<string>();
        menu.Add("Add task");
        menu.Add("Remove task");
        menu.Add("Toggle task completion");
        menu.Add("Add user");
        menu.Add("Assign task to user");
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
                    IMyCollection<TaskItem> tasks = _service.GetAllTasks();
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("\nThere are no tasks to delete, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
                    int idxTasks = NavigateMenu(tasks, 0);
                    TaskItem selectedTask = GetAtIndex(tasks, idxTasks);
                    _service.RemoveTask(selectedTask.ID);
                    break;

                case 2:
                    IMyCollection<TaskItem> tasksToggle = _service.GetAllTasks();
                    if (tasksToggle.Count == 0)
                    {
                        Console.WriteLine("\nThere are no tasks to toggle, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
                    int idxTasksToggle = NavigateMenu(tasksToggle, 0);
                    TaskItem selectedTaskToggle = GetAtIndex(tasksToggle, idxTasksToggle);
                    _service.ToggleTaskCompletion(selectedTaskToggle.ID);
                    break;

                case 3:
                    string? name = Prompt("\nEnter user name: ");
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Invalid name, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
                    _userService.AddUser(name);
                    break;

                case 4:

                    var tasksAssign = _service.GetAllTasks();
                    if (tasksAssign.Count == 0)
                    {
                        Console.WriteLine("There are no tasks");
                        Console.ReadKey();
                        break;
                    }

                    int idx = NavigateMenu(tasksAssign, 0);
                    // TaskItem task = tasksAssign[idx];

                    var taskIterator = tasksAssign.GetIterator();
                    int j = 0;
                    TaskItem selectedTaskAssign = null!;
                    while (taskIterator.HasNext())
                    {
                        var task = taskIterator.Next();
                        if (j == idx)
                        {
                            selectedTaskAssign = task;
                            break;
                        }
                        j++;
                    }

                    var users = _userService.GetAllUsers();
                    if (users.Count == 0)
                    {
                        Console.WriteLine("There aro no users");
                        Console.Clear();
                        break;
                    }

                    int userIdx = NavigateMenu(users, 0);

                    var iterator = users.GetIterator();
                    int i = 0;
                    User selectedUser = null!;
                    while (iterator.HasNext())
                    {
                        var u = iterator.Next();
                        if (i == userIdx)
                        {
                            selectedUser = u;
                            break;
                        }
                        i++;
                    }
                    if (selectedTaskAssign == null || selectedUser == null)
                    {
                        Console.WriteLine("Something went wrong");
                        Console.ReadKey();
                        break;
                    }


                    _taskUserService.Assign(selectedTaskAssign.ID, selectedUser.UserID);

                    break;

                case 5:
                    return;
            }
        }
    }

    private T GetAtIndex<T>(IMyCollection<T> collection, int index) //akif
    {
        var it = collection.GetIterator();
        int i = 0;

        while (it.HasNext())
        {
            var item = it.Next();
            if (i == index)
                return item;
            i++;
        }

        throw new Exception("Index out of range");
    }

    private int NavigateMenu<T>(IMyCollection<T> options, int startLine, bool clear = true) //akif
    {
        int selectedIndex = 0;
        ConsoleKey key;

        if (clear)
            Console.Clear();

        Console.CursorVisible = false;

        while (true)
        {
            var iterator = options.GetIterator();
            int i = 0;

            while (iterator.HasNext())
            {
                var item = iterator.Next();

                Console.SetCursorPosition(0, startLine + i);

                if (i == selectedIndex)
                    Console.Write($"> {item}   ");
                else
                    Console.Write($"  {item}   ");

                i++;
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

// note:
// The view depends only on the collection interface and uses iterators 
// instead of indexing so it works with any collection implementation 
// like arrays, linked lists, or other data structures.