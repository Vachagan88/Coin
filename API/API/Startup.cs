using Core.Contracts;
using Core.Entities;
using Core.Enums;
using DAL;
using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace API;

public class Startup
{
    public IConfiguration Configuration { get; set; }
    public IWebHostEnvironment HostingEnvironment { get; set; }

    public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        Configuration = configuration;
        HostingEnvironment = hostingEnvironment;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddDbContext<ChatDbContext>(options =>
        {
            options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"]);
        });

        services.TryAddScoped<IUnitOfWork, UnitOfWork>();
    }

    public virtual void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseCors("CorsPolicy");

        UpdateDatabase(app);
    }

    private static void UpdateDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<ChatDbContext>();
        context.Database.Migrate();
        var resources = context.Resources.ToList().ToDictionary(x => x.Type);
        foreach (var value in Enum.GetValues(typeof(ResourceType)))
        {
            if ((ResourceType)value == ResourceType.Undefined)
                continue;
            
            if (!resources.ContainsKey((ResourceType)value))
                context.Resources.Add(new Resource() { Type = (ResourceType)value });
        }
        context.SaveChanges();
    }
}
