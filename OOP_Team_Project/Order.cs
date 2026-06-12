using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{

    public delegate void OrderCompletedHandler(string message);
    // 1. 일반 클래스 (1/10)
    public class Order
    {
        private int orderCount = 0;
        private List<Menu> items = new List<Menu>(); // 21. 컬렉션
        public Customer OrderCustomer { get; set; }
        public bool whereToEat;
        
        public Order(Customer customer)
        {
            OrderCustomer = customer;
        }

        //15. 인덱서
        public Menu this[int index]
        {
            get { return items[index]; }
        }

        //4. 매소드 오버로딩
        public void AddItem(Menu item)
        {
            orderCount++;
            items.Add(item);
        }
        //4. 매소드 오버로딩
        public void AddItem(Menu item, int count)
        {
            for (int i = 0; i < count; i++)
            {
                items.Add(item);
            }
        }
        //4. 매소드 오버로딩
        public void AddItem(List<Menu> itemList)
        {
            items.AddRange(itemList);
        }

        public void DeleteItem(int index)
        {

            if (index >= 0 && index < items.Count)
            {
                items.RemoveAt(index);
                orderCount--;
            }
        }

        // 람다식(Sum)
        public void CalculateTotal(out decimal total)
        {
            total = items.Sum(item => item.CalculatePrice()); // 람다식1
        }

        public void PrintOrder()
        {
            Console.WriteLine("\n--- " + OrderCustomer.GetName() + "님의 주문 내역 ---");
            items.ForEach(item => item.PrintReceipt()); // 람다식2

            CalculateTotal(out decimal total);
            Console.WriteLine("----------------------------");
            Console.WriteLine("총 금액: " + total.ToString("#,#0") + " 원");
        }

        // 결제 처리 기능 (델리게이트 활용)
        public void ProcessPayment(IPayable paymentMethod, OrderCompletedHandler callback)
        {
            if (items.Count == 0)
            {
                // throw 예외 강제 발생 만족
                throw new InvalidOrderException("장바구니가 비어 있어 결제할 수 없습니다.");
            }

            CalculateTotal(out decimal total);
            paymentMethod.Pay(total);

            // 델리게이트 콜백 실행
            callback?.Invoke($"{OrderCustomer.GetName()}님의 {total:C} 결제가 완료되었습니다!");
            items.Clear(); // 결제 완료 후 장바구니 비우기
        }

        public int getOrderCount()
        {
            return orderCount;
        }
    }
}
