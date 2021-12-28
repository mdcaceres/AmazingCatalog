namespace CatalogRestApi.Settings
{
    public class MongoDBSettings 
    {
        public string Host {get; set;}
        public int Port {get; set;}
        public string ConnectionString
        {
            get
            {  
                return $"mongodb://{Host}:{Port}";
            }
        }
    }
}