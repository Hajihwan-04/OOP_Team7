using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    // 1. 일반 클래스 (1/10)
    public class Order
    {
        private int orderId = 0;
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

        // 람다식(Sum)
        public void CalculateTotal(out decimal total)
        {
            total = items.Sum(item => item.CalculatePrice()); // 람다식1
        }

        public void PrintOrder()
        {
            Console.WriteLine("\n--- " + OrderCustomer.getName() + "님의 주문 내역 ---");
            items.ForEach(item => item.PrintReceipt()); // 람다식2

            CalculateTotal(out decimal total);
            Console.WriteLine("----------------------------");
            Console.WriteLine("총 금액: " + total.ToString("C"));
        }
    }
}
