namespace cafe;

public class CafeManagementSystem
{
    private Cafe cafe;
    private int nextOrderNumber;

    public CafeManagementSystem(Cafe cafe)
    {
        this.cafe = cafe;
        nextOrderNumber = cafe.Orders.Count == 0 ? 1 : cafe.Orders.Max(order => order.OrderNumber) + 1;
    }

    public void Run()
    {
        bool running = true;

        while (running)
        {
            ShowMainMenu();
            string input = ReadInput("0");

            try
            {
                if (input == "1")
                {
                    cafe.ShowMenus();
                    Wait();
                }
                else if (input == "2")
                {
                    RegisterOrder();
                }
                else if (input == "3")
                {
                    cafe.ShowOrders();
                    Wait();
                }
                else if (input == "4")
                {
                    StockMenu();
                }
                else if (input == "5")
                {
                    EmployeeMenu();
                }
                else if (input == "6")
                {
                    cafe.ShowSalesReport();
                    Wait();
                }
                else if (input == "7")
                {
                    cafe.ShowCafeStatus();
                    Wait();
                }
                else if (input == "8")
                {
                    CancelOrderRecord();
                }
                else if (input == "9")
                {
                    KioskOrderMenu();
                }
                else if (input == "0")
                {
                    running = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"메뉴 처리 중 오류가 발생했습니다: {ex.Message}");
                Wait();
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }

    private void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine($"[{cafe.Name} 카페 관리 시스템]");
        Console.WriteLine("1. 메뉴 조회");
        Console.WriteLine("2. 주문 기록 저장");
        Console.WriteLine("3. 주문 기록 조회");
        Console.WriteLine("4. 재고 관리");
        Console.WriteLine("5. 직원 관리");
        Console.WriteLine("6. 매출 리포트");
        Console.WriteLine("7. 카페 상태");
        Console.WriteLine("8. 주문 기록 취소");
        Console.WriteLine("9. 손님 주문 (키오스크)");
        Console.WriteLine("0. 종료");
        Console.Write("선택: ");
    }

    private void RegisterOrder()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("[주문 기록 저장]");

            Console.Write("고객 이름: ");
            string customerName = ReadInput("비회원");

            Console.Write("고객 전화번호: ");
            string phone = ReadInput("없음");

            Customer customer = new Customer(customerName, phone);
            OrderType orderType = SelectOrderType();
            Order order = new Order(nextOrderNumber, customer, orderType);

            bool addingMenu = true;

            while (addingMenu)
            {
                cafe.ShowMenus();
                Console.Write("판매 기록에 넣을 메뉴 번호(0 입력 시 완료): ");
                int menuNumber = ReadInt(0);

                if (menuNumber == 0)
                {
                    addingMenu = false;
                }
                else
                {
                    Menu? menu = cafe.FindMenuByNumber(menuNumber);

                    if (menu == null)
                    {
                        Console.WriteLine("없는 메뉴 번호입니다.");
                    }
                    else
                    {
                        Console.Write("판매 수량: ");
                        int quantity = ReadInt(1);
                        order.AddMenu(menu, quantity);
                        Console.WriteLine($"{menu.Name} {quantity}개를 주문 기록에 추가했습니다.");
                    }
                }
            }

            if (order.MenuCount == 0)
            {
                throw new InvalidOrderException("메뉴가 없어 주문을 등록할 수 없습니다.");
            }

            if (orderType == OrderType.TakeOut || orderType == OrderType.Delivery)
            {
                Console.Write("일회용품 사용? (y/n): ");
                string input = ReadInput("n");
                order.SetUseDisposable(input == "y" || input == "Y");
            }

            Console.Write("개인 텀블러 사용? (y/n): ");
            string tumblerInput = ReadInput("n");
            order.SetUseTumbler(tumblerInput == "y" || tumblerInput == "Y");

            Payment payment = CreatePaymentRecord(order.TotalPrice());
            order.Pay(payment);
            cafe.ProcessOrder(order);

            nextOrderNumber++;
            Console.WriteLine("주문 기록이 정상 저장되었습니다.");
            order.ShowOrder();
        }
        catch (CafeException ex)
        {
            Console.WriteLine($"주문 처리 오류: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"주문 기록 저장 중 예상하지 못한 오류: {ex.Message}");
        }
        finally
        {
            Wait();
        }
    }

