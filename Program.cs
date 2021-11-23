using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using dashboard_api.Models;
using dashboard_api.Helpers;
using dashboard_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CovidContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("CovidDb")));

var dataTable = CSVHelper.GetDataTable("F:\\DL\\owid-covid-data.csv");

builder.Services.AddScoped<CovidItemService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var covidService = services.GetService<CovidItemService>();
    var time = await covidService.AddBulkDataAsync(dataTable);

    Console.WriteLine($"Execution time: {time}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
