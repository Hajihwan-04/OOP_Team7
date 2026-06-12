using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    // 1. 일반 클래스 (5/10) 상속 2단
    public class Coffee : Beverage
    {
        protected int shotCount;

        public Coffee(int id, string name, decimal price, bool temp, int shotCount)
            : base(id, name, price, temp)
        {
            this.shotCount = shotCount;
        }
        //7. 매서드 오버라이딩 (3/5)
        public override decimal CalculatePrice()
        {
            return price + (shotCount * 500m);
        }

        //7. 매서드 오버라이딩 (4/5)
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("└ 샷 추가: " + shotCount + "번 (최종가: " + CalculatePrice().ToString("#,#0") + "원)");
        }
    }
}
