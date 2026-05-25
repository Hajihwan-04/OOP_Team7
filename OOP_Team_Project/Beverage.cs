using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    // 1. 일반 클래스 (3/10) , 상속 1단
    public class Beverage : Menu
    {
        protected bool temperature;

        public Beverage(int id, string name, decimal price, bool temp)
            : base(id, name, price)
        {
            this.temperature = temp;
        }

        //7. 매서드 오버라이딩 (1/5)
        public override decimal CalculatePrice() => price;

        //7. 매서드 오버라이딩 (2/5)
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("└ 온도: " + (temperature ? "HOT" : "ICED"));
        }
    }
}
