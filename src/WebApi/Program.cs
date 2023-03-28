using Mapster;
using MapsterMapper;
using Lattice.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Lattice.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<LatticeDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("LatticeSql")));

        builder.Services.AddSingleton(ConfigureMapper());
        builder.Services.AddScoped<IMapper, ServiceMapper>();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITeamService, TeamService>();
        builder.Services.AddScoped<IBoardService, BoardService>();
        builder.Services.AddScoped<ISectionService, SectionService>();
        builder.Services.AddScoped<ICardService, CardService>();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>  
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        builder.Services.AddApiVersioning();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c => 
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Lattice",
                Version = "v1",
                Description = "A Web API for mananing tasks",
            });

            string[] projectsXmlFiles = new string[]
            {
                "Lattice.WebApi.xml",
                "Lattice.Infrastructure.xml",
                "Lattice.Domain.xml",
            };

            foreach (var files in projectsXmlFiles)
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, files);
                if (File.Exists(filePath)) c.IncludeXmlComments(filePath);
            }
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
   }

    public static TypeAdapterConfig ConfigureMapper()
    {
        var config = new TypeAdapterConfig();

        // config.NewConfig<Source, Destination>

        config.NewConfig<UserCreateDto, UserAccount>()
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);

        config.NewConfig<UserCreateDto, UserAccount>()
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);

        config.NewConfig<UserTeam, TeamDto>()
            .Map(dest => dest.Name, src => src.Team!.Name)
            .Map(dest => dest.Owner, src => src.Team!.Owner);

        config.NewConfig<UserTeam, UserDto>()
            .Map(dest => dest.Name, src => src.User!.Name)
            .Map(dest => dest.Id, src => src.User!.Id);

        config.NewConfig<Team, TeamDto>()
            .Map(dest => dest.Members, src => src.UserTeams);

        config.NewConfig<Board, BoardDto>()
            .Map(
                dest => dest.Creator,
                src => src.Creator!.Adapt<UserDto>()
            )
            .Map(
                dest => dest.Sections,
                src => src.Sections!.Adapt<List<SectionDto>>()
            )
            .IgnoreNullValues(true);

        config.NewConfig<Section, SectionDto>()
            .Map(
                dest => dest.Cards,
                src => src.Cards!.Adapt<List<CardDto>>()
            )
            .IgnoreNullValues(true);

        return config;
    }
}
