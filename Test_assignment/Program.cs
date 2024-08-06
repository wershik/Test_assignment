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

// Настройка Serilog как провайдера логирования
// Конфигурация логгера считывается из файла конфигурации, обогащает логи информацией о контексте и записывает логи в консоль и в файл с ежедневным вращением.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Устанавливаем Serilog в качестве провайдера логирования для приложения

// Добавление сервисов в контейнер зависимостей
builder.Services.AddControllers(); // Добавление контроллеров MVC
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings")); // Конфигурация параметров API
builder.Services.AddHttpClient<IAirportRepository, AirportRepository>(); // Регистрация HTTP клиента для получения данных об аэропортах
builder.Services.AddScoped<IDistanceCalculationService, DistanceCalculationService>(); // Регистрация сервиса расчета расстояний
builder.Services.AddScoped<IAirportService, AirportService>(); // Регистрация сервиса для работы с аэропортами

// Настройка Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Добавление Swagger генерации для документации API

var app = builder.Build();

// Настройка пайплайна обработки запросов
app.UseRouting(); // Добавление маршрутизации

// Настройка Swagger UI для визуализации API
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); // Установка точки входа для Swagger UI
});

app.UseAuthorization(); // Включение авторизации

app.MapControllers(); // Маппинг контроллеров для обработки запросов

app.Run(); // Запуск приложения