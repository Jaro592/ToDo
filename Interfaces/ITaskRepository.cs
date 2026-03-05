using System.Collections.Generic;

interface ITaskRepository
{
    MyArray<TaskItem> LoadTasks(); // I mycollection instead of list
    void SaveTasks(MyArray<TaskItem> tasks); // I mycollection instead of list
}