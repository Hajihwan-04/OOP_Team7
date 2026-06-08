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

        public void StartOrder(Order myOrder)
        {
            while (true)
            {
                int stop = 0;
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
                        myOrder.PrintOrder();
                        Console.WriteLine("0. 이전 페이지\n");
                        int deleteItem = GetIntInput();
                        if (deleteItem == 0) break;
                        myOrder.DeleteItem(myOrder[deleteItem]);
                        break;
                    default:
                        Console.WriteLine("주문 취소.");

                        stop = 1;
                        break;

                }
                if (stop == 1) break;
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
    }
}
