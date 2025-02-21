var builder = DistributedApplication.CreateBuilder(args);

var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithBindMount("../grafana/config", "/etc/grafana")
    .WithBindMount("../grafana/dashboards", "/var/lib/grafana/dashboards")
    .WithEndpoint(3000, 3000, name: "grafana-http", scheme: "http");

builder.AddContainer("prometheus", "prom/prometheus")
    .WithBindMount("../prometheus", "/etc/prometheus")
    .WithEndpoint(9090, 9090, scheme: "http");

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(x => x.WithHostPort(5050))
    .WithDataVolume("dummystore-postgres-data")
    .AddDatabase("dummystore");

builder.AddProject<Projects.DummyStore_Api>("dummystore-api")
  .WithReference(postgres)
  .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"));

builder.AddProject<Projects.DummyStore_MigrationService>("dummystore-migrationservice")
  .WithReference(postgres)
  .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("grafana-http"));

builder.Build().Run();