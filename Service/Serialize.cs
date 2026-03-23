public class Serialize // jaro
{
    private static readonly Random random = new();
    protected string NewSerializeString()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        int randomNumber = random.Next(1000, 9999);
        
        int anotherOne = random.Next(1000, 9999);
        return $"{timestamp}_{randomNumber}_{anotherOne}";

    }
    // this is orderable with the following method (but then ofc not using list)

    
    // public List<ToDoTask> GetSortedTasks()
    // {
    //     // Omdat de string begint met de datum, zet OrderBy ze chronologisch
    //     return _tasks.OrderBy(t => t.ID).ToList();
    // }
    
}