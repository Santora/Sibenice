# Šibenice – slovní hra (Blazor PWA)

Česká slovní hra „šibenice" pro Android. Blazor WebAssembly, .NET 10, instalovatelná jako PWA (funguje offline).

## Fáze vývoje
1. **Zvuky a vibrace** – hotovo (přepínače 🔊 / 📳 v záhlaví)
2. Režim pro 2+ hráče – připravuje se
3. Kvíz s otázkami – připravuje se

## Hra
- 5 kategorií (zvířata, Česko, jídlo, technologie, sport) + „Vše"
- hádá se bez diakritiky (Ř = R), po uhodnutí se písmeno zobrazí správně s diakritikou
- 7 životů, nápověda za 1 život, skóre, série výher a rekord (uložený v telefonu)
- zvuky jsou syntetizované (Web Audio), vibrace přes `navigator.vibrate` (na iPhonu nefunguje, na Androidu ano)

## Lokální spuštění
```bash
dotnet run
```

## Nasazení na GitHub Pages (workflow je hotový)
1. Na GitHubu vytvoř **veřejný** repozitář (např. `sibenice`).
2. V adresáři projektu:
   ```bash
   git init -b main
   git add .
   git commit -m "Šibenice – první verze"
   git remote add origin https://github.com/<tvoje-jmeno>/sibenice.git
   git push -u origin main
   ```
3. V repozitáři otevři *Settings → Pages* a v *Source* zvol **GitHub Actions**.
4. Po doběhnutí workflow (záložka *Actions*) je hra na `https://<tvoje-jmeno>.github.io/sibenice/`.
5. V Chrome na Androidu otevři adresu → menu ⋮ → **Nainstalovat aplikaci**.

Každý další `git push` do `main` nasadí novou verzi. Aplikace si po instalaci drží offline cache,
novou verzi načte při dalším spuštění s připojením (případně ji zavři a otevři znovu).

## Přidání slov
Seznam je v `Models/WordBank.cs`. Slova piš velkými písmeny s diakritikou, mezery jsou povolené.
