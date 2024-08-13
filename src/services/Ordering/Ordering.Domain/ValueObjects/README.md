## Value Object trong Thiết Kế Entity

### Định nghĩa:
**Value Object** là một khái niệm quan trọng trong Domain-Driven Design (DDD). Khác với Entity, Value Object không có danh tính (identity) riêng. Chúng được định nghĩa hoàn toàn dựa trên các giá trị mà chúng chứa đựng. Hai Value Object được coi là bằng nhau nếu tất cả các thuộc tính của chúng bằng nhau.

### Đặc điểm của Value Object:
- **Không có Identity**: Không giống như Entity, Value Object không có một ID duy nhất để phân biệt. Điều này có nghĩa là hai Value Object có thể được coi là giống nhau nếu chúng có các giá trị giống nhau.
- **Bất biến**: Value Object thường là bất biến sau khi được tạo ra. Bất kỳ sự thay đổi nào trong các giá trị của chúng đều dẫn đến việc tạo ra một Value Object mới.
- **So sánh bằng giá trị**: Value Object được so sánh dựa trên các thuộc tính của chúng, thay vì so sánh dựa trên danh tính như Entity.

### Khi nào nên sử dụng Value Object:
- Khi bạn cần biểu diễn các khái niệm đơn giản trong domain mà không yêu cầu ID duy nhất.
- Khi các thuộc tính của đối tượng không cần thay đổi sau khi được khởi tạo.
- Khi bạn muốn tận dụng tính bất biến để đảm bảo tính toàn vẹn dữ liệu và tránh lỗi do thay đổi trạng thái.

### Ví dụ về Value Object:

```csharp
public class Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }

    public Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    // Override Equals and GetHashCode for proper comparison
    public override bool Equals(object obj)
    {
        if (obj is Address other)
        {
            return Street == other.Street &&
                   City == other.City &&
                   State == other.State &&
                   ZipCode == other.ZipCode;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode);
    }
}

// Sử dụng
public class Customer
{
    public int Id { get; }
    public string Name { get; }
    public Address Address { get; }  // Address là một Value Object

    public Customer(int id, string name, Address address)
    {
        Id = id;
        Name = name;
        Address = address;
    }
}


```

### Lợi ích của Value Object:
- Đơn giản hóa code: Việc sử dụng Value Object giúp giảm bớt sự phức tạp của hệ thống bằng cách gom các giá trị liên quan lại với nhau.
- Bất biến: Giúp tránh các vấn đề liên quan đến thay đổi trạng thái của đối tượng, làm tăng tính an toàn và đáng tin cậy của hệ thống.
- Tái sử dụng: Value Object có thể dễ dàng được tái sử dụng trong các phần khác nhau của ứng dụng mà không cần lo lắng về việc thay đổi không mong muốn.

### Chú ý trong việc Configuration khi tạo bảng trong cơ sở dữ liệu

# Cấu Hình Value Object Trong Entity Framework Core

## 1. Định Nghĩa Value Object

Đầu tiên, bạn cần định nghĩa lớp **Value Object** của mình. Value Object này thường bao gồm các thuộc tính cơ bản và không có ID riêng.

```csharp
public class Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }

    // Constructor
    public Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    // Override Equals and GetHashCode for proper comparison
    public override bool Equals(object obj)
    {
        if (obj is Address other)
        {
            return Street == other.Street &&
                   City == other.City &&
                   State == other.State &&
                   ZipCode == other.ZipCode;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode);
    }
}
```

## 2. Sử Dụng Value Object trong Entity

- Tiếp theo, bạn sử dụng Value Object này trong một Entity. Entity sẽ chứa Value Object như một thuộc tính:

```csharp
public class Customer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }  // Address là một Value Object

    public Customer(string name, Address address)
    {
        Name = name;
        Address = address;
    }
}
```


## 3. Cấu Hình Value Object Trong `DbContext`

- Khi sử dụng Entity Framework Core, bạn cần cấu hình cách mà Entity Framework ánh xạ các thuộc tính của Value Object vào cơ sở dữ liệu. Điều này được thực hiện trong phương thức `OnModelCreating` của lớp `DbContext`.

