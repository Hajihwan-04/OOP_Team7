using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace OOP_Team_Project
{
    public class Kiosk
    {
        private MenuManager menuManager;

        public Kiosk(MenuManager manager)
        {
            this.menuManager = manager;
        }

        public bool StartOrder(Order myOrder)
        {
            while (true)
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("==================상품  주문==================");
                Console.WriteLine("==============================================");
                Console.Write("1. 음료\n2. 디저트\n3. 장바구니\n4. 결제\n(종료는 아무 키)\n=>");
                int ordering = GetIntInput();

                switch (ordering)
                {
                    case 1:
                        Console.Clear();
                        OrderBeverage(myOrder);
                        break;
                    case 2:
                        Console.Clear();
                        OrderDessert(myOrder);
                        break;

                    case 3:
                        ManageCart(myOrder);
                        break;
                    case 4:
                        Console.Clear();
                        if (ProcessKioskPayment(myOrder)) return true; // 결제 성공 시 루프 종료
                        break;
                    default:
                        Console.WriteLine("주문 취소.");
                        return false;
                }
            }
        }


        private void OrderBeverage(Order myOrder)
        {
            int stop = 0;
            do
            {

                Console.WriteLine("==============================================");
                Console.WriteLine("=====================음료=====================");
                Console.WriteLine("==============================================");
                Console.Write("0. 이전 페이지\n1. 커피\n=>");
                int kindOfBeverage = GetIntInput();
                if (kindOfBeverage == 0) return;
                switch (kindOfBeverage)
                {
                    case 1:
                        Console.Clear();
                        menuManager.PrintCoffeeMenu();
                        Console.Write("\n원하는 음료 번호 선택 =>\n0) 이전 페이지\n");
                        int chooseCoffee = GetIntInput();
                        if (chooseCoffee == 0) break;
                        Coffee original = (Coffee)menuManager.GetCoffee(chooseCoffee);

                        Console.Write("\n1. ICED\n2. HOT\n=>");
                        int temp = GetIntInput();

                        Console.Write("\n샷 추가 횟수(원하지 않으시면 0)\n=>");
                        int shotCount = GetIntInput();

                        Coffee currentMenu = new Coffee(original.Id, original.Name, original.Price, (temp == 2 ? true : false), shotCount);
                        currentMenu.DisplayInfo();
                        myOrder.AddItem(currentMenu);
                        myOrder.PrintOrder();
                        stop = 1;
                        break;
                    case 2:
                        break;

                }
            } while (stop != 1);
        }

        private void OrderDessert(Order myOrder)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("====================디저트====================");
            Console.WriteLine("==============================================");
            
            menuManager.PrintDessertMenu();
            int chooseDessert = GetIntInput();
            if (chooseDessert == 0) return;
            Dessert currentMenu = menuManager.GetDessert(chooseDessert);
            currentMenu.DisplayInfo();
            myOrder.AddItem(currentMenu);
            myOrder.PrintOrder();
        }

        public static int GetIntInput()
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int inputI) == false)
            {
                return 0;
            }
            return inputI;
        }

        private void ManageCart(Order myOrder)
        {
            myOrder.PrintOrder();
            if (myOrder.getOrderCount() == 0) return;

            Console.Write("\n삭제하고 싶은 아이템의 인덱스 번호를 입력하세요 (취소는 -1) => ");
            int deleteIndex = GetIntInput();
            if (deleteIndex == -1) return;

            if (deleteIndex >= 0 && deleteIndex < myOrder.getOrderCount())
            {
                myOrder.DeleteItem(deleteIndex);
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
                myOrder.PrintOrder();
                Console.Write("\n결제 수단을 선택하세요 (1. 카드 / 2. 현금 / 0. 취소) => ");
                int payChoice = GetIntInput();

                IPayable payment = null;
                if (payChoice == 1) payment = new CardPayment();
                else if (payChoice == 2) payment = new CashPayment();
                else return false;

                myOrder.ProcessPayment(payment, (msg) => {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n🎉 {msg} 🎉");
                    Console.ResetColor();
                });

                return true;
            }
            catch (InvalidOrderException ex) // 💥 의미 있는 사용자 정의 예외 처리 
            {
                Console.WriteLine($"[결제 실패] {ex.Message}");
                 return false;
            }
        }
    }
}
