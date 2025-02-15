using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlertMessageApi.Models;
using MongoDB.Bson;

namespace AlertMessageApi.Services
{
    public class AlertService
    {
        private readonly IMongoCollection<AlertMessage> _alertMessages;

        public AlertService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("AlertMessageDB");
            _alertMessages = database.GetCollection<AlertMessage>("AlertMessages");
        }

        public async Task<List<AlertMessage>> GetAlertMessagesAsync()
        {
            return await _alertMessages.Find(_ => true).ToListAsync();
        }

        public async Task<AlertMessage> GetAlertMessageByIdAsync(string id)
        {
            return await _alertMessages.Find(am => am.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAlertMessageAsync(AlertMessage alertMessage)
        {
            alertMessage.Id = ObjectId.GenerateNewId().ToString();
            alertMessage.Date = DateTime.UtcNow;
            await _alertMessages.InsertOneAsync(alertMessage);

            // Mostrar mensaje al crear un mensaje de alerta
            Console.WriteLine($"Nuevo mensaje de alerta creado: {alertMessage.Message}");
        }

        public async Task<bool> UpdateAlertMessageAsync(string id, AlertMessage updatedAlertMessage)
        {
            var result = await _alertMessages.ReplaceOneAsync(am => am.Id == id, updatedAlertMessage);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAlertMessageAsync(string id)
        {
            var result = await _alertMessages.DeleteOneAsync(am => am.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
