



var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
ConfigurationManager configuration = builder.Configuration;


app.Run();