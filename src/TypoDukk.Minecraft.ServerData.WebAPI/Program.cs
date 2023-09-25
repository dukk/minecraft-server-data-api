using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Configuration;
using TypoDukk.Minecraft.ServerRcon.WebAPI.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("./rcon.json", true, true);

        var rconConfiguration = builder.Configuration.GetSection("Rcon").Get<RconConfiguration>();

        if (rconConfiguration is not null)
            builder.Services.AddSingleton(rconConfiguration);

        builder.Services.AddSingleton<CoreRconService>();
        builder.Services.AddSingleton<IRconService>((provider) => provider.GetRequiredService<CoreRconService>());

        builder.Services.AddSingleton<ValidationService>();
        builder.Services.AddSingleton<IValidationService>((provider) => provider.GetRequiredService<ValidationService>());

        builder.Services.AddSingleton<RconServerService>();
        builder.Services.AddSingleton<IServerService>((provider) => provider.GetRequiredService<RconServerService>());

        builder.Services.AddSingleton<RconPlayerService>();
        builder.Services.AddSingleton<IPlayerService>((provider) => provider.GetRequiredService<RconPlayerService>());

        builder.Services.AddSingleton<RconEconomyService>();
        builder.Services.AddSingleton<IEconomyService>((provider) => provider.GetRequiredService<RconEconomyService>());

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Minecraft Server RCON API",
                Description = "Exposes a REST API to send RCON commands to minecraft servers. Designed for use on https://minecraft.dukk.org/ but may be applicable to others.",
                Contact = new OpenApiContact()
                {
                    Name = "dukk",
                    Url = new Uri("https://github.com/dukk/minecraft-server-rcon-api")
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}