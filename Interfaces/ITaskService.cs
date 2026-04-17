using System.Collections.Generic;

public interface ITaskService
{
    IMyCollection<TaskItem> GetAllTasks();
    void AddTask(string? description, int priority = 1); // akif
    void SetTaskPriority(string id, int priority); //akif
    void RemoveTask(string id);
    bool ToggleTaskCompletion(string id);
    bool AddDependency(string taskId, string dependencyId);
    void SaveAll(); // jaro



    // void AddUser(string name);
    // User FindUser(string name);
    // void AssignTaskToUser(int taskId, string userName);

}