
using System.Collections.Generic;
using EventsApi.Models;
using MongoDB.Driver;

namespace EventsApi.Services{
    public class UserService{
        private readonly IMongoCollection<User> _userCollection;
         public UserService(IEventsDatabaseSettings settings,IMongoClient mongoClient)
        {
            // var client = new MongoClient(settings.ConnectionString);
            var client=mongoClient;
            var database = client.GetDatabase(settings.DatabaseName);
            _userCollection = database.GetCollection<User>(settings.UsersCollectionName);
        }
        
        public List<User> GetAllUsers(){
            return _userCollection.Find(user=>true).ToList();
        }
        public User GetUserById(double id){
            return _userCollection.Find(user=>user.UserId==id).FirstOrDefault();
        }
        public User CreateUser(User user)
        {
            _userCollection.InsertOne(user);
            return user;
        }

        public User UpdateUser(double id,User updatedUser){
            _userCollection.ReplaceOne(user => user.UserId == id, updatedUser);
            return updatedUser;
        } 
    }
}