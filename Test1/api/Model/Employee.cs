using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test1.Api.Model
{
    public class Employee
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [BsonDateTimeOptions]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
