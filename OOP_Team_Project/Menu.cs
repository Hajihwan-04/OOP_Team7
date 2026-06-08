using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    //10. 추상 클래스 1개 이상
    public abstract class Menu : IPrintable, IComparable<Menu>
    {
        protected int id;
        protected string name = "";
        protected decimal price;

        public Menu(int id, string name, decimal price)
        {
            this.id = id;
            this.name = name;
            this.price = price;
        }

        public string Name { get => name; set => name = value; }
        public decimal Price { get => price; set => price = value; }


        public abstract decimal CalculatePrice();

        //7. 매서드 오버라이딩
        public virtual void DisplayInfo()
        {
            Console.WriteLine("메뉴: " + name + " | 기본가: " + price + "원");
        }

        public void PrintReceipt()
        {
            Console.WriteLine(id + ") {0,-15} {1,10}", name, CalculatePrice().ToString("C"));
        }

        public int CompareTo(Menu other)
        {
            if (other == null) return 1;
            return this.CalculatePrice().CompareTo(other.CalculatePrice());
        }

        public int Id
        {
            get { return id; }
        }
    }
}