    private Payment CreatePaymentRecord(int amount)
    {
        try
        {
            Console.WriteLine("1. 현금");
            Console.WriteLine("2. 카드");
            Console.Write("결제수단 기록: ");

            string input = ReadInput("1");

            if (input == "2")
            {
                return new CardPayment(amount);
            }

            return new CashPayment(amount);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"결제 정보 오류: {ex.Message}");
            return new CashPayment(amount);
        }
        finally
        {
            Console.WriteLine("결제수단 기록을 종료합니다.");
        }
    }

    private void CancelOrderRecord()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("[주문 기록 취소]");
            Console.Write("취소할 주문 번호: ");
            int orderNumber = ReadInt(0);

            if (orderNumber <= 0)
            {
                throw new ArgumentException("주문 번호는 1 이상이어야 합니다.");
            }

            cafe.CancelOrder(orderNumber);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"주문 취소 오류: {ex.Message}");
        }
        finally
        {
            Wait();
        }
    }

    private OrderType SelectOrderType()
    {
        Console.WriteLine("1. 매장");
        Console.WriteLine("2. 포장");
        Console.WriteLine("3. 배달");
        Console.Write("주문 유형 선택: ");

        string input = ReadInput("1");

        if (input == "2")
        {
            return OrderType.TakeOut;
        }

        if (input == "3")
        {
            return OrderType.Delivery;
        }

        return OrderType.InStore;
    }

    private void StockMenu()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("[재고 관리]");
            Console.WriteLine("1. 재고 현황");
            Console.WriteLine("2. 재고 구매");
            Console.WriteLine("3. 부족 재고 확인");
            Console.WriteLine("4. 재료 상세 조회");
            Console.WriteLine("0. 돌아가기");
            Console.Write("선택: ");

            string input = ReadInput("0");

            if (input == "1")
            {
                cafe.Inventory.ShowStock();
            }
            else if (input == "2")
            {
                BuyStock();
            }
            else if (input == "3")
            {
                cafe.Inventory.ShowLowStock(100);
            }
            else if (input == "4")
            {
                ShowStockInfo();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"재고 관리 오류: {ex.Message}");
        }
        finally
        {
            Wait();
        }
    }

    private void BuyStock()
    {
        try
        {
            Console.Write("재료 이름: ");
            string ingredient = ReadInput("");

            if (ingredient == "")
            {
                throw new ArgumentException("재료 이름을 입력해야 합니다.");
            }

            Console.Write("구매 수량: ");
            int amount = ReadInt(0);

            Console.Write("구매 단가: ");
            int unitPrice = ReadInt(0);

            cafe.BuyStock(ingredient, amount, unitPrice);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"재고 구매 오류: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("재고 구매 처리를 종료합니다.");
        }
    }

    private void ShowStockInfo()
    {
        Console.Write("조회할 재료 이름: ");
        string ingredient = ReadInput("");

        if (cafe.Inventory.TryGetStockInfo(ingredient, out int amount, out int price))
        {
            Console.WriteLine($"{ingredient}: 재고 {amount}개, 단가 {price}원");
        }
        else
        {
            Console.WriteLine("등록되지 않은 재료입니다.");
        }
    }

    private void EmployeeMenu()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("[직원 관리]");
            Console.WriteLine("1. 직원 목록");
            Console.WriteLine("2. 직원 등록");
            Console.WriteLine("3. 직원 삭제");
            Console.WriteLine("4. 현재 근무 상태");
            Console.WriteLine("5. 근무 일정 등록");
            Console.WriteLine("0. 돌아가기");
            Console.Write("선택: ");

            string input = ReadInput("0");

            if (input == "1")
            {
                cafe.ShowEmployees();
            }
            else if (input == "2")
            {
                RegisterEmployee();
            }
            else if (input == "3")
            {
                DeleteEmployee();
            }
            else if (input == "4")
            {
                cafe.ShowEmployeeWorkStatus();
            }
            else if (input == "5")
            {
                RegisterEmployeeSchedule();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"직원 관리 오류: {ex.Message}");
        }
        finally
        {
            Wait();
        }
    }

    private void RegisterEmployee()
    {
        Console.Write("[직원 직접 등록] 이름: ");
        string name = ReadInput("");

        if (name == "")
        {
            throw new ArgumentException("직원 이름을 입력해야 합니다.");
        }

        Console.Write("전화번호: ");
        string phone = ReadInput("없음");

        Console.Write("급여 입력: ");
        int pay = ReadInt(0);

        if (pay <= 0)
        {
            throw new ArgumentException("급여는 1원 이상이어야 합니다.");
        }

        EmploymentType employmentType = SelectEmploymentType();
        Diligence diligence = SelectDiligence();

        Console.WriteLine("1. 바리스타");
        Console.WriteLine("2. 매니저");
        Console.Write("직원 종류 선택: ");
        string roleInput = ReadInput("1");

        Employee employee;

        if (roleInput == "2")
        {
            employee = new Manager(name, phone, pay, employmentType, diligence);
            cafe.HireEmployee(employee);
            Console.WriteLine($"{name} 매니저를 등록했습니다.");
        }
        else
        {
            employee = new Barista(name, phone, pay, employmentType, diligence);
            cafe.HireEmployee(employee);
            Console.WriteLine($"{name} 바리스타를 등록했습니다.");
        }

        Console.Write("근무 일정을 바로 등록할까요? (y/n): ");
        string scheduleInput = ReadInput("n");

        if (scheduleInput == "y" || scheduleInput == "Y")
        {
            AddScheduleToEmployee(employee);
        }
    }

    private void DeleteEmployee()
    {
        Console.Write("[직원 삭제] 삭제할 직원 이름: ");
        string name = ReadInput("");

        if (name == "")
        {
            throw new ArgumentException("삭제할 직원 이름을 입력해야 합니다.");
        }

        cafe.RemoveEmployee(name);
    }

    private void RegisterEmployeeSchedule()
    {
        Console.Write("[근무 일정 등록] 직원 이름: ");
        string name = ReadInput("");

        if (name == "")
        {
            throw new ArgumentException("직원 이름을 입력해야 합니다.");
        }

        if (!cafe.TryFindEmployee(name, out Employee? employee) || employee == null)
        {
            Console.WriteLine($"{name} 직원을 찾을 수 없습니다.");
            return;
        }

        AddScheduleToEmployee(employee);
    }

    private void AddScheduleToEmployee(Employee employee)
    {
        DayOfWeek day = SelectDayOfWeek();

        Console.Write("시작 시간(0~23): ");
        int startHour = ReadInt(9);

        Console.Write("종료 시간(1~24): ");
        int endHour = ReadInt(18);

        employee.AddSchedule(day, startHour, endHour);
        Console.WriteLine($"{employee.Name} 직원 근무 일정을 등록했습니다.");
    }

    private DayOfWeek SelectDayOfWeek()
    {
        Console.WriteLine("1. 월요일");
        Console.WriteLine("2. 화요일");
        Console.WriteLine("3. 수요일");
        Console.WriteLine("4. 목요일");
        Console.WriteLine("5. 금요일");
        Console.WriteLine("6. 토요일");
        Console.WriteLine("7. 일요일");
        Console.Write("요일 선택: ");

        string input = ReadInput("1");

        if (input == "2")
        {
            return DayOfWeek.Tuesday;
        }

        if (input == "3")
        {
            return DayOfWeek.Wednesday;
        }

        if (input == "4")
        {
            return DayOfWeek.Thursday;
        }

        if (input == "5")
        {
            return DayOfWeek.Friday;
        }

        if (input == "6")
        {
            return DayOfWeek.Saturday;
        }

        if (input == "7")
        {
            return DayOfWeek.Sunday;
        }

        return DayOfWeek.Monday;
    }

    private EmploymentType SelectEmploymentType()
    {
        Console.WriteLine("1. 파트타이머");
        Console.WriteLine("2. 정식직원");
        Console.Write("근무 형태 선택: ");

        string input = ReadInput("1");

        if (input == "2")
        {
            return EmploymentType.FullTime;
        }

        return EmploymentType.PartTime;
    }

    private Diligence SelectDiligence()
    {
        Console.WriteLine("1. 매우 좋음");
        Console.WriteLine("2. 좋음");
        Console.WriteLine("3. 보통");
        Console.WriteLine("4. 나쁨");
        Console.Write("성실도 선택: ");

        string input = ReadInput("3");

        if (input == "1")
        {
            return Diligence.VeryGood;
        }

        if (input == "2")
        {
            return Diligence.Good;
        }

        if (input == "4")
        {
            return Diligence.Bad;
        }

        return Diligence.Normal;
    }

    private string ReadInput(string defaultValue)
    {
        string? input = Console.ReadLine();

        if (input == null || input.Trim() == "")
        {
            return defaultValue;
        }

        return input.Trim();
    }

    private int ReadInt(int defaultValue)
    {
        string input = ReadInput("");

        if (int.TryParse(input, out int value))
        {
            return value;
        }

        return defaultValue;
    }

    private void Wait()
    {
        Console.WriteLine();
        Console.Write("Enter...");
        Console.ReadLine();
    }

    private void KioskOrderMenu()
    {
        bool continueKiosk = true;

        while (continueKiosk)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("==============손님 주문 (키오스크)==============");
                Console.WriteLine("==============================================\n");
                Console.WriteLine("(메인 메뉴로 돌아가려면 'quit' 입력)\n");

                // 손님 정보 입력
                Console.Write("고객 이름: ");
                string customerName = ReadInput("손님");

                // quit 명령어 확인
                if (customerName.ToLower() == "quit")
                {
                    Console.WriteLine("[취소] 키오스크를 나갑니다.");
                    Wait();
                    continueKiosk = false;
                    return;
                }

                Console.Write("고객 전화번호: ");
                string phone = ReadInput("없음");

                Customer customer = new Customer(customerName, phone);

                // 주문 유형 선택
                OrderType orderType = SelectOrderType();

                // Order 생성
                Order order = new Order(nextOrderNumber, customer, orderType);

                // 키오스크 안에서 결제까지 진행되므로, 가격에 영향을 주는 옵션은 결제 전에 먼저 정한다.
                if (orderType == OrderType.TakeOut || orderType == OrderType.Delivery)
                {
                    Console.Write("\n일회용품 사용? (y/n): ");
                    string disposableInput = ReadInput("n");
                    order.SetUseDisposable(disposableInput == "y" || disposableInput == "Y");
                }

                Console.Write("개인 텀블러 사용? (y/n): ");
                string tumblerInput = ReadInput("n");
                order.SetUseTumbler(tumblerInput == "y" || tumblerInput == "Y");

                // Kiosk 실행
                Kiosk kiosk = new Kiosk(cafe, customer);

                if (kiosk.StartOrder(order))
                {
                    // 주문 기록 저장
                    cafe.ProcessOrder(order);
                    nextOrderNumber++;

                    Console.WriteLine("\n[완료] 주문이 성공적으로 처리되었습니다!");
                    Console.WriteLine($"주문번호: {order.OrderNumber}");
                    Console.WriteLine("\n아무 키나 누르면 다음 손님으로 이동합니다...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\n[취소] 주문이 취소되었습니다.");
                    Console.WriteLine("아무 키나 누르면 다음 손님으로 이동합니다...");
                    Console.ReadKey();
                }
            }
            catch (InvalidOrderException ex)
            {
                Console.WriteLine($"[오류] {ex.Message}");
                Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[오류] 예상치 못한 오류: {ex.Message}");
                Wait();
            }
        }
    }
}
