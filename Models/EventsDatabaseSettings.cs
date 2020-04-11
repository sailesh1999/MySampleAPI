namespace EventsApi.Models
{
    public class EventsDatabaseSettings : IEventsDatabaseSettings
    {
        public string VolunteersCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IEventsDatabaseSettings
    {
        string VolunteersCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}