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

    
}