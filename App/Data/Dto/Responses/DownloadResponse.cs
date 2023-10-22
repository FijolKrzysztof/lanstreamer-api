using System.ComponentModel.DataAnnotations;

namespace lanstreamer_api.Models;

public class DownloadResponse
{
    [Required]
    public string link;
}