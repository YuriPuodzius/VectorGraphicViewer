using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;
using Wscad.VectorGraphicViewer.Application.Orchestration;
using Wscad.VectorGraphicViewer.Application.Orchestration.Interfaces;
using Wscad.VectorGraphicViewer.Domain.Contracts;
using Wscad.VectorGraphicViewer.Domain.Contracts.Repositories;

// Domain
using Wscad.VectorGraphicViewer.Domain.Enums;
using Wscad.VectorGraphicViewer.Domain.Services;

// Infrastructure (DataSources + Options)
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Options;
using Wscad.VectorGraphicViewer.Infrastructure.DataProviders.Sources;
using Wscad.VectorGraphicViewer.Infrastructure.Repository;
using Wscad.VectorGraphicViewer.WpfApp;


// Alias to avoid conflict with System.Windows.Application
using WpfApplication = System.Windows.Application;

namespace Wscad.VectorGraphicViewer;

public partial class App : WpfApplication
{
    public static IHost Host { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables();
            })
            .ConfigureServices((ctx, services) =>
            {
                // UI
                services.AddSingleton<MainWindow>();

                // === DataSourceConfig ===
                IConfiguration ds = ctx.Configuration.GetSection("DataSourceConfig");

                // Lemos número (1,2,3...) e convertemos para o enum. Default = Json.
                int typeInt = ds.GetValue<int?>("PrimitiveDataSourceType") ?? (int)PrimitiveDataSourceTypeEnum.Json;
                PrimitiveDataSourceTypeEnum kind = (PrimitiveDataSourceTypeEnum)typeInt;


                switch (kind)
                {
                    case PrimitiveDataSourceTypeEnum.Json:
                        services.Configure<PrimitivesJsonOptions>(ds.GetSection("Json"));
                        services.AddSingleton<IPrimitivesDataSource, PrimitivesJsonSource>();
                        break;

                    case PrimitiveDataSourceTypeEnum.Xml:
                        services.Configure<PrimitivesXmlOptions>(ds.GetSection("Xml"));
                        services.AddSingleton<IPrimitivesDataSource, PrimitivesXmlSource>();
                        break;

                    case PrimitiveDataSourceTypeEnum.Api:
                        services.Configure<PrimitivesApiOptions>(ds.GetSection("Api"));
                        services.AddSingleton<IPrimitivesDataSource, PrimitivesApiSource>();
                        break;

                    default:
                        throw new InvalidOperationException("Unknown DataSourceConfig.PrimitiveDataSourceType value.");
                }

                services.AddSingleton<IPrimitiveService, PrimitiveService>(); // Orchestrator
                services.AddSingleton<IGeometryService, GeometryService>();   // Domain service (puro)
                services.AddSingleton<IPrimitiveRepository, PrimitiveRepository>();
                services.AddSingleton<MainViewModel>();
            })
            .ConfigureLogging(lb => lb.AddConsole())
            .Build();

        base.OnStartup(e);

        //IPrimitiveService service = Host.Services.GetRequiredService<IPrimitiveService>();

        //IReadOnlyList<Primitive> all = service.GetAll(); // test GetAll
        //Primitive? onlyCircles = service.GetByType(PrimitiveTypeEnum.Circle); // test GetByType

        MainWindow main = Host.Services.GetRequiredService<MainWindow>();
        main.DataContext = Host.Services.GetRequiredService<MainViewModel>();

        main.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Host.Dispose();
        base.OnExit(e);
    }
}