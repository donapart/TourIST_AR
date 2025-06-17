# TourIST°FREY AR Unity Starterpaket

## Quickstart

1. Öffne Unity 2021.3+ und wähle 'Open Project'.
2. Navigiere zum Ordner `TourIST_FreY_Starter` und öffne ihn.
3. Installiere im Package Manager:
   - AR Foundation
   - ARCore XR Plugin
   - ARKit XR Plugin
4. Öffne `Assets/Scenes/MainARScene.unity` (erstelle ggf. Platzhalter-Szene).
5. Importiere Appoly/ARCore-Location in das Projekt.
6. Ziehe `GPSManager` auf ein leeres GameObject und weise `LocationProvider` zu.
7. Platziere `DummyMarker`-Prefabs in der Szene und passe Koordinaten an.
8. Teste das Projekt auf Android und iOS.

## SurrealDB Setup

1. Starte eine SurrealDB Instanz (lokal oder remote).
2. Erstelle Namespace `touristfrey` und Database `arapp`.
3. Lege eine `marker` Tabelle mit Feldern `id`, `label`, `latitude`, `longitude` an.
4. Passe in `SurrealDBClient` die `restUrl`, `username` und `password` an.

Die REST‑Abfrage `SELECT * FROM marker;` liefert ein JSON Array ähnlich:

```json
[
  {
    "result": [
      { "id": "marker:demo", "label": "Museum", "latitude": 48.137, "longitude": 11.576 }
    ]
  }
]
```

Dieses Format wird von `MarkerManager` erwartet und kann optional über `PlayerPrefs` zwischengespeichert werden.

## Struktur

- `Assets/Scripts`: Enthält alle Kern-Scripts.
- `Assets/Prefabs`: Prefabs ablegen (Panels, Marker, etc.).
- `Assets/Images`: Platz für dein Logo.
- `Impressum.txt`: Muster-Impressum.
- `README.md`: Diese Datei.

Viel Erfolg mit deinem AR-Projekt!
\n## Weitere Ressourcen

- `TourIST_AR-main` enthält eine minimale README für das Hauptprojekt.
- `TourIST_FreY_Doku` beinhaltet die HTML-Dokumentation.
- `TourIST_FreY_Starter` stellt die eigentlichen Unity-Dateien zur Verfügung.
