namespace lanstreamer_api.App.Data.Models.Enums;

public class ApplicationBuildPath
{
    public const string LanstreamerWindows = "/application-builds/lanstreamer-windows.zip";
    public const string LanstreamerLinux = "/application-builds/lanstreamer-windows.zip";

    public static string GetPath(OperatingSystem os)
    {
        switch (os)
        {
            case OperatingSystem.Linux:
                return LanstreamerLinux;
            case OperatingSystem.Windows:
                return LanstreamerWindows;
            default:
                throw new ArgumentException("Unsupported operating system");
        }
    }
}