namespace cafe;

// 매출 요약 "구조체" struct
public struct SalesSummary
{
    public int OrderCount { get; }
    public int SoldQuantity { get; }
    public int Sales { get; }
    public int Cost { get; }
    public int Profit { get; }

    public SalesSummary(int orderCount, int soldQuantity, int sales, int cost)
    {
        OrderCount = orderCount;
        SoldQuantity = soldQuantity;
        Sales = sales;
        Cost = cost;
        Profit = sales - cost;
    }

    public void Show()
    {
        Console.WriteLine("[매출 리포트]");
        Console.WriteLine($"총 주문 수: {OrderCount}");
        Console.WriteLine($"총 판매 수량: {SoldQuantity}개");
        Console.WriteLine($"누적 매출: {Sales}원");
        Console.WriteLine($"누적 비용: {Cost}원");
        Console.WriteLine($"순이익: {Profit}원");
    }
}
