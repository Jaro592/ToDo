using Spectre.Console;
public class SpectreTaskView // Basel
{
    private readonly IUserService _userService;
    private readonly ITaskUserService _taskUserService;

    public SpectreTaskView(IUserService userService, ITaskUserService taskUserService)
    {
        _userService = userService;
        _taskUserService = taskUserService;
    }

    public void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        var table = new Table();
        table.AddColumn("[yellow]Description[/]");
        table.AddColumn("[blue]Status[/]");
        table.AddColumn("[green]Users[/]");
        table.AddColumn("[magenta]Priority[/]");

        var iter = tasks.GetIterator();
        iter.Reset();

        while (iter.HasNext())
        {
            var task = iter.Next();

            string status = task.Completed ? "[green]Done[/]" : "[red]In Progress[/]";

            var usersIds = _taskUserService.GetUsersForTask(task.ID);

            var users = _userService.GetUsersByIds(usersIds);

            string userNames = "";

            var userIter = users.GetIterator();
            while (userIter.HasNext())
            {
                var user = userIter.Next();

                if (userNames.Length > 0)
                    userNames += ", ";

                userNames += user.Name;
            }
            if (userNames == "")
                userNames = "[grey]No users[/]";

            table.AddRow(
                $"[white]{task.Description}[/]",
                $"[red]{status}[/]",
                $"[cyan]{userNames}[/]"
            );
        }

        AnsiConsole.Write(table);
    }
}