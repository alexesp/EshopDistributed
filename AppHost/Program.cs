var builder = DistributedApplication.CreateBuilder(args);

//Backing Services
var postgres = builder
    .AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase("catalogdb");

var cache = builder
    .AddRedis("cache")
    .WithRedisInsight()
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);


//Projects

var catalog = builder
    .AddProject<Projects.Catalog>("catalog")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

//builder.AddProject<Projects.Catalog>("catalog");

//builder.AddProject<Projects.Basket>("basket");

var basket = builder
    .AddProject<Projects.Basket>("basket")
    .WithReference(cache)
    .WaitFor(cache);

//builder.AddProject<Projects.Catalog>("catalog");

builder.Build().Run();
