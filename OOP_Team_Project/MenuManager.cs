using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    public class MenuManager
    {
        private List<Coffee> coffeeMenus = new List<Coffee>();
        private List<Dessert> dessertMenus = new List<Dessert>();

        public MenuManager()
        {
            coffeeMenus.Add(new Coffee(1, "아메리카노", 4000m, true, 0));
            coffeeMenus.Add(new Coffee(2, "카페라떼", 4500m, true, 0));
            coffeeMenus.Add(new Coffee(3, "바닐라라떼", 5000m, true, 0));
            coffeeMenus.Add(new Coffee(4, "카페모카", 5200m, true, 0));
            coffeeMenus.Add(new Coffee(5, "카라멜 마키아토", 5500m, false, 0));

            dessertMenus.Add(new Dessert(1, "치즈 케이크", 6500m, new NutritionInfo(450, 30)));
            dessertMenus.Add(new Dessert(2, "초코 브라우니", 5500m, new NutritionInfo(520, 45)));
            dessertMenus.Add(new Dessert(3, "딸기 타르트", 7000m, new NutritionInfo(380, 28)));
            dessertMenus.Add(new Dessert(4, "플레인 크로플", 4500m, new NutritionInfo(340, 12)));
            dessertMenus.Add(new Dessert(5, "블루베리 베이글", 3800m, new NutritionInfo(290, 8)));
        }

        public void PrintCoffeeMenu()
        {
            Console.WriteLine("- 커피 -\n0) 이전 페이지");
            coffeeMenus.ForEach(coffeeMenu => coffeeMenu.PrintReceipt());
        }

        public Coffee GetCoffee(int id)
        {
            return coffeeMenus.Find(m => m.Id == id);
        }

        public void PrintDessertMenu()
        {
            Console.WriteLine("- 디저트 -\n0) 이전 페이지");
            dessertMenus.ForEach(item => item.PrintReceipt());
        }

        public Dessert GetDessert(int id)
        {
            return dessertMenus.Find(m => m.Id == id);
        }
    }
}
