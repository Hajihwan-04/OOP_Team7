namespace cafe;

public abstract class Payment : IDisplayable, IPriced
{
    protected int amount;
    protected DateTime paidAt; // 결제 시각

    public int Amount => amount;
    public DateTime PaidAt => paidAt;

    public Payment(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("결제 금액은 음수가 될 수 없습니다.");
        }

        this.amount = amount;
        paidAt = DateTime.Now;
    }

    public abstract bool Pay();

    public int TotalPrice()
    {
        return amount;
    }

// 결제 정보 : 나중에 더 추가
    public virtual string Info()
    {
        return $"결제 금액: {amount}원, 결제 시각: {paidAt}";
    }
}

// 현금
public class CashPayment : Payment
{
    public CashPayment(int amount) : base(amount)
    {
    }

    public override bool Pay()
    {
        Console.WriteLine("현금 결제 기록.");
        return true;
    }

    public override string Info()
    {
        return base.Info() + ", 결제 방식: 현금";
    }
}

public class CardPayment : Payment
{
    public CardPayment(int amount) : base(amount)
    {
    }

    public override bool Pay()
    {
        Console.WriteLine("카드 결제 기록.");
        return true;
    }

    public override string Info()
    {
        return base.Info() + ", 결제 방식: 카드";
    }
}
