using System.Collections.Generic;

public interface ITaskRepository // jaro
{
    IMyCollection<TaskItem> LoadTasks(); // I mycollection instead of list jaro
    void SaveTasks(IMyCollection<TaskItem> tasks); // I mycollection instead of list jaro
    TaskItem? GetById(string taskId); // Jaro
}