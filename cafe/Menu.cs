namespace cafe;

// 메뉴 사이즈 enum(선택)
public enum MenuSize
{
    Tall,
    Grande,
    Venti
}
// 추상, 인터페이스 구현
public abstract class Menu : IComparable<Menu>, IDisplayable, IStockUsable
{
    protected string name;
    protected int price;
    protected Recipe recipe;

// 캡슐화
    public string Name => name;
    public int Price => price;
    public Recipe Recipe => recipe;

    public Menu(string name, int price, Recipe recipe)
    {
        if (price < 0)
        {
            throw new ArgumentException("가격을 다시 입력해주세요.");
        }

        this.name = name;
        this.price = price;
        this.recipe = recipe;
    }

    public abstract int HowMuch();

    public virtual string Info()
    {
        return $"메뉴: {name}, 가격: {HowMuch()}원";
    }

// 메뉴에 필요한 재고 목록
    public Dictionary<string, int> RequiredIngredients()
    {
        return recipe.Ingredients;
    }

// 메뉴 비교
    public int CompareTo(Menu? other) // null 일 수 있음
    {
        if (other == null) return 1; // null이면 그냥 더 크다고 생각
        return HowMuch().CompareTo(other.HowMuch()); // 가격 비교
    }
}

public class CoffeeMenu : Menu
{
    private MenuSize size;

    public MenuSize Size => size;

    public CoffeeMenu(string name, int price, Recipe recipe, MenuSize size)
        : base(name, price, recipe) // name, price, recipe는 Menu(부모)가 처리
    {
        this.size = size;
    }

// 사이즈 추가금
    private int SizePrice()
    {
        if (size == MenuSize.Tall)
        {
            return 0;
        }

        if (size == MenuSize.Grande)
        {
            return 500;
        }

        if (size == MenuSize.Venti)
        {
            return 1000;
        }

        throw new ArgumentOutOfRangeException(); // 예외 throw
    }

// 사이즈 이름 그냥 한국어로 처리하는거
    private string SizeName()
    {
        if (size == MenuSize.Tall)
        {
            return "톨";
        }

        if (size == MenuSize.Grande)
        {
            return "그란데";
        }

        return "벤티";
    }

// Menu의 HowMuch() 오버라이드
    public override int HowMuch()
    {
        return price + SizePrice();
    }

// Menu의 Info() 오버라이드
    public override string Info()
    {
        return base.Info() + $", 종류: 커피, 온도: 주문할 때 선택 가능, 사이즈: {SizeName()}";
    }
}
