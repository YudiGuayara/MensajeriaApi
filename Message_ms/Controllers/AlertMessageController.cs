using Microsoft.AspNetCore.Mvc;
using AlertMessageApi.Models;
using AlertMessageApi.Services;
using MongoDB.Bson; // Importar espacio de nombres para ObjectId
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlertMessageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly AlertService _alertService;

        public AlertController(AlertService alertService)
        {
            _alertService = alertService;
        }

        // GET: api/alert
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlertMessage>>> GetAlertMessages()
        {
            try
            {
                var alertMessages = await _alertService.GetAlertMessagesAsync();
                return Ok(alertMessages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener mensajes de alerta: {ex.Message}");
                return StatusCode(500, "Error al obtener mensajes de alerta. Por favor, inténtalo de nuevo.");
            }
        }

        // GET: api/alert/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AlertMessage>> GetAlertMessage(string id)
        {
            try
            {
                var alertMessage = await _alertService.GetAlertMessageByIdAsync(id);
                if (alertMessage == null)
                {
                    return NotFound();
                }
                return Ok(alertMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el mensaje de alerta con ID {id}: {ex.Message}");
                return StatusCode(500, "Error al obtener el mensaje de alerta. Por favor, inténtalo de nuevo.");
            }
        }

        // POST: api/alert
        [HttpPost]
        public async Task<ActionResult<AlertMessage>> PostAlertMessage([FromBody] AlertMessage alertMessage)
        {
            try
            {
                if (alertMessage == null)
                {
                    return BadRequest("Alert message data is null.");
                }

                alertMessage.Id = ObjectId.GenerateNewId().ToString();

                await _alertService.CreateAlertMessageAsync(alertMessage);
                return CreatedAtAction(nameof(GetAlertMessage), new { id = alertMessage.Id }, alertMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear mensaje de alerta: {ex.Message}");
                return StatusCode(500, "Error al crear mensaje de alerta. Por favor, inténtalo de nuevo.");
            }
        }

        // PUT: api/alert/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlertMessage(string id, [FromBody] AlertMessage updatedAlertMessage)
        {
            if (updatedAlertMessage == null || updatedAlertMessage.Id != id)
            {
                return BadRequest("Invalid alert message data");
            }

            try
            {
                var result = await _alertService.UpdateAlertMessageAsync(id, updatedAlertMessage);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Alert message updated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el mensaje de alerta con ID {id}: {ex.Message}");
                return StatusCode(500, "Error al actualizar el mensaje de alerta. Por favor, inténtalo de nuevo.");
            }
        }

        // DELETE: api/alert/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlertMessage(string id)
        {
            try
            {
                var result = await _alertService.DeleteAlertMessageAsync(id);
                if (!result)
                {
                    return NotFound(new { Message = "Alert message not found" });
                }
                return Ok(new { Message = "Alert message deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el mensaje de alerta con ID {id}: {ex.Message}");
                return StatusCode(500, "Error al eliminar el mensaje de alerta. Por favor, inténtalo de nuevo.");
            }
        }
    }
}
