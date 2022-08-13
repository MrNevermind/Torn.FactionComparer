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
                webBuilder.ConfigureKestrel((context, serverOptions) => {
                    var kestrelSection = context.Configuration.GetSection("Kestrel");
                    serverOptions.Configure(kestrelSection);
                });
                webBuilder.UseStartup<Startup>();
            });
}