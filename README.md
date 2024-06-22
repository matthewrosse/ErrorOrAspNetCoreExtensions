# ErrorOrAspNetCoreExtensions 🔥
A collection of extension methods designed 
to reduce the amount of boilerplate code 🥱 needed 
when returning appropriate HTTP responses.

Significantly improves the developer experience of using discriminated unions
in ASP.NET Core applications 😎

## Installation

Via dotnet cli:
```shell
dotnet add package ErrorOrAspNetCoreExtensions
```

Or via package manager console:
```shell
Install-Package ErrorOrAspNetCoreExtensions
```

## Registering problem details services (optional, but recommended)

Example configuration:

```csharp
builder.Services
       .AddProblemDetails(options =>
           options.CustomizeProblemDetails = ctx =>
           {
               ctx.ProblemDetails.Extensions.Add(
                   "instance",
                   $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}"
               );
           }
       );
```

## Usage:

When using the methods this package provides, errors are resolved like this by default:

- ErrorType.Validation   => 400 BadRequest
- ErrorType.Unauthorized => 401 Unauthorized
- ErrorType.Forbidden    => 403 Forbidden
- ErrorType.NotFound     => 404 NotFound
- ErrorType.Conflict     => 409 Conflict
- Any other type         => 500 InternalServerError

All errors are returned in ProblemDetails format, like that:
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "The requested todo item was not found.",
  "status": 404,
  "instance": "GET /api/todos/420",
  "trace-id": "0HN4IA8I0CGOG:00000001"
}
```

However, if you have some specific use case where you want to use
ErrorOr's feature of custom errors, then you can register the appropriate
HTTP status code in the error's metadata, using the key ErrorOrAspNetCoreExtensions.StatusCodeKey

I'm aware that you may not want to pollute your domain/application space with HTTP related stuff,
but as these are only extension methods that are meant to reduce repeating logic, I can't do much more
other than provide this little "hack".

If you have some scenario for which the default implementation doesn't suit your needs,
then probably you want to handle it manually, or throw an exception either way.

### ToOk extension methods

Method that just returns 200 OK:

```csharp
app.MapGet(
    "/api/todos/{id:int}",
    async (
        int id,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        var query = new GetTodoQuery(id);
        var result = await mediator.Send(query, cancellationToken);

        return result.ToOkWithoutBody();
    }
);
```

Method that returns the service/query result directly without mapping:

```csharp
app.MapGet(
    "/api/todos/{id:int}",
    async (
        int id,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        var query = new GetTodoQuery(id);
        var result = await mediator.Send(query, cancellationToken);

        return result.ToOk();
    }
);
```

Method that returns the response mapped to the API contract:

```csharp
app.MapGet(
    "/api/todos/{id:int}",
    async (
        int id,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        var query = new GetTodoQuery(id);
        var result = await mediator.Send(query, cancellationToken);

        return result.ToOk(_mapper.Map<GetTodoResponse>);
    }
);
```

### ToCreated extension methods

Method that just returns 201 Created:

```csharp
app.MapPost(
    "/api/todos",
    async (
        CreateTodoRequest request,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        var command = mapper.Map<CreateTodoCommand>(request);
        var result = await mediator.Send(command, cancellationToken);

        return result.ToCreatedWithoutBody();
    }
);
```

Method that returns the service/command response directly:
```csharp
app.MapPost(
    "/api/todos",
    async (
        CreateTodoRequest request,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        var command = mapper.Map<CreateTodoCommand>(request);
        var result = await mediator.Send(command, cancellationToken);

        // or you can construct the URI like that: value => new Uri($"/api/todos/{value.Id}")
        return result.ToCreated(value => $"/api/todos/{value.Id}");
    }
);
```

Method that returns mapped result to the API contract model:
```csharp
app.MapPost(
    "/api/todos",
    async (
        CreateTodoRequest request,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        var command = mapper.Map<CreateTodoCommand>(request);
        var result = await mediator.Send(command, cancellationToken);

        // or you can construct the URI like that: value => new Uri($"/api/todos/{value.Id}")
        return result.ToCreated(value => $"/api/todos/{value.Id}", _mapper.Map<CreateTodoResponse>);
    }
);
```

### ToNoContent extension method

```csharp
app.MapDelete(
    "/api/todos/{id:int}",
    async (
        int id,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        var command = new DeleteTodoCommand(id);
        var result = await mediator.Send(command, cancellationToken);

        return result.ToNoContent();
    }
);
```

### ToFileStream extension method

[!IMPORTANT] 
You don't need to worry about disposing the IFileStreamResult.FileContent Stream,
because ASP.NET Core handles that for you under the hood when sending the HTTP response.
For the curious, that behavior is defined in the FileStreamHttpResult.ExecuteAsync() method.

```csharp
app.MapDelete(
    "/api/files/{id:int}",
    async (
        int id,
        ISender mediator,
        IMapper mapper,
        CancellationToken cancellationToken
    ) =>
    {
        // service/query should return a response that implements the IFileStreamResult (see below)
        var query = new GetFileQuery(id);
        var result = await mediator.Send(query, cancellationToken);

        // you can also pass some arguments:
        // return result.ToFileStream(enableRangeProcessing: true, entityTag: ...);
        return result.ToFileStream();
    }
);
```

If you want to use the ToFileStream() method, the result of the operation should
implement the IFileStreamResult:
```csharp
interface IFileStreamResult
{
    Stream FileContent { get; }
    string? ContentType { get; }
    string? DownloadFileName { get; }
    DateTimeOffset? LastModified { get; }
}
```

## Issues

If you encounter any bugs or have any suggestions for improvements, please [open an issue](https://github.com/matthewrosse/ErrorOrAspNetCoreExtensions/issues).


## License

This project is licensed under the [MIT License](https://github.com/matthewrosse/ErrorOrAspNetCoreExtensions/blob/main/LICENSE).