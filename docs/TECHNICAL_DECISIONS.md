# Technical decisions

## Target runtime
Standalone Meta Quest 3 application, Android ARM64. PCVR is not a target for the initial release.

## Proposed stack
- Unity 6 (exact editor revision to be pinned after compatibility checks)
- OpenXR
- Meta XR SDK / Interaction SDK
- Unity Input System
- Yandex MapKit Full Android SDK, if its current distribution and supported integration permit the required panorama experience

## Key risk: panorama rendering inside Unity
A mobile map SDK may render through Android native views or platform-specific surfaces that are not automatically composited into a Unity OpenXR stereo scene. Before implementing navigation or UI, build a minimal integration spike to prove that the official panorama viewer can be displayed correctly in Quest 3.

Do not assume that a WebView/native surface can be projected into stereo VR without additional work. Do not use undocumented endpoints or scrape Yandex imagery.

## Input design (initial proposal)
- Right B: toggle map panel
- Right thumbstick: choose nearest valid panorama link by direction; use click/debounce to avoid repeated accidental transitions
- Left thumbstick up/down: reserved for historical capture selection
- Trigger: select arrow or map point
- Grip: optional recenter / comfort action, to be decided after prototype testing

## Credentials
Yandex API keys must be stored locally in the developer environment or injected through a secure build configuration. Never commit API keys, signing keys, keystores, or passwords to this repository.

## Open questions to resolve before implementation
1. Current MapKit Full SDK package and supported Unity/native bridge approach.
2. Panorama viewer support and licensing for an independent standalone Quest application.
3. Whether the official panorama view can be rendered into the headset's stereo output at acceptable performance.
4. Required Yandex attribution and any restrictions on screenshots, caching, or interaction.
