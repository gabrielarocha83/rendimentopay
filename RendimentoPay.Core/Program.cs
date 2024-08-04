using RendimentoPay.Core.Services.Interface;
using RendimentoPay.Core.Services.Services;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
   // .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
    //.WriteTo.MongoDB("mongodb://localhost:27017/logs", collectionName: "logCollection")
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
        columnOptions: GetSqlColumnOptions() )
    .CreateLogger();

builder.Host.UseSerilog(logger);


// Register RedisManagerService
builder.Services.AddSingleton<IRedisManagerService, RedisManagerService>();
// Configure Redis connection
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));

// Add services to the container.
builder.Services.AddScoped<IContaCargaService, ContaCargaService>();
builder.Services.AddScoped<IOrdemPagamentoService, OrdemPagamentoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

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
app.MapHealthChecks("/health");

app.Run();

// Método auxiliar para configurar as colunas do SQL Server
static ColumnOptions GetSqlColumnOptions()
{
    var columnOptions = new ColumnOptions();
    columnOptions.Store.Remove(StandardColumn.Properties);
    columnOptions.Store.Add(StandardColumn.LogEvent);
    columnOptions.LogEvent.DataLength = 2048;

    return columnOptions;
}
