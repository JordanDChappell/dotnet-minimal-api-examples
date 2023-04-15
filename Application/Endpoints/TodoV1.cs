using Lib.Model.Todo;
using Lib.Service.Todo;

namespace Application.Endpoints;

public static class TodoV1
{
  public static RouteGroupBuilder Register(RouteGroupBuilder builder)
  {
    // This endpoint does not follow a consistent naming pattern and returns a flat array structure.
    // The route for this endpoint '/all' is unnecessary in a RESTful application.
    builder
      .MapGet("/all", async (TodoService service) => await service.GetAllTodosAsync())
      .WithName("GetTodosV1")
      .MapToApiVersion(1);

    // This endpoint is using a query parameter to fetch by a resource identifier - resources should be identified through
    // their route.
    // The route for this endpoint '/item' would be unnecessary in a RESTful application (if other methods used HTTP verbs)
    // The resource does not make use of response types when returning content, the client will always recieve a generic
    // 'success' (200) response, even if data is not fetched.
    builder
      .MapGet("/item", async (int id, TodoService service) => await service.GetTodoAsync(id))
      .WithName("GetTodoV1")
      .MapToApiVersion(1);

    // This endpoint should be a POST with no '/create' route, a POST in a RESTful service will always create a new
    // entity.
    builder
      .MapGet("/create", async (TodoRequest request, TodoService service) => await service.CreateTodoAsync(request))
      .WithName("CreateTodoV1")
      .MapToApiVersion(1);

    // 
    builder
      .MapGet("/update", async (int id, TodoRequest request, TodoService service) => await service.UpdateTodoAsync(id, request))
      .WithName("UpdateTodoV1")
      .MapToApiVersion(1);

    builder
      .MapGet("/delete", async (int id, TodoService service) => await service.DeleteTodoAsync(id))
      .WithName("DeleteTodoV1")
      .MapToApiVersion(1);

    return builder;
  }
}