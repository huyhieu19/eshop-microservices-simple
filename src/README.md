
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

--------------------------------

# Docker

## Configure Docker for SQL server

[Visit microsoft learn](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-docker-container-configure?view=sql-server-ver16&pivots=cs1-bash)




### docker-compose.yml

```

  orderdb:
    image: mcr.microsoft.com/mssql/server

```

### docker-compose.override.yml

```

  orderdb:
    container_name: orderdb
    environment:
        - ACCEPT_EULA=Y
        - SA_PASSWORD=SwN12345678
    restart: always
    ports:
        - "1433:1433"

```

### Configure Docker for 


### docker-compose.yml

### docker-compose.override.yml