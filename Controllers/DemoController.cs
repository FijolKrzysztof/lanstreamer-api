using lanstreamer_api.services;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.Controllers;

[ApiController]
[Route("api/demo")]
public class DemoController : Controller
{
    public DemoController(DemoService demoService)
    {
        _demoService = demoService;
    }

    private readonly DemoService _demoService;
    
    [HttpGet("preview/{filename}/{timestampId}")]
    public async Task<Stream> Preview(string filename, int timestampId)
    {
        return await _demoService.Preview(filename, timestampId);
    }

    [HttpGet("video/{filename}")]
    public async Task<Stream> Video(string filename)
    {
        return await _demoService.Video(filename);
    }

    [HttpGet("poster/{filename}")]
    public async Task<Stream> Poster(string filename)
    {
        return await _demoService.Poster(filename);
    }
}