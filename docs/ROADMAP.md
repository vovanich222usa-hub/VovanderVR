# Vovander VR — roadmap

## Phase 0 — feasibility and project foundation
- [x] Create repository and document scope
- [ ] Confirm current Yandex MapKit Full Android SDK access, license, attribution, and panorama API availability
- [ ] Confirm Unity/OpenXR/Quest compatibility for the selected Yandex panorama rendering path
- [ ] Choose Unity and Meta XR SDK versions
- [x] Add initial Unity C# prototype scripts and setup guide
- [ ] Add complete Unity project skeleton and pinned package manifest

## Phase 1 — panorama proof of concept
- [ ] Initialize OpenXR for Meta Quest 3
- [ ] Request a panorama at a test coordinate using the supported Yandex API
- [ ] Display panorama in both eyes with correct 360° orientation
- [ ] Verify head tracking, performance, and loading/error states on Quest 3
- [ ] Produce first installable APK

## Phase 2 — navigation
- [ ] Read available panorama navigation links/positions from supported API
- [ ] Add gaze/controller-selectable navigation arrows
- [ ] Map right thumbstick directions to valid neighboring panorama links (optional secondary navigation)
- [ ] Add comfortable transition (fade / short movement cue)
- [x] Specify left-stick locomotion: X = smooth yaw turn, Y = forward/back movement
- [x] Specify unique runtime control rebinding with local persistence
- [x] Specify Meta runtime controller models and animated input visuals

## Phase 3 — in-world map
- [ ] Show/hide map with right controller B
- [ ] Center map on current panorama coordinates
- [ ] Show player position and heading
- [ ] Select a map point and resolve a panorama there

## Phase 4 — historical imagery
- [ ] Verify historical panorama endpoint and available date metadata
- [ ] Add left-stick up/down year selection
- [ ] Display selected capture date and unavailable-state feedback

## Phase 5 — polish
- [ ] Favorites and recent locations
- [ ] Settings and comfort options
- [ ] Loading indicators and network error recovery
- [ ] Performance profiling and release APK

## Acceptance criteria for first milestone
1. Installs and launches directly on Meta Quest 3.
2. Opens a real Yandex panorama through an authorized SDK/API route.
3. Head rotation changes the view naturally in stereo.
4. No desktop/PC connection is required during normal use.
