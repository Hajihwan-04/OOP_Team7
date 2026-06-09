namespace cafe;

// 예외
// Exception(C# 기본) 상속
public class CafeException : Exception
{
    // 오류 메세지 받고, Excetpion에 전달
    public CafeException(string message) : base(message)
    {
    }
}

// 재고 부족 예외
public class StockShortageException : CafeException
{
    public StockShortageException(string message) : base(message)
    {
    }
}

// 주문 예외
public class InvalidOrderException : CafeException
{
    public InvalidOrderException(string message) : base(message)
    {
    }
}
