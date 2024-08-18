
# Interceptor

Entity Framework Core (EF Core) interceptors are a feature that allows you to intercept and customize the behavior of EF Core at different points during the execution of database operations. Interceptors provide hooks that can be used to monitor, log, modify, or completely replace the default behavior of EF Core.

### Key Concepts of EF Core Interceptors:

1. **Interception Points**:
   - **Connection Interception**: You can intercept and modify the behavior when a connection is being opened or closed.
   - **Command Interception**: This allows you to intercept SQL commands before they are executed against the database. You can modify, log, or completely cancel the execution of the command.
   - **Transaction Interception**: You can intercept the creation, commit, and rollback of transactions.
   - **SaveChanges Interception**: Intercepts calls to `SaveChanges` or `SaveChangesAsync` on the `DbContext`, allowing you to run code before or after the changes are saved.

2. **Interceptors vs. Logging**:
   - While EF Core has a built-in logging mechanism that allows you to log executed SQL commands and other database interactions, interceptors give you more control. With interceptors, you can not only observe what's happening but also modify or influence the behavior.

3. **How to Use EF Core Interceptors**:
   - **Creating an Interceptor**: Implement an interface provided by EF Core that corresponds to the type of interception you want to perform. For example, implement `IDbCommandInterceptor` to intercept database commands.
   - **Registering an Interceptor**: Once created, an interceptor needs to be registered with the `DbContext`. This can be done during the configuration of the `DbContext` in the `OnConfiguring` method or via dependency injection in ASP.NET Core.

### Example: Command Interceptor

Here’s an example of how to create a command interceptor that logs the SQL commands being executed:

```csharp
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class MyCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Debug.WriteLine($"Executing SQL: {command.CommandText}");
        return base.ReaderExecuting(command, eventData, result);
    }
    
    public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        Debug.WriteLine($"Executing SQL: {command.CommandText}");
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}
```

Then, register the interceptor in your `DbContext`:

```csharp
public class MyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(new MyCommandInterceptor())
            .UseSqlServer("YourConnectionString");
    }
}
```

- **In this Project**:

```csharp

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? dbContext)
    {
        if (dbContext == null)
        {
            return;
        }
        foreach (var entry in dbContext.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State == EntityState.Detached)
            {
                entry.Entity.CreateBy = "John Nguyen";
                entry.Entity.CreateAt = DateTime.UtcNow;
            }
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = "John Nguyen";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }
}
public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}


```
Then, register the interceptor in your `DbContext`:

```csharp

// Add services to the container.
services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
//services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

services.AddDbContext<OrderingDbContext>((sp, options) =>
{
    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
    options.UseSqlServer(connectionString);
});

```

### When to Use Interceptors

Interceptors are useful in scenarios where you need to:
- **Log or Audit**: Capture all SQL commands for auditing or logging purposes.
- **Modify Commands**: Change the SQL generated by EF Core, for instance, to add hints or modify queries dynamically.
- **Enforce Security**: Add security checks or modify queries based on user roles or permissions.
- **Custom Error Handling**: Handle specific exceptions or errors in a customized way.

### Conclusion

EF Core interceptors are powerful tools for developers needing fine-grained control over the database operations performed by EF Core. They provide a flexible mechanism to hook into and alter EF Core's behavior at runtime.



-----------------------


# Domain Event In DDD

***Part 1***

In Domain-Driven Design (DDD), **Domain Events** are an essential pattern used to model business logic. They represent something that has happened in the domain that is of interest to the business or other parts of the system. Domain Events help to decouple different parts of the system, enabling them to react to changes in the domain in a more loosely coupled manner.

### Key Concepts of Domain Events in DDD:

#### 1. **What is a Domain Event?**
   - A **Domain Event** is a record of something that happened in the domain. It is a first-class citizen in the domain model, meaning it is not just a side effect but an essential part of the model itself.
   - For example, in an e-commerce system, "OrderPlaced," "PaymentProcessed," or "ProductOutOfStock" could be domain events.

