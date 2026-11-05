namespace SlovniHra.Models;

public static class WordBank
{
    public const string AllCategories = "Vše";

    public static readonly IReadOnlyDictionary<string, string[]> Words = new Dictionary<string, string[]>
    {
        ["🐾 Zvířata"] =
        [
            "KROKODÝL", "VEVERKA", "NOSOROŽEC", "CHAMELEON", "TUČŇÁK", "JEŽEK", "LIŠKA", "DELFÍN",
            "ŽIRAFA", "PAPOUŠEK", "VELBLOUD", "KAPYBARA", "SLON", "TYGR", "LEV", "VLK",
            "MEDVĚD", "ZAJÍC", "KRÁLÍK", "SRNA", "JELEN", "VYDRA", "BOBR", "SOVA",
            "ČÁP", "LABUŤ", "OREL", "SÝKORA", "KOHOUT", "SLEPICE", "KACHNA", "HUSA",
            "KŮŇ", "OSLÍK", "KOZA", "OVCE", "PRASE", "ŽELVA", "ŽÁBA", "MOTÝL"
        ],
        ["🏰 Česko"] =
        [
            "OSTRAVA", "PLZEŇ", "LIBEREC", "OLOMOUC", "KARLOVY VARY", "ČESKÝ KRUMLOV", "HAVÍŘOV", "ZLÍN",
            "PARDUBICE", "ŠPINDLERŮV MLÝN", "KUTNÁ HORA", "TŘEBOŇ", "PRAHA", "BRNO", "JIHLAVA", "OPAVA",
            "KARVINÁ", "FRÝDEK MÍSTEK", "ČESKÉ BUDĚJOVICE", "HRADEC KRÁLOVÉ", "ÚSTÍ NAD LABEM", "MLADÁ BOLESLAV", "PÍSEK", "TÁBOR",
            "ZNOJMO", "MIKULOV", "TELČ", "LITOMYŠL", "KROMĚŘÍŽ", "LEDNICE", "KONOPIŠTĚ", "KŘIVOKLÁT",
            "KARLŠTEJN", "PRADĚD", "ŠUMAVA", "KRKONOŠE", "MORAVA", "SÁZAVA", "VLTAVA", "ŘÍP"
        ],
        ["🍲 Jídlo"] =
        [
            "SVÍČKOVÁ", "KNEDLÍK", "PALAČINKY", "GULÁŠ", "TRDELNÍK", "BRAMBORÁK", "ŘÍZEK", "OPEKANÝ SÝR",
            "KOLÁČ", "ZELÍ", "LEČO", "BUCHTY", "OVOCNÉ KNEDLÍKY", "ČESNEČKA", "BRAMBORAČKA", "KULAJDA",
            "RAJSKÁ OMÁČKA", "KOPROVÁ OMÁČKA", "PEČENÁ KACHNA", "VEPŘO KNEDLO ZELO", "SEKANÁ", "ŘEZNICKÝ PREJT", "ŠUNKOFLEKY", "RIZOTO",
            "LASAGNE", "PIZZA", "ŠPAGETY", "HAMBURGER", "HOT DOG", "BRAMBOROVÝ SALÁT", "VAJEČINA", "TOPINKA",
            "CHLEBA", "ROHLÍK", "VÁNOČKA", "BÁBOVKA", "OVOCNÝ KOLÁČ", "ZMRZLINA", "PERNÍK", "JABLEČNÝ ZÁVIN"
        ],
        ["💻 Technologie"] =
        [
            "PROCESOR", "KLÁVESNICE", "ALGORITMUS", "SERVER", "PROGRAMÁTOR", "DATABÁZE", "PROHLÍŽEČ", "ROUTER",
            "KOMPILÁTOR", "APLIKACE", "HESLO", "OBRAZOVKA", "NOTEBOOK", "TABLET", "TELEFON", "MONITOR",
            "TISKÁRNA", "SKENER", "MYŠ", "REPRODUKTOR", "MIKROFON", "SLUCHÁTKA", "OPERAČNÍ SYSTÉM", "PROGRAM",
            "SOUBOR", "SLOŽKA", "INTERNET", "SÍŤ", "WIFI", "BLUETOOTH", "PAMĚŤ", "PEVNÝ DISK",
            "FLASH DISK", "GRAFICKÁ KARTA", "ZÁKLADNÍ DESKA", "ZDROJ", "KABEL", "KONEKTOR", "ŠIFROVÁNÍ", "ROBOT"
        ],
        ["⚽ Sport"] =
        [
            "HOKEJ", "FOTBAL", "LYŽOVÁNÍ", "TENIS", "PLAVÁNÍ", "VOLEJBAL", "ATLETIKA", "ŠERMOVÁNÍ",
            "CYKLISTIKA", "BIATLON", "BASKETBAL", "VESLOVÁNÍ", "HÁZENÁ", "FLORBAL", "BADMINTON", "GOLF",
            "BOX", "JUDO", "KARATE", "ZÁPAS", "SKOKY NA LYŽÍCH", "SNOWBOARDING", "BĚH", "MARATON",
            "TRIATLON", "STOLNÍ TENIS", "KANOISTIKA", "RAFTING", "LEZENÍ", "KULTURISTIKA", "BRUSLENÍ", "KRIKET",
            "RUGBY", "BASEBALL", "SOFTBALL", "ŠACHY", "DOSTIHY", "JÍZDA NA KONI", "SPORTOVNÍ GYMNASTIKA", "AUTOMOBILOVÉ ZÁVODY"
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
