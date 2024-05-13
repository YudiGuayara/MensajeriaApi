using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AlertMessageApi.Models
{
    public class AlertMessage
    {
        [BsonId]  
        [BsonElement("_id")]
        public string? Id { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Level")]
        public AlertLevel Level { get; set; }

        [BsonElement("Message")]
        public string? Message { get; set; }

        [BsonElement("UserId")]
        public string? UserId { get; set; }

        [BsonElement("EventId")]
        public string? EventId { get; set; }
    }

    public enum AlertLevel
    {
        Verde,
        Amarillo,
        Naranja,
        Rojo
    }
}
