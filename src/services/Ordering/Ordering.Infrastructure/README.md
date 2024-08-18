
## Bước config
### 1. NuGet Package
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer
- dotnet add package Microsoft.EntityFrameworkCore.Tools
- dotnet add package Microsoft.EntityFrameworkCore.Design

### 2. Cấu Hình Chuỗi Kết Nối (Connection String)
- Chuỗi kết nối là thông tin về cách ứng dụng kết nối tới cơ sở dữ liệu. Bạn cần cấu hình nó trong appsettings.json hoặc trực tiếp trong lớp DbContext.
- Ví dụ cấu hình trong appsettings.json:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;"
  }
}
```

- Cấu hình chuỗi kết nối trong DbContext:

```
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MyEntity> MyEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        }
    }
}

```

### 2.1: Register EF Core DBContext to Asp.Net
- Register into DependenceInjection.cs

``` csharp
services.AddDbContext<OrderingDbContext>((sp, options) =>
{
    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
    options.UseSqlServer(connectionString);
});
```


### 3. Tạo Migration
- Sau khi đã cấu hình xong DbContext, bạn có thể tạo migration bằng cách sử dụng CLI.
```
dotnet ef migrations add InitialCreate
```
- Lệnh trên sẽ tạo một thư mục Migrations trong dự án của bạn với các tệp tin chứa mã code để tạo và cập nhật cơ sở dữ liệu.

- Command create Migration folder direction to choose folder.

```
Add-Migration InitialCreate -OutputDir <folder address (Data/Migrations)> -<Project Ordering.Infrastructure(name Project)> -<StartupProject Odering.API(name Project API)>


Add-Migration InitialCreate -OutputDir Data/Migrations -Project Ordering.Infrastructure -StartupProject Ordering.API
```

### 4. Áp Dụng Migration Vào Database
- Để áp dụng migration vào cơ sở dữ liệu, bạn có thể sử dụng lệnh sau:
```
dotnet ef database update
```
- Lệnh này sẽ áp dụng tất cả các migration chưa được áp dụng vào cơ sở dữ liệu của bạn.

### 5. Cấu Hình Thêm Trong DbContext
- Trong một số trường hợp, bạn có thể cần cấu hình thêm trong DbContext để xử lý các kịch bản phức tạp hơn.

```
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Cấu hình khóa chính
    modelBuilder.Entity<MyEntity>()
        .HasKey(e => e.Id);

    // Cấu hình mối quan hệ
    modelBuilder.Entity<MyEntity>()
        .HasOne(e => e.RelatedEntity)
        .WithMany(r => r.MyEntities)
        .HasForeignKey(e => e.RelatedEntityId);

    base.OnModelCreating(modelBuilder);
}

```

### 6. Migration và Seed Dữ Liệu
- Nếu bạn muốn seed dữ liệu ban đầu vào cơ sở dữ liệu sau khi thực hiện migration, bạn có thể sử dụng phương thức OnModelCreating hoặc tạo riêng một lớp DbInitializer để seed dữ liệu.

- Seed dữ liệu trong OnModelCreating:
```
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<MyEntity>().HasData(
        new MyEntity { Id = 1, Name = "Seed Data" }
    );
}
```
### 7. Kiểm Tra Và Quản Lý Migration
- Bạn có thể kiểm tra các migration hiện có và quản lý chúng bằng các lệnh:

- Xem danh sách migration:

``` 
dotnet ef migrations list 
```

- Loại bỏ migration cuối cùng:

```
dotnet ef migrations remove
```


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



```
Then, register the interceptor in your `DbContext`:

```csharp



```

### When to Use Interceptors

Interceptors are useful in scenarios where you need to:
- **Log or Audit**: Capture all SQL commands for auditing or logging purposes.
- **Modify Commands**: Change the SQL generated by EF Core, for instance, to add hints or modify queries dynamically.
- **Enforce Security**: Add security checks or modify queries based on user roles or permissions.
- **Custom Error Handling**: Handle specific exceptions or errors in a customized way.

### Conclusion

EF Core interceptors are powerful tools for developers needing fine-grained control over the database operations performed by EF Core. They provide a flexible mechanism to hook into and alter EF Core's behavior at runtime.