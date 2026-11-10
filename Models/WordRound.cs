namespace SlovniHra.Models;

/// <summary>Stav jednoho slova: hádaná písmena (bez diakritiky) a z nich odvozené chyby / výhra / prohra.</summary>
public sealed class WordRound
{
    public const int MaxWrong = 7;

    public string Word { get; }
    public string Category { get; }
    public HashSet<char> Guessed { get; } = [];

    public WordRound(string word, string category, string guessed = "")
    {
        Word = word;
        Category = category;
        foreach (var c in guessed)
        {
            Guessed.Add(c);
        }
    }

    public bool Contains(char key) => Word.Any(w => WordBank.Normalize(w) == key);

    public int Count(char key) => Word.Count(w => WordBank.Normalize(w) == key);

    public bool IsRevealed(char c) => Guessed.Contains(WordBank.Normalize(c));

    public int Wrong => Guessed.Count(g => !Contains(g));

    public bool Won => Word.Length > 0 && Word.Where(char.IsLetter).All(IsRevealed);

    public bool Lost => Wrong >= MaxWrong;

    public bool Finished => Won || Lost;

    /// <summary>Vrací null, pokud se tah ignoruje; jinak true (písmeno je ve slově) / false (chyba).</summary>
    public bool? Guess(char key)
    {
        if (Finished || !Guessed.Add(key))
        {
            return null;
        }

        return Contains(key);
    }
}
