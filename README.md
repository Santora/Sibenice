# Šibenice – slovní hra (Blazor PWA)

Česká slovní hra „šibenice" pro Android. Blazor WebAssembly, .NET 10, instalovatelná jako PWA (funguje offline).

## Hra
- 5 kategorií (zvířata, Česko, jídlo, technologie, sport) + „Vše"
- hádá se bez diakritiky (Ř = R), po uhodnutí se písmeno zobrazí správně s diakritikou
- 7 životů, nápověda za 1 život, skóre, série výher a rekord (uložený v telefonu)

## Spuštění
```bash
dotnet run
```
Otevři `http://localhost:5000` (port viz výpis).

## Instalace na Android
PWA se dá nainstalovat jen přes **HTTPS** (nebo localhost):

1. `dotnet publish -c Release -o publish`
2. Obsah `publish/wwwroot` nahraj na libovolný statický hosting s HTTPS (Netlify, Cloudflare Pages, Azure Static Web Apps…).
3. V Chrome na Androidu otevři adresu → menu ⋮ → **Nainstalovat aplikaci**.

Rychlý test z PC: připoj telefon přes USB, v Chrome na PC otevři `chrome://inspect/#devices`,
nastav port forwarding (nebo `adb reverse tcp:5000 tcp:5000`) a otevři `http://localhost:5000` v telefonu.

Při nasazení do podadresáře (např. GitHub Pages) uprav `<base href>` v `wwwroot/index.html`,
`scope`/`start_url` v `manifest.webmanifest` a `base` v `service-worker.published.js`.

## Přidání slov
Seznam je v `Models/WordBank.cs`. Slova piš velkými písmeny s diakritikou, mezery jsou povolené.
