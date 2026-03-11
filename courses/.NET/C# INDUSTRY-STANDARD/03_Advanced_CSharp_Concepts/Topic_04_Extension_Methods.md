# 📖 Topic 04: Extension Methods

## 1. Concept Explanation

**Extension methods** let you add methods to existing types without modifying them.

---

## 2. Creating Extension Methods

```csharp
public static class StringExtensions
{
    public static bool IsPalindrome(this string str)
    {
        var reversed = new string(str.Reverse().ToArray());
        return str.Equals(reversed, StringComparison.OrdinalIgnoreCase);
    }
    
    public static string ToTitleCase(this string str)
    {
        return System.Globalization.CultureInfo.CurrentCulture
            .TextInfo.ToTitleCase(str.ToLower());
    }
}

// Usage
string word = "Racecar";
bool isPalindrome = word.IsPalindrome();  // true
string title = "hello world".ToTitleCase();  // Hello World
```

---

## ✅ Next Topic

Continue to: [Topic 05 - Records](./Topic_05_Records.md)
