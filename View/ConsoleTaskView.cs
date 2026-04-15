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

    public void Run() //akif
    {
        Console.Clear();
        IMyCollection<string> menu = new MyArray<string>();
        menu.Add("📋 View Tasks");              //0
        menu.Add("➕ Add task");                //1
        menu.Add("🗑️ Remove task");            //2
        menu.Add("✔ Toggle task completion");  //3
        menu.Add("👤 Add user");               //4
        menu.Add("🔗 Assign task to user");    //5
        menu.Add("📄 View tasks for user");    //6
        menu.Add("👥 View users for task");    //7
        menu.Add("❌ Remove user");            //8
        menu.Add("💾 Save");                  //9
        menu.Add("🚪 Exit");                  //10

        while (true)
        {
            // Console.Clear();
            int menuStartLine = Console.CursorTop + 1;
            int selectedIndex = NavigateMenu(menu, menuStartLine, false);
            switch (selectedIndex)
            {
                case 0: // Basel

                    var tmp = _service.GetAllTasks();
                    var spectre = new SpectreTaskView(_userService, _taskUserService);
                    spectre.DisplayTasks(tmp);
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 1:
                    string? description = Prompt("\nEnter task description: (type :qa to exit)\n>");
                    if (description == ":qa")
                    {
                        System.Console.WriteLine("no task added, press enter to continue");
                        Console.ReadKey();
                        break;
                    }
                    _service.AddTask(description);
                    break;

                case 2:
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

                case 3:
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

                case 4:
                    string? name = Prompt("\nEnter user name: ");
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("\nInvalid name, press enter to continue");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
                    var added = _userService.AddUser(name);
                    if (!added)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("User already exists, press enter to continue");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                    }

                    break;

                case 5: // Basel

                    var tasksAssign = _service.GetAllTasks();
                    if (tasksAssign.Count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("There are no tasks");
                        Console.ReadKey();
                        break;
                    }

                    int idx = NavigateMenu(tasksAssign, 0);
                    if (idx == -1)
                    {
                        Console.Clear();
                        Console.WriteLine("No task selected");
                        Console.ReadKey();
                        break;
                    }

                    TaskItem selectedTaskAssign = GetAtIndex(tasksAssign, idx);

                    var users = _userService.GetAllUsers();
                    if (users.Count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("There are no users");
                        Console.ReadKey();
                        break;
                    }

                    int userIdx = NavigateMenu(users, 0);
                    if (userIdx == -1)
                    {
                        Console.Clear();
                        Console.WriteLine("No user selected");
                        Console.ReadKey();
                        break;
                    }

                    User selectedUser = GetAtIndex(users, userIdx);

                    var assigned = _taskUserService.Assign(selectedTaskAssign.ID, selectedUser.UserID);

                    if (!assigned)
                    {
                        Console.Clear();
                        Console.WriteLine("User already assigned to this task");
                        Console.ReadKey();
                        break;
                    }
                    Console.Clear();
                    Console.WriteLine("User assigned successfully");
                    Console.ReadKey();
                    break;
                case 6:
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


                    var view = new SpectreTaskView(_userService, _taskUserService);
                    var tasksForUser = _taskUserService.GetTasksForUser(selectedUserView.UserID);
                    view.DisplayTasks(tasksForUser);

                    Console.WriteLine($"\nTasks for {selectedUserView}:");
                    Console.WriteLine("\nPress enter to continue");
                    Console.ReadKey();
                    break;
                case 7:
                    var tasksView = _service.GetAllTasks();
                    if (tasksView.Count == 0)
                    {
                        Console.WriteLine("There are no tasks");
                        Console.ReadKey();
                        break;
                    }
                    int taskIdx = NavigateMenu(tasksView, 0);
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
                            Console.Write(user.Name);
                        }
                        Console.WriteLine();
                        Console.ResetColor();
                    }

                    Console.WriteLine("Press enter to continue");
                    Console.ReadKey();
                    break;



                case 8:
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

                case 9:

                    _userService.SaveAll();
                    _service.SaveAll();
                    _taskUserService.SaveAll();
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

// note:
// The view depends only on the collection interface and uses iterators 
// instead of indexing so it works with any collection implementation 
// like arrays, linked lists, or other data structures.