using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogRestApi.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogRestApi.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private const string databaseName = "CatalogDb";
        private const string collectionName = "items"; 
        private readonly IMongoCollection<Item> itemsCollection; 

        //filterBulder metodo de mongo para conseguir un objeto en una coleccion  
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter; 
        public MongoDbItemsRepository(IMongoClient mongoclient)
        {
            IMongoDatabase database = mongoclient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        public async Task CreateItemAsync(Item item)
        {
            await itemsCollection.InsertOneAsync(item); 
        }

        public async Task DeleteItem(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id); 
            await itemsCollection.DeleteOneAsync(filter); 
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id); 
            return await itemsCollection.Find(filter).SingleOrDefaultAsync(); 
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync(); 
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id); 
            await itemsCollection.ReplaceOneAsync(filter,item); 
        }
    }
}