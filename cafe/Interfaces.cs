namespace cafe;

// 인포 인터페이스
public interface IDisplayable
{
    string Info();
}

// 가격 인터페이스
public interface IPriced
{
    int TotalPrice();
}

// 재고 인터페이스
public interface IStockUsable
{
    Dictionary<string, int> RequiredIngredients(); // 재료, 수량
}
