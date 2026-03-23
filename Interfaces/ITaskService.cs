using System.Collections.Generic;

public interface ITaskService
{
    IMyCollection<TaskItem> GetAllTasks();
    void AddTask(string? description);
    void RemoveTask(string id);
    void ToggleTaskCompletion(string id);

    // void AddUser(string name);
    // User FindUser(string name);
    // void AssignTaskToUser(int taskId, string userName);

}