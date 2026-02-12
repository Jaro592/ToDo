using System.Collections.Generic

interface ITaskRepository {
    List<TaskItem> LoadTasks();

}