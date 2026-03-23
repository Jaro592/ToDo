using System.Collections.Generic;

public interface ITaskService
{
    IMyCollection<TaskItem> GetAllTasks();
    void AddTask(string? description);
    void RemoveTask(int id);
    void ToggleTaskCompletion(int id);

    // void AddUser(string name);
    // User FindUser(string name);
    // void AssignTaskToUser(int taskId, string userName);

}