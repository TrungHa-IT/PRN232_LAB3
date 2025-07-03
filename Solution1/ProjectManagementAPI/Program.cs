var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// ✅ Thêm Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

// ✅ Thêm Swagger Middleware
app.UseSwagger();
app.UseSwaggerUI(); // => http://localhost:7047/swagger

app.MapControllers();

app.Run();
