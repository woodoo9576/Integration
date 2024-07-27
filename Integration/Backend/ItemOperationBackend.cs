using System.Collections.Concurrent;
using Integration.Common;
using System.Linq;

namespace Integration.Backend;

public sealed class ItemOperationBackend
{
    private int _identitySequence;
    //remove setter
    private ConcurrentBag<Item> SavedItems { get; } = new();

    public async Task<Item> SaveItemAsync(string itemContent)
    {
        // This simulates how long it takes to save
        // the item content. Forty seconds, give or take.
        Thread.Sleep(2_000);

        var item = Task.Run(() => new Item
        {
            Content = itemContent,
            Id = GetNextIdentityAsync().Result
        });

        /*
        var item = new Item
        {
            Content = itemContent,
            Id = GetNextIdentityAsync()
        };
        */

        if (FindItemsWithContentAsync(itemContent).Result.Count == 0)
        {
            
            //await item; //redundant
            SavedItems.Add(item.Result);
        }
        //if (FindItemsWithContentAsync(itemContent).Count == 0) SavedItems.Add(item);

        return await item;
    }

    public async Task<List<Item>> FindItemsWithContentAsync(string itemContent)
    {
        // for await keyword
        //var r = await Task.FromResult(SavedItems.Where(x => x.Content == itemContent).ToList());
        // Entity Framework Core
        //var r = await SavedItems.Where(x => x.Content == itemContent).TolistAsync();
        //return r;
        return await Task.FromResult(SavedItems.Where( x => x.Content == itemContent).ToList());
    }

    private async Task<int> GetNextIdentityAsync()
    {

        return 
           await Task.Run(()=> Interlocked.Increment(ref _identitySequence));
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await Task.FromResult(SavedItems.ToList());
    }
}