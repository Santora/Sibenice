#!/usr/bin/env python3
"""Validate SlovniHra category JSON files without external dependencies."""
from __future__ import annotations
import json, re, sys, unicodedata
from pathlib import Path

ALLOWED = re.compile(r"^[A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ][A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ -]*$")

def key(value: str) -> str:
    return " ".join(unicodedata.normalize("NFC", value.strip().upper()).split())

def main() -> int:
    paths = [Path(p) for p in sys.argv[1:]] or [p for p in sorted(Path("wwwroot/data").glob("*.json")) if p.name != "dictionary.json"]
    if not paths:
        print("Nebyly nalezeny žádné JSON soubory.")
        return 1
    failed = False
    for path in paths:
        try:
            data = json.loads(path.read_text(encoding="utf-8"))
        except Exception as exc:
            print(f"CHYBA {path}: JSON nelze načíst: {exc}")
            failed = True
            continue
        category = data.get("category", {})
        words = data.get("words")
        errors = []
        if data.get("language") != "cs-CZ": errors.append("language musí být cs-CZ")
        if not category.get("id") or not category.get("name"): errors.append("chybí category.id nebo category.name")
        if not isinstance(words, list): errors.append("words musí být pole")
        else:
            seen = set()
            for i, item in enumerate(words, 1):
                if not isinstance(item, dict):
                    errors.append(f"položka {i}: očekáván objekt")
                    continue
                word = item.get("text")
                difficulty = item.get("difficulty")
                if not isinstance(word, str) or not ALLOWED.fullmatch(word):
                    errors.append(f"položka {i}: neplatný formát slova {word!r}")
                    continue
                if difficulty not in (1, 2, 3): errors.append(f"{word}: difficulty musí být 1, 2 nebo 3")
                normalized = key(word)
                if normalized in seen: errors.append(f"duplicitní slovo: {word}")
                seen.add(normalized)
        if errors:
            failed = True
            print(f"\nCHYBY {path}:")
            for error in errors: print(f"  - {error}")
        else:
            counts = {level: sum(w["difficulty"] == level for w in words) for level in (1,2,3)}
            print(f"OK {path}: {len(words)} slov (lehké {counts[1]}, střední {counts[2]}, těžké {counts[3]})")
    return 1 if failed else 0

if __name__ == "__main__":
    raise SystemExit(main())
