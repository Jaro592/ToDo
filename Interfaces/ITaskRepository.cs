using System.Collections.Generic;

interface ITaskRepository
{
    IMyCollection<TaskItem> LoadTasks(); // I mycollection instead of list
    void SaveTasks(IMyCollection<TaskItem> tasks); // I mycollection instead of list
}