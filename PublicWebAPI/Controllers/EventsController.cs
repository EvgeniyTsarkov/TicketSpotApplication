using Microsoft.AspNetCore.Mvc;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController(IEventService eventService) : Controller
{
    private readonly IEventService _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _eventService.GetAllAsync();

        return Ok(events);
    }

    [HttpGet("{event_id:int}/sections/{section_id:int}/seats")]
    public async Task<IActionResult> GetByIdAndSectionId(int event_id, char section_id)
    {
        var seats = await _eventService.GetByIdAndSectionId(event_id, 'C');

        return Ok(seats);
    }
}
