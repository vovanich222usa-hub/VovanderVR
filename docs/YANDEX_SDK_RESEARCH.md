# Yandex SDK research — 2026-11-10

## Confirmed from official documentation

- Yandex MapKit is an Android/iOS mobile SDK and lists panoramas among its capabilities.
- Android support requires API level 26 or higher.
- Panorama Player is part of the **Full MapKit SDK** (not the Lite SDK).
- The Android Panorama Player API exposes:
  - opening a panorama by ID;
  - current panorama ID and geographic position;
  - view direction and view-span controls;
  - enabling/disabling movement arrows and rotation;
  - a `historicalPanoramas()` method that returns earlier panoramas published at the current panorama position. Its result is updated after the panorama-change callback.
- Yandex's usage terms say panorama display must occur inside an application using the corresponding MapKit Mobile SDK; displaying Yandex panoramas in a third-party service or over third-party map material is prohibited.

## Official references

- MapKit overview and supported platforms: https://yandex.com/maps-api/docs/mapkit/index.html
- Android setup and API-key initialization: https://yandex.com/maps-api/docs/mapkit/android/generated/getting_started.html
- Panorama Player API: https://yandex.com/maps-api/docs/mapkit/com/yandex/mapkit/places/panorama/Player.html
- Russian MapKit usage terms: https://www.yandex.ru/legal/maps_api/ru/

## Not yet proven

The Android SDK documentation does not by itself prove that the native panorama player can be composited as a correct stereo 360° environment inside a Unity/OpenXR Quest application. This is the highest-risk technical item.

Before building the complete Unity interaction layer, create a minimal Android/Unity integration spike and verify:
1. MapKit Full can be initialized on Quest 3 (Android ARM64).
2. The official panorama view can be presented inside the headset without relying on a PC.
3. Head tracking can drive the panorama view in a way that feels correct in VR.
4. The renderer maintains acceptable frame rate and avoids a flat mono panel if immersive stereo is required.
5. The approach complies with Yandex SDK terms and attribution requirements.

If native view embedding into Unity is not viable, evaluate a native Android VR host or another Yandex-authorized integration before committing to a rendering architecture.

## API key handling

Create an API key in the Yandex developer console. Keep it out of Git. Inject it through a local, ignored configuration file or a secure build setting. Never send the key in chat or commit it to this repository.

## Historical panorama control

The existence of `historicalPanoramas()` confirms a historical-panorama list API. The implementation still needs to inspect the returned objects for capture date/ID and verify whether every location has historical imagery. The left-stick year selector remains a later milestone.
