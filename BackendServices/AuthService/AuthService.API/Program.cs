using AuthService.BusinessLogicLayer;
using AuthService.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddBusinessLogicLayer();
builder.Services.AddDataAccessLayer(builder.Configuration);

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();