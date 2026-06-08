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

            dessertMenus.Add(new Dessert(1, "치즈 케이크", 6500m, new NutritionInfo(450, 30)));
        }

        public void PrintCoffeeMenu()
        {
            Console.WriteLine("- 커피 -");
            coffeeMenus.ForEach(coffeeMenu => coffeeMenu.PrintReceipt());
        }

        public Coffee GetCoffee(int id)
        {
            return coffeeMenus.Find(m => m.Id == id);
        }

        public void PrintDessertMenu()
        {
            Console.WriteLine("- 디저트 -");
            dessertMenus.ForEach(item => item.PrintReceipt());
        }

        public Dessert GetDessert(int id)
        {
            return dessertMenus.Find(m => m.Id == id);
        }
    }
}
