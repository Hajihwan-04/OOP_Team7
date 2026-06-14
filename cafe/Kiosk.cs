namespace cafe;

public class Kiosk
{
    private Cafe cafe;
    private Customer customer;

    public Kiosk(Cafe cafe, Customer customer)
    {
        this.cafe = cafe;
        this.customer = customer;
    }

    public bool StartOrder(Order myOrder)
    {
        while (true)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("==================상품  주문==================");
            Console.WriteLine("==============================================");
            Console.Write("1. 메뉴 선택\n2. 장바구니\n3. 결제\n(종료는 아무 키)\n=>");
            int ordering = GetIntInput();

            switch (ordering)
            {
                case 1:
                    Console.Clear();
                    SelectMenu(myOrder);
                    break;
                case 2:
                    Console.Clear();
                    ManageCart(myOrder);
                    break;
                case 3:
                    Console.Clear();
                    if (ProcessKioskPayment(myOrder)) return true;
                    break;
                default:
                    Console.WriteLine("주문 취소.");
                    return false;
            }
        }
    }

    private void SelectMenu(Order myOrder)
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("======================메뉴=====================");
        Console.WriteLine("==============================================");

        if (cafe.Menus.Count == 0)
        {
            Console.WriteLine("사용 가능한 메뉴가 없습니다.");
            return;
        }

        for (int i = 0; i < cafe.Menus.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {cafe.Menus[i].Info()}");
        }

        Console.Write("\n0. 이전 페이지\n원하는 메뉴 번호 선택 => ");
        int choice = GetIntInput();

        if (choice == 0) return;

        if (choice > 0 && choice <= cafe.Menus.Count)
        {
            Menu selectedMenu = cafe.Menus[choice - 1];
            Console.Write("수량을 입력하세요 (기본값: 1) => ");
            int quantity = GetIntInput();
            if (quantity <= 0) quantity = 1;

            myOrder.AddMenu(selectedMenu, quantity);
            Console.WriteLine($"[시스템] {selectedMenu.Name} x {quantity}개가 장바구니에 추가되었습니다.");
            Console.Write("\n계속하려면 아무 키를 누르세요...");
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine("[시스템] 잘못된 번호입니다.");
        }
    }

    public static int GetIntInput()
    {
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int inputI) == false)
        {
            return 0;
        }
        return inputI;
    }

    private void ManageCart(Order myOrder)
    {
        if (myOrder.LineCount == 0)
        {
            Console.WriteLine("장바구니가 비어있습니다.");
            Console.Write("\n계속하려면 아무 키를 누르세요...");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        myOrder.ShowOrder();

        Console.Write("\n삭제하고 싶은 아이템의 인덱스 번호를 입력하세요 (취소는 0) => ");
        int deleteIndex = GetIntInput();
        if (deleteIndex == 0)
        {
            Console.Clear();
            return;
        }

        if (deleteIndex > 0 && deleteIndex <= myOrder.LineCount)
        {
            myOrder.RemoveLineAt(deleteIndex - 1);
            Console.WriteLine("[시스템] 아이템이 삭제되었습니다.");
        }
        else
        {
            Console.WriteLine("[시스템] 잘못된 인덱스입니다.");
        }
    }

    private bool ProcessKioskPayment(Order myOrder)
    {
        try
        {
            if (myOrder.LineCount == 0)
            {
                Console.WriteLine("[시스템] 주문한 항목이 없습니다.");
                return false;
            }

            myOrder.ShowOrder();
            Console.Write("\n결제 수단을 선택하세요 (1. 카드 / 2. 현금 / 0. 취소) => ");
            int payChoice = GetIntInput();

            Payment payment;

            if (payChoice == 1)
            {
                payment = new CardPayment(myOrder.TotalPrice());
            }
            else if (payChoice == 2)
            {
                payment = new CashPayment(myOrder.TotalPrice());
            }
            else
            {
                return false;
            }

            bool result = myOrder.Pay(payment);
            if (result)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[완료] 결제가 완료되었습니다!");
                Console.ResetColor();
                Console.WriteLine($"결제 정보: {payment.Info()}");
            }

            return result;
        }
        catch (InvalidOrderException ex)
        {
            Console.WriteLine($"[결제 실패] {ex.Message}");
            return false;
        }
    }
}
