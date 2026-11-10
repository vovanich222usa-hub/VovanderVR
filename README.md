# Vovander VR

Standalone VR street-panorama explorer for Meta Quest 3, using Yandex Maps panorama services through supported SDK/API integrations.

## Project status

**Phase 0 — repository initialized / technical validation in progress.**

The first milestone is a Quest 3 APK that can display a real Yandex panorama in stereo VR and respond to head movement. Navigation arrows, thumbstick movement, and an in-world map follow after the panorama integration is proven.

## Target platform

- Meta Quest 3
- Android / ARM64
- Unity 6
- OpenXR
- Meta XR SDK / Interaction SDK
- Yandex MapKit Full, subject to current SDK terms and technical compatibility

## Planned controls

| Input | Action |
|---|---|
| Head movement | Look around the 360° panorama |
| Right thumbstick | Navigate between available panorama nodes |
| Panorama arrow selection | Move to linked panorama |
| Right controller B | Toggle the in-world map |
| Map selection | Move to a selected panorama location |
| Left thumbstick up/down | Reserved for historical panorama selection (later phase) |

## Important integration constraint

Do not download, extract, cache, or repackage Yandex panorama imagery as independent textures unless the applicable Yandex license and SDK explicitly permit it. The implementation must use an authorized display path and preserve required attribution.

Historical panorama browsing is deferred until the current SDK's historical panorama API and date metadata are verified.

## Repository layout

- `docs/` — architecture, milestones, SDK validation notes
- `UnityProject/` — Unity project will be added after SDK/version setup is confirmed
- `README.md` — project overview

## Build status

No APK has been built yet. A working Unity editor installation, Android build modules, Meta Quest SDK setup, and Yandex API credentials are required for a device build.
