using Microsoft.AspNetCore.Mvc;
using medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs;
using medii_si_platforme_de_dezvoltare_avansate_events.Services;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Obține toate evenimentele
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventDto>>>> GetAllEvents()
        {
            var result = await _eventService.GetAllEventsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obține evenimentele active (cu înscrierea deschisă)
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventDto>>>> GetActiveEvents()
        {
            var result = await _eventService.GetActiveEventsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obține un eveniment după ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EventDto>>> GetEventById(int id)
        {
            var result = await _eventService.GetEventByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Creează un eveniment nou (doar Admin)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<EventDto>>> CreateEvent([FromBody] CreateEventRequest request, [FromQuery] int adminUserId)
        {
            var result = await _eventService.CreateEventAsync(request, adminUserId);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetEventById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Modifică un eveniment (doar Admin care l-a creat)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EventDto>>> UpdateEvent(int id, [FromBody] UpdateEventRequest request, [FromQuery] int adminUserId)
        {
            var result = await _eventService.UpdateEventAsync(id, request, adminUserId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Șterge un eveniment (doar Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteEvent(int id, [FromQuery] int adminUserId)
        {
            var result = await _eventService.DeleteEventAsync(id, adminUserId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obține lista de participanți la un eveniment (doar Admin)
        /// </summary>
        [HttpGet("{id}/participants")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventRegistrationDto>>>> GetEventParticipants(int id, [FromQuery] int adminUserId)
        {
            var result = await _eventService.GetEventParticipantsAsync(id, adminUserId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
