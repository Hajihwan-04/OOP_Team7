using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    // 카드 결제 클래스
    public class CardPayment : IPayable
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"[IC 카드 결제] {amount:C} 가 승인되었습니다.");
        }
    }

    // 현금 결제 클래스
    public class CashPayment : IPayable
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"[현금 결제] {amount:C} 원을 받았습니다. 영수증을 발행합니다.");
        }
    }
}
