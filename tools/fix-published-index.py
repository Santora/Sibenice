"""Po publikaci opraví v index.html odkaz na Blazor, pokud se soubor jmenuje s otiskem (fingerprint).

Použití: python3 tools/fix-published-index.py release/wwwroot
Pokud už je index.html v pořádku (nebo prostý soubor existuje), neudělá nic.
"""
import base64
import hashlib
import json
import pathlib
import re
import sys

root = pathlib.Path(sys.argv[1])
index = root / "index.html"
fw = root / "_framework"

html = index.read_bytes().decode("utf-8")
plain = fw / "blazor.webassembly.js"
script_re = re.compile(r'<script src="_framework/blazor\.webassembly[^"]*\.js"></script>')

print("Soubory Blazoru v _framework:")
for p in sorted(fw.glob("blazor.webassembly*")):
    print("  ", p.name)

if "#[" not in html and 'src="_framework/blazor.webassembly.js"' not in html:
    print("index.html už odkazuje na konkrétní soubor, nic neupravuji.")
    sys.exit(0)

if plain.exists():
    new = html.replace("#[.{fingerprint}]", "")
    print("Prostý blazor.webassembly.js existuje, jen odstraňuji případný zástupný zápis.")
else:
    cands = [
        p.name for p in fw.glob("blazor.webassembly.*.js")
        if re.fullmatch(r"blazor\.webassembly\.[A-Za-z0-9]+\.js", p.name)
    ]
    if len(cands) != 1:
        print(f"::error::Nelze určit soubor Blazoru, nalezeno: {cands}")
        sys.exit(1)

    imports = {}
    for p in fw.glob("*.js"):
        m = re.fullmatch(r"(.+)\.([a-z0-9]{10})\.js", p.name)
        if m:
            imports[f"./_framework/{m.group(1)}.js"] = f"./_framework/{p.name}"

    importmap = '<script type="importmap">' + json.dumps({"imports": imports}) + "</script>"
    new_tags = importmap + '\n    <script src="_framework/' + cands[0] + '"></script>'
    new = script_re.sub(lambda _: new_tags, html, count=1)
    print(f"Odkaz přepsán na {cands[0]}, importmap obsahuje {len(imports)} položek.")

if script_re.search(new) is None or "#[" in new:
    print("::error::index.html se nepodařilo opravit.")
    sys.exit(1)

index.write_bytes(new.encode("utf-8"))

# Kontrolní součet index.html musí sedět v seznamu souborů pro service worker.
sw = root / "service-worker-assets.js"
if sw.exists():
    text = sw.read_bytes().decode("utf-8")
    s, e = text.index("{"), text.rindex("}") + 1
    data = json.loads(text[s:e])
    digest = "sha256-" + base64.b64encode(hashlib.sha256(new.encode("utf-8")).digest()).decode()
    for asset in data["assets"]:
        if asset["url"] == "index.html":
            asset["hash"] = digest
    sw.write_bytes((text[:s] + json.dumps(data) + text[e:]).encode("utf-8"))
    print("Kontrolní součet index.html v service-worker-assets.js aktualizován.")

print("Hotovo.")
