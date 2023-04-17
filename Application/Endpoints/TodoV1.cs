using Lib.Contracts.Todo;
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
        "- Well defined method names: The /todos base route is self-describing and does not require the /all route.\n" +
        "- Consistent response structure: Response is a flat array which is not consistent with other routes and is hard to extend in the future."
      ))
      .Produces<IEnumerable<TodoResponse>>(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/item", async ([FromQuery] int id, [FromServices] TodoService service) => await service.GetTodoAsync(id))
      .WithName("GetTodoV1")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A bad example of:\n" +
        "- Well defined method names: The /item route does not describe what this endpoint does in any detail.\n" +
        "- Provide meaningful responses: This endpoint returns a 200 'OK' status in all cases, a 404 'Not Found' would be useful."
      ))
      .Produces<TodoResponse>(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/create", async ([AsParameters] TodoRequest request, [FromServices] TodoService service) =>
        await service.CreateTodoAsync(request))
      .WithName("CreateTodoV1")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A bad example of:\n" +
        "- Utilise HTTP actions: This 'create' method should be a POST request."
      ))
      .Produces(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/getorcreate", async ([FromQuery] int id, [AsParameters] TodoRequest request, [FromServices] TodoService service) =>
        {
          var response = await service.GetTodoAsync(id);
          response ??= await service.CreateTodoAsync(request);
          return response;
        })
      .WithName("GetOrCreateTodoV1")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A bad example of:\n" +
        "- Avoid side effects: This [GET] method has multiple responsibilities and breaks the RESTful pattern."
      ))
      .Produces<TodoResponse>(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/update", async ([FromQuery] int id, [AsParameters] TodoRequest request, [FromServices] TodoService service) =>
        await service.UpdateTodoAsync(id, request))
      .WithName("UpdateTodoV1")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A bad example of:\n" +
        "- Utilise HTTP actions: This 'update' method should be a PUT request.\n" +
        "- Provide meaningful responses: This endpoint returns a 200 'OK' status in all cases, a 404 'Not Found' would be useful."
      ))
      .Produces(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    builder
      .MapGet("/delete", async ([FromQuery] int id, [FromServices] TodoService service) => await service.DeleteTodoAsync(id))
      .WithName("DeleteTodoV1")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A bad example of:\n" +
        "- Utilise HTTP actions: This 'delete' method should be a DELETE request.\n" +
        "- Provide meaningful responses: This endpoint returns a 200 'OK' status in all cases, a 404 'Not Found' would be useful."
      ))
      .Produces(StatusCodes.Status200OK)
      .MapToApiVersion(1);

    return builder;
  }
}