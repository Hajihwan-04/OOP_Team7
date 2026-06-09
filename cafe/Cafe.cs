namespace cafe;

// 메인
public class Cafe
{
    // 델리게이트
    public delegate void OrderRecordedHandler(Order order);

    // 주문 기록 저장
    public event OrderRecordedHandler? OrderRecorded;

    public string Name { get; set; }
    public string Address { get; set; }
    public int BranchNumber { get; set; } // 지점 번호
    public List<Menu> Menus { get; } = new();
    public List<Employee> Employees { get; } = new();
    public List<Order> Orders { get; } = new();
    public InventoryManager Inventory { get; } = new();

    private int totalSales;
    private int totalCost;

    public int TotalSales => totalSales;
    public int TotalCost => totalCost;

    public Cafe(string name, int branchNumber, string address)
    {
        Name = name;
        BranchNumber = branchNumber;
        Address = address;
        totalSales = 0;
        totalCost = 0;
    }

    public void AddMenu(Menu menu)
    {
        if (menu == null)
        {
            throw new ArgumentNullException(nameof(menu), "메뉴를 추가할 수 없습니다.");
        }

        Menus.Add(menu);
    }

    public void ShowMenus()
    {
        Console.WriteLine("[메뉴 목록]");

        if (Menus.Count == 0)
        {
            Console.WriteLine(" - 등록된 메뉴가 없습니다.");
            return;
        }

        var sortedMenus = Menus.OrderBy(menu => menu.HowMuch()).ToList(); // 가격 기준 정렬

        for (int i = 0; i < sortedMenus.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {sortedMenus[i].Info()}");
        }
    }

    public Menu? FindMenuByNumber(int menuNumber)
    {
        int index = menuNumber - 1;
        var sortedMenus = Menus.OrderBy(menu => menu.HowMuch()).ToList();

        if (index < 0 || index >= sortedMenus.Count)
        {
            return null;
        }

        return sortedMenus[index];
    }

    public bool TryFindMenuByName(string menuName, out Menu? menu)
    {
        return SearchHelper.TryFind(Menus, currentMenu => currentMenu.Name == menuName, out menu);
    }

