using Microsoft.EntityFrameworkCore;
using movie_explorer.Data;
using movie_explorer.Repositories;
using movie_explorer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IExternalMovieService, ExternalMovieService>();
builder.Services.AddHttpClient<ExternalMovieService>();

builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IExternalGenreService, ExternalGenreService>();
builder.Services.AddHttpClient<ExternalGenreService>();

builder.Services.AddAutoMapper(typeof(Program));    

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);



builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlServer("Server=localhost,1433;Database=movie_db;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;")
);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MovieExplorer_";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
