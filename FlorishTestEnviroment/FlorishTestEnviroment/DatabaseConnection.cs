using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FlorishTestEnviroment
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
