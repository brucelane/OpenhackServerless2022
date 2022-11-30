using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureServices(s => s.AddHttpClient())
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
