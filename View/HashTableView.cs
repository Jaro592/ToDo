public class HashTableView : Serialize
{
    private HashTable<string, TaskItem> _table;

    public HashTableView(HashTable<string, TaskItem> table)
    {
        _table = table;
    }

    public void Fill()
    {
        for (int i = 1; i <= 5; i++)
        {
            var task = new TaskItem
            {
                ID = NewSerializeString(),
                Description = $"Task {i}",
                Completed = i % 2 == 0
            };

            _table.Add(task.ID, task);
        }
    }

    public void Display()
    {
        Console.WriteLine("Tasks in HashTable:");

        var it = _table.GetIterator();

        while (it.HasNext())
        {
            var entry = it.Next();
            Console.WriteLine($"{entry.Key} -> {entry.Value}");
        }
    }

    public void TestBehavior()
    {
        Console.WriteLine("\n=== Behavior Test ===");


        var it = _table.GetIterator();
        if (!it.HasNext()) return;

        var first = it.Next();


        Console.WriteLine("\nGet test:");
        var found = _table.Get(first.Key);
        Console.WriteLine(found != null ? "Found ✔" : "Not Found ❌");


        Console.WriteLine("\nContains test:");
        Console.WriteLine(_table.ContainsKey(first.Key));

        Console.WriteLine("\nRemove test:");
        _table.Remove(first.Key);
        Console.WriteLine(_table.ContainsKey(first.Key));
        Console.WriteLine("\nResize test:");

        for (int i = 0; i < 50; i++)
        {
            var t = new TaskItem
            {
                ID = NewSerializeString(),
                Description = $"Bulk {i}",
                Completed = false
            };

            _table.Add(t.ID, t);
        }

        bool ok = true;

        var it2 = _table.GetIterator();
        while (it2.HasNext())
        {
            var e = it2.Next();
            if (_table.Get(e.Key) == null)
            {
                ok = false;
                break;
            }
        }

        Console.WriteLine(ok ? "Resize OK ✔" : "Resize BROKEN ❌");
    }
}