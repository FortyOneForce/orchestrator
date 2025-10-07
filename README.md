# OrchestratR

[![NuGet](https://img.shields.io/nuget/v/FortyOne.OrchestratR.svg)](https://www.nuget.org/packages/FortyOne.OrchestratR/)
[![License](https://img.shields.io/github/license/FortyOneForce/orchestrator)](LICENSE)

A lightweight .NET library implementing the CQRS patterns. OrchestratR provides a clean architecture for organizing application business logic with support for .NET 8 and .NET 9.

## Features

- **CQRS Implementation** - Separate command and query paths with specialized interfaces
- **Interceptors** - Middleware-style interceptors for cross-cutting concerns
- **Event Publishing** - Publish and subscribe to events with multiple handlers
- **Dependency Injection** - Seamless integration with Microsoft's DI container
- **Assembly Scanning** - Automatic handler discovery and registration

## Installation
```
dotnet add package FortyOne.OrchestratR
```
For abstractions only:
```
dotnet add package FortyOne.OrchestratR.Abstractions
```

## Core Concepts

### Requests

OrchestratR provides three types of requests:

- **Commands** (`ICommand`, `ICommand<TResponse>`) - Write operations that modify state
- **Queries** (`IQuery<TResponse>`) - Read-only operations that retrieve data
- **Notifications** (`INotification`) - Events that can be handled by multiple subscribers

### Handlers

Each request type has a corresponding handler:

- `IRequestHandler<TRequest>` - For void-returning commands
- `IRequestHandler<TRequest, TResponse>` - For commands/queries that return data
- `INotificationHandler<TNotification>` - For notification subscribers

### Registration

Register OrchestratR with the dependency injection container:

```csharp
services.AddOrchestrator(configure => 
{ 
    // Register handlers from assembly 
    configure.RegisterServicesFromAssembly(typeof(Program).Assembly);

    // Add pipeline interceptors
    configure.AddRequestInterceptor(typeof(LoggingInterceptor<,>));

    // Configure handler lifetime
    configure.WithHandlerTypeLifetime(_ => ServiceLifetime.Scoped);
});
```

### Usage

Use the `IOrchestrator` interface to send requests:

```csharp
// Commands (write operations) 
await orchestrator.SendAsync(new CreateOrderCommand { ... });

// Queries (read operations) 
var result = await orchestrator.QueryAsync(new GetOrderQuery { ... });

// Notifications (events) 
await orchestrator.NotifyAsync(new OrderCreatedNotification { ... });
```


### Interception Pipeline

Add cross-cutting concerns with interceptors:
```csharp
public class LoggingInterceptor<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse> 
{ 
    public async Task<TResponse> HandleAsync( 
        TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken) 
    { 
        // Pre-processing logic 
        // var response = await next(); 
        // Post-processing logic return response; 
    } 
}
```
## License

This project is licensed under the MIT License - see the LICENSE file for details.