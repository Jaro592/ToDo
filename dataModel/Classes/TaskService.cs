using System.Collections.Generic;

class TaskSerivce : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly List<TaskItem> _tasks;

}