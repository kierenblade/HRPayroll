using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using HRPayroll.Classes.Models;
using HRPayroll.Audit;
using MongoDB.Bson;
using System.Reflection;
using System.Linq;
using System.Text;

namespace FlorishTestEnviroment
{
    public static class CRUDExtensions
    {
        //Please note this class uses lazy instantiation. If more clearification is needed, send me a discord message and
        //ill change the code accordling to make it abit more readable.

        /// <summary>
        /// Will insert the calling object into the database into the relevant 
        /// </summary>
        public static bool InsertDocument(this CRUDAble obj, string databaseName = "FlourishDB")
        {
                //the following method will do the following tasks in order:

                // 1. establishes a connection to the database with the databasename specified, if none specified then it will use its
                // predefined database name of 'FlourishDB'.
                //2. Once established, it will obtain the collection that is of the same type of the object that initiated the method.
                //3. Once collection is obtained, it then inserts the object that initiated the method into the database in the correct Collection and returns a True bool.
                // ~ if the object already exists in the document then it will return false to the system that called the method.

                IMongoCollection<CRUDAble> collection = new DatabaseConnection().DatabaseConnect(databaseName).GetCollection<CRUDAble>(obj.GetType().Name); //  ~ Consists of Point 1
                if(collection.AsQueryable().Where(p=> p.Id == obj.Id).ToList().Count > 0) //  used to check if the current object already exists
                {
                    return false; //  returns a false statement if it does
                }
            
                    collection.InsertOne((CRUDAble)obj); // this will insert the object
                    return true; // everything completed successfully
        }


        /// <summary>
        /// Will update the calling object in the database with its new data
        /// </summary>
        public static void UpdateManyDocument(this List<CRUDAble> changedObjects, string databaseName = "FlourishDB")
        {

            List<CRUDAble> updatedQuery = new List<CRUDAble>(); // holds a collection of objects that will be updated
            foreach (CRUDAble item in changedObjects) // will check each item in the list of items that have changed
            {
                var selectedCollection = new DatabaseConnection().DatabaseConnect(databaseName).GetCollection<CRUDAble>(item.GetType().Name); // obtains the collection that is linked to the object that is currently being viewed
                List<CRUDAble> query = selectedCollection.AsQueryable().Where(sb => sb.Id == item.Id).ToList(); // queries the collection to make sure the document already exists
                if (query.Count > 0)
                {
                    List<ChangeLog> changes = new List<ChangeLog>(); // holds a list of changes that were made for that specific document
                    foreach (var prop in item.GetType().GetProperties()) // this will iterate through the objects properties to check what has changed
                    {

                        if (!prop.PropertyType.Assembly.GetName().Name.Contains("System")) // if the property type is user defined and not System defined we do additional checks for the sub objects
                        {
                            var chosenObject = prop.GetValue(item); // gets the new object within the item
                            var existingObject = prop.GetValue(query[0]); // gets the old object within the item
                            if (chosenObject != null) //  checks if the object was orginally null
                            {
                                foreach (var otherProp in chosenObject.GetType().GetProperties()) // this will iterate through the objects properties to check what has changed
                                {
                                    var afterChangeSub = otherProp.GetValue(chosenObject); // gets the new value for that specific property
                                    var beforeChangeSub = otherProp.GetValue(existingObject); //  gets the old value for that specific property
                                    if (beforeChangeSub != null && afterChangeSub != null) //  if they not null, we will continue
                                    {
                                        if (beforeChangeSub.ToString() != afterChangeSub.ToString()) // if they not the same, we add to the ChnageLog which will be saved into a Audit Database
                                        {
                                            changes.Add(new ChangeLog() { Column = prop.Name + " - " + otherProp.Name, From = otherProp.GetValue(existingObject).ToString(), To = otherProp.GetValue(chosenObject).ToString() });
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            // this does the same as the above, with the only difference that its a System defined
                            var afterChangeMain = prop.GetValue(item);
                            var beforeChangeMain = prop.GetValue(query[0]);
                            if (beforeChangeMain != null && afterChangeMain != null)
                            {
                                if (beforeChangeMain.ToString() != afterChangeMain.ToString())
                                {
                                    changes.Add(new ChangeLog() { Column = prop.Name, From = prop.GetValue(query[0]).ToString(), To = prop.GetValue(item).ToString() });
                                }
                            }
                        }



                    }
                    string audIdentifier = item.GetType().Name; // gets the name of the class that the item is associated with
                    switch (audIdentifier) // this will create a audit document within the relevant collection based on the document type
                    {
                        case "Employee":
                            new EmployeeAud() { EmployeeId = 1, FirstName = "change later", LastName = "change later", ChangeBy = "System", ChangeTime = DateTime.Now, ChangeLog = changes }.InsertDocument("FlourishAUD_DB");
                            break;
                        case "LoginDetails":
                            new LoginDetailsAud() { Username = "change later", Hash = "change later", ChangeBy = "System", ChangeTime = DateTime.Now, ChangeLog = changes }.InsertDocument("FlourishAUD_DB");
                            break;
                            //case "ContactDetails":
                            //    new ContactDetailsAud() { EmployeeId = 1, FirstName = "change later", LastName = "change later", ChangeBy = "System", ChangeTime = DateTime.Now, ChangeLog = changes }.InsertDocument("FlourishAUD_DB");
                            //    break;
                            //case "Company":
                            //    new CompanyAud() { EmployeeId = 1, FirstName = "change later", LastName = "change later", ChangeBy = "System", ChangeTime = DateTime.Now, ChangeLog = changes }.InsertDocument("FlourishAUD_DB");
                            //    break;


                    }

                    updatedQuery.Add(item); // adds the item to the list that will be iterated through to update
                }
            }
            foreach (CRUDAble item in updatedQuery) //  iterates through the collection updated each document one by one
            {
                var filter = Builders<CRUDAble>.Filter.Eq("Id", item.Id); // filters out the document by its unique _Id
                var selectedCollection = new DatabaseConnection().DatabaseConnect().GetCollection<CRUDAble>(item.GetType().Name); // gets the collection associated with the object
                var result = selectedCollection.ReplaceOneAsync(filter, item);// replaces the old document with the new document

            }



        }

        /// <summary>
        /// This will search for a collection of objects based on a user specified list of criteria (In- Progress)
        /// </summary>

        public static void SearchDocument(this CRUDAble searchObject, Dictionary<string,string> queryDictionary, string databaseName = "FlourishDB")
        {
            var selectedCollection = new DatabaseConnection().DatabaseConnect(databaseName).GetCollection<CRUDAble>(searchObject.GetType().Name);
            StringBuilder queryBuilder = new StringBuilder();
            var query = selectedCollection.AsQueryable();
            //Non-meta properties
            // query = query.Where(a => a.SomeNonMetaProperty == "Something");
            //And now meta properties
            var q = (from key in queryDictionary.Keys
                     from value in queryDictionary.Values
                     select selectedCollection).ToList();

            List<CRUDAble> quewry = selectedCollection.AsQueryable().ToList();


            //foreach (var keyAndValue in queryDictionary)
            //{
            //    query = query.Where(m =>
            //      m.Name == keyAndValue.Key
            //      && m.Value == keyAndValue.Value);
            //}
            
            //List<CRUDAble> query = selectedCollection.AsQueryable().Where(sb => sb.Id == searchObject.Id).Where(sb => queryBuilder) .ToList();
        }
    }
}
