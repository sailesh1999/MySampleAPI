using EventsApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace EventsApi.Services
{
    public class VolunteerService
    {
        private readonly IMongoCollection<Volunteer> _volunteers;

        public VolunteerService(IEventsDatabaseSettings settings,IMongoClient mongoClient)
        {
            // var client = new MongoClient(settings.ConnectionString);
            var client=mongoClient;
            var database = client.GetDatabase(settings.DatabaseName);

            _volunteers = database.GetCollection<Volunteer>(settings.VolunteersCollectionName);
        }

        public List<Volunteer> Get() =>
            _volunteers.Find(volunteer => true).ToList();

        public Volunteer GetVolunteerByIdAndPassword(string id,string password) =>
            _volunteers.Find<Volunteer>(volunteer => volunteer.EmployeeId == id && volunteer.Password==password).FirstOrDefault();

        public Volunteer Create(Volunteer volunteer)
        {
            _volunteers.InsertOne(volunteer);
            return volunteer;
        }

        public void Update(string id, Volunteer volunteerIn) =>
            _volunteers.ReplaceOne(volunteer => volunteer.Id == id, volunteerIn);

        public void Remove(Volunteer volunteerIn) =>
            _volunteers.DeleteOne(volunteer => volunteer.Id == volunteerIn.Id);

        public void Remove(string id) => 
            _volunteers.DeleteOne(volunteer => volunteer.Id == id);
    }
}