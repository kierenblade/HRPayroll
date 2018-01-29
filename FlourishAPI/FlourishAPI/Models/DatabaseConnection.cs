using MongoDB.Driver;

namespace FlourishAPI.Models
{
    public class DatabaseConnection
    {
       // static string connectionString = "mongodb://localhost:27017";
        public  DatabaseConnection()
        {
            
        }

        public IMongoDatabase DatabaseConnect(string databaseName = "FlourishDB")
        {
            return new MongoClient().GetDatabase(databaseName);
        }


    }
}