#### 2. **Characteristics of Domain Events:**
   - **Past Tense**: Domain events are named in the past tense because they represent something that has already happened.
   - **Immutable**: Once created, a domain event's state should not change. It captures the state of the domain at the moment the event occurred.
   - **Business-Relevant**: The event should reflect a significant change or action in the domain that stakeholders care about.

#### 3. **When to Use Domain Events?**
   - When a specific change in the domain model is significant and other parts of the system need to be aware of it.
   - To enforce consistency across aggregates or bounded contexts in a distributed system.
   - To trigger side effects in response to state changes in the domain model, such as sending an email, updating a read model, or integrating with another bounded context.

#### 4. **Implementing Domain Events:**
   - **Define the Event**: Create a class that represents the domain event, typically including details of what happened and the relevant data.

     **Example of a Domain Event Class**:
     ```csharp
     public class OrderPlacedEvent : IDomainEvent
     {
         public Guid OrderId { get; }
         public DateTime OccurredOn { get; }

         public OrderPlacedEvent(Guid orderId)
         {
             OrderId = orderId;
             OccurredOn = DateTime.UtcNow;
         }
     }
     ```

   - **Raise the Event**: The domain event is raised in the aggregate root when a significant change occurs.

     **Raising a Domain Event in an Aggregate Root**:
     ```csharp
     public class Order : AggregateRoot
     {
         public Guid Id { get; private set; }
         public DateTime OrderDate { get; private set; }

         public void PlaceOrder()
         {
             // Business logic for placing the order

             // Raise the domain event
             var orderPlacedEvent = new OrderPlacedEvent(this.Id);
             AddDomainEvent(orderPlacedEvent);
         }
     }
     ```

   - **Handle the Event**: Other parts of the system, such as other aggregates or external services, can react to the domain event.

     **Example Event Handler**:
     ```csharp
     public class OrderPlacedEventHandler : IDomainEventHandler<OrderPlacedEvent>
     {
         public void Handle(OrderPlacedEvent domainEvent)
         {
             // Handle the event (e.g., send an email, update a read model)
         }
     }
     ```

#### 5. **Domain Event Dispatching:**
   - Domain events can be dispatched immediately or stored (often alongside the aggregate) and dispatched later. This can be done synchronously or asynchronously depending on the requirements.

   **Example of Synchronous Dispatching**:
   ```csharp
   public void DispatchEvents(AggregateRoot aggregate)
   {
       var domainEvents = aggregate.GetDomainEvents();
       foreach (var domainEvent in domainEvents)
       {
           // Dispatch to handlers
           _domainEventDispatcher.Dispatch(domainEvent);
       }
   }
   ```

#### 6. **Benefits of Domain Events:**
   - **Decoupling**: Domain events decouple the parts of your system by allowing different components to react to changes without tightly coupling them.
   - **Consistency**: They help maintain consistency across different parts of the domain, especially in distributed systems.
   - **Business Logic Encapsulation**: Domain events encapsulate business logic, making it easier to understand and modify when needed.

#### 7. **Considerations:**
   - **Performance**: Depending on how events are handled (synchronously or asynchronously), they can introduce latency.
   - **Complexity**: Using domain events adds complexity to the system architecture, especially in managing and dispatching events.
   - **Error Handling**: Proper error handling strategies must be in place, especially in distributed environments.

### Conclusion
Domain Events are a powerful tool in Domain-Driven Design, allowing you to model business processes more naturally and decouple different parts of your system. By capturing and reacting to significant events in your domain, you can build more robust, maintainable, and scalable systems. However, careful consideration should be given to the complexity they introduce, particularly in distributed systems.


---------------------
***Part 2***

## Dispatch Domain Event w/EF Core SaveChangesInterceptor

### Steps disparch Events:
- Create a SaveChangesInterceptor
- Identify and Dispatch Domain Events


```csharp



```
- Dependency Injection:
```csharp



```