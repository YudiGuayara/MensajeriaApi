using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlertMessageApi.Models;
using AlertMessageApi.Services;
 
namespace AlertMessageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly IMongoCollection<AlertMessage> _alertMessages;

        public AlertController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("AlertMessageDB");
            _alertMessages = database.GetCollection<AlertMessage>("AlertMessages");
        }

        // GET: api/alert
        [HttpGet]
        public async Task<IActionResult> GetAlertMessages()
        {
            try
            {
                var alertMessages = await _alertMessages.Find(_ => true).ToListAsync();
                return Ok(alertMessages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/alert/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlertMessage(string id)
        {
            try
            {
                var alertMessage = await _alertMessages.Find(am => am.Id == id).FirstOrDefaultAsync();
                if (alertMessage == null)
                {
                    return NotFound();
                }
                return Ok(alertMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/alert
        [HttpPost]
        public async Task<IActionResult> PostAlertMessage([FromBody] AlertMessage alertMessage)
        {
            try
            { 
                alertMessage.Date = DateTime.UtcNow;  // Asigna la fecha y hora actual en UTC

                await _alertMessages.InsertOneAsync(alertMessage);  // Inserta el mensaje de alerta en la base de datos
                return CreatedAtAction(nameof(GetAlertMessage), new { id = alertMessage.Id }, new { Message = "Alert message created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/alert/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlertMessage(string id, [FromBody] AlertMessage updatedAlertMessage)
        {
            try
            {
                var result = await _alertMessages.ReplaceOneAsync(am => am.Id == id, updatedAlertMessage);
                if (result.MatchedCount == 0)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Report updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/alert/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlertMessage(string id)
        {
            try
            {
                var result = await _alertMessages.DeleteOneAsync(am => am.Id == id);
                if (result.DeletedCount == 0)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Report deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
