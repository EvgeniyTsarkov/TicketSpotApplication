using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PublicWebAPI.Business.Dtos;
using PublicWebAPI.Business.Services.Interfaces;

namespace PublicWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class VenuesController(IVenueService venueService) : Controller
{
    private readonly IVenueService _venueService = venueService ?? throw new ArgumentNullException(nameof(venueService));

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var venues = await _venueService.GetAllAsync();

        return Ok(venues);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSections(int id)
    {
        SectionsToVenueDto sectionsToVenueDto;

        try
        {
            sectionsToVenueDto = await _venueService.GetSectionsForVenue(id);
        }
        catch (RecordNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(sectionsToVenueDto);
    }
}
