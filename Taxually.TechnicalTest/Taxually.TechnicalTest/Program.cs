using Taxually.TechnicalTest.Clients.HttpClient;
using Taxually.TechnicalTest.Clients.QueueClient;
using Taxually.TechnicalTest.Serializers;
using Taxually.TechnicalTest.Services.Vat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVatRegistrationRequestSerializer, VatRegistrationRequestSerializer>();
builder.Services.AddScoped<ITaxuallyHttpClient, TaxuallyHttpClient>();
builder.Services.AddScoped<ITaxuallyQueueClient, TaxuallyQueueClient>();
builder.Services.AddScoped<ITaxuallyVatService, TaxuallyVatService>();

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
