namespace cafe;

public class Program
{
    public static void Main(string[] args)
    {
        var cafe1 = new Cafe("본점", 1, "대구");
        cafe1.OrderRecorded += order => Console.WriteLine($"[알림] 주문 {order.OrderNumber}번 기록이 저장되었습니다.");

        var americanoRecipe = new Recipe("아메리카노");
        americanoRecipe.AddIngredient("커피원두", 10);
        americanoRecipe.AddIngredient("물", 200);

        var latteRecipe = new Recipe("라떼");
        latteRecipe.AddIngredient("커피원두", 10);
        latteRecipe.AddIngredient("우유", 150);

        var vanillaLatteRecipe = new Recipe("바닐라라떼");
        vanillaLatteRecipe.AddIngredient("커피원두", 10);
        vanillaLatteRecipe.AddIngredient("우유", 150);
        vanillaLatteRecipe.AddIngredient("바닐라시럽", 20);

        var americano = new CoffeeMenu("아메리카노", 4700, americanoRecipe, MenuSize.Tall);
        var latte = new CoffeeMenu("라떼", 5200, latteRecipe, MenuSize.Grande);
        var vanillaLatte = new CoffeeMenu("바닐라라떼", 5800, vanillaLatteRecipe, MenuSize.Grande);

        cafe1.AddMenu(americano);
        cafe1.AddMenu(latte);
        cafe1.AddMenu(vanillaLatte);

        cafe1.BuyStock("커피원두", 1000, 40);
        cafe1.BuyStock("물", 5000, 1);
        cafe1.BuyStock("우유", 3000, 5);
        cafe1.BuyStock("바닐라시럽", 1000, 12);

        var barista = new Barista("박바리", "010-3333-4444", 10000, EmploymentType.PartTime);
        barista.AddSchedule(DayOfWeek.Monday, 9, 18);
        barista.AddSchedule(DayOfWeek.Tuesday, 9, 18);
        barista.AddSchedule(DayOfWeek.Wednesday, 9, 18);
        barista.AddSchedule(DayOfWeek.Thursday, 9, 18);
        barista.AddSchedule(DayOfWeek.Friday, 9, 18);
        cafe1.HireEmployee(barista);

        var manager = new Manager("김매니저", "010-5555-6666", 2800000, EmploymentType.FullTime, Diligence.Good);
        manager.AddSchedule(DayOfWeek.Monday, 10, 19);
        manager.AddSchedule(DayOfWeek.Tuesday, 10, 19);
        manager.AddSchedule(DayOfWeek.Wednesday, 10, 19);
        manager.AddSchedule(DayOfWeek.Thursday, 10, 19);
        manager.AddSchedule(DayOfWeek.Friday, 10, 19);
        cafe1.HireEmployee(manager);

        var order1 = new Order(1, new Customer("홍길동", "010-1111-1111"), OrderType.InStore);
        order1.AddMenu(americano, 2);
        order1.Pay(new CashPayment(order1.TotalPrice()));
        cafe1.ProcessOrder(order1);

        var order2 = new Order(2, new Customer("김영희", "010-2222-2222"), OrderType.TakeOut);
        order2.AddMenu(latte, 1);
        order2.SetUseDisposable(true);
        order2.Pay(new CardPayment(order2.TotalPrice()));
        cafe1.ProcessOrder(order2);

        var order3 = new Order(3, new Customer("이철수", "010-7777-8888"), OrderType.Delivery);
        order3.AddMenu(americano, 1);
        order3.AddMenu(vanillaLatte, 2);
        order3.SetUseDisposable(true);
        order3.Pay(new CardPayment(order3.TotalPrice()));
        cafe1.ProcessOrder(order3);

        var order4 = new Order(4, new Customer("취소고객", "010-9999-0000"), OrderType.InStore);
        order4.AddMenu(vanillaLatte, 1);
        order4.SetUseTumbler(true);
        order4.Pay(new CashPayment(order4.TotalPrice()));
        cafe1.ProcessOrder(order4);
        cafe1.CancelOrder(4);

        var system = new CafeManagementSystem(cafe1);
        system.Run();
    }
}
