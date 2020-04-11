using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using EventsApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventsApi.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapperController : ControllerBase
    {
        private IMongoCollection<User> _userCollection;
        public ScrapperController(IEventsDatabaseSettings settings,IMongoClient mongoClient){
            var client=mongoClient;
            var database = client.GetDatabase(settings.DatabaseName);
            _userCollection = database.GetCollection<User>(settings.UsersCollectionName);
        }

        [HttpGet]
        public void CsvToMongoDb(){
            var reader = new StreamReader("Event Registration.csv");
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Configuration.HeaderValidated=null;
            csv.Configuration.MissingFieldFound=null;
            csv.Configuration.PrepareHeaderForMatch=(string header,int index)=>{
                switch(header){
                    case "UserName":
                        return "Name";
                    case "UserDesignation":
                        return "Designation";
                    case "E-Mail":
                        return "Email";
                }
                return header;
            };
            var records = csv.GetRecords<User>();
            foreach(var record in records){
                Console.WriteLine(record.Name+" "+record.Designation);
                var lastUser = _userCollection
                    .Find(new BsonDocument())
                    .Limit(1)
                    .SortByDescending(rec=>rec.UserId)
                    .FirstOrDefault();
                if(lastUser==null){
                    record.UserId=1;
                }
                else{
                    record.UserId=lastUser.UserId+1;
                }
                _userCollection.InsertOne(record);
            }
        }
    }
}