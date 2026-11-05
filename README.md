# SlovníHra – datový slovník

Samostatná datová vrstva pro pět současných kategorií. Aplikace zatím není přepojena na JSON; tento balíček obsahuje pouze data, manifest a pomocné nástroje.

## Soubory

- `wwwroot/data/dictionary.json` – manifest kategorií, názvy, soubory a počty slov podle obtížnosti.
- `wwwroot/data/animals.json` – zvířata.
- `wwwroot/data/czech.json` – místa, regiony, řeky a památky v Česku.
- `wwwroot/data/food.json` – jídla, suroviny a pokrmy.
- `wwwroot/data/technology.json` – zařízení, software a technické pojmy.
- `wwwroot/data/sport.json` – sporty a sportovní disciplíny.

Každá položka má `text` a `difficulty` (1 = lehká, 2 = střední, 3 = těžká). Víceslovné výrazy jsou podporované.

## Kontrola

Z kořene projektu spusť:

```powershell
py tools\validate_dictionary.py
```

Případně lze předat konkrétní soubory jako argumenty.

## Poznámky k redakci

Obtížnost je pracovní redakční zařazení, nikoli objektivní jazyková norma. Seznamy jsou kurátorované herní kandidáty; před nasazením je vhodná ještě lidská obsahová korektura, zejména u odbornějších výrazů a místních názvů. Generátor `tools/build_dictionaries.py` vytváří JSON soubory a manifest z definovaných dat.
