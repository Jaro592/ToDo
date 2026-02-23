using System.Collections.Generic;

interface ITaskRepository 
{
    List<TaskItem> LoadTasks();
    void SaveTasks(IMyCollection<TaskItem> tasks); // I mycollection instead of list
}