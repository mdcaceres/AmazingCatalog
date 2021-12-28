using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogRestApi.Entities;

public interface IItemsRepository
{
    //Los metodos deben retornar TASK. eso es porque es la forma de señalar que esos metodos no van a seguir siendo metodos sincronos.
    // si no, que van a hacer metodos Asincronos. 
    Task<Item> GetItemAsync(Guid id);
    Task<IEnumerable<Item>> GetItemsAsync();
    Task CreateItemAsync(Item item); 

    Task UpdateItemAsync(Item item);
    
    Task DeleteItem(Guid id);
}