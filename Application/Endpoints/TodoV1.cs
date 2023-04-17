using Lib.Model.Todo;
using Lib.Service.Todo;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Endpoints;

public static class TodoV1
{
  public static RouteGroupBuilder Register(RouteGroupBuilder builder)
  {
    builder
      .MapGet("/all", async ([FromServices] TodoService service) => await service.GetAllTodosAsync())
      .WithName("GetTodosV1")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A bad example of:\n" +
        "- Well defined method names: the /todos base route is self-describing and does not require the /all route\n" +
        "- Consistent response structure: response is a flat array which is not consistent with other routes and is hard to extend in the future"
      ))
      .Produces<IEnumerable<TodoResponse>>(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    // This endpoint is using a query parameter to fetch by a resource identifier - resources should be identified through
    // their route.
    // The route for this endpoint '/item' would be unnecessary in a RESTful application (if other methods used HTTP verbs)
    // The resource does not make use of response types when returning content, the client will always receive a generic
    // 'success' (200) response, even if data is not fetched.
    builder
      .MapGet("/item", async ([FromQuery] int id, [FromServices] TodoService service) => await service.GetTodoAsync(id))
      .WithName("GetTodoV1")
      .WithMetadata(
        "A bad example of:\n" +
        "- Well defined method names: "
      )
      .Produces<TodoResponse>(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/create", async ([AsParameters] TodoRequest request, [FromServices] TodoService service) => await service.CreateTodoAsync(request))
      .WithName("CreateTodoV1")
      .WithDescription("")
      .Produces(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/update", async ([FromQuery] int id, [AsParameters] TodoRequest request, [FromServices] TodoService service) => await service.UpdateTodoAsync(id, request))
      .WithName("UpdateTodoV1")
      .WithDescription("")
      .Produces(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/delete", async ([FromQuery] int id, [FromServices] TodoService service) => await service.DeleteTodoAsync(id))
      .WithName("DeleteTodoV1")
      .WithDescription("")
      .Produces(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    return builder;
  }
}