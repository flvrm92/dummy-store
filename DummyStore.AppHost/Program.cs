
var builder = DistributedApplication.CreateBuilder(args);

//var username = builder.AddParameter("username", "postgres");
//var password = builder.AddParameter("password", "Postgres@123", secret: true);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(x => x.WithHostPort(5050))
    .WithDataVolume("dummystore-postgres-data")
    .AddDatabase("dummystore");    

builder.AddProject<Projects.DummyStore_Api>("dummystore-api")
      .WithReference(postgres);

builder.AddProject<Projects.DummyStore_MigrationService>("dummystore-migrationservice")
  .WithReference(postgres);

builder.Build().Run();