using Marten;
using DummyStore.Data.Context;
using DummyStore.ServiceDefaults;
using DummyStore.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<DummyStoreContext>("dummystore");

// Add services to the container.

builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMetrics();

builder.Services.AddMarten(options =>
{
  options.Connection(builder.Configuration.GetConnectionString("dummystore")!);
  options.Events.DatabaseSchemaName = "event_source";
  options.DatabaseSchemaName = "event_source";
});

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new() { Title = "DummyStore.Api", Version = "v1" });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DummyStore.Api v1");
    c.RoutePrefix = string.Empty;
  });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
