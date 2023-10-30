using Microsoft.AspNetCore.Mvc;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Modules.Admin;

public class AdminService
{
    public ActionResult UploadDesktopApp(OperatingSystem operatingSystem, IFormFile file)
    {
        try
        {
            if (file != null && file.Length > 0)
            {
                string filePath = ""
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}