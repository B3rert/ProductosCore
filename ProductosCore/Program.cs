var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
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

//Configutrar CORS
app.UseCors(options =>
{
    options.AllowAnyHeader(); //permitir cualquier encabezado
    options.WithOrigins("*"); //permitir cualquier direccion de origen 
    options.AllowAnyMethod(); //permitir cualquier metodo 
    options.WithMethods("*"); //espicificar metodos http
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
