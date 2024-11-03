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

    [HttpGet("{eventId:int}/sections/{sectionId:int}/seats")]
    public async Task<IActionResult> GetByIdAndSectionId(int eventId, int sectionId)
    {
        var seatWithProcesDtos = await _eventService.GetByIdAndSectionId(eventId, sectionId);

        return Ok(seatWithProcesDtos);
    }
}
