namespace SlovniHra.Models;

public static class WordBank
{
    public const string AllCategories = "Vše";

    public static readonly IReadOnlyDictionary<string, string[]> Words = new Dictionary<string, string[]>
    {
        ["🐾 Zvířata"] =
        [
            "KROKODÝL", "VEVERKA", "NOSOROŽEC", "CHAMELEON", "TUČŇÁK", "JEŽEK",
            "LIŠKA", "DELFÍN", "ŽIRAFA", "PAPOUŠEK", "VELBLOUD", "KAPYBARA"
        ],
        ["🏰 Česko"] =
        [
            "OSTRAVA", "PLZEŇ", "LIBEREC", "OLOMOUC", "KARLOVY VARY", "ČESKÝ KRUMLOV",
            "HAVÍŘOV", "ZLÍN", "PARDUBICE", "ŠPINDLERŮV MLÝN", "KUTNÁ HORA", "TŘEBOŇ"
        ],
        ["🍲 Jídlo"] =
        [
            "SVÍČKOVÁ", "KNEDLÍK", "PALAČINKY", "GULÁŠ", "TRDELNÍK", "BRAMBORÁK",
            "ŘÍZEK", "OPEKANÝ SÝR", "KOLÁČ", "ZELÍ", "LEČO", "BUCHTY"
        ],
        ["💻 Technologie"] =
        [
            "PROCESOR", "KLÁVESNICE", "ALGORITMUS", "SERVER", "PROGRAMÁTOR", "DATABÁZE",
            "PROHLÍŽEČ", "ROUTER", "KOMPILÁTOR", "APLIKACE", "HESLO", "OBRAZOVKA"
        ],
        ["⚽ Sport"] =
        [
            "HOKEJ", "FOTBAL", "LYŽOVÁNÍ", "TENIS", "PLAVÁNÍ", "VOLEJBAL",
            "ATLETIKA", "ŠERMOVÁNÍ", "CYKLISTIKA", "BIATLON", "BASKETBAL", "VESLOVÁNÍ"
        ],
    };

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

    public static IEnumerable<(string Category, string Word)> Pick(string? category)
    {
        foreach (var (cat, words) in Words)
        {
            if (category is null || category == AllCategories || category == cat)
            {
                foreach (var w in words) yield return (cat, w);
            }
        }
    }
}
