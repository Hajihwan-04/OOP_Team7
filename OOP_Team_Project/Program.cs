namespace OOP_Team_Project
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            CustomerManager custManager = new CustomerManager();
            MenuManager menuManager = new MenuManager();
            Kiosk kiosk = new Kiosk(menuManager);
            while (true)
            {
                try
                {
                    Console.WriteLine("==============================================");
                    Console.WriteLine("==========Team7 Cafe Ordering System==========");
                    Console.WriteLine("==============================================");
                    Console.Write("이용방식\n1. 매장이용\n2. 포장\n=> ");
                    string whereToEat = Console.ReadLine();
                    if (whereToEat == "q") break;
                    else if (whereToEat != "1" && whereToEat != "2" && whereToEat != "q")
                    {
                        throw new Exception("잘못된 입력입니다.");
                    }
                    Console.Clear();
                    Console.WriteLine("==============================================");
                    Console.WriteLine("=================고객정보확인=================");
                    Console.WriteLine("==============================================");
                    Console.Write("전화번호를 입력해주세요\n=> ");
                    string phoneNumber = Console.ReadLine();
                    Customer currentCustomer;
                    if (custManager.IsCustomerExist(phoneNumber))
                    {
                        currentCustomer = custManager.GetCustomer(phoneNumber);
                        Console.Clear();
                        Console.WriteLine("=> " + currentCustomer.GetName() + " 회원님 환영합니다.");
                    }
                    else
                    {
                        Console.Write("처음 오셨군요? 이름을 입력해주세요\n=>");
                        string newName = Console.ReadLine();
                        currentCustomer = custManager.RegisterCustomer(phoneNumber, newName);
                        Console.Clear();
                        Console.WriteLine("=> " + currentCustomer.GetName() + "님 회원가입 되셨습니다.");
                    }
                    
                    Order myOrder = new Order(currentCustomer);

                    kiosk.StartOrder(myOrder);

                   

                    // 영수증 출력
                    myOrder.PrintOrder();
                    bool isPaid = kiosk.StartOrder(myOrder);

                    if (isPaid)
                    {
                        Console.WriteLine("\n[시스템] 이용해 주셔서 감사합니다. 대기 번호표를 확인해 주세요!");
                    }
                }
                catch (ArgumentException ex) // 예외 처리 (1) 
                {
                    Console.WriteLine($"[입력 오류] {ex.Message}");
                }
                catch (NullReferenceException ex) // 예외 처리 (2) 
                {
                    Console.WriteLine($"[시스템 오류] {ex.Message}");
                }
                catch (Exception ex) // 예외 처리 (3) 
                {
                    Console.WriteLine($"[알 수 없는 에러] {ex.Message}");
                }
                finally // 의미 있는 finally 
                {
                    Console.WriteLine("\n[안내] 다음 손님 정산을 위해 시스템을 리셋합니다.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            Console.WriteLine("프로그램을 마감합니다.");

        }
    }
}
