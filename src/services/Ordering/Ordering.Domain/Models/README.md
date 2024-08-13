# Anemic Domain Model & Rich Domain Model

## 1. Anemic Domain Model

### Đặc điểm:
- **Dữ liệu tập trung, logic phân tán**: Các lớp trong mô hình chỉ chứa dữ liệu và không chứa hoặc rất ít chứa logic nghiệp vụ. Các đối tượng domain thường là các đối tượng "POCO" (Plain Old CLR Object) hoặc "POJO" (Plain Old Java Object), chỉ có các thuộc tính (properties) và không có hoặc có rất ít phương thức (methods) xử lý logic.
- **Logic nằm ở bên ngoài**: Logic nghiệp vụ được xử lý chủ yếu ở các lớp dịch vụ hoặc lớp điều khiển (services or controllers), thay vì nằm trong chính các đối tượng domain.

### Ưu điểm:
- **Dễ hiểu và đơn giản**: Cấu trúc đơn giản, dễ hiểu, phù hợp với các hệ thống nhỏ hoặc đơn giản.
- **Dễ dàng duy trì và mở rộng**: Vì logic nghiệp vụ không bị ràng buộc trong các đối tượng domain, nên việc thay đổi hoặc mở rộng logic có thể thực hiện một cách độc lập.

### Nhược điểm:
- **Phân tán logic nghiệp vụ**: Logic nghiệp vụ bị phân tán khắp nơi trong hệ thống, điều này có thể gây khó khăn trong việc duy trì và đảm bảo tính toàn vẹn (integrity) của hệ thống.
- **Không tuân thủ nguyên tắc thiết kế hướng đối tượng**: Việc tách rời dữ liệu và logic làm cho mô hình không thực sự phản ánh đúng bản chất của đối tượng trong domain thực tế.

### Code Example:

```csharp
// Anemic Domain Model: chỉ chứa dữ liệu
public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}

// Service xử lý logic nghiệp vụ
public class OrderService
{
    public void ProcessOrder(Order order)
    {
        if (order.TotalAmount > 100)
        {
            ApplyDiscount(order);
        }
        // Các logic khác
    }

    private void ApplyDiscount(Order order)
    {
        order.TotalAmount -= 10;
    }
}
```

---

## 2. Rich Domain Model (còn gọi là Domain-Driven Design)

### Đặc điểm:
- **Logic gắn liền với dữ liệu**: Trong Rich Domain Model, các đối tượng domain không chỉ chứa dữ liệu mà còn bao gồm cả logic nghiệp vụ liên quan trực tiếp đến dữ liệu đó. Các phương thức (methods) trong đối tượng domain chịu trách nhiệm xử lý các hoạt động nghiệp vụ.
- **Domain tập trung**: Mọi logic nghiệp vụ liên quan đến một đối tượng cụ thể được đóng gói trong chính đối tượng đó. Các đối tượng domain tự quản lý và bảo vệ trạng thái của chúng.

### Ưu điểm:
- **Phản ánh đúng mô hình nghiệp vụ**: Rich Domain Model phản ánh chính xác hơn cách mà nghiệp vụ hoạt động trong thế giới thực, giúp cho mô hình phần mềm trở nên trực quan và gần gũi hơn với các chuyên gia nghiệp vụ (domain experts).
- **Tăng tính bảo trì và mở rộng**: Logic nghiệp vụ được tập trung trong các đối tượng, giúp việc bảo trì, mở rộng và tái sử dụng mã nguồn trở nên dễ dàng hơn. Nó cũng giúp tránh việc vi phạm các quy tắc nghiệp vụ do logic bị phân tán.

### Nhược điểm:
- **Phức tạp hơn**: Việc tập trung logic vào các đối tượng domain có thể làm cho hệ thống trở nên phức tạp hơn, đặc biệt khi có nhiều quy tắc nghiệp vụ cần xử lý.
- **Khó triển khai đối với các hệ thống đơn giản**: Với các hệ thống đơn giản, việc áp dụng Rich Domain Model có thể quá mức cần thiết, làm tăng chi phí phát triển và bảo trì.

### Code Example:
```csharp
// Rich Domain Model: chứa cả dữ liệu và logic nghiệp vụ
public class Order
{
    public int OrderId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public decimal TotalAmount { get; private set; }

    public Order(int orderId, DateTime orderDate, decimal totalAmount)
    {
        OrderId = orderId;
        OrderDate = orderDate;
        TotalAmount = totalAmount;
    }

    public void ProcessOrder()
    {
        if (TotalAmount > 100)
        {
            ApplyDiscount();
        }
    }

    private void ApplyDiscount()
    {
        TotalAmount -= 10;
    }
}
```
---

## So sánh giữa Anemic Domain Model và Rich Domain Model

| **Đặc điểm**                   | **Anemic Domain Model**                          | **Rich Domain Model**                         |
|--------------------------------|-------------------------------------------------|----------------------------------------------|
| **Logic nghiệp vụ**            | Phân tán, nằm ngoài đối tượng domain            | Tập trung, gắn liền với đối tượng domain      |
| **Phản ánh nghiệp vụ thực tế** | Không tốt, chỉ tập trung vào dữ liệu            | Tốt, mô hình hóa gần gũi với nghiệp vụ thực tế|
| **Độ phức tạp**                | Thấp, đơn giản hơn                              | Cao hơn, phức tạp hơn                         |
| **Dễ bảo trì**                 | Dễ bảo trì với hệ thống đơn giản                | Dễ bảo trì với hệ thống phức tạp              |
| **Tính toàn vẹn của hệ thống** | Có thể dễ bị vi phạm do logic phân tán          | Được đảm bảo tốt hơn nhờ logic tập trung      |

---

## Khi nào nên sử dụng

