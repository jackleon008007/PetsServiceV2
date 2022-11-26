using GrupoWebBackend.DomainAdoptionsRequests.Domain.Repositories;
using GrupoWebBackend.DomainAdoptionsRequests.Domain.Services;
using GrupoWebBackend.DomainAdoptionsRequests.Persistence.Repositories;
using GrupoWebBackend.DomainAdoptionsRequests.Services;
using GrupoWebBackend.DomainAdvertisements.Domain.Repositories;
using GrupoWebBackend.DomainAdvertisements.Domain.Services;
using GrupoWebBackend.DomainAdvertisements.Persistence.Repositories;
using GrupoWebBackend.DomainAdvertisements.Services;
using GrupoWebBackend.DomainDistrict.Domain.Repositories;
using GrupoWebBackend.DomainDistrict.Domain.Services;
using GrupoWebBackend.DomainDistrict.Persistence.Repositories;
using GrupoWebBackend.DomainDistrict.Services;
using GrupoWebBackend.DomainPets.Domain.Repositories;
using GrupoWebBackend.DomainPets.Domain.Services;
using GrupoWebBackend.DomainPets.Persistence.Repositories;
using GrupoWebBackend.DomainPets.Services;
using GrupoWebBackend.DomainPublications.Domain.Repositories;
using GrupoWebBackend.DomainPublications.Domain.Services;
using GrupoWebBackend.DomainPublications.Persistence.Repositories;
using GrupoWebBackend.DomainPublications.Services;
using GrupoWebBackend.Mapping;
using GrupoWebBackend.Security.Authorization.Handlers.Implementations;
using GrupoWebBackend.Security.Authorization.Handlers.Interfaces;
using GrupoWebBackend.Security.Domain.Repositories;
using GrupoWebBackend.Security.Domain.Services;
using GrupoWebBackend.Security.Persistence.Repositories;
using GrupoWebBackend.Security.Services;
using GrupoWebBackend.Shared.Domain.Repositories;
using GrupoWebBackend.Shared.Persistence.Context;
using GrupoWebBackend.Shared.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//string kvURL = builder.Configuration["KeyVaultConfig:KVUrl"];
//string tenantId = builder.Configuration["KeyVaultConfig:KVUrl"];
//string clientId = builder.Configuration["KeyVaultConfig:KVUrl"];
//string clientSecret = builder.Configuration["KeyVaultConfig:KVUrl"];






var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySQL(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors());


builder.Services.AddRouting(options => options.LowercaseUrls = true);


// Dependency Injection Configuration
builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IPublicationRepository, PublicationRepository>();
builder.Services.AddScoped<IPublicationService, PublicationService>();
builder.Services.AddScoped<IAdoptionsRequestsRepository,AdoptionsRequestsRepository>();
builder.Services.AddScoped<IAdoptionsRequestsService,AdoptionsRequestsService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();

//
builder.Services.AddAutoMapper(
    typeof(ModelToResourceProfile), 
    typeof(ResourceToModelProfile));



var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<AppDbContext>())
{
    context.Database.EnsureCreated();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();