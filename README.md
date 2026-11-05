# SlovníHra – datové slovníky

Samostatná datová vrstva pro pět stávajících kategorií. Herní aplikace zatím není přepojena na JSON; tento balíček obsahuje data, manifest a validátor.

## Obsah

- `wwwroot/data/dictionary.json` – manifest kategorií, názvy, soubory a počty slov podle obtížnosti.
- `wwwroot/data/animals.json` – zvířata.
- `wwwroot/data/czech.json` – místa, regiony, řeky a památky v Česku.
- `wwwroot/data/food.json` – jídla, suroviny a pokrmy.
- `wwwroot/data/technology.json` – zařízení, software a technické pojmy.
- `wwwroot/data/sport.json` – sporty a sportovní disciplíny.
- `tools/validate_dictionary.py` – kontrola struktury, znaků, obtížnosti a duplicit.

Každá položka obsahuje `text` a `difficulty` (1 = lehká, 2 = střední, 3 = těžká). Víceslovné výrazy jsou podporované.

## Aktuální rozsah

| Kategorie | Počet |
|---|---:|
| Zvířata | 174 |
| Česko | 143 |
| Jídlo | 220 |
| Sport | 140 |
| Technologie | 181 |
| **Celkem** | **858** |

## Kontrola

Z kořene balíčku spusťte:

```powershell
py tools\validate_dictionary.py
```

## Redakční zásady a omezení

Obtížnost je pracovní herní zařazení, nikoli objektivní jazyková norma. Seznamy jsou české herní kandidáty; validátor ověřuje technickou správnost dat, nikoli faktickou správnost každého pojmu ani jeho vhodnost pro každého hráče. Před ostrým nasazením je proto vhodná obsahová korektura, zejména u odbornějších výrazů, názvů disciplín a místních názvů.

Slovník je záměrně oddělený od herní logiky. Další rozšiřování nebo úpravy obsahu nevyžadují změnu UI.

## Obtížnost hry

Obtížnost se vybírá přímo v hlavním rozhraní:

- **Vše** – slova ze všech úrovní
- **Lehká** – `difficulty: 1`
- **Střední** – `difficulty: 2`
- **Těžká** – `difficulty: 3`

Poslední zvolená obtížnost se ukládá v `localStorage` pod klíčem
`sibenice.difficulty`. Slovník se načítá z `wwwroot/data/dictionary.json`
a z jednotlivých souborů kategorií. Publikovaná PWA ukládá JSON soubory
do offline cache stejně jako ostatní statické prostředky.
