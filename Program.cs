using lanstreamer_api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://0.0.0.0:80", "https://0.0.0.0:443"); // TODO: czy potrzebne?
                webBuilder.UseStartup<Startup>();
            });
}
