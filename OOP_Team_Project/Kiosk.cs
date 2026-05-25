using System;
using System.Collections.Generic;
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
                Console.WriteLine("==============================================");
                Console.WriteLine("==================상품  주문==================");
                Console.WriteLine("==============================================");
                Console.Write("1. 음료\n2. 디저트\n3. 결제\n(종료는 아무 키)\n=>");
                int ordering = GetIntInput();

                switch (ordering)
                {
                    case 1:
                        Console.Clear();
                        OrderBeverage(myOrder);


                        break;
                    default:
                        Console.WriteLine("Error");
                        break;

                }
                break;
            }
        }


        private void OrderBeverage(Order myOrder)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("=====================음료=====================");
            Console.WriteLine("==============================================");
            Console.Write("1. 커피\n=>");
            int kindOfBeverage = GetIntInput();
            switch (kindOfBeverage)
            {
                case 1:
                    menuManager.PrintCoffeeMenu();
                    Console.Write("\n원하는 음료 번호 선택 => ");
                    int chooseBeverage = GetIntInput();
                    Coffee original = (Coffee)menuManager.GetCoffee(chooseBeverage);

                    Console.Write("\n1. ICED\n2. HOT\n=>");
                    int temp = GetIntInput();

                    Console.Write("\n샷 추가 횟수(원하지 않으시면 0)\n=>");
                    int shotCount = GetIntInput();

                    Coffee currentMenu = new Coffee(original.Id, original.Name, original.Price, (temp == 2 ? true : false), shotCount);
                    currentMenu.DisplayInfo();
                    myOrder.AddItem(currentMenu);
                    myOrder.PrintOrder();
                    break;
            }
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