- Dưới đây là ví dụ về cách cấu hình Value Object `Address` trong `DbContext`:

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình Address là một Value Object
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street");
                address.Property(a => a.City).HasColumnName("City");
                address.Property(a => a.State).HasColumnName("State");
                address.Property(a => a.ZipCode).HasColumnName("ZipCode");
            });
        });
        // hoac su dun cai nay
            builder.ComplexProperty(
                o => o.ShippingAddress, addressBuilder =>
                {
                    addressBuilder.Property(a => a.FirstName)
                        .HasMaxLength(50)
                        .IsRequired();

                    addressBuilder.Property(a => a.LastName)
                        .HasMaxLength(50)
                        .IsRequired();

                    addressBuilder.Property(a => a.EmailAddress)
                        .HasMaxLength(50);

                    addressBuilder.Property(a => a.AddressLine)
                        .HasMaxLength(180)
                        .IsRequired();

                    addressBuilder.Property(a => a.Country)
                        .HasMaxLength(50);

                    addressBuilder.Property(a => a.State)
                        .HasMaxLength(50);

                    addressBuilder.Property(a => a.ZipCode)
                        .HasMaxLength(5)
                        .IsRequired();
                });
    }
}
```

## 4. Sử Dụng `OwnsOne` Để Ánh Xạ Value Object

- Trong Entity Framework Core, để ánh xạ một Value Object vào cơ sở dữ liệu, bạn sử dụng phương thức `OwnsOne`. Phương thức này cho phép bạn chỉ định rằng một thuộc tính trong Entity là một Value Object và các thuộc tính của Value Object sẽ được ánh xạ thành các cột riêng trong bảng của Entity.

- Dưới đây là ví dụ về cách sử dụng `OwnsOne` để ánh xạ Value Object `Address` trong Entity `Customer`:

```csharp
modelBuilder.Entity<Customer>(entity =>
{
    entity.OwnsOne(e => e.Address, address =>
    {
        address.Property(a => a.Street).HasColumnName("Street");
        address.Property(a => a.City).HasColumnName("City");
        address.Property(a => a.State).HasColumnName("State");
        address.Property(a => a.ZipCode).HasColumnName("ZipCode");
    });
});
```

## 5. Tạo Migration và Áp Dụng Vào Cơ Sở Dữ Liệu

- Sau khi cấu hình Value Object trong `DbContext`, bạn cần tạo và áp dụng migration để cập nhật cấu trúc cơ sở dữ liệu. Migration sẽ tạo ra các lệnh SQL cần thiết để tạo hoặc cập nhật các bảng trong cơ sở dữ liệu theo cấu hình đã định.

### 5.1 Tạo Migration

- Sử dụng lệnh sau để tạo một migration mới. Migration này sẽ chứa các thay đổi cần thiết cho cơ sở dữ liệu dựa trên cấu hình hiện tại trong `DbContext`.

```
dotnet ef migrations add AddCustomerAddress
```

### 5.2 Áp Dụng Migration
- Sau khi tạo migration, bạn cần áp dụng nó vào cơ sở dữ liệu để cập nhật cấu trúc bảng theo các thay đổi đã định. Sử dụng lệnh sau để thực hiện việc này:

```
dotnet ef database update
```

## 6. Kết Quả Cấu Trúc Bảng

- Sau khi áp dụng migration, cấu trúc bảng trong cơ sở dữ liệu sẽ được cập nhật để phản ánh các thay đổi cấu hình cho Value Object. Trong ví dụ này, bảng `Customers` sẽ bao gồm các cột tương ứng với các thuộc tính của Value Object `Address`.

- Dưới đây là cấu trúc bảng `Customers` sau khi áp dụng migration:

| Id  | Name  | Street   | City   | State | ZipCode |
|-----|-------|----------|--------|-------|---------|
| 1   | John  | 123 Main | Seattle| WA    | 98101   |

- **Id**: Khóa chính của bảng.
- **Name**: Tên của khách hàng.
- **Street**: Địa chỉ đường phố từ Value Object `Address`.
- **City**: Thành phố từ Value Object `Address`.
- **State**: Bang từ Value Object `Address`.
- **ZipCode**: Mã bưu điện từ Value Object `Address`.

Với cấu trúc này, dữ liệu từ Value Object `Address` được lưu trữ trong các cột riêng biệt trong bảng `Customers`, giúp tổ chức dữ liệu một cách rõ ràng và có thể truy vấn dễ dàng.

