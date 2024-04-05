using AI.Devs.Reloaded.API;
using AI.Devs.Reloaded.API.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddOptions()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCustomHttpClients(builder.Configuration)
    .AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddTaskEndpoints();
app.Run();
