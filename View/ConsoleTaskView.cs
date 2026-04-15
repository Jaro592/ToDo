using System;
using System.Linq.Expressions;
using Spectre.Console;

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

    private string? Prompt(string prompt) //akif
    {
        Console.CursorVisible = true;
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    private void Pause(string message = "\nPress enter to continue") // akif
    {
        Console.WriteLine(message); // akif
        Console.ReadKey(); // akif
    }

    public void Run() //akif
    {
        Console.Clear(); // akif
        IMyCollection<string> menu = new MyArray<string>();

        // menu.Add("📋 View Tasks");              //0
        menu.Add("➕ Add task");                //0
        menu.Add("🗑️ Remove task");            //1
        menu.Add("✔ Toggle task completion");  //2
        menu.Add("👤 Add user");               //3
        menu.Add("🔗 Assign task to user");    //4
        menu.Add("📄 View tasks for user");    //5
        menu.Add("👥 View users for task");    //6
        menu.Add("❌ Remove user");            //7
        menu.Add("🔗 Add dependency");         //8
        menu.Add("💾 Save");                  //9
        menu.Add("🚪 Exit");                  //10



        while (true)
        {
            Console.Clear(); // akif

            var currentTasks = _service.GetAllTasks(); // akif
            var spectre = new SpectreTaskView(_userService, _taskUserService); // akif
            spectre.DisplayTasks(currentTasks); // akif

            Console.WriteLine(); // spacing

            int selectedIndex = NavigateMenu(menu, Console.CursorTop, false); // akif

            switch (selectedIndex)
            {
                case 0:
                    Console.Clear(); // akif
                    string? description = Prompt("\nEnter task description: (type :qa to exit)\n>");
                    if (description == ":qa")
                    {
                        Pause("no task added"); // akif
                        break;
                    }
                    _service.AddTask(description); // akif
                    Pause("task added"); // akif
                    break;

                case 1:
                    Console.Clear(); // akif
                    IMyCollection<TaskItem> taskListToRemove = _service.GetAllTasks(); // akif
                    if (taskListToRemove.Count == 0)
                    {
                        Pause("\nThere are no tasks to delete"); // akif
                        break;
                    }
                    int idxTasks = NavigateMenu(taskListToRemove, 0); // akif
                    if (idxTasks == -1)
                    {
                        Pause("\nNo task selected"); // akif
                        break;
                    }
                    TaskItem selectedTask = GetAtIndex(taskListToRemove, idxTasks); // akif
                    _service.RemoveTask(selectedTask.ID);
                    Pause("task removed"); // akif
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
                    var success = _service.ToggleTaskCompletion(selectedTaskToggle.ID);

                    if (!success)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nCannot complete task, dependencies not finished");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;

                case 3:
                    string? name = Prompt("\nEnter user name: ");
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Pause("\nInvalid name");
                        break;
                    }
                    var added = _userService.AddUser(name);
                    if (!added)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("User already exists");
                        Console.ResetColor();
                        Pause();
                    }
                    else
                    {
                        Pause("user added"); // akif
                    }
                    Console.WriteLine("User Added successfully");
                    Console.ReadKey();
                    Console.Clear();
                    break;

                case 4: // Basel
                    Console.Clear();
                    var tasksAssign = _service.GetAllTasks();
                    if (tasksAssign.Count == 0)
                    {
                        Pause("There are no tasks");
                        break;
                    }

                    int idx = NavigateMenu(tasksAssign, 0);
                    if (idx == -1)
                    {
                        Pause("No task selected");
                        break;
                    }

                    TaskItem selectedTaskAssign = GetAtIndex(tasksAssign, idx);

                    var users = _userService.GetAllUsers();
                    if (users.Count == 0)
                    {
                        Pause("There are no users");
                        break;
                    }

                    int userIdx = NavigateMenu(users, 0);
                    if (userIdx == -1)
                    {
                        Pause("No user selected");
                        break;
                    }

                    User selectedUser = GetAtIndex(users, userIdx);

                    var assigned = _taskUserService.Assign(selectedTaskAssign.ID, selectedUser.UserID);

                    if (!assigned)
                    {
                        Pause("User already assigned to this task");
                        break;
                    }

                    Pause("User assigned successfully"); // akif
                    break;

                case 5:
                    Console.Clear(); // akif
                    var usersView = _userService.GetAllUsers(); // Jaro
                    if (usersView.Count == 0)
                    {
                        Pause("There are no users");
                        break;
                    }

                    int userIdxView = NavigateMenu(usersView, 0);
                    if (userIdxView == -1)
                    {
                        Pause("No user selected");
                        break;
                    }

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

                    var view = new SpectreTaskView(_userService, _taskUserService);
                    var tasksForUser = _taskUserService.GetTasksForUser(selectedUserView.UserID);
                    view.DisplayTasks(tasksForUser);

                    Console.WriteLine($"\nTasks for {selectedUserView.Name}"); // akif
                    Pause(); // akif
                    break;

                case 6:
                    Console.Clear();
                    var tasksView = _service.GetAllTasks();
                    if (tasksView.Count == 0)
                    {
                        Pause("There are no tasks");
                        break;
                    }
                    int taskIdx = NavigateMenu(tasksView, 0);
                    if (taskIdx == -1)
                    {
                        Pause("No task selected");
                        break;
                    }
                    TaskItem selectedTaskView = GetAtIndex(tasksView, taskIdx);
                    Console.Clear();

                    var userIds = _taskUserService.GetUsersForTask(selectedTaskView.ID); //  selectedTaskView.ID = taskid 
                    var usersForTask = _userService.GetUsersByIds(userIds);
                    if (usersForTask.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No users assigned to this task.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"Users for this task: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;

                        var iteratorUsers = usersForTask.GetIterator();

                        while (iteratorUsers.HasNext())
                        {
                            var user = iteratorUsers.Next();
                            Console.Write(user.Name + " "); // akif
                        }
                        Console.WriteLine();
                        Console.ResetColor();
                    }

                    Pause(); // akif
                    break;

                case 7:
                    Console.Clear(); // akif
                    var usersForDeletion = _userService.GetAllUsers();
                    if (usersForDeletion.Count == 0)
                    {
                        Pause("\nThere are no users to delete."); // akif
                        break;
                    }
                    int selectedIndexDeletion = NavigateMenu(usersForDeletion, 0);
                    if (selectedIndexDeletion == -1)
                    {
                        Pause("\nNo user selected"); // akif
                        break;
                    }
                    var userDeletion = GetAtIndex(usersForDeletion, selectedIndexDeletion);
                    _userService.DeleteUser(userDeletion.Name);

                    Pause("User removed"); // akif
                    break;


                case 8:
                    var tasksDep = _service.GetAllTasks();

                    if (tasksDep.Count == 0)
                    {
                        Console.WriteLine("No tasks available");
                        Console.ReadKey();
                        break;
                    }
                    Console.Clear();
                    Console.WriteLine("\nSelect task (the one you want to complete): ");

                    int dtaskIdx = NavigateMenu(tasksDep, Console.CursorTop, false);
                    if (dtaskIdx == -1) break;

                    TaskItem task = GetAtIndex(tasksDep, dtaskIdx);

                    Console.Clear();
                    Console.WriteLine("\nSelect dependency (must be completed first): ");


                    int depIdx = NavigateMenu(tasksDep, Console.CursorTop, false);
                    if (depIdx == -1) break;

                    TaskItem dependency = GetAtIndex(tasksDep, depIdx);

                    var Depadded = _service.AddDependency(task.ID, dependency.ID);

                    if (!Depadded)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nFailed to add dependency");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nDependency added successfully");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    Console.ResetColor();
                    break;

                case 9:

                    _userService.SaveAll();
                    _service.SaveAll();
                    _taskUserService.SaveAll();
                    Pause("Saved"); // akif
                    break;

                case 10:

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
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"▶ ");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{item}    ");
                    Console.ResetColor();
                }
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