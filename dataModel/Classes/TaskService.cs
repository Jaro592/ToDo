using System.Collections.Generic;

class TaskSerivce : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly List<TaskItem> _tasks;

    public TaskSerivce(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
    }
    public IEnumerable<TaskItem> GetAllTasks() => _tasks;

    public void AddTask(string description)
    {
        int newId = _tasks.Count > 0 ? _tasks[_tasks.Count - 1].ID + 1 : 1;
        var newTask = new TaskItem { ID = newId, Description = description, Completed = false};
        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);

    }
    public void RemoveTask(int id)
    {
        var task = _tasks.Find(t => t.ID == id);
        if (task != null){
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
        }
    }
    public void ToggleTaskCompletion(int id)
    {
        var task = _tasks.Find(t => t.ID == id);
        if (task != null)
        {
            task.Completed = !task.Completed;
            _repository.SaveTasks(_tasks);
        }
    }

}