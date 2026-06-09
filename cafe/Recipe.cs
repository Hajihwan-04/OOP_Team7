namespace cafe;

public class Recipe
{
    public string Name { get; set; }
    public Dictionary<string, int> Ingredients { get; set; } = new();

    public Recipe(string name)
    {
        Name = name;
    }

    public void AddIngredient(string name, int amount)
    {
        Ingredients[name] = amount;
    }

    public void ShowRecipe()
    {
        Console.WriteLine($"[{Name} 레시피]");
        foreach (var item in Ingredients)
        {
            Console.WriteLine($" - {item.Key}: {item.Value}");
        }
    }
}