- **Anemic Domain Model**: Thường được sử dụng trong các hệ thống đơn giản, nơi mà logic nghiệp vụ không quá phức tạp và việc tách biệt logic khỏi dữ liệu giúp cho mã nguồn dễ hiểu và dễ bảo trì.
  
- **Rich Domain Model**: Phù hợp cho các hệ thống phức tạp, nơi mà việc gắn kết logic nghiệp vụ với dữ liệu giúp mô hình phản ánh chính xác nghiệp vụ thực tế, đồng thời giúp bảo vệ tính toàn vẹn của hệ thống.

- 
## Lời giải thích thêm:
- **Anemic Domain Model**: Trong ví dụ này, lớp `Order` chỉ đóng vai trò lưu trữ dữ liệu. Logic nghiệp vụ, như việc áp dụng giảm giá, được đặt bên ngoài lớp này, cụ thể là trong lớp `OrderService`.
- **Rich Domain Model**: Ngược lại, trong ví dụ này, lớp `Order` không chỉ lưu trữ dữ liệu mà còn tự xử lý logic nghiệp vụ. Điều này giúp đảm bảo rằng tất cả các quy tắc nghiệp vụ liên quan đến đối tượng `Order` được tập trung và dễ dàng bảo trì hơn.



# Primitive Obsession & Strongly Typed IDs in Domain-Driven Design (DDD)

## 1. Primitive Obsession

### Định nghĩa:
**Primitive Obsession** là một anti-pattern phổ biến trong thiết kế phần mềm, đặc biệt là trong các hệ thống lớn. Anti-pattern này xảy ra khi các kiểu dữ liệu nguyên thủy (primitive types) như `int`, `string`, `decimal`, v.v., được sử dụng một cách tràn lan để đại diện cho các khái niệm phức tạp hơn trong domain.

### Ví dụ về Primitive Obsession:
- Sử dụng `string` để đại diện cho các loại dữ liệu như `Email`, `PhoneNumber`, hoặc `Currency`, thay vì tạo ra các đối tượng domain cụ thể cho những loại dữ liệu này.
- Sử dụng `int` để đại diện cho `OrderId`, `UserId`, hoặc `ProductId`, thay vì tạo các kiểu dữ liệu riêng biệt.

### Hậu quả của Primitive Obsession:
- **Khó bảo trì**: Việc sử dụng các kiểu dữ liệu nguyên thủy có thể dẫn đến việc nhầm lẫn và lỗi không mong muốn, đặc biệt khi các giá trị này có ý nghĩa khác nhau trong các ngữ cảnh khác nhau.
- **Mất đi sự rõ ràng**: Khi code chỉ sử dụng các kiểu dữ liệu nguyên thủy, nó không phản ánh rõ ràng các khái niệm của domain, làm giảm khả năng đọc hiểu và làm việc của code.
- **Tăng rủi ro lỗi**: Sử dụng các kiểu dữ liệu nguyên thủy cho các giá trị có ý nghĩa đặc biệt có thể dẫn đến việc so sánh hoặc tính toán sai lầm.

### Ví dụ về Primitive Obsession:

```csharp
public class Order
{
    public int OrderId { get; set; }
    public string CustomerEmail { get; set; }

    public void SendConfirmationEmail()
    {
        // Logic gửi email xác nhận
    }
}
```

## 2. Strongly Typed IDs

### Định nghĩa:
**Strongly Typed IDs** là một kỹ thuật trong Domain-Driven Design (DDD) nhằm giải quyết vấn đề **Primitive Obsession** bằng cách tạo ra các kiểu dữ liệu riêng biệt cho các giá trị đặc thù trong domain. Thay vì sử dụng các kiểu dữ liệu nguyên thủy, các giá trị như `OrderId`, `UserId`, `Email`, v.v. sẽ được đại diện bằng các lớp hoặc cấu trúc riêng.

### Lợi ích của Strongly Typed IDs:
- **Tăng tính rõ ràng**: Sử dụng các kiểu dữ liệu riêng biệt giúp code trở nên rõ ràng hơn và dễ đọc hơn, vì mỗi kiểu dữ liệu thể hiện một khái niệm cụ thể trong domain.
- **Giảm rủi ro lỗi**: Việc tạo các kiểu dữ liệu riêng biệt giúp giảm thiểu nguy cơ sử dụng sai kiểu dữ liệu, vì mỗi loại ID hoặc giá trị đều có một lớp riêng với các hành vi và kiểm tra riêng.
- **Dễ bảo trì**: Khi cần thay đổi hoặc mở rộng, bạn chỉ cần sửa đổi trong lớp đại diện cho giá trị đó, thay vì phải tìm kiếm và sửa chữa tất cả các nơi sử dụng kiểu dữ liệu nguyên thủy.

### Ví dụ về Strongly Typed IDs:

```csharp
public class OrderId
{
    public int Value { get; }

    public OrderId(int value)
    {
        if (value <= 0) throw new ArgumentException("OrderId must be positive.");
        Value = value;
    }

    // Override Equals and GetHashCode for proper comparison
    public override bool Equals(object obj)
    {
        if (obj is OrderId other)
            return Value == other.Value;
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();
}

public class Email
{
    public string Address { get; }

    public Email(string address)
    {
        if (!IsValidEmail(address)) throw new ArgumentException("Invalid email address.");
        Address = address;
    }

    private bool IsValidEmail(string email)
    {
        // Implement email validation logic
        return true;
    }
}

public class Order
{
    public OrderId OrderId { get; private set; }
    public Email CustomerEmail { get; private set; }

    public Order(OrderId orderId, Email customerEmail)
    {
        OrderId = orderId;
        CustomerEmail = customerEmail;
    }

    public void SendConfirmationEmail()
    {
        // Logic gửi email xác nhận
    }
}

