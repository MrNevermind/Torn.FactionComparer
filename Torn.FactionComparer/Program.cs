namespace Torn.FactionComparer;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
#if !DEBUG
                webBuilder.UseKestrel();
                webBuilder.UseUrls("http://176.223.137.147:8080");
#endif
                webBuilder.UseStartup<Startup>();
            });
}