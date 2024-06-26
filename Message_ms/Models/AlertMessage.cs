using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AlertMessageApi.Models
{
    [DataContract]
    public class AlertMessage
    {
        public AlertMessage() {}

        public AlertMessage(string id, DateTime date, AlertLevel level, string message, string userId, string eventId)
        {
            Id = id;
            Date = date;
            Level = level;
            Message = message;
            UserId = userId;
            EventId = eventId;
        }

        [Key]
        [DataMember(Name = "id", EmitDefaultValue = false, IsRequired = false)]
        public string Id { get; set; }

        [DataMember(Name = "date", EmitDefaultValue = false, IsRequired = false)]
        public DateTime Date { get; set; }

        [DataMember(Name = "level", EmitDefaultValue = false, IsRequired = false)]
        public AlertLevel Level { get; set; }

        [DataMember(Name = "message", EmitDefaultValue = false, IsRequired = false)]
        [Required(ErrorMessage = "The Message field is required.")]
        public string Message { get; set; }

        [DataMember(Name = "userId", EmitDefaultValue = false, IsRequired = false)]
        public string UserId { get; set; }

        [DataMember(Name = "eventId", EmitDefaultValue = false, IsRequired = false)]
        public string EventId { get; set; }
    }

    public enum AlertLevel
    {
        [EnumMember(Value = "Verde")]
        Verde,

        [EnumMember(Value = "Amarillo")]
        Amarillo,

        [EnumMember(Value = "Naranja")]
        Naranja,

        [EnumMember(Value = "Rojo")]
        Rojo
    }
}
