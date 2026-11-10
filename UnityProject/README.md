# Unity prototype setup

This folder contains the first C# prototype scripts. It is not yet a complete Unity project and cannot be built into an APK until the Unity project, package versions, scenes, and licensed SDK assets are installed.

## Required packages

- Unity Input System
- Unity OpenXR Plugin
- Meta XR Core SDK
- Meta XR Interaction SDK (for Meta controller/runtime model prefabs)

Use the current Meta setup guide and pin compatible package versions in the actual Unity project before committing generated package lock files.

## Scene wiring

1. Create the Quest OpenXR rig and configure Android / ARM64.
2. Add `VovanderLocomotion` to the player rig root. Assign a Vector2 Input Action bound to the left controller primary 2D axis. Left X rotates; left Y moves forward/back.
3. Add `VovanderMapToggle` and bind its action to the right controller B / secondary button. Assign the world-space map panel.
4. Add `VovanderRebindingManager` to the settings object. Add all rebindable InputActionReferences to its Actions list. Connect settings buttons to `BeginRebind(actionListIndex, bindingIndex)`.
5. Add `ControllerInputAnimator` under each controller visual. Assign trigger/grip/stick actions and the corresponding moving pivots from the model hierarchy.

## Meta controller models

Use Meta's `OVRRuntimeControllerPrefab` under the left and right controller anchors, configured as L Touch and R Touch. Enable Render Model Support in OVR Manager. The runtime model is supplied by the Quest runtime and tracks the actual controller. See the official Meta guide linked in `docs/CONTROLLERS_AND_INPUT.md`.

The animator script is optional and requires the relevant pivots to exist in the selected model hierarchy. Do not assume pivot names: inspect the actual runtime model / SDK sample and assign them in the Inspector. If the runtime model already animates controls natively, avoid double-driving those parts.

## Important

The scripts are a prototype foundation. They have not yet been compiled or tested in Unity or on a Quest 3 device. The Yandex panorama rendering integration is a separate feasibility milestone.