    public void HireEmployee(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee), "직원을 추가할 수 없습니다.");
        }

        Employees.Add(employee);
    }

    public bool TryFindEmployee(string name, out Employee? employee)
    {
        return SearchHelper.TryFind(Employees, currentEmployee => currentEmployee.Name == name, out employee);
    }

    public bool RemoveEmployee(string name)
    {
        if (!TryFindEmployee(name, out Employee? employee) || employee == null)
        {
            Console.WriteLine($"{name} 직원을 찾을 수 없습니다.");
            return false;
        }

        Employees.Remove(employee);
        Console.WriteLine($"{employee.Name} 직원 정보를 삭제했습니다.");
        return true;
    }

    public void ShowEmployees()
    {
        Console.WriteLine("[직원 목록]");

        if (Employees.Count == 0)
        {
            Console.WriteLine(" - 직원이 없습니다.");
            return;
        }

        foreach (var employee in Employees)
        {
            Console.WriteLine($" - {employee.Info()}");
        }
    }

    private void AddOrder(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order), "주문을 추가할 수 없습니다.");
        }

        Orders.Add(order);
        AddSales(order.TotalPrice());
        OrderRecorded?.Invoke(order);
    }

    public void ShowOrders()
    {
        Console.WriteLine("[주문 기록]");

        if (Orders.Count == 0)
        {
            Console.WriteLine(" - 저장된 주문 기록이 없습니다.");
            return;
        }

        foreach (var order in Orders.OrderByDescending(order => order.OrderNumber))
        {
            Console.WriteLine($" - {order.Info()}");
        }
    }

    public bool CancelOrder(int orderNumber)
    {
        Order? order = Orders.FirstOrDefault(order => order.OrderNumber == orderNumber);

        if (order == null)
        {
            Console.WriteLine($"{orderNumber}번 주문 기록을 찾을 수 없습니다.");
            return false;
        }

        if (!order.CancelOrder(orderNumber))
        {
            Console.WriteLine($"{orderNumber}번 주문은 이미 취소된 기록입니다.");
            return false;
        }

        totalSales -= order.TotalPrice();
        Console.WriteLine($"{orderNumber}번 주문 기록을 취소했습니다.");
        return true;
    }

    private void AddSales(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("매출은 음수로 추가할 수 없습니다.");
        }

        totalSales += amount;
    }

    private void AddCost(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("비용은 음수로 추가할 수 없습니다.");
        }

        totalCost += amount;
    }

    public int Profit()
    {
        return totalSales - totalCost;
    }

    public void BuyStock(string ingredient, int amount, int unitPrice)
    {
        if (amount <= 0)
        {
            Console.WriteLine("구매 수량은 1개 이상이어야 합니다.");
            return;
        }

        if (unitPrice <= 0)
        {
            Console.WriteLine("구매 단가는 1원 이상이어야 합니다.");
            return;
        }

        int cost = unitPrice * amount;

        AddCost(cost);
        Inventory.SetIngredientPrice(ingredient, unitPrice);
        Inventory.AddStock(ingredient, amount);
        Console.WriteLine($"{ingredient} 재고 {amount}개를 구매했습니다. 비용: {cost}원");
    }

    public int WorkingEmployeeCount()
    {
        int count = 0;

        foreach (var employee in Employees)
        {
            if (employee.IsWorking())
            {
                count++;
            }
        }

        return count;
    }

    public void ShowEmployeeWorkStatus()
    {
        Console.WriteLine("[현재 직원 근무 상태]");

        if (Employees.Count == 0)
        {
            Console.WriteLine(" - 직원이 없습니다.");
            return;
        }

        foreach (var employee in Employees)
        {
            Console.WriteLine($" - {employee.Name}: {employee.GetWorkingStatusText(DateTime.Now)}");
        }
    }

    private bool CanMake(OrderLine line)
    {
        foreach (var item in line.RequiredIngredients())
        {
            if (!Inventory.HasStock(item.Key, item.Value))
            {
                return false;
            }
        }

        return true;
    }

    private void UseIngredients(OrderLine line)
    {
        foreach (var item in line.RequiredIngredients())
        {
            Inventory.UseStock(item.Key, item.Value);
        }
    }

    public bool ProcessOrder(Order order)
    {
        if (order.LineCount == 0)
        {
            throw new InvalidOrderException("메뉴가 없습니다.");
        }

        if (order.Payment == null)
        {
            throw new InvalidOrderException("결제수단을 기록해주세요.");
        }

        foreach (var line in order.Lines)
        {
            if (!CanMake(line))
            {
                throw new StockShortageException($"{line.Menu.Name} 재고가 부족합니다.");
            }
        }

        foreach (var line in order.Lines)
        {
            UseIngredients(line);
        }

        AddOrder(order);
        return true;
    }

    public void ShowCafeStatus()
    {
        Console.WriteLine($"[{Name} 카페 상태]");
        Console.WriteLine($"주소: {Address}");
        Console.WriteLine($"지점 번호: {BranchNumber}");
        Console.WriteLine($"메뉴 수: {Menus.Count}");
        Console.WriteLine($"직원 수: {Employees.Count}");
        Console.WriteLine($"출근 직원 수: {WorkingEmployeeCount()}");
        Console.WriteLine($"주문 수: {Orders.Count}");
        Console.WriteLine($"누적 매출: {totalSales}원");
        Console.WriteLine($"누적 비용: {totalCost}원");
        Console.WriteLine($"순이익: {Profit()}원");
    }

    public void ShowSalesReport()
    {
        int completedOrderCount = Orders.Count(order => order.Status == OrderStatus.Completed);
        int soldQuantity = Orders
            .Where(order => order.Status == OrderStatus.Completed)
            .Sum(order => order.TotalQuantity());

        SalesSummary summary = new SalesSummary(completedOrderCount, soldQuantity, totalSales, totalCost);
        summary.Show();
        ShowOrderTypeSummary();
        ShowMenuSalesSummary();
    }

    private void ShowOrderTypeSummary()
    {
        Console.WriteLine("[주문 유형별 매출]");
        ShowOrderTypeLine(OrderType.InStore, "매장");
        ShowOrderTypeLine(OrderType.TakeOut, "포장");
        ShowOrderTypeLine(OrderType.Delivery, "배달");
    }

    private void ShowOrderTypeLine(OrderType orderType, string title)
    {
        var orders = Orders.Where(order => order.Status == OrderStatus.Completed && order.Type == orderType).ToList();
        int count = orders.Count;
        int quantity = orders.Sum(order => order.TotalQuantity());
        int sales = orders.Sum(order => order.TotalPrice());

        Console.WriteLine($" - {title}: 주문 {count}건, 판매 {quantity}개, 매출 {sales}원");
    }

    private void ShowMenuSalesSummary()
    {
        Console.WriteLine("[메뉴별 판매 수량]");

        var menuGroups = Orders
            .Where(order => order.Status == OrderStatus.Completed)
            .SelectMany(order => order.Lines)
            .GroupBy(line => line.Menu.Name)
            .OrderByDescending(group => group.Sum(line => line.Quantity));

        bool hasMenuSales = false;

        foreach (var group in menuGroups)
        {
            int quantity = group.Sum(line => line.Quantity);
            int sales = group.Sum(line => line.TotalPrice());
            Console.WriteLine($" - {group.Key}: {quantity}개, 매출 {sales}원");
            hasMenuSales = true;
        }

        if (!hasMenuSales)
        {
            Console.WriteLine(" - 판매된 메뉴가 없습니다.");
        }
    }

    public class InventoryManager
    {
        private Dictionary<string, int> stock = new();
        private Dictionary<string, int> ingredientPrice = new();

        public void AddStock(string ingredient, int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("재고 추가량은 음수가 될 수 없습니다.");
            }

            if (!stock.ContainsKey(ingredient))
            {
                stock[ingredient] = 0;
            }

            stock[ingredient] += amount;
        }

        public void SetIngredientPrice(string ingredient, int price)
        {
            if (price < 0)
            {
                throw new ArgumentException("재료 가격은 음수가 될 수 없습니다.");
            }

            ingredientPrice[ingredient] = price;
        }

        public int GetIngredientPrice(string ingredient)
        {
            if (!ingredientPrice.ContainsKey(ingredient))
            {
                return 0;
            }

            return ingredientPrice[ingredient];
        }

        public int GetStockAmount(string ingredient)
        {
            if (!stock.ContainsKey(ingredient))
            {
                return 0;
            }

            return stock[ingredient];
        }

        public bool TryGetStockInfo(string ingredient, out int amount, out int price)
        {
            amount = GetStockAmount(ingredient);
            price = GetIngredientPrice(ingredient);
            return stock.ContainsKey(ingredient) || ingredientPrice.ContainsKey(ingredient);
        }

        public bool HasStock(string ingredient, int amount)
        {
            return stock.ContainsKey(ingredient) && stock[ingredient] >= amount;
        }

        public bool UseStock(string ingredient, int amount)
        {
            if (!HasStock(ingredient, amount))
            {
                return false;
            }

            stock[ingredient] -= amount;
            return true;
        }

        public void ShowStock()
        {
            Console.WriteLine("[재고 현황]");

            if (stock.Count == 0)
            {
                Console.WriteLine(" - 등록된 재고가 없습니다.");
                return;
            }

            foreach (var item in stock.OrderBy(item => item.Key))
            {
                int price = GetIngredientPrice(item.Key);
                Console.WriteLine($" - {item.Key}: {item.Value}, 현재 단가: {price}원");
            }
        }

        public void ShowLowStock(int standardAmount)
        {
            Console.WriteLine($"[부족 재고 목록: {standardAmount}개 이하]");

            bool hasLowStock = false;

            foreach (var item in stock.Where(item => item.Value <= standardAmount))
            {
                Console.WriteLine($" - {item.Key}: {item.Value}개");
                hasLowStock = true;
            }

            if (!hasLowStock)
            {
                Console.WriteLine(" - 부족한 재고가 없습니다.");
            }
        }
    }
}
