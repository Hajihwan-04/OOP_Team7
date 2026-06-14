namespace cafe;

public enum OrderStatus
{
    Completed, // 주문처리
    Canceled // 취소
}

public enum OrderType
{
    InStore, // 매장
    TakeOut, // 포장
    Delivery // 배달
}

public class OrderLine : IDisplayable, IPriced, IStockUsable
{
    private Menu menu;
    private int quantity;
    private int unitPrice; // 주문 당시의 가격 (메뉴 가격 변경후 정산 대비)

    public Menu Menu => menu;
    public int Quantity => quantity;
    public int UnitPrice => unitPrice;

// 주문 상세
    public OrderLine(Menu menu, int quantity)
    {
        if (menu == null)
        {
            throw new InvalidOrderException("주문 메뉴를 입력해야 합니다.");
        }

        if (quantity <= 0)
        {
            throw new InvalidOrderException("주문 수량은 1개 이상이어야 합니다.");
        }

        this.menu = menu;
        this.quantity = quantity;

        unitPrice = menu.HowMuch(); // 지금 메뉴 단가
    }

    public int TotalPrice()
    {
        return unitPrice * quantity;
    }

// 재료 계산
    public Dictionary<string, int> RequiredIngredients()
    {
        Dictionary<string, int> result = new();

        foreach (var item in menu.RequiredIngredients())
        {
            result[item.Key] = item.Value * quantity;
        }

        return result;
    }

    public string Info()
    {
        return $"{menu.Name} x {quantity}개, 단가: {unitPrice}원, 합계: {TotalPrice()}원";
    }
}
// 메인 오더 클래스
public class Order : IDisplayable, IPriced
{
    private int orderNumber;
    private Customer customer;
    private List<OrderLine> lines;
    private OrderStatus status;
    private OrderType ordertype;
    private bool useDisposable;
    private bool useTumbler;
    private Payment? payment; // 결제 아직 안됐을 수 있어서 null 허용
    private DateTime orderedAt;

    public int OrderNumber => orderNumber;
    public Customer Customer => customer;
    public IReadOnlyList<OrderLine> Lines => lines;
    public OrderStatus Status => status;
    public OrderType Type => ordertype;
    public bool UseDisposable => useDisposable;
    public bool UseTumbler => useTumbler;
    public Payment? Payment => payment;
    public DateTime OrderedAt => orderedAt;

    public int LineCount => lines.Count;
    public int MenuCount => TotalQuantity();

    public Order(int orderNumber, Customer customer, OrderType ordertype)
    {
        this.orderNumber = orderNumber;
        this.customer = customer;
        this.ordertype = ordertype;

        lines = new List<OrderLine>();
        status = OrderStatus.Completed;
        useDisposable = false;
        useTumbler = false;
        payment = null;
        orderedAt = DateTime.Now;
    }

// 인덱서. OrderLine 리스트에 접근할 수 있도록
    public OrderLine this[int index]
    {
        get
        {
            return lines[index];
        }
    }

// 수량 안넣으면 걍 1개
    public void AddMenu(Menu menu)
    {
        AddMenu(menu, 1);
    }

    public void AddMenu(Menu menu, int quantity)
    {
        OrderLine line = new OrderLine(menu, quantity);
        lines.Add(line);
    }

    public bool RemoveLineAt(int index)
    {
        if (index < 0 || index >= lines.Count)
        {
            return false;
        }

        lines.RemoveAt(index);
        return true;
    }

    public bool CancelOrder(int orderNumber)
    {
        if (this.orderNumber == orderNumber && status != OrderStatus.Canceled)
        {
            status = OrderStatus.Canceled;
            return true;
        }

        return false;
    }

    public void SetUseDisposable(bool useDisposable)
    {
        if (useDisposable)
        {
            useTumbler = false;
        }

        this.useDisposable = useDisposable;
    }

    public void SetUseTumbler(bool useTumbler)
    {
        if (useTumbler)
        {
            useDisposable = false;
        }

        this.useTumbler = useTumbler;
    }

    public int TotalQuantity()
    {
        int quantity = 0;

        foreach (var line in lines)
        {
            quantity += line.Quantity;
        }

        return quantity;
    }

    public int MenuPrice()
    {
        int menuprice = 0;

        foreach (var line in lines)
        {
            menuprice += line.TotalPrice();
        }

        return menuprice;
    }

    public int OptionPrice()
    {
        int optionprice = 0;

        if (ordertype == OrderType.Delivery)
        {
            optionprice += 3000;
        }

        if (useDisposable)
        {
            optionprice += 100 * TotalQuantity();
        }

        if (useTumbler)
        {
            optionprice -= 500 * TotalQuantity();
        }

        return optionprice;
    }

    public int TotalPrice()
    {
        int total = MenuPrice() + OptionPrice();

        if (total < 0)
        {
            return 0;
        }

        return total;
    }

    public bool Pay(Payment payment)
    {
        if (payment.TotalPrice() != TotalPrice())
        {
            throw new InvalidOrderException("결제 금액이 주문 금액과 다릅니다.");
        }

        bool result = payment.Pay();

        if (result)
        {
            this.payment = payment;
        }

        return result;
    }

// 한글
    public string TypeText()
    {
        if (ordertype == OrderType.TakeOut)
        {
            return "포장";
        }

        if (ordertype == OrderType.Delivery)
        {
            return "배달";
        }

        return "매장";
    }

    public string Info()
    {
        return $"주문번호: {orderNumber}, 일시: {orderedAt:yyyy-MM-dd HH:mm}, 유형: {TypeText()}, 수량: {TotalQuantity()}개, 상태: {status}, 금액: {TotalPrice()}원";
    }

// 한글
    private string UseText(bool value)
    {
        return value ? "사용" : "미사용";
    }

    public void ShowOrder()
    {
        Console.WriteLine($"[주문 번호: {orderNumber}]");
        Console.WriteLine($"기록 일시: {orderedAt:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"고객: {customer.Name}");
        Console.WriteLine($"주문 유형: {TypeText()}");
        Console.WriteLine($"주문 상태: {status}");
        Console.WriteLine($"일회용품 사용 여부: {UseText(useDisposable)}");
        Console.WriteLine($"텀블러 사용 여부: {UseText(useTumbler)}");
        Console.WriteLine("주문 상세:");

        foreach (var line in lines)
        {
            Console.WriteLine($" - {line.Info()}");
        }

        Console.WriteLine($"총 판매 수량: {TotalQuantity()}개");
        Console.WriteLine($"메뉴 금액: {MenuPrice()}원");
        Console.WriteLine($"옵션 금액: {OptionPrice()}원");
        Console.WriteLine($"총 가격: {TotalPrice()}원");

        if (payment != null)
        {
            Console.WriteLine(payment.Info());
        }
    }
}
