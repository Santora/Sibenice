namespace SlovniHra.Models;

/// <summary>
/// Sdílená pravidla pro práci se slovníkem. Samotná slova jsou načítána z JSON souborů.
/// </summary>
public static class WordBank
{
    public const string AllCategories = "Vše";

    private static readonly Dictionary<char, char> Diacritics = new()
    {
        ['Á'] = 'A', ['Č'] = 'C', ['Ď'] = 'D', ['É'] = 'E', ['Ě'] = 'E', ['Í'] = 'I',
        ['Ň'] = 'N', ['Ó'] = 'O', ['Ř'] = 'R', ['Š'] = 'S', ['Ť'] = 'T', ['Ú'] = 'U',
        ['Ů'] = 'U', ['Ý'] = 'Y', ['Ž'] = 'Z'
    };

    /// <summary>Převede písmeno na velké bez diakritiky (Ř -> R), aby šlo hrát na jednoduché klávesnici.</summary>
    public static char Normalize(char c)
    {
        c = char.ToUpperInvariant(c);
        return Diacritics.TryGetValue(c, out var plain) ? plain : c;
    }
}

public sealed class DictionaryManifest
{
    public int Version { get; set; }
    public string Language { get; set; } = "";
    public List<DictionaryCategoryManifest> Categories { get; set; } = [];
}

public sealed class DictionaryCategoryManifest
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string File { get; set; } = "";
    public int WordCount { get; set; }
    public Dictionary<string, int> DifficultyCounts { get; set; } = [];
}

public sealed class CategoryDictionary
{
    public int Version { get; set; }
    public string Language { get; set; } = "";
    public DictionaryCategoryInfo Category { get; set; } = new();
    public List<DictionaryWord> Words { get; set; } = [];
}

public sealed class DictionaryCategoryInfo
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}

public sealed class DictionaryWord
{
    public string Text { get; set; } = "";
    public int Difficulty { get; set; }
}
