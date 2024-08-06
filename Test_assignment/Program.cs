using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Test_assignment.Configurations;
using Test_assignment.Repositories.Implementations;
using Test_assignment.Repositories.Interfaces;
using Test_assignment.Services.Implementations;
using Test_assignment.Services.Interfaces;




var builder = WebApplication.CreateBuilder(args);

// ��������� Serilog ��� ���������� �����������
// ������������ ������� ����������� �� ����� ������������, ��������� ���� ����������� � ��������� � ���������� ���� � ������� � � ���� � ���������� ���������.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // ������������� Serilog � �������� ���������� ����������� ��� ����������

// ���������� �������� � ��������� ������������
builder.Services.AddControllers(); // ���������� ������������ MVC
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings")); // ������������ ���������� API
builder.Services.AddHttpClient<IAirportRepository, AirportRepository>(); // ����������� HTTP ������� ��� ��������� ������ �� ����������
builder.Services.AddScoped<IDistanceCalculationService, DistanceCalculationService>(); // ����������� ������� ������� ����������
builder.Services.AddScoped<IAirportService, AirportService>(); // ����������� ������� ��� ������ � �����������

// ��������� Swagger ��� ������������ API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // ���������� Swagger ��������� ��� ������������ API

var app = builder.Build();

// ��������� ��������� ��������� ��������
app.UseRouting(); // ���������� �������������

// ��������� Swagger UI ��� ������������ API
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); // ��������� ����� ����� ��� Swagger UI
});

app.UseAuthorization(); // ��������� �����������

app.MapControllers(); // ������� ������������ ��� ��������� ��������

app.Run(); // ������ ����������