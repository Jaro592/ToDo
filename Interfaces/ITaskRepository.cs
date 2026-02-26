using System.Collections.Generic;

interface ITaskRepository 
{
    List<TaskItem> LoadTasks(); // I mycollection instead of list
    void SaveTasks(List<TaskItem> tasks); // I mycollection instead of list
}