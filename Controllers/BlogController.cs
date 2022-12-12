using lanstreamer_api.services;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : Controller
{
    public BlogController(BlogService blogService)
    {
        _blogService = blogService;
    }
    
    private readonly BlogService _blogService;
    
    [HttpGet("content/{title}")]
    public Task<string> GetBlogContent(string title)
    {
        return _blogService.GetBlogContent(title);
    }

    [HttpGet("image/{title}")]
    public Task<Stream> GetBlogImage(string title)
    {
        return _blogService.GetBlogImage(title);
    }

    [HttpGet("list")]
    public Task<List<string>> GetBlogs()
    {
        return _blogService.GetBlogs();
    }
}
