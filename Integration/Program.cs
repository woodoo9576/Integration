using Integration.Service;

namespace Integration;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var service = new ItemIntegrationService();
        
        ThreadPool.QueueUserWorkItem(_ => service.SaveItemAsync("a"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItemAsync("b"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItemAsync("c"));

        Thread.Sleep(500);

        ThreadPool.QueueUserWorkItem(_ => service.SaveItemAsync("a"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItemAsync("b"));
        ThreadPool.QueueUserWorkItem(_ => service.SaveItemAsync("c"));

        Thread.Sleep(5000);

        Console.WriteLine("Everything recorded:");


        service.GetAllItemsAsync().Result.ForEach(Console.WriteLine);
        //I think Output is:
        /*
         * 3:a
         * 1:b
         * 2:c
         */


        Console.ReadLine();
    }
}