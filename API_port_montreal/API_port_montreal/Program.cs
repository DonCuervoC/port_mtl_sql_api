using API_port_montreal.Data;
using API_port_montreal.Mappers;
using API_port_montreal.Repository;
using API_port_montreal.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// //CONFIGURATION DE LA CONNEXION AU SERVEUR MSSQL(MICROSOFT SQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSql"));
});


// // NOUS AJOUTONS LES REPOSITORIES
builder.Services.AddScoped<IArriveesRepository, AriveesRepository>();
builder.Services.AddScoped<IDepartsRepository, DepartsRepository>();

//  AJOUTER L'AUTOMAPPER
builder.Services.AddAutoMapper(typeof(MyMapper));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
