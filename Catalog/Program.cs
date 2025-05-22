
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.AddNpgsqlDbContext<ProductDbContext>(connectionName: "catalogdb");
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.MapDefaultEndpoints();

//if(app.Environment.IsDevelopment())
//{
    app.UseMigration();
//}


// Configure the HTTP request pipeline.

app.MapProductEndpoints();

app.UseHttpsRedirection();



app.Run();
