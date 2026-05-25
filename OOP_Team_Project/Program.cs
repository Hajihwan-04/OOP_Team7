namespace OOP_Team_Project
{
    public interface IPrintable
    {
        void PrintReceipt();
    }

    
    
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
                        Console.WriteLine("=> " + currentCustomer.getName() + " 회원님 환영합니다.");
                    }
                    else
                    {
                        Console.Write("이름을 입력해주세요\n=>");
                        string newName = Console.ReadLine();
                        currentCustomer = custManager.RegisterCustomer(phoneNumber, newName);
                        Console.Clear();
                        Console.WriteLine("=> " + currentCustomer.getName() + "님 회원가입 되셨습니다.");
                    }
                    
                    Order myOrder = new Order(currentCustomer);

                    kiosk.StartOrder(myOrder);

                   

                    // 영수증 출력
                    myOrder.PrintOrder();
                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"에러 발생: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine("시스템: 첫 번째 주문 처리 종료.\n");
                    
                }
                //break;
            }
        }
    }
}
