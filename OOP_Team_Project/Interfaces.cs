using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_Team_Project
{
    // 1. 직접 정의한 인터페이스 (1/3)
    public interface IPrintable
    {
        void PrintReceipt();
    }

    // 2. 직접 정의한 인터페이스 (2/3)
    public interface IPayable
    {
        void Pay(decimal amount);
    }

    // 3. 직접 정의한 인터페이스 (3/3) - 다중 인터페이스 상속용 기반
    public interface IDiscountable
    {
        decimal ApplyDiscount(decimal originalPrice);
    }

    // 다중 인터페이스 상속 만족 (IDiscountable과 IPayable을 동시에 만족하는 VIP 결제용)
    public interface IVipPayment : IDiscountable, IPayable { }
}
