using Lib.Contracts.Generics;
using Lib.Contracts.Todo;
using Lib.Service.Todo;
using Lib.Validators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Endpoints;

public static class TodoV2
{
  public static RouteGroupBuilder Register(RouteGroupBuilder builder)
  {
    builder
      .MapGet("/", async ([FromServices] TodoService service) => await service.GetAllTodosAsync())
      .WithName("GetTodosV2")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A good example of:\n" +
        "- Well defined method names: The base /todos route implies that the current endpoint will query the entire collection.\n" +
        "- Consistent response structure: This route utilises a generic 'ItemsResponse' structure that is extensible."
      ))
      .Produces<ItemsResponse<TodoResponse>>(StatusCodes.Status200OK)
      .MapToApiVersion(2);

    builder
      .MapGet("/{id}", async ([FromRoute] int id, [FromServices] TodoService service) =>
        await service.GetTodoAsync(id) is TodoResponse response
            ? Results.Ok(response)
            : Results.NotFound())
      .WithName("GetTodoV2")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A good example of:\n" +
        "- Well defined method names: A query for a single todo item is implied based on the hierarchy in the route.\n" +
        "- Provide meaningful responses: This endpoint can return a 200 or a 404 depending on data state."
      ))
      .Produces<TodoResponse>(StatusCodes.Status200OK, "application/json")
      .Produces(StatusCodes.Status404NotFound)
      .MapToApiVersion(2);

    builder
      .MapPost("/", async ([FromBody] TodoRequest request, [FromServices] TodoService service) =>
        {
          var response = await service.CreateTodoAsync(request);
          return Results.Created($"/todos/{response.Id}", response);
        })
      .WithName("CreateTodoV2")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A good example of:\n" +
        "- Utilise HTTP actions: This 'create' action is implied from the route and HTTP POST action being used.\n" +
        "Provide meaningful responses: This endpoint will return a 201 'Created' or a 400 'Bad Request' (has request validation)."
      ))
      .Accepts<TodoRequest>("application/json")
      .Produces(StatusCodes.Status201Created)
      .Produces(StatusCodes.Status400BadRequest)
      .AddEndpointFilter<ValidationFilter<TodoRequest>>()
      .MapToApiVersion(2);

    builder
      .MapPut("/{id}", async ([FromRoute] int id, [FromBody] TodoRequest request, [FromServices] TodoService service) =>
        {
          if (await service.UpdateTodoAsync(id, request) is TodoResponse response)
            return Results.NoContent();
          return Results.NotFound();
        })
      .WithName("UpdateTodoV2")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A good example of:\n" +
        "Utilise HTTP actions: This 'update' action is implied from the route and HTTP PUT action being used.\n" +
        "Provide meaningful responses: This endpoint can return a 204 'No Content' a 400 or a 404 depending on request and data state."
      ))
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status400BadRequest)
      .Produces(StatusCodes.Status404NotFound)
      .AddEndpointFilter<ValidationFilter<TodoRequest>>()
      .MapToApiVersion(2);

    builder
      .MapDelete("/{id}", async ([FromRoute] int id, [FromServices] TodoService service) =>
        {
          if (await service.DeleteTodoAsync(id) is TodoResponse response)
            return Results.NoContent();
          return Results.NotFound();
        })
      .WithName("DeleteTodoV2")
      .WithMetadata(new SwaggerOperationAttribute(description:
        "A good example of:\n" +
        "Utilise HTTP actions: This 'delete' action is implied from the route and HTTP DELETE action being used.\n" +
        "Provide meaningful responses: This endpoint can return a 204 or a 404 depending on data state."
      ))
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status404NotFound)
      .MapToApiVersion(2);

    return builder;
  }
}