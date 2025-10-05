# OrchestratR

# OrchestratR

[![NuGet](https://img.shields.io/nuget/v/FortyOne.OrchestratR.svg)](https://www.nuget.org/packages/FortyOne.OrchestratR/)
[![License](https://img.shields.io/github/license/FortyOneForce/orchestrator)](LICENSE)

OrchestratR is a lightweight .NET library implementing the mediator and CQRS patterns, providing a clean and efficient way to organize your application's business logic.

## Features

- **CQRS Implementation**: Separate command and query paths with specialized interfaces
- **Pipeline Behavior**: Middleware-style interceptors for cross-cutting concerns
- **Event Publishing**: Publish and subscribe to events with multiple handlers
- **Dependency Injection**: Seamless integration with Microsoft's DI container
- **Assembly Scanning**: Automatic handler discovery and registration
- **Modern .NET Support**: Built for .NET 8 and .NET 9

## Installation

```
dotnet add package FortyOne.OrchestratR
```

If you only need the abstractions:
```
dotnet add package FortyOne.OrchestratR.Abstractions
```


## Quick Start

### 1. Define your requests and handlers
```csharp
// Command with no return value 

public class CreateOrderCommand : IActionRequest 
{ 
  public string CustomerId { get; set; } = null!; 
  public List<OrderItem> Items { get; set; } = new(); 
}

public class CreateOrderHandler : IActionHandler<CreateOrderCommand> 
{ 
  public async Task HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken) 
  { 
    // Implementation for creating an order 
    await _repository.CreateOrderAsync(command.CustomerId, command.Items, cancellationToken); 
  } 
}

// Query with return value 
public class GetOrderQuery : IActionRequest<OrderDto> 
{ 
  public string OrderId { get; set; } = null!; 
}

public class GetOrderHandler : IActionHandler<GetOrderQuery, OrderDto> 
{ 
  public async Task<OrderDto> HandleAsync(GetOrderQuery query, CancellationToken cancellationToken) 
  { 
    // Implementation for retrieving order data 
    var order = await _repository.GetOrderByIdAsync(query.OrderId, cancellationToken); 
    return _mapper.Map<OrderDto>(order); 
  } 
}
```


### 2. Define your events and handlers

```csharp
public class OrderCreatedEvent : IEventSignal 
{ 
  public string OrderId { get; set; } = null!; 
  public string CustomerId { get; set; } = null!; 
}

public class EmailNotificationHandler : IEventHandler<OrderCreatedEvent> 
{ 
  public async Task HandleAsync(OrderCreatedEvent signal, CancellationToken cancellationToken) 
  { 
    await _emailService.SendOrderConfirmationAsync(signal.CustomerId, signal.OrderId, cancellationToken); 
  } 
}

public class AnalyticsHandler : IEventHandler<OrderCreatedEvent> 
{ 
  public async Task HandleAsync(OrderCreatedEvent signal, CancellationToken cancellationToken) 
  { 
    await _analyticsService.TrackOrderCreatedAsync(signal.OrderId, cancellationToken); 
  } 
}
```

### 3. Configure services
```csharp
services.AddOrchestrator(options => 
  { 
    // Register all handlers in the assembly 
    options.RegisterHandlersFromAssembly(typeof(Program).Assembly);

    // Add pipeline interceptors
    options.UseActionExecutionInterceptor(typeof(LoggingInterceptor<,>));
  });
```