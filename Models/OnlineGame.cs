namespace SlovniHra.Models;

/// <summary>
/// Celý stav online hry. Posílá se při každé změně celý (ne jednotlivé tahy), takže ztracená zpráva nevadí:
/// přijímá se vždy stav s vyšší verzí V. Ve hře má vždy jen jeden hráč právo měnit stav (ten, kdo je na tahu).
/// </summary>
public sealed class GameSync
{
    public int V { get; set; }

    /// <summary>"A" = střídání nad stejným slovem, "B" = jeden zadává slovo a druhý hádá.</summary>
    public string Mode { get; set; } = "A";

    /// <summary>setting (někdo zadává slovo) | playing | over</summary>
    public string Phase { get; set; } = "setting";

    public string Word { get; set; } = "";
    public string Category { get; set; } = "";

    /// <summary>Hádaná písmena v pořadí tahů.</summary>
    public string Guessed { get; set; } = "";

    /// <summary>Režim A: kdo je na tahu. Režim B: kdo hádá. (0 = hostitel, 1 = host)</summary>
    public int Turn { get; set; }

    public int Round { get; set; }
    public int[] Scores { get; set; } = [0, 0];
    public string[] Names { get; set; } = ["", ""];
}

public sealed class Msg
{
    /// <summary>hello (host se hlásí) | sync (celý stav) | req (pošli mi stav)</summary>
    public string K { get; set; } = "";

    public string From { get; set; } = "";
    public string? Name { get; set; }
    public GameSync? S { get; set; }
}

public static class OnlineGame
{
    /// <summary>Režim B: kdo zadává slovo v daném kole (hostitel začíná, pak se střídají).</summary>
    public static int SetterOf(int round) => (round - 1) % 2;

    public static void EnterSetting(GameSync s)
    {
        s.Round++;
        s.Phase = "setting";
        s.Word = "";
        s.Category = "";
        s.Guessed = "";
        s.V++;
    }

    public static void Begin(GameSync s, string word, string category)
    {
        s.Word = word;
        s.Category = category;
        s.Guessed = "";
        s.Phase = "playing";
        s.Turn = s.Mode == "B" ? 1 - SetterOf(s.Round) : (s.Round - 1) % 2;
        s.V++;
    }

    /// <summary>Provede tah. Vrací false, pokud ho nelze provést (není na tahu, písmeno už padlo, hra skončila).</summary>
    public static bool ApplyGuess(GameSync s, char key, int actor)
    {
        if (s.Phase != "playing" || s.Turn != actor)
        {
            return false;
        }

        var r = new WordRound(s.Word, s.Category, s.Guessed);
        var result = r.Guess(key);
        if (result is null)
        {
            return false;
        }

        s.Guessed += key;

        if (s.Mode == "B")
        {
            if (r.Won)
            {
                s.Scores[actor] += 10 + (WordRound.MaxWrong - r.Wrong);
                s.Phase = "over";
            }
            else if (r.Lost)
            {
                s.Scores[1 - actor] += 10;
                s.Phase = "over";
            }
        }
        else if (result == true)
        {
            s.Scores[actor] += r.Count(key);
            if (r.Won)
            {
                s.Scores[actor] += 5;
                s.Phase = "over";
            }
        }
        else if (r.Lost)
        {
            s.Phase = "over";
        }
        else
        {
            s.Turn = 1 - s.Turn;
        }

        s.V++;
        return true;
    }
}
