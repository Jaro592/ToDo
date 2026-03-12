using System.Collections.Generic;

public interface ITaskService
{
    IMyCollection<TaskItem> GetAllTasks();
    void AddTask(string? description);
    void RemoveTask(int id);
    void ToggleTaskCompletion(int id);

    
}   