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
        menu.Add("Add task");//0
        menu.Add("Remove task");//1
        menu.Add("Toggle task completion");//2
        menu.Add("Add user");//3
        menu.Add("Assign task to user"); //4
        menu.Add("View tasks for user"); // 5
        menu.Add("Remove user"); //6
        menu.Add("Exit"); // 7

        while (true)
        {
            var tmp = _service.GetAllTasks();
            DisplayTasks(tmp);

            int menuStartLine = Console.CursorTop + 1;
            int selectedIndex = NavigateMenu(menu, menuStartLine, false);

            switch (selectedIndex)
            {
                case 0:
                    string? description = Prompt("\nEnter task description: (type :qa to exit)\n>");
                    if (description == ":qa")
                    {
                        System.Console.WriteLine("no task added, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
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
                    if (idxTasks == -1)
                    {
                        System.Console.WriteLine("\nNo task selected, press any enter to continue");
                        Console.ReadKey();
                        break;
                    }
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
                    if (idxTasksToggle == -1)
                    {
                        System.Console.WriteLine("\nNo task selected, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
                    TaskItem selectedTaskToggle = GetAtIndex(tasksToggle, idxTasksToggle);
                    _service.ToggleTaskCompletion(selectedTaskToggle.ID);
                    break;

                case 3:
                    string? name = Prompt("\nEnter user name: ");
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("\nInvalid name, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
                    _userService.AddUser(name);
                    break;

                case 4:

                    var tasksAssign = _service.GetAllTasks();
                    if (tasksAssign.Count == 0)
                    {
                        Console.WriteLine("\nThere are no tasks");
                        Console.ReadKey();
                        break;
                    }

                    int idx = NavigateMenu(tasksAssign, 0);
                    // TaskItem task = tasksAssign[idx];
                    if (idx == -1)
                    {
                        System.Console.WriteLine("\nNo task selected, press enter to continue");
                        Console.ReadKey();
                        break;
                    }

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
                        Console.WriteLine("\nThere are no users");
                        Console.Clear();
                        break;
                    }

                    int userIdx = NavigateMenu(users, 0);

                    if (userIdx == -1)
                    {
                        System.Console.WriteLine("\nNo user selected, press enter to continue");
                        Console.ReadKey();
                        break;
                    }

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
                        Console.WriteLine("\nSomething went wrong");
                        Console.ReadKey();
                        break;
                    }


                    _taskUserService.Assign(selectedTaskAssign.ID, selectedUser.UserID);

                    break;
                case 5:
                    var usersView = _userService.GetAllUsers(); // Jaro
                    if (usersView.Count == 0)
                    {
                        Console.WriteLine("There are no users");
                        Console.ReadKey();
                        break;
                    }

                    int userIdxView = NavigateMenu(usersView, 0);

                    var iteratorView = usersView.GetIterator();
                    int iView = 0;
                    User selectedUserView = null!;
                    while (iteratorView.HasNext())
                    {
                        var u = iteratorView.Next();
                        if (iView == userIdxView)
                        {
                            selectedUserView = u;
                            break;
                        }
                        iView++;
                    }
                    if (selectedUserView == null)
                    {
                        Console.WriteLine("Something went wrong");
                        Console.ReadKey();
                        break;
                    }

                    var tasksForUser = _taskUserService.GetTasksForUser(selectedUserView.UserID);
                    DisplayTasks(tasksForUser);
                    Console.WriteLine($"\nTasks for {selectedUserView}:");
                    Console.WriteLine("\nPress enter to continue");
                    Console.ReadKey();
                    break;
                case 6:
                    var usersForDeletion = _userService.GetAllUsers();
                    if (usersForDeletion.Count == 0)
                    {
                        System.Console.WriteLine("\nThere are no users to delete, press enter to continue.");
                        Console.ReadKey();
                        break;
                    }
                    int selectedIndexDeletion = NavigateMenu(usersForDeletion, 0);
                    if (selectedIndexDeletion == -1)
                    {
                        System.Console.WriteLine("\nNo user selected, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
                    var userDeletion = GetAtIndex(usersForDeletion, selectedIndexDeletion);
                    _userService.DeleteUser(userDeletion.Name);                        

                    System.Console.WriteLine("\nNothing to do.");


                    break;
                case 7:
                    _userService.SaveAll();
                    _service.SaveAll();
                    _taskUserService.SaveAll();
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

    private int NavigateMenu<T>(IMyCollection<T> options, int startLine, bool clear = true, int startingIndex = 0) //akif
    {
        int selectedIndex = startingIndex;
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
            else if (key == ConsoleKey.Escape)
            {
                return -1;
            }
        }
    }
}

// note:
// The view depends only on the collection interface and uses iterators 
// instead of indexing so it works with any collection implementation 
// like arrays, linked lists, or other data structures.