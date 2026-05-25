using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{

    // 2. 구조체 1개 이상
    public struct NutritionInfo
    {
        public int Calories { get; set; }
        public int Sugar { get; set; }

        public NutritionInfo(int calories, int sugar)
        {
            Calories = calories;
            Sugar = sugar;
        }
    }
    // 1. 일반 클래스 (4/10)
    public class Dessert : Menu
    {
        private NutritionInfo nutrition;

        public Dessert(int id, string name, decimal price, NutritionInfo nutrition)
            : base(id, name, price)
        {
            this.nutrition = nutrition;
        }
        //7. 매서드 오버라이딩 (5/5)
        public override decimal CalculatePrice() => price;

        //7. 매서드 오버라이딩 (6/5)
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("└ 칼로리: " + nutrition.Calories + "kcal, 당류: " + nutrition.Sugar + "g");
        }
    }
}
