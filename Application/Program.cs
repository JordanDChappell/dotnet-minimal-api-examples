using Application.Endpoints;
using Lib.Infrastructure;

var app = Configuration.ConfigureApplication(args);
var todo = app.NewVersionedApi("todos");
var todoItems = todo.MapGroup("/api/v{version:apiVersion}/todos")
  .WithName("Todos")
  .HasApiVersion(1)
  .HasApiVersion(2);

TodoV1.Register(todoItems);
TodoV2.Register(todoItems);

app.ConfigureSwaggerUi();
app.Run();
