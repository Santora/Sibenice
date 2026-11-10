using System.Net.Http.Json;
using System.Text.Json;

namespace SlovniHra.Models;

public sealed record DictionaryWord(string Text, int Difficulty);
public sealed record DictionaryCategory(string Id, string Name, string File, int WordCount);
public sealed class DictionaryManifest
{
    public int Version { get; set; }
    public string Language { get; set; } = "";
    public List<DictionaryCategory> Categories { get; set; } = [];
}
public sealed class CategoryDictionary
{
    public DictionaryCategory Category { get; set; } = new("", "", "", 0);
    public List<DictionaryWord> Words { get; set; } = [];
}

public static class WordBank
{
    public const string AllCategories = "Vše";
    public const string AllDifficulties = "Všechny";
    private static readonly Dictionary<char, char> Diacritics = new()
    {
        ['Á'] = 'A', ['Č'] = 'C', ['Ď'] = 'D', ['É'] = 'E', ['Ě'] = 'E', ['Í'] = 'I',
        ['Ň'] = 'N', ['Ó'] = 'O', ['Ř'] = 'R', ['Š'] = 'S', ['Ť'] = 'T', ['Ú'] = 'U',
        ['Ů'] = 'U', ['Ý'] = 'Y', ['Ž'] = 'Z'
    };

    private static IReadOnlyDictionary<string, List<DictionaryWord>> _words =
        new Dictionary<string, List<DictionaryWord>>();
    public static IReadOnlyDictionary<string, List<DictionaryWord>> Words => _words;
    public static bool IsLoaded => _words.Count > 0;

    public static async Task LoadAsync(HttpClient http)
    {
        if (IsLoaded) return;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var manifest = await http.GetFromJsonAsync<DictionaryManifest>("data/dictionary.json", options)
            ?? throw new InvalidDataException("Manifest slovníku se nepodařilo načíst.");
        var loaded = new Dictionary<string, List<DictionaryWord>>();
        foreach (var item in manifest.Categories)
        {
            var data = await http.GetFromJsonAsync<CategoryDictionary>($"data/{item.File}", options)
                ?? throw new InvalidDataException($"Slovník {item.File} se nepodařilo načíst.");
            if (data.Words.Count == 0) throw new InvalidDataException($"Slovník {item.File} je prázdný.");
            loaded[item.Name] = data.Words;
        }
        _words = loaded;
    }

    public static char Normalize(char c)
    {
        c = char.ToUpperInvariant(c);
        return Diacritics.TryGetValue(c, out var plain) ? plain : c;
    }

    public static bool TryCleanWord(string? input, out string clean)
    {
        clean = "";
        if (string.IsNullOrWhiteSpace(input)) return false;
        var chars = new List<char>();
        var lastSpace = true;
        foreach (var raw in input.Trim())
        {
            if (char.IsWhiteSpace(raw))
            {
                if (!lastSpace) { chars.Add(' '); lastSpace = true; }
                continue;
            }
            var c = char.ToUpperInvariant(raw);
            var n = Normalize(c);
            if (n < 'A' || n > 'Z') return false;
            chars.Add(c);
            lastSpace = false;
        }
        clean = new string(chars.ToArray()).Trim();
        var letters = clean.Count(char.IsLetter);
        return letters is >= 2 and <= 25 && clean.Length <= 30;
    }

    public static IEnumerable<(string Category, string Word, int Difficulty)> Pick(string? category, int? difficulty = null)
    {
        foreach (var (cat, words) in Words)
        {
            if (category is not null && category != AllCategories && category != cat) continue;
            foreach (var word in words)
                if (difficulty is null || word.Difficulty == difficulty)
                    yield return (cat, word.Text, word.Difficulty);
        }
    }
}
