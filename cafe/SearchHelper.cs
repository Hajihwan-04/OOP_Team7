namespace cafe;

// 검색. static
public static class SearchHelper
{
    // 제너릭, out 사용
    public static bool TryFind<T>(List<T> items, Predicate<T> condition, out T? result)
    {
        foreach (var item in items)
        {
            if (condition(item)) // Predicate condition: 조건 만족인지 true, false
            {
                result = item;
                return true;
            }
        }

        result = default;
        return false;
    }
}
