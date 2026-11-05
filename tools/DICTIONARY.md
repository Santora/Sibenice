# Slovník SlovníHry

Datové soubory slovníku jsou oddělené od herního kódu. Každá kategorie může být samostatný JSON soubor.

## Formát

- `version`: verze formátu dat
- `language`: jazyk slovníku (`cs-CZ`)
- `category.id`: stabilní technický identifikátor
- `category.name`: zobrazovaný název
- `words[].text`: hádané slovo, velkými písmeny
- `words[].difficulty`: obtížnost 1 (lehká), 2 (střední), 3 (těžká)

## Kontrola

Z kořenové složky projektu spusť:

```powershell
py tools/validate_dictionary.py
```

Nebo ověř konkrétní soubor:

```powershell
py tools/validate_dictionary.py wwwroot/data/animals.json
```

Validátor používá pouze standardní knihovnu Pythonu.

## Stav

`animals.json` obsahuje první pracovní dávku 100 kandidátů. Není to ještě finální tisícovka; před nasazením projde jazykovou a herní redakcí.
