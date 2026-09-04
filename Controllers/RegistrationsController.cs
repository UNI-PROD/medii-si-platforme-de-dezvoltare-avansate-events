using Microsoft.AspNetCore.Mvc;
using medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs;
using medii_si_platforme_de_dezvoltare_avansate_events.Services;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public RegistrationsController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        /// <summary>
        /// Utilizatorul se înscrie la un eveniment
        /// </summary>
        [HttpPost("join")]
        public async Task<ActionResult<ApiResponse<EventRegistrationDto>>> JoinEvent([FromBody] JoinEventRequest request, [FromQuery] int userId)
        {
            var result = await _eventService.JoinEventAsync(userId, request.EventId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Utilizatorul se retrage de la un eveniment
        /// </summary>
        [HttpPost("cancel")]
        public async Task<ActionResult<ApiResponse<bool>>> CancelRegistration([FromQuery] int userId, [FromQuery] int eventId)
        {
            var result = await _eventService.CancelRegistrationAsync(userId, eventId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obține înscrierii utilizatorului
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EventRegistrationDto>>>> GetUserRegistrations(int userId)
        {
            var result = await _userService.GetUserRegistrationsAsync(userId);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
