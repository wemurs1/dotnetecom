var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sqlServer").WithDataVolume();
var database = sqlServer.AddDatabase("OnlineShop");

builder.AddProject<Projects.OnlineShop>("OnelineShop").WithReference(database).WaitFor(database);

builder.Build().Run();